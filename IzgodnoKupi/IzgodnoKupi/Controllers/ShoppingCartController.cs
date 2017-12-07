using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.OrderViewModels;
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

            this.CartItems = new List<OrderItemViewModel>();
        }

        public IList<OrderItemViewModel> CartItems { get; set; }

        [HttpGet]
        [Authorize]
        public IActionResult MyCart()
        {
            ViewBag.Count = this.CartItems.Count;

            return View("MyCart", this.CartItems);
        }

        [HttpPost]
        [Authorize]
        public IActionResult OrderNow(Guid id, int quantity)
        {
            var productToAdd = new ProductViewModel(this.productsService.GetById(id));

            var orderItem = new OrderItemViewModel(productToAdd, quantity);

            this.CartItems.Add(orderItem);

            return RedirectToAction("MyCart");
        }
    }
}
