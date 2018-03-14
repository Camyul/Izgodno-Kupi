using Bytes2you.Validation;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Models;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace IzgodnoKupi.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categiriesService;

        public HomeController(IProductsService productsService, ICategoriesService categiriesService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categiriesService, "categiriesService").IsNull().Throw();

            this.productsService = productsService;
            this.categiriesService = categiriesService;
        }

        public IActionResult Index()
        {
            List<CategoriesNavigationViewModel> viewCategoryPc = this.categiriesService
                                                                             .GetAllCategoriesSortedByName()
                                                                             .Where(x => x.CategoriesGroup == CategoriesGroup.Pc)
                                                                             .Select(c => new CategoriesNavigationViewModel(c))
                                                                             .ToList();

            List<CategoriesNavigationViewModel> categoriesSmartPhone = this.categiriesService
                                                                             .GetAllCategoriesSortedByName()
                                                                             .Where(x => x.CategoriesGroup == CategoriesGroup.SmartPhoneAndAccessoaries)
                                                                             .Select(c => new CategoriesNavigationViewModel(c))
                                                                             .ToList();

            List<CategoriesNavigationViewModel> categoriesSmallWhiteGoods = this.categiriesService
                                                                             .GetAllCategoriesSortedByName()
                                                                             .Where(x => x.CategoriesGroup == CategoriesGroup.SmallWhiteGoods)
                                                                             .Select(c => new CategoriesNavigationViewModel(c))
                                                                             .ToList();

            var products = this.productsService
                    .GetAll()
                    .Where(p => p.IsPublished == true)
                    .Take(Constants.CountOfPartOfProducts)
                    //.Take(Constants.CountOfProductsInHomePage)
                    .ToList();

            IList<PreviewProductViewModel> randomProducts = new List<PreviewProductViewModel>();

            if (products.Count >= Constants.CountOfProductsInHomePage)
            {
                for (int i = 0; i < Constants.CountOfProductsInHomePage; i++)
                {

                    Random rand = new Random();
                    var skip = rand.Next(0, products.Count - 1);
                    var randomProduct = products
                                        .Skip(skip)
                                        .Take(1)
                                        .Select(x => new PreviewProductViewModel(x))
                                        .First();

                    randomProducts.Add(randomProduct);

                }
                
                foreach (var product in randomProducts)
                {
                    if (product.Name.Length > Constants.ProductPreviewNameLength)
                    {
                        product.Name = product.Name.Substring(0, Constants.ProductPreviewNameLength) + "...";
                    }
                }
            }

            //IDictionary<CategoriesNavigationViewModel, int> numberOfProducts = new Dictionary<CategoriesNavigationViewModel, int>();

            //foreach (var cat in viewCategoryPc)
            //{
            //    numberOfProducts[cat] = this.productsService
            //                                .GetByCategory(cat.Id)
            //                                .Count();
            //}
            //foreach (var cat in categoriesSmartPhone)
            //{
            //    numberOfProducts[cat] = this.productsService
            //                                .GetByCategory(cat.Id)
            //                                .Count();
            //}
            //foreach (var cat in categoriesSmallWhiteGoods)
            //{
            //    numberOfProducts[cat] = this.productsService
            //                                .GetByCategory(cat.Id)
            //                                .Count();
            //}


            //ViewData["numberOfProducts"] = numberOfProducts;
            ViewData["categoriesPc"] = viewCategoryPc;
            ViewData["categoriesSmartPhone"] = categoriesSmartPhone;
            ViewData["categoriesSmallWhiteGoods"] = categoriesSmallWhiteGoods;
            ViewData["products"] = randomProducts;
            //ViewData["biggestDiscountProducts"] = biggestDiscountProducts;

            return View();
        }

        public IActionResult TermAndConditions()
        {
            return View();
        }

        public IActionResult Shipping()
        {
            return View();
        }

        public IActionResult Payment()
        {
            return View();
        }

        public IActionResult ForOur()
        {
            return View();
        }

        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            return View();
        }
    }
}
