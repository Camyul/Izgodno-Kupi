using Bytes2you.Validation;
using HtmlAgilityPack;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.Category;
using IzgodnoKupi.Web.Areas.Admin.Models.JsonPayloadModel;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;


namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class StantekCrowlerController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categorieService;
        private readonly IHostingEnvironment _environment;
        private readonly ILogger _logger;

        public StantekCrowlerController(IProductsService productsService, ICategoriesService categorieService, IHostingEnvironment environment, ILogger<StantekCrowlerController> logger)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categorieService, "categiriesService").IsNull().Throw();

            this.productsService = productsService;
            this.categorieService = categorieService;
            this._environment = environment;
            this._logger = logger;
        }

        public IActionResult Index()
        {
            var categories = this.categorieService.GetAll()
                      .Select(c => new CategoryViewModel(c))
                      .OrderBy(x => x.Name)
                      .ToList();

            return View(categories);
        }

        public IActionResult DeleteProductsFromCategory(Guid id)
        {
            SetProductsFromCategoryNotPublished(id);

            return RedirectToAction("Index", "StantekCrowler", new { area = "Admin" });
        }

        public async Task<IActionResult> GetProductsFromCategory(Guid id)
        {
            Category category = this.categorieService.GetById(id);

            SetProductsFromCategoryNotPublished(id);

            IList<int> productIds = await GetProductIdsFromCategory(category);

            IList<ProductStantekViewModel> products = await GetProductsFromCategory(productIds, category);

            AddProductsToDb(products);

            return RedirectToAction("Index", "StantekCrowler", new { area = "Admin" });
        }

        public IActionResult DeleteOnly(bool deleteOnly = false)
        {
            if (deleteOnly)
            {
                this.TempData["DeleteOnly"] = true;
            }
            else
            {
                this.TempData["DeleteOnly"] = false;
            }

            return RedirectToAction("Index", "StantekCrowler", new { area = "Admin" });
        }

        public IActionResult DeleteAllPictures()
        {
            string webRootPath = this._environment.WebRootPath;
            string pathToDirectory = Path.Combine(webRootPath, "productImages");
            bool isDirectoryExists = Directory.Exists(pathToDirectory);

            if (isDirectoryExists)
            {
                Directory.Delete(pathToDirectory, true);
            }
            return RedirectToAction("Index", "StantekCrowler", new { area = "Admin" });
        }

        private async Task<IList<ProductStantekViewModel>> GetProductsFromCategory(IList<int> productIds, Category category)
        {

            var builder = new UriBuilder("https://stantek.com");
            builder.Port = -1;
            var query = HttpUtility.ParseQueryString(builder.Query);


            HttpClient httpClient = new HttpClient();
            IList<ProductStantekViewModel> products = new List<ProductStantekViewModel>();
            foreach (var id in productIds)
            {
                query["i"] = id.ToString();
                builder.Query = query.ToString();
                string url = builder.ToString();

                ProductStantekViewModel product = await GetProductDetails(url, httpClient);
                product.Category = category.Name;

                products.Add(product);
            }
            


            return products;
        }

        private async Task<ProductStantekViewModel> GetProductDetails(string url, HttpClient httpClient)
        {
            string html;
            try
            {
                html = await httpClient.GetStringAsync(url);
            }
            catch (Exception)
            {
                throw new Exception();
            }

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var productDiv = htmlDocument.DocumentNode.Descendants("div")
                                           .Where(node => node.GetAttributeValue("class", "")
                                           .Equals("item-info-desc"))
                                           .FirstOrDefault();

            // TODO https://weblog.west-wind.com/posts/2012/jul/19/net-html-sanitation-for-rich-html-input - Make class HtmlSanitizer

            string name = "";
            if (productDiv != null)
            {

                name = productDiv.Descendants("div")
                                           .LastOrDefault()
                                           .InnerText
                                           .Trim();

                if (name.Length > ValidationConstants.StandartMaxLength)
                {
                    name = name.Substring(0, ValidationConstants.StandartMaxLength);
                }
            }

            var pricesDivs = htmlDocument.DocumentNode.Descendants("div")
                                           .Where(node => node.GetAttributeValue("class", "")
                                           .Equals("item-info-order"))
                                           .FirstOrDefault()
                                           .Descendants("div")
                                           .Where(node => node.GetAttributeValue("class", "")
                                           .Equals("item-price"))
                                           .FirstOrDefault()
                                           .Descendants("div")
                                           .ToArray();

            decimal oldPrice = 0;
            decimal price = 0;
            if (pricesDivs != null)
            {

                string oldPriceLeva = pricesDivs[1].InnerText.Trim();
                // Remove "лв."
                oldPriceLeva = oldPriceLeva.Substring(0, oldPriceLeva.Length - 3); 

                oldPrice = decimal.Parse(oldPriceLeva) / 100;

                string priceLeva = pricesDivs[2].InnerText.Trim();
                // Remove "лв."
                priceLeva = priceLeva.Substring(0, priceLeva.Length - 3);

                price = decimal.Parse(priceLeva) / 100;
            }

            string pictureUrl = htmlDocument.DocumentNode.Descendants("img")
                                           .Where(node => node.GetAttributeValue("id", "")
                                           .Equals("item-info-big-image"))
                                           .FirstOrDefault()
                                           .ChildAttributes("src")
                                           .FirstOrDefault()
                                           .Value;

            
            string fullUrl;
            if (pictureUrl.StartsWith("https://"))
            {
                fullUrl = pictureUrl;
            } 
            else
            {
                fullUrl = "https://stantek.com" + pictureUrl;
            }
            string extension = Path.GetExtension(fullUrl);
            string fileName = DateTime.Now.ToString("yymmddssfff") + extension;
            bool isPictureStoraged = false;
            try
            {
                WebClient myWebClient = new WebClient();

                byte[] imageBytes = myWebClient.DownloadData(fullUrl);
                Stream imageStream = new MemoryStream(imageBytes);

                if (imageStream == null || imageStream.Length == 0)
                {
                    throw new Exception("Missing Image Stream");
                }

                if (imageStream.Length < 5120000)
                {
                    if (string.IsNullOrEmpty(extension))
                    {
                        fileName = fileName + ".jpg";
                    }
                    string webRootPath = this._environment.WebRootPath;
                    string pathToSave = Path.Combine(webRootPath, "productImages", fileName);
                    string pathToDirectory = Path.Combine(webRootPath, "productImages");

                    if (!Directory.Exists(pathToDirectory))
                    {
                        Directory.CreateDirectory(pathToDirectory);
                    }

                    using (FileStream fileStream = new FileStream(pathToSave, FileMode.Create))
                    {
                        await imageStream.CopyToAsync(fileStream);
                        isPictureStoraged = true;
                    }
                }
               
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex.Message);
            }

            var fullDescriptionNode = htmlDocument.DocumentNode.Descendants("div")
                                                              .Where(node => node.GetAttributeValue("class", "")
                                                              .Equals("more-info-panel"))
                                                              .FirstOrDefault();

            string fullDescription = fullDescriptionNode == null ? "" : fullDescriptionNode.InnerHtml;

            ProductStantekViewModel productViewModel = new ProductStantekViewModel()
            {
                Name = name,
                PictureUrl = isPictureStoraged ? "../../productImages/" + fileName : "../../images/no-image.jpg",
                Price = price,
                OldPrice = oldPrice,
                Discount = oldPrice > 0 ? Math.Round((double)(100 - (oldPrice/price * 100)), MidpointRounding.AwayFromZero) : 0,
                FullDescription = fullDescription,
            };

            return productViewModel;
        }

        private async Task<IList<int>> GetProductIdsFromCategory(Category category)
        {
            var rootUrl = "	https://stantek.com/snippet/items-list";
            var httpClient = new HttpClient();

           
            JsonPayloadModel jsonModel = new JsonPayloadModel(category.Name);

            int productsCount;
            IList<int> productsFromCategoryIds = new List<int>();

            do
            {
                string jsonPayload = JsonConvert.SerializeObject(jsonModel);
                StringContent content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PostAsync(rootUrl, content);
                string html = await response.Content.ReadAsStringAsync();

                if (html == String.Empty || html == null)
                {
                    break;
                }

                ParseIdsFromHtml(html, productsFromCategoryIds);
                productsCount = productsFromCategoryIds.Count;

                // 36 represent products witch are return in one post request
                // if they are less than 36 that is a last request
                jsonModel.n += 36;
            } while (productsCount % 36 == 0);

             

            return productsFromCategoryIds;
        }

        private void ParseIdsFromHtml(string html, IList<int> productsFromCategoryIds)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var pagesDivs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("item"))
                .ToList();

            foreach (var div in pagesDivs)
            {
                string id = div.Descendants("div")
                                       .Where(node => node.GetAttributeValue("itemprop", "")
                                       .Equals("identifier"))
                                       .FirstOrDefault()
                                       .InnerText
                                       .Trim();

                productsFromCategoryIds.Add(int.Parse(id));

            }
        }

        private void SetProductsFromCategoryNotPublished(Guid id)
        {
            var products = this.productsService
                                    .GetByCategory(id)
                                    .Where(x => x.Supplier == Supplier.Stantek)
                                    .Where(x => x.IsPublished == true)
                                    .ToList();

            foreach (var product in products)
            {
                product.IsPublished = false;
                productsService.Update(product);
            }
        }

        private void SetAllProductsNotPublished()
        {
            var products = productsService.GetAll()
                                          .Where(x => x.Supplier == Supplier.Stantek)
                                          .Where(x => x.IsPublished == true)
                                          .ToList();
            if (products.Count > 0)
            {
                foreach (var product in products)
                {
                    product.IsPublished = false;
                    productsService.Update(product);
                }
            }
        }

        private void AddProductsToDb(IList<ProductStantekViewModel> products)
        {             
            foreach (var product in products)
            {
                Product dbProduct = productsService.GetByName(product.Name)
                                                   .Where(x => x.Supplier == Supplier.Stantek)
                                                   .FirstOrDefault();
                if (dbProduct != null)
                {
                    dbProduct.Category = this.categorieService.GetByName(product.Category);
                    dbProduct.Name = product.Name;
                    dbProduct.Discount = product.Discount;
                    dbProduct.FullDescription = product.FullDescription;
                    dbProduct.IsPublished = true;
                    dbProduct.OldPrice = product.OldPrice;
                    dbProduct.Pictures.Clear();
                    dbProduct.Pictures.Add(new Picture() { ImageUrl = product.PictureUrl });
                    dbProduct.Price = product.Price;
                    dbProduct.Supplier = Supplier.Stantek;

                    productsService.Update(dbProduct);
                }
                else
                {
                    Product newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.Category = this.categorieService.GetByName(product.Category);
                    newProduct.Discount = product.Discount;
                    newProduct.FullDescription = product.FullDescription;
                    newProduct.IsPublished = true;
                    newProduct.OldPrice = product.OldPrice;
                    newProduct.Pictures.Add(new Picture() { ImageUrl = product.PictureUrl });
                    newProduct.Price = product.Price;
                    newProduct.Supplier = Supplier.Stantek;

                    productsService.AddProduct(newProduct);
                }
            }
        }

        private void AddCategoriesToDb(IList<CategoryStantekViewModel> categories)
        {
            foreach (var cat in categories)
            {
                bool isCategoryExist = this.categorieService.GetByName(cat.Name) != null;

                if (isCategoryExist)
                {
                    continue;
                }

                Category categoryToAdd = new Category()
                {
                    Name = cat.Name,
                    ShowOnHomePage = false,
                    CategoriesGroup = CategoriesGroup.Pc
                };

                this.categorieService.AddCategory(categoryToAdd);
            }
        }
    }
}
