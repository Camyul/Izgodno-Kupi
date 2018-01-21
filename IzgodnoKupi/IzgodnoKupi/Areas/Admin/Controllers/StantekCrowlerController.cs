using Bytes2you.Validation;
using HtmlAgilityPack;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.Category;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class StantekCrowlerController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categiriesService;

        public StantekCrowlerController(IProductsService productsService, ICategoriesService categiriesService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categiriesService, "categiriesService").IsNull().Throw();

            this.productsService = productsService;
            this.categiriesService = categiriesService;
        }

        public async Task<IActionResult> GetProducts()
        {
            var rootUrl = "http://stantek.com/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(rootUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            IList<CategoryStantekViewModel> categories = GetCategoriesToList(htmlDocument);
            //AddCategoriesToDb(categories);

            //foreach (var cat in categories)
            //{

            //}

            
            IList <ProductStantekViewModel> products = await GetProductsFromCategory(httpClient, rootUrl, categories[0].CategoryUrl);
            //AddProductsToDb(products);

            
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        private void AddProductsToDb(IList<ProductStantekViewModel> products)
        {
            throw new NotImplementedException();
        }

        private async Task<IList<ProductStantekViewModel>> GetProductsFromCategory(HttpClient httpClient, string rootUrl, string categoryUrl)
        {
            var html = await httpClient.GetStringAsync(rootUrl + categoryUrl);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            var pagesDiv = htmlDocument.DocumentNode.Descendants("div")
                .Where(node => node.GetAttributeValue("class", "")
                .Equals("pages"))
                .FirstOrDefault();

            IList<ProductStantekViewModel> productFromPage = new List<ProductStantekViewModel>();
            if (pagesDiv != null)
            {
                IList<HtmlNode> pagesAnchor = pagesDiv.Descendants("a").ToList();
                var pageOneUrl = pagesAnchor[0].ChildAttributes("href").FirstOrDefault().Value;
                var lastPageUrl = pagesAnchor[pagesAnchor.Count - 2].ChildAttributes("href").FirstOrDefault().Value;
                var pagesUrl = pageOneUrl.Substring(0, pageOneUrl.Length - 1);
                var pagesNumber = int.Parse(lastPageUrl.Substring(pagesUrl.Length));

                productFromPage = await GetProductsFromPage(httpClient, rootUrl, pagesUrl + "1");
                //for (int i = 1; i <= pagesNumber; i++)
                //{
                //      var productFromPage = GetProductsFromPage(pagesUrl + i.ToString());
                //}
            }
            else
            {
                productFromPage = await GetProductsFromPage(httpClient, rootUrl, categoryUrl);
            }

            IList<ProductStantekViewModel> result = new List<ProductStantekViewModel>();
                
            return productFromPage;
        }

        private async Task<IList<ProductStantekViewModel>> GetProductsFromPage(HttpClient httpClient, string rootUrl, string productPageUrl)
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

            if (name.Length > 199)
            {
                name = name.Substring(0, 199);
            }

            ProductStantekViewModel product = new ProductStantekViewModel();
            product.ProductToAdd.Name = name;

            return null;
        }

        private void AddCategoriesToDb(IList<CategoryStantekViewModel> categories)
        {
            foreach (var cat in categories)
            {
                bool isCategoryExist = categiriesService.GetByName(cat.Name) != null;

                if (isCategoryExist)
                {
                    continue;
                }

                Category categoryToAdd = new Category()
                {
                    Name = cat.Name,
                    ShowOnHomePage = false
                };

                categiriesService.AddCategory(categoryToAdd);
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
