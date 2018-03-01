using Bytes2you.Validation;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Models;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.ContactInfoViewModels;
using IzgodnoKupi.Web.Models.OrderItemViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IzgodnoKupi.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly UserManager<User> userManager;
        private IProductsService productsService;
        private IOrdersService ordersService;
        private IFullContactInfosService fullContactInfosService;

        public ShoppingCartController
            (
                IProductsService productsService, 
                UserManager<User> userManager, 
                IOrdersService ordersService,
                IFullContactInfosService fullContactInfosService
            )
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(userManager, "userManager").IsNull().Throw();
            Guard.WhenArgument(ordersService, "ordersService").IsNull().Throw();
            Guard.WhenArgument(fullContactInfosService, "fullContactInfosService").IsNull().Throw();

            this.productsService = productsService;
            this.userManager = userManager;
            this.ordersService = ordersService;
            this.fullContactInfosService = fullContactInfosService;
        }

        [HttpGet]
        [Authorize]
        public IActionResult EmptiCart()
        {
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            myOrder.OrderItems.Clear();
            myOrder.TotalAmountInclTax = 0m;

            ordersService.Update(myOrder);

            //HttpContext.Session.SetString(Constants.SessionKey, "");

            return RedirectToAction("MyCart");
        }

        public IActionResult DeleteOrderItem(Guid id)
        {
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            var searchedItem = myOrder.OrderItems.FirstOrDefault(i => i.ProductId == id);

            if (searchedItem != null)
            {
                myOrder.OrderItems.Remove(searchedItem);

                myOrder.TotalAmountInclTax = myOrder.TotalAmountInclTax - searchedItem.SubTotal;

                ordersService.Update(myOrder);
            }

            //var sessionData = HttpContext.Session.GetString(Constants.SessionKey);

            //if (sessionData != null && sessionData != string.Empty)
            //{
            //    var data = JsonConvert.DeserializeObject<IList<OrderItemViewModel>>(sessionData);
            //    this.CartItems = data;
            //    var searchedItem = this.CartItems
            //                        .FirstOrDefault(i => i.Product.Id == id);

            //    this.CartItems.Remove(searchedItem);

            //    var serializedData = JsonConvert.SerializeObject(this.CartItems);
            //    HttpContext.Session.SetString(Constants.SessionKey, serializedData);
            //}

            return RedirectToAction("MyCart");
        }

        [HttpGet]
        [Authorize]
        public IActionResult MyCart()
        {
            MyCartViewModel myCartModel = new MyCartViewModel();
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            bool isItemsAvailible = true;

            if (myOrder != null)
            {
                foreach (var item in myOrder.OrderItems)
                {
                    Product product = this.productsService.GetById(item.ProductId);
                    ProductViewModel productView = new ProductViewModel(product);
                    OrderItemViewModel newItem = new OrderItemViewModel(productView, item.Quantity);
                    newItem.TotalPrice = productView.Price * item.Quantity;
                    myCartModel.OrderItems.Add(newItem);
                    myCartModel.TotalAmountInclTax += newItem.TotalPrice;

                    if (!product.IsPublished)
                    {
                        isItemsAvailible = false;
                    }
                }
            }
            else
            {
                myCartModel.OrderItems = new List<OrderItemViewModel>();
                myOrder = new Order();
            }

            var userId = this.userManager.GetUserId(this.User);
            var contactInfo = this.fullContactInfosService.GetDefaultByUser(userId);

            if (contactInfo != null)
            {
                myCartModel.FullContactInfo = new FullContactInfoViewModel(contactInfo);
            }
            else
            {
                myCartModel.FullContactInfo = new FullContactInfoViewModel();
            }

            myCartModel.TotalAmountExclTax = Math.Round(myCartModel.TotalAmountInclTax / Constants.TaxAmount, 2);
            myCartModel.TaxAmount = myCartModel.TotalAmountInclTax - myCartModel.TotalAmountExclTax;

            if (myCartModel.TotalAmountInclTax < Constants.MinPriceFreeShipping && myCartModel.OrderItems.Count > 0)
            {
                myCartModel.ShippingTax = Constants.ShippingTax;
            }

            myCartModel.TotalAmount = myCartModel.TotalAmountInclTax + myCartModel.ShippingTax;
            //myCartModel.TotalAmount = myCartModel.TotalAmountInclTax;

            ViewBag.Count = myCartModel.OrderItems.Count;
            ViewBag.TotalAmount = myCartModel.TotalAmount;
            ViewBag.IsItemsAvailible = isItemsAvailible;

            return View("MyCart", myCartModel);
        }

        [HttpGet]
        [Authorize]
        public IActionResult OrderNow(Guid id)
        {
            return RedirectToAction("Details", "Product", new { id = id });
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult OrderNow(Guid id, int quantity)
        {
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            if (myOrder == null)
            {
                myOrder = new Order();
                myOrder.UserId = this.userManager.GetUserId(this.User);
                this.ordersService.AddOrder(myOrder);
            }

            Product productToAdd = this.productsService.GetById(id);
            OrderItem item = new OrderItem();
            item.ProductId = productToAdd.Id;
            item.Quantity = quantity;
            item.UnitPrice = productToAdd.Price;
            item.SubTotal = productToAdd.Price * quantity;

            myOrder.OrderItems.Add(item);

            decimal sum = 0m;
            foreach (var orderItem in myOrder.OrderItems)
            {
                sum += orderItem.SubTotal;
            }
            myOrder.TotalAmountInclTax = sum;

            ordersService.Update(myOrder);

            //var sessionData = HttpContext.Session.GetString(Constants.SessionKey);
            //if (!string.IsNullOrEmpty(sessionData))
            //{
            //    this.CartItems = JsonConvert.DeserializeObject<List<OrderItemViewModel>>(sessionData);
            //}

            //this.CartItems.Add(orderItemView);

            //var serializedData = JsonConvert.SerializeObject(this.CartItems);
            //HttpContext.Session.SetString(Constants.SessionKey, serializedData);

            //var currentUser = this.User;
            //bool IsAdmin = currentUser.IsInRole("Admin");
            //var userId = this.userManager.GetUserId(User);


            return RedirectToAction("MyCart");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult CheckOut(FullContactInfoViewModel fullContactInfo)
        {
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            if (myOrder == null || myOrder.OrderItems.Count == 0)
            {
                return View("EmptyCart");
            }

            //Check OrderItems is availible to Order
            foreach (var item in myOrder.OrderItems)
            {
                bool isAvailible = this.productsService.GetById(item.ProductId).IsPublished;

                if (!isAvailible)
                {
                    return RedirectToAction("MyCart");
                }
            }

            if (myOrder.TotalAmountInclTax < Constants.MinPriceFreeShipping && myOrder.OrderItems.Count > 0)
            {
                myOrder.ShippingTax = Constants.ShippingTax;
            }
            else
            {
                myOrder.ShippingTax = 0m;
            }

            var userId = this.userManager.GetUserId(User);
            myOrder.UserId = userId;

            myOrder.OrderDate = DateTime.UtcNow;
            myOrder.OrderStatus = OrderStatus.Confirmed;
            myOrder.PaymentMethod = PaymentMethod.PayToCourier;
            myOrder.ShippingMethod = ShippingMethod.ToAddress;

            myOrder.TotalAmountExclTax = Math.Round(myOrder.TotalAmountInclTax / Constants.TaxAmount, 2);
            myOrder.TaxAmount = myOrder.TotalAmountInclTax - myOrder.TotalAmountExclTax;
            //myOrder.TotalAmountInclTax += myOrder.ShippingTax;

            var contactInfo = this.fullContactInfosService.GetDefaultByUser(userId);
            if (contactInfo == null)
            {
                FullContactInfo newInfo = new FullContactInfo()
                {
                    UserID = this.userManager.GetUserId(User),
                    FirstName = fullContactInfo.FirstName,
                    LastName = fullContactInfo.LastName,
                    PhoneNumber = fullContactInfo.PhoneNumber,
                    Address = fullContactInfo.Address,
                    City = fullContactInfo.City,
                    Area = fullContactInfo.Area,
                    PostCode = fullContactInfo.PostCode,
                    CompanyName = fullContactInfo.CompanyName,
                    EIK = fullContactInfo.EIK,
                    BGEIK = fullContactInfo.BGEIK,
                    CompanyCity = fullContactInfo.CompanyCity,
                    CompanyAddress = fullContactInfo.CompanyAddress,
                    MOL = fullContactInfo.MOL,
                    Note = fullContactInfo.Note

                };
                newInfo.Orders.Add(myOrder);
                fullContactInfosService.Add(newInfo);
            }
            else
            {
                contactInfo.FirstName = fullContactInfo.FirstName;
                contactInfo.LastName = fullContactInfo.LastName;
                contactInfo.PhoneNumber = fullContactInfo.PhoneNumber;
                contactInfo.Address = fullContactInfo.Address;
                contactInfo.City = fullContactInfo.City;
                contactInfo.Area = fullContactInfo.Area;
                contactInfo.PostCode = fullContactInfo.PostCode;
                contactInfo.CompanyName = fullContactInfo.CompanyName;
                contactInfo.EIK = fullContactInfo.EIK;
                contactInfo.BGEIK = fullContactInfo.BGEIK;
                contactInfo.CompanyCity = fullContactInfo.CompanyCity;
                contactInfo.CompanyAddress = fullContactInfo.CompanyAddress;
                contactInfo.MOL = fullContactInfo.MOL;
                contactInfo.Note = fullContactInfo.Note;
                
                contactInfo.Orders.Add(myOrder);
                fullContactInfosService.Update(contactInfo);
            }

            ordersService.Update(myOrder);

            MyCartViewModel myCartModel = new MyCartViewModel();

            return View("OrderCompleted", myCartModel);
        }
    }
}
