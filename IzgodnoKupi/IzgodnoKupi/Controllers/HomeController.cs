using Bytes2you.Validation;
using IzgodnoKupi.Models;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Mvc;
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
               .Select(x => new CategoriesNavigationViewModel(x))
               .ToList();

         
                var products = this.productsService
               .GetAll()
               .OrderByDescending(x => x.CreatedOn)
               .Take(8)
               .Select(x => new PreviewProductViewModel(x))
               .ToList();

            //var viewCategory = new List<CategoriesNavigationViewModel>();

            //foreach (var cat in categories)
            //{
            //    viewCategory.Add(new CategoriesNavigationViewModel(cat));
            //}

            ViewData["categories"] = categories;
            ViewData["products"] = products;

            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            //ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
