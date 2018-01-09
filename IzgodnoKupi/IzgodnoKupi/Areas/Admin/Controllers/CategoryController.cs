using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
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

        [HttpGet]
        public IActionResult AddCategory()
        {
            var category = new CategoryViewModel();

            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(CategoryViewModel categoryModel)
        {
            Category category = new Category()
            {
                Name = categoryModel.Name,
                ShowOnHomePage = categoryModel.ShowOnHomePage
                //Products = categoryModel.Products
            };

            this.categoryService.AddCategory(category);

            //return RedirectToAction("Index", "Products");
            return RedirectToAction("Index");
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
