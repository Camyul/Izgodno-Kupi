﻿using Bytes2you.Validation;
using IzgodnoKupi.Common;
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
            var categories = this.categiriesService.GetAllCategoriesSortedByName()
                .ToList();

            var viewCategory = new List<CategoriesNavigationViewModel>();

            foreach (var cat in categories)
            {
                viewCategory.Add(new CategoriesNavigationViewModel(cat));
            }

            var products = this.productsService
                    .GetAll()
                    .Where(x => x.OldPrice != 0)
                    .Take(Constants.CountOfPartOfProducts)
                    //.Take(Constants.CountOfProductsInHomePage)
                    .ToList();

            IList<PreviewProductViewModel> randomProducts = new List<PreviewProductViewModel>();

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

            //var biggestDiscountProducts = new List<PreviewProductViewModel>();

            //for (int i = 0; i < 2; i++)
            //{
            //    double maxDiscount = double.MaxValue;
            //    int numInList = int.MinValue;

            //    for (int j = 0; j < randomProducts.Count; j++)
            //    {
            //        if (maxDiscount > randomProducts[j].Discount)
            //        {
            //            maxDiscount = randomProducts[j].Discount;
            //            numInList = j;
            //        }

            //    }

            //    biggestDiscountProducts.Add(randomProducts[numInList]);
            //    randomProducts.RemoveAt(numInList);
            //}

            ViewData["categories"] = viewCategory;
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
