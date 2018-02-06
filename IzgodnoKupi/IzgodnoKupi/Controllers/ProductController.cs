﻿using Bytes2you.Validation;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using IzgodnoKupi.Web.Models.IndexPageViewModel;
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

            if (!product.IsPublished)
            {
                return RedirectToAction("Error404", "Error");
            }

            ProductDetailsViewModel viewModel = new ProductDetailsViewModel(product);


            var products = this.productsService
               .GetAll()
               .Where(p => p.IsPublished == true)
               .Take(Constants.CountOfPartOfProducts)
               .ToList();

            IList<ProductSimilarViewModel> randomProducts = new List<ProductSimilarViewModel>();

            for (int i = 0; i < Constants.CountOfProductsInDetailsPage; i++)
            {

                Random rand = new Random();
                var skip = rand.Next(0, Constants.CountOfPartOfProducts);
                var randomProduct = products
                                    .Skip(skip)
                                    .Take(1)
                                    .Select(x => new ProductSimilarViewModel(x))
                                    .First();

                randomProducts.Add(randomProduct);

            }

            ViewData["products"] = randomProducts;

            return View(viewModel);
        }

        public IActionResult ByCategory(Guid? id, int? page)
        {
            Guard.WhenArgument(id, "Category Id").IsNull().Throw();

            var products = this.productsService
                            .GetByCategory(id)
                            .OrderBy(x => x.Price)
                            .ToList();

            var categories = this.categiriesService.GetAllCategoriesSortedByName()
                .ToList();

            var currentCategory = new CategoriesNavigationViewModel(categiriesService.GetById(id));
            var viewCategory = new List<CategoriesNavigationViewModel>();

            foreach (var cat in categories)
            {
                viewCategory.Add(new CategoriesNavigationViewModel(cat));
            }

            var viewProducts = new List<PreviewProductViewModel>();
            foreach (var product in products)
            {
                PreviewProductViewModel viewProduct = new PreviewProductViewModel(product);
                if (viewProduct.Name.Length > Constants.ProductPreviewNameLength)
                {
                    viewProduct.Name = viewProduct.Name.Substring(0, Constants.ProductPreviewNameLength);
                    viewProduct.Name = viewProduct.Name + "...";
                }
                viewProducts.Add(viewProduct);
            }

            Pager pager = new Pager(viewProducts.Count(), page);

            IndexPageViewModel viewPageIndexModel = new IndexPageViewModel
            {
                Items = viewProducts.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            ViewData["categories"] = viewCategory;
            ViewData["products"] = viewPageIndexModel;

            return View(currentCategory);
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
