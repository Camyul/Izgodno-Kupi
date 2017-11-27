using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IzgodnoKupi.Web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            Guard.WhenArgument(categoryService, "categoryService").IsNull().Throw();

            this.categoryService = categoryService;
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
        public IActionResult AddCategory(CategoryViewModel categoryModel)
        {
            Category category = new Category()
            {
                Name = categoryModel.Name,
                //Products = categoryModel.Products
            };

            this.categoryService.AddCategory(category);

            return RedirectToAction("Index", "Products");
        }
    }
}