using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IzgodnoKupi.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoriesService categoryService;

        public CategoryController(ICategoriesService categoryService)
        {
            Guard.WhenArgument(categoryService, "categoryService").IsNull().Throw();

            this.categoryService = categoryService;
        }

        public IActionResult Index()
        {
            var categories = this.categoryService.GetAll()
                       .Select(c => new CategoryViewModel(c)).ToList();

            return View(categories);
        }

        [HttpGet]
        [Authorize]
        public IActionResult AddCategory()
        {
            var category = new CategoryViewModel();

            return View(category);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(CategoryViewModel categoryModel)
        {
            Category category = new Category()
            {
                Name = categoryModel.Name,
                //Products = categoryModel.Products
            };

            this.categoryService.AddCategory(category);

            //return RedirectToAction("Index", "Products");
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel viewModelCategory = new CategoryViewModel(category);

            return View(viewModelCategory);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    categoryService.Update(category);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        public IActionResult Details(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel viewModelCategory = new CategoryViewModel(category);

            return View(viewModelCategory);
        }

        [HttpGet]
        [Authorize]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            CategoryViewModel viewModelCategory = new CategoryViewModel(category);

            return View(viewModelCategory);
        }

        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category category = categoryService.GetById(id);

            if (category == null)
            {
                return NotFound();
            }

            category.DeletedOn = DateTime.Now;

            categoryService.Delete(category);

            return RedirectToAction("Index");
        }

        private bool CategoryExists(Guid id)
        {
            return categoryService.GetById(id) == null ? false : true;
        }
    }
}