using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using IzgodnoKupi.Web.Extensions;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
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

        [HttpGet]
        public IActionResult Index(int? page)
        {
            var products = this.productsService
                .GetAll()
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new ProductViewModel(x))
                .ToList();

            Pager pager = new Pager(products.Count(), page);

            IndexAdminPageViewModel viewPageIndexModel = new IndexAdminPageViewModel
            {
                Items = products.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            return View(viewPageIndexModel);
        }

        [HttpGet]
        public IActionResult SearchProduct(string searchTerm, int? page)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return RedirectToAction("Index");
            }

            var products = this.productsService
                .GetAll()
                .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()))
                .OrderByDescending(x => x.CreatedOn)
                .Select(x => new ProductViewModel(x))
                .ToList();

            Pager pager = new Pager(products.Count(), page);

            IndexAdminPageViewModel viewPageIndexModel = new IndexAdminPageViewModel
            {
                Items = products.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            ViewData["searchTerm"] = searchTerm;

            return View(viewPageIndexModel);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = productsService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel viewModelProduct = new ProductViewModel(product);

            var categories = this.categiriesService.GetAllCategoriesSortedByName()
               // .Select(x => new CategoriesNavigationViewModel(x))
               .ToList();

            var viewCategory = new List<CategoriesNavigationViewModel>();

            foreach (var cat in categories)
            {
                viewCategory.Add(new CategoriesNavigationViewModel(cat));
            }

            ViewData["categories"] = viewCategory;
            ViewData["product"] = viewModelProduct;

            return View(viewModelProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    productsService.Update(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }


        [HttpGet]
        public IActionResult AddProduct()
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
        [ValidateAntiForgeryToken]
        public IActionResult AddProduct(ProductViewModel productModel)
        {
            Product product = new Product()
            {
                Name = productModel.Name,
                ShortDescription = productModel.ShortDescription,
                FullDescription = productModel.FullDescription,
                CategoryId = productModel.CategoryId,
                Quantity = productModel.Quantity,
                Price = productModel.Price,
                Discount = productModel.Discount,
                IsPublished = productModel.IsPublished,
                ProductAvailability = productModel.ProductAvailability,
                IsFreeShipping = productModel.IsFreeShipping,
                Weight = productModel.Weight
            };
            product.Pictures.Add(productModel.Picture);

            this.productsService.AddProduct(product);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = productsService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            ProductViewModel viewModelProduct = new ProductViewModel(product);

            return View(viewModelProduct);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = productsService.GetById(id);

            if (product == null)
            {
                return NotFound();
            }

            product.DeletedOn = DateTime.Now;

            productsService.Delete(product);

            return RedirectToAction("Index");
        }

        private bool ProductExists(Guid id)
        {
            return productsService.GetById(id) == null ? false : true;
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
