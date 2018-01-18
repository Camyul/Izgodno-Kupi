﻿using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IzgodnoKupi.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductsService productsService;
        private readonly ICategoriesService categiriesService;

        public ProductController(IProductsService productsService, ICategoriesService categiriesService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categiriesService, "categiriesService").IsNull().Throw();

            this.productsService = productsService;
            this.categiriesService = categiriesService;
        }

        public IActionResult Details(Guid? id)
        {
            Guard.WhenArgument(id, "Details Id").IsNull().Throw();

            Product product = this.productsService.GetById(id);

            ProductDetailsViewModel viewModel = new ProductDetailsViewModel(product);

            var products = this.productsService
               .GetAll()
               .OrderByDescending(x => x.CreatedOn)
               .Take(5)
               .Select(x => new ProductSimilarViewModel(x))
               .ToList();

            ViewData["products"] = products;

            return View(viewModel);
        }

        public IActionResult ByCategory(Guid? id)
        {
            Guard.WhenArgument(id, "Category Id").IsNull().Throw();

            var products = this.productsService
                            .GetByCategory(id)
                            .ToList();

            var categories = this.categiriesService.GetAllCategoriesSortedByName()
                .ToList();

            var viewCategory = new List<CategoriesNavigationViewModel>();

            foreach (var cat in categories)
            {
                viewCategory.Add(new CategoriesNavigationViewModel(cat));
            }

            var viewProducts = new List<ProductViewModel>();
            foreach (var product in products)
            {
                viewProducts.Add(new ProductViewModel(product));
            }

            ViewData["categories"] = viewCategory;
            ViewData["products"] = viewProducts;

            return View();
        }

        //[HttpPost]
        //[AjaxOnly]
        //public IActionResult FilteredProducts(string searchName)
        //{

        //    if (string.IsNullOrEmpty(searchName))
        //    {
        //        return this.Index();
        //    }
        //    else
        //    {
        //        var filteredProducts = this.productsService.GetByName(searchName).ToList();

        //        var viewProducts = new List<ProductViewModel>();
        //        foreach (var product in filteredProducts)
        //        {

        //            viewProducts.Add(new ProductViewModel(product));
        //        }

        //        return this.PartialView("_FilteredProductsPartial", viewProducts);
        //    }
        //}
    }
}
