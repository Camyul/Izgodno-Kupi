using Bytes2you.Validation;
using HtmlAgilityPack;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.Category;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
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
        private readonly IDictionary<string, int> categoriesNames;

        public StantekCrowlerController(IProductsService productsService, ICategoriesService categorieService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categorieService, "categiriesService").IsNull().Throw();

            this.productsService = productsService;
            this.categorieService = categorieService;

            this.categoriesNames = GetCategoriesNames();
        }

        public IActionResult Index()
        {
            var categories = this.categorieService.GetAll()
                      .Select(c => new CategoryViewModel(c))
                      .OrderBy(x => x.Name)
                      .ToList();

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

            SetAllProductsNotPublished();

            for (int i = 0; i < categories.Count; i++)
            {
                IList<ProductStantekViewModel> products = await GetProductsFromCategory(httpClient, rootUrl, categories[i].CategoryUrl, categories[i].Name);
                AddProductsToDb(products);
            }
            
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        public async Task<IActionResult> UpdateCategories(Guid id)
        {
            var rootUrl = "http://stantek.com/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(rootUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            IList<CategoryStantekViewModel> categories = GetCategoriesToList(htmlDocument);
            AddCategoriesToDb(categories);

            return RedirectToAction("Index", "StantekCrowler", new { area = "Admin" });
        }

        public async Task<IActionResult> GetProductsFromCategory(Guid id)
        {
            Category category = this.categorieService.GetById(id);

            var rootUrl = "http://stantek.com/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(rootUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            IList<CategoryStantekViewModel> categories = GetCategoriesToList(htmlDocument);
            CategoryStantekViewModel categoryToSync = categories
                                                            .Where(c => c.Name == category.Name)
                                                            .FirstOrDefault();

            SetProductsFromCategoryNotPublished(id);

            IList<ProductStantekViewModel> products = await GetProductsFromCategory(httpClient, rootUrl, categoryToSync.CategoryUrl, categoryToSync.Name);
            AddProductsToDb(products);

            return RedirectToAction("Index", "StantekCrowler", new { area = "Admin" });
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
                Product dbProduct = productsService.GetByName(product.Name).FirstOrDefault();
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
                ProductStantekViewModel product = await GetProduct(httpClient, rootUrl, link, categoryName);
                product.Category = categoryName;

                products.Add(product);
            }


            return products;
        }

        private async Task<ProductStantekViewModel> GetProduct(HttpClient httpClient, string rootUrl, string link, string categoryName)
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


            int categoryNameLength = this.categoriesNames[categoryName];

            //In this category have 2 different subcategories
            if (categoryName == "Флашки и USB HDD" && name[0] == 'H')
            {
                categoryNameLength = categoryNameLength - 4;
            }

            if (name.Length > ValidationConstants.StandartMaxLength + categoryNameLength)
            {
                name = name.Substring(categoryNameLength, ValidationConstants.StandartMaxLength);
            }
            else
            {
                name = name.Substring(categoryNameLength);
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

        //Represent length of name of category in English 
        private IDictionary<string, int> GetCategoriesNames()
        {
            IDictionary<string, int> result = new Dictionary<string, int>
            {
                {"CD и DVD записвачки", 7 },
                {"I/O карти", 9 },
                {"LED осветление", 13 },
                {"MP3/MP4 плейъри и донгъли", 7 },
                {"SSD", 4 },
                {"TV, FM тунери и Video Capture", 14 },
                {"Аксесоари за лаптоп", 19 },
                {"Артикули втора у-ба", 12 },
                {"Батерии", 12 },
                {"Батерии за лаптоп", 21 },
                {"Видео камери", 13 },
                {"Видео карти", 11 },
                {"Външни кутии за хард дискове", 12 },
                {"Джойстици, кормила за игри", 15 },
                {"Дисплей/Tъч за смартфони", 12 },
                {"Дисплей/Tъч за таблети", 15 },
                {"Други...", 6 },
                {"Дънни платки", 3 },
                {"За видео камери", 23 },
                {"За принтери", 10 },
                {"За смартфони", 14 },
                {"За таблети", 17 },
                {"За телевизори", 13 },
                {"За фотоапарати", 16 },
                {"Зарядни", 14 },
                {"Захранване за компютри", 13 },
                {"Звукови карти", 11 },
                {"Кабели и преходници", 6 },
                {"Клавиатури", 4 },
                {"Кутии за Компютри", 5 },
                {"Лаптопи", 9 },
                {"Мишки", 6 },
                {"Монитори", 8 },
                {"Мрежа, LAN, Wi-Fi", 15 },
                {"Мрежови карти", 9 },
                {"Настолни Компютри", 9 },
                {"Оперативна памет RAM", 4 },
                {"Охладители", 7 },
                {"Памет за лаптоп", 17 },
                {"Подложки за мишки", 10 },
                {"Празни CD и DVD дискове", 6 },
                {"Принтери", 8 },
                {"Проектори", 10 },
                {"Процесори", 4 },
                {"Резервни части", 15 },
                {"Скенери", 8 },
                {"Слушалки и микрофони", 21 },
                {"Смартфони", 4 },
                {"Софтуер", 9 },
                {"Стабилизатори и UPS", 4 },
                {"Стъклени протектори", 10 },
                {"Таблети", 7 },
                {"Телевизори", 3 },
                {"Телефонни апарати", 6 },
                {"Тон колони", 9 },
                {"Уеб/Скайп камерки", 7 },
                {"Факс апарати", 11 },
                {"Флашки и USB HDD", 17 },
                {"Фотоапарати", 6 },
                {"Хард дискове HDD", 4 },
                {"Хард дискове за лаптоп", 17 },
                {"Хард дискове за сървър", 11 },
                {"Чанти, раници за лаптоп", 4 }
            };

            return result;
        }
    }
}
