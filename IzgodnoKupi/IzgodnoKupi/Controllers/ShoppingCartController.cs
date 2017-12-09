﻿using Bytes2you.Validation;
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
        public IActionResult MyCart()
        {
            MyCartViewModel myCartModel = new MyCartViewModel();

            var sessionData = HttpContext.Session.GetString(Constants.SessionKey);
            if (sessionData != null)
            {
                var data = JsonConvert.DeserializeObject<IList<OrderItemViewModel>>(sessionData);
                myCartModel.OrderItems = data;

            }

            myCartModel.ShippingTax = Constants.ShippingTax;

            foreach (var item in myCartModel.OrderItems)
            {
                myCartModel.TotalAmountInclTax += item.TotalPrice;
            }

            myCartModel.TotalAmountExclTax = myCartModel.TotalAmountInclTax / Constants.TaxAmount;
            myCartModel.TaxAmount = myCartModel.TotalAmountInclTax - myCartModel.TotalAmountExclTax;
            myCartModel.TotalAmount = myCartModel.TotalAmountInclTax + myCartModel.ShippingTax;

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
