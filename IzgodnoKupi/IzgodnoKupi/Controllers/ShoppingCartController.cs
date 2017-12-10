using Bytes2you.Validation;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Models;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.OrderViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IzgodnoKupi.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<User> userManager;
        private IProductsService productsService;

        public ShoppingCartController(IProductsService productsService, UserManager<User> userManager)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(userManager, "userManager").IsNull().Throw();

            this.productsService = productsService;
            this.userManager = userManager;

            this.CartItems = new List<OrderItemViewModel>();
        }

        public IList<OrderItemViewModel> CartItems { get; set; }

        [HttpGet]
        [Authorize]
        public IActionResult EmptiCart()
        {
            this.CartItems.Clear();
            HttpContext.Session.SetString(Constants.SessionKey, "");
            
            return RedirectToAction("MyCart");
        }

        public IActionResult DeleteOrderItem(Guid id)
        {
            var sessionData = HttpContext.Session.GetString(Constants.SessionKey);

            if (sessionData != null && sessionData != string.Empty)
            {
                var data = JsonConvert.DeserializeObject<IList<OrderItemViewModel>>(sessionData);
                this.CartItems = data;
                var searchedItem = this.CartItems
                                    .FirstOrDefault(i => i.Product.Id == id);

                this.CartItems.Remove(searchedItem);

                var serializedData = JsonConvert.SerializeObject(this.CartItems);
                HttpContext.Session.SetString(Constants.SessionKey, serializedData);
            }

            return RedirectToAction("MyCart");
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyCart()
        {
            MyCartViewModel myCartModel = new MyCartViewModel();

            var sessionData = HttpContext.Session.GetString(Constants.SessionKey);
            //var sessionData = null;
            if (sessionData != null && sessionData != string.Empty)
            {
                var data = JsonConvert.DeserializeObject<IList<OrderItemViewModel>>(sessionData);
                myCartModel.OrderItems = data;

            }
            else
            {
                myCartModel.OrderItems = new List<OrderItemViewModel>();
            }

            myCartModel.ShippingTax = Constants.ShippingTax;

            foreach (var item in myCartModel.OrderItems)
            {
                myCartModel.TotalAmountInclTax += item.TotalPrice;
            }

            myCartModel.TotalAmountExclTax = Math.Round(myCartModel.TotalAmountInclTax / Constants.TaxAmount, 2);
            myCartModel.TaxAmount = myCartModel.TotalAmountInclTax - myCartModel.TotalAmountExclTax;
            //myCartModel.TotalAmount = myCartModel.TotalAmountInclTax + myCartModel.ShippingTax;
            myCartModel.TotalAmount = myCartModel.TotalAmountInclTax;

            ViewBag.Count = myCartModel.OrderItems.Count;
            ViewBag.TotalAmount = myCartModel.TotalAmount;

            return View("MyCart", myCartModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult OrderNow(Guid id, int quantity)
        {
            var productToAdd = new ProductViewModel(this.productsService.GetById(id));
            var orderItemView = new OrderItemViewModel(productToAdd, quantity);

            orderItemView.TotalPrice = productToAdd.Price * quantity;

            var sessionData = HttpContext.Session.GetString(Constants.SessionKey);
            if (!string.IsNullOrEmpty(sessionData))
            {
                this.CartItems = JsonConvert.DeserializeObject<List<OrderItemViewModel>>(sessionData);
            }

            this.CartItems.Add(orderItemView);

            var serializedData = JsonConvert.SerializeObject(this.CartItems);
            HttpContext.Session.SetString(Constants.SessionKey, serializedData);

            //var currentUser = this.User;
            ////bool IsAdmin = currentUser.IsInRole("Admin");
            //var userId = this.userManager.GetUserId(User);

            return RedirectToAction("MyCart");
        }
    }
}
