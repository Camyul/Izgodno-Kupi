using Bytes2you.Validation;
using HtmlAgilityPack;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using IzgodnoKupi.Web.Areas.Admin.Models.Category;

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
            var url = "http://stantek.com/";
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync(url);

            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(html);

            IList<CategoryStantekViewModel> categories = GetCategoriesToList(htmlDocument);
            AddCategoriesToDb(categories);
            

            return RedirectToAction("Index", "Home", new { area = "Admin" });
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
                    ShowOnHomePage = true
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
