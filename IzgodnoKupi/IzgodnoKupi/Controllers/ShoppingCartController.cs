using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IzgodnoKupi.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IProductsService productsService;

        public ShoppingCartController(IProductsService productsService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();

            this.productsService = productsService;
        }

        //public List<OrderDetailViewModel> CartItems { get; set; }

        [HttpGet]
        [Authorize]
        public ActionResult MyCart()
        {

            return View("MyCart"); //, this.CartItems);
        }

        [HttpPost]
        [Authorize]
        public IActionResult OrderNow(Guid id, int quantity)
        {


            return RedirectToAction("MyCart");
        }
    }
}
