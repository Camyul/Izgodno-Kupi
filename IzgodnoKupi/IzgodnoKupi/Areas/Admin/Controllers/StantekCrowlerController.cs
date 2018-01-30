using Bytes2you.Validation;
using HtmlAgilityPack;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.Category;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class StantekCrowlerController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categorieService;

        public StantekCrowlerController(IProductsService productsService, ICategoriesService categorieService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categorieService, "categiriesService").IsNull().Throw();

            this.productsService = productsService;
            this.categorieService = categorieService;
        }

        public IActionResult Index()
        {
            var categories = this.categorieService.GetAll()
                      .Select(c => new CategoryViewModel(c)).ToList();

            return View(categories);
        }

        public async Task<IActionResult> GetAllProducts()
        {
            var rootUrl = "http://stantek.com/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(rootUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            IList<CategoryStantekViewModel> categories = GetCategoriesToList(htmlDocument);
            AddCategoriesToDb(categories);

            //SetAllProductsNotPublished();

            for (int i = 0; i < categories.Count; i++)
            {
                IList<ProductStantekViewModel> products = await GetProductsFromCategory(httpClient, rootUrl, categories[i].CategoryUrl, categories[i].Name);
                AddProductsToDb(products);
            }
            
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        private void SetAllProductsNotPublished()
        {
            var products = productsService.GetAll()
                                          .Where(x => x.IsPublished == true)
                                          .ToList();

            foreach (var product in products)
            {
                product.IsPublished = false;
                productsService.Update(product);
            }

        }

        private void AddProductsToDb(IList<ProductStantekViewModel> products)
        {             
            foreach (var product in products)
            {
                Product dbProduct = productsService.GetByName(product.Name).FirstOrDefault();
                if (dbProduct != null)
                {
                    dbProduct.Category = this.categorieService.GetByName(product.Category);
                    dbProduct.Discount = product.Discount;
                    dbProduct.FullDescription = product.FullDescription;
                    dbProduct.IsPublished = true;
                    dbProduct.OldPrice = product.OldPrice;
                    dbProduct.Pictures.Clear();
                    dbProduct.Pictures.Add(new Picture() { ImageUrl = product.PictureUrl });
                    dbProduct.Price = product.Price;

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

                    productsService.AddProduct(newProduct);
                }
            }
        }

        private async Task<IList<ProductStantekViewModel>> GetProductsFromCategory(HttpClient httpClient, string rootUrl, string categoryUrl, string categoryName)
        {
            var html = await httpClient.GetStringAsync(rootUrl + categoryUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var pagesDiv = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("pages"))
                .FirstOrDefault();

            IList<ProductStantekViewModel> productsFromPages = new List<ProductStantekViewModel>();
            if (pagesDiv != null)
            {
                IList<HtmlNode> pagesAnchor = pagesDiv.Descendants("a").ToList();
                var pageOneUrl = pagesAnchor[0].ChildAttributes("href").FirstOrDefault().Value;
                var lastPageUrl = pagesAnchor[pagesAnchor.Count - 2].ChildAttributes("href").FirstOrDefault().Value;
                var pagesUrl = pageOneUrl.Substring(0, pageOneUrl.Length - 1);
                var pagesNumber = int.Parse(lastPageUrl.Substring(pagesUrl.Length));

                //productFromPage = await GetProductsFromPage(httpClient, rootUrl, pagesUrl + "1", categoryName);
                for (int i = 1; i <= pagesNumber; i++)
                {
                    IList<ProductStantekViewModel> productFromOnePage = await GetProductsFromPage(httpClient, rootUrl, pagesUrl + i.ToString(), categoryName);
                    foreach (var product in productFromOnePage)
                    {
                        productsFromPages.Add(product);
                    }
                }
            }
            else
            {
                productsFromPages = await GetProductsFromPage(httpClient, rootUrl, categoryUrl, categoryName);
            }

            IList<ProductStantekViewModel> result = new List<ProductStantekViewModel>();
                
            return productsFromPages;
        }

        private async Task<IList<ProductStantekViewModel>> GetProductsFromPage(HttpClient httpClient, string rootUrl, string productPageUrl, string categoryName)
        {
            var html = await httpClient.GetStringAsync(rootUrl + productPageUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var productsDivs = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("item"))
                .ToList();

            IList<string> productsLinks = new List<string>();

            foreach (var div in productsDivs)
            {
                var link = div.Descendants("a").FirstOrDefault()
                                    .ChildAttributes("href").FirstOrDefault()
                                    .Value;

                productsLinks.Add(link);
            }

            IList<ProductStantekViewModel> products = new List<ProductStantekViewModel>();

            foreach (var link in productsLinks)
            {
                ProductStantekViewModel product = await GetProduct(httpClient, rootUrl, link);
                product.Category = categoryName;

                products.Add(product);
            }


            return products;
        }

        private async Task<ProductStantekViewModel> GetProduct(HttpClient httpClient, string rootUrl, string link)
        {
            var html = await httpClient.GetStringAsync(rootUrl + link);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var productDiv = htmlDocument.DocumentNode.Descendants("div")
                                           .Where(node => node.GetAttributeValue("class", "")
                                           .Equals("itemonly"))
                                           .FirstOrDefault();

            string name = productDiv.Descendants("div")
                                       .Where(node => node.GetAttributeValue("class", "")
                                       .Equals("title"))
                                       .FirstOrDefault()
                                       .InnerText
                                       .Trim();
            
            if (name.Length > ValidationConstants.StandartMaxLength)
            {
                name = name.Substring(0, ValidationConstants.StandartMaxLength);
            }

            var pictureUrl = productDiv.Descendants("img").FirstOrDefault()
                                    .ChildAttributes("src").FirstOrDefault()
                                    .Value;

            string price = productDiv.Descendants("div")
                                       .Where(node => node.GetAttributeValue("class", "")
                                       .Equals("price"))
                                       .FirstOrDefault()
                                       .InnerText
                                       .Trim();
            price = price.Substring(0, price.Length - 3);

            string fullDescription = productDiv.Descendants("p")
                                                .FirstOrDefault()
                                                .OuterHtml;

            ProductStantekViewModel productViewModel = new ProductStantekViewModel()
            {
                Name = name,
                PictureUrl = pictureUrl,
                Price = decimal.Parse(price) / 100,
                FullDescription = fullDescription
            };

            var oldPriceNode = productDiv.Descendants("div")
                              .Where(node => node.GetAttributeValue("class", "")
                              .Equals("price old"))
                              .FirstOrDefault();

            if (oldPriceNode != null)
            {
                string oldPrice = oldPriceNode
                                        .InnerText
                                        .Trim();

                oldPrice = oldPrice.Substring(0, oldPrice.Length - 3);

                string percent = productDiv.Descendants("div")
                                          .Where(node => node.GetAttributeValue("class", "")
                                          .Equals("percent"))
                                          .FirstOrDefault()
                                          .InnerText
                                          .Trim();

                percent = percent.Substring(0, percent.Length - 1);

                productViewModel.OldPrice = decimal.Parse(oldPrice) / 100;
                productViewModel.Discount = double.Parse(percent);
            }

            

            return productViewModel;
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
                    ShowOnHomePage = false
                };

                this.categorieService.AddCategory(categoryToAdd);
            }
        }

        private IList<CategoryStantekViewModel> GetCategoriesToList(HtmlDocument htmlDocument)
        {
            var categoriesDiv = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("categories"))
                .FirstOrDefault();

            IList<HtmlNode> categoriesLi = categoriesDiv.Descendants("li").ToList();

            IList<CategoryStantekViewModel> categories = new List<CategoryStantekViewModel>();

            foreach (var li in categoriesLi)
            {
                if (li.Descendants("a").FirstOrDefault() == null)
                {
                    continue;
                }

                var catModel = new CategoryStantekViewModel()
                {
                    Name = li.Descendants("a").FirstOrDefault().InnerText,
                    CategoryUrl = li.Descendants("a").FirstOrDefault()
                                    .ChildAttributes("href").FirstOrDefault()
                                    .Value


                };
                categories.Add(catModel);
            }

            return categories;
        }
    }
}
