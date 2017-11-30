using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using IzgodnoKupi.Web.Models.ProductViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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

        // GET: Products
        [HttpGet]
        public ActionResult Index()
        {
            // Without AutoMapper
            var products = this.productsService
                .GetAll()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new ProductViewModel(x))
                .ToList();

            var categories = this.categiriesService.GetAllCategoriesSortedByName()
             //.Select(x => new CategoriesNavigationViewModel(x))
            .ToList();
            var viewCategory = new List<CategoriesNavigationViewModel>();
            foreach (var cat in categories)
            {
                viewCategory.Add(new CategoriesNavigationViewModel(cat));
            }

            ViewData["categories"] = viewCategory;
            ViewData["products"] = products;

            return View();
        }

        public ActionResult Details(Guid? id)
        {
            Product product = this.productsService.GetById(id);

            ProductViewModel viewModel = new ProductViewModel(product);

            return View(viewModel);
        }

        //public ActionResult ProductsByCategory(Guid? id)
        //{
        //    Guard.WhenArgument(id, "Category Id").IsNull().Throw();

        //    var products = this.productsService
        //                    .GetByCategory(id)
        //                    .ToList();

        //    var categories = this.categiryService.GetAllCategoriesSortedByName()
        //        .ToList();

        //    var viewCategory = new List<CategoriesNavigationViewModel>();
        //    foreach (var cat in categories)
        //    {
        //        viewCategory.Add(new CategoriesNavigationViewModel(cat));
        //    }

        //    var viewProducts = new List<ProductViewModel>();
        //    foreach (var product in products)
        //    {
        //        viewProducts.Add(new ProductViewModel(product));
        //    }

        //    ViewData["categories"] = viewCategory;
        //    ViewData["products"] = viewProducts;

        //    return View("Index");
        //}

        [HttpGet]
        [Authorize]
        public ActionResult AddProduct()
        {
            var product = new ProductViewModel();

            var categories = this.categiriesService.GetAllCategoriesSortedByName()
               // .Select(x => new CategoriesNavigationViewModel(x))
               .ToList();

            var viewCategory = new List<CategoriesNavigationViewModel>();

            foreach (var cat in categories)
            {
                viewCategory.Add(new CategoriesNavigationViewModel(cat));
            }

            ViewData["categories"] = viewCategory;
            ViewData["product"] = product;

            return View(product);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddProduct(ProductViewModel productModel)
        {
            Product product = new Product()
            {
                Name = productModel.Name,
                ShortDescription = productModel.ShortDescription,
                FullDescription = productModel.FullDescription,
                CategoryId = productModel.CategoryId,
                Quantity = productModel.Quantity,
                PictureUrl = productModel.PictureUrl,
                Price = productModel.Price,
                IsPublished = productModel.IsPublished,
                ProductAvailability = productModel.ProductAvailability,
                IsFreeShipping = productModel.IsFreeShipping,
                Weight = productModel.Weight
            };

            
            this.productsService.AddProduct(product);

            return RedirectToAction("Index");
        }

        //[HttpPost]
        //[AjaxOnly]
        //public ActionResult FilteredProducts(string searchName)
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
