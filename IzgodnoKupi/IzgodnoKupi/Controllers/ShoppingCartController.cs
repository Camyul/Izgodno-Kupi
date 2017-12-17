using Bytes2you.Validation;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Models;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Models.ContactInfoViewModels;
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
using System.Threading.Tasks;

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

            //this.CartItems = new List<OrderItemViewModel>();
        }

        //public IList<OrderItemViewModel> CartItems { get; set; }

        [HttpGet]
        [Authorize]
        public IActionResult EmptiCart()
        {
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));
            myOrder.OrderItems.Clear();

            ordersService.Update(myOrder);

            //this.CartItems.Clear();
            //HttpContext.Session.SetString(Constants.SessionKey, "");

            return RedirectToAction("MyCart");
        }

        public IActionResult DeleteOrderItem(Guid id)
        {
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            var searchedItem = myOrder.OrderItems.FirstOrDefault(i => i.ProductId == id);

            myOrder.OrderItems.Remove(searchedItem);

            ordersService.Update(myOrder);
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

            if (myOrder != null)
            {
                foreach (var item in myOrder.OrderItems)
                {
                    ProductViewModel productView = new ProductViewModel(item.Product);
                    OrderItemViewModel newItem = new OrderItemViewModel(productView, item.Quantity);
                    myCartModel.OrderItems.Add(newItem);
                    myCartModel.TotalAmountInclTax += newItem.TotalPrice;
                }
            }
            else
            {
                myCartModel.OrderItems = new List<OrderItemViewModel>();
            }
            //var sessionData = HttpContext.Session.GetString(Constants.SessionKey);
            ////var sessionData = null;
            //if (sessionData != null && sessionData != string.Empty)
            //{
            //    var data = JsonConvert.DeserializeObject<IList<OrderItemViewModel>>(sessionData);
            //    myCartModel.OrderItems = data;

            //}
            //else
            //{
            //    myCartModel.OrderItems = new List<OrderItemViewModel>();
            //}

            //foreach (var item in myCartModel.OrderItems)
            //{
            //    myCartModel.TotalAmountInclTax += item.TotalPrice;
            //}

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

            return View("MyCart", myCartModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public IActionResult OrderNow(Guid id, int quantity)
        {
            //MyCartViewModel myCartModel = new MyCartViewModel();
            //var productToAdd = new ProductViewModel(this.productsService.GetById(id));
            //var orderItemView = new OrderItemViewModel(productToAdd, quantity);

            //orderItemView.TotalPrice = productToAdd.Price * quantity;

            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));
            Product productToAdd = this.productsService.GetById(id);
            OrderItem item = new OrderItem();
            item.ProductId = productToAdd.Id;
            item.Quantity = quantity;

            myOrder.OrderItems.Add(item);

            ordersService.Update(myOrder);

            //if (myOrder != null)
            //{
            //        //ProductViewModel productView = new ProductViewModel(item.Product);
            //        //OrderItem newItem = new OrderItem(productView, item.Quantity);
            //        //myOrder.OrderItems.Add(newItem);
            //        //myCartModel.TotalAmountInclTax += newItem.TotalPrice;
            //}
            //else
            //{
            //    myCartModel.OrderItems = new List<OrderItemViewModel>();
            //}
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
        public IActionResult CheckOut(FullContactInfoViewModel fullContactInfo)
        {
            //Order newOrder = new Order();
            //var sessionData = HttpContext.Session.GetString(Constants.SessionKey);
            ////var sessionData = null;
            //if (sessionData != null && sessionData != string.Empty)
            //{
            //    var orderItemsViewModels = JsonConvert.DeserializeObject<IList<OrderItemViewModel>>(sessionData);
            //    var orderItems = new HashSet<OrderItem>();

            //    foreach (var orderItem in orderItemsViewModels)
            //    {
            //        OrderItem item = new OrderItem()
            //        {
            //            ProductId = orderItem.Product.Id.Value,
            //            Quantity = orderItem.Quantity,
            //            UnitPrice = orderItem.Product.Price,
            //            SubTotal = orderItem.TotalPrice,
            //            OrderedDate = DateTime.UtcNow,
            //            ItemWeight = orderItem.Quantity * orderItem.Product.Weight,
            //            ItemDiscount = ((double)orderItem.TotalPrice * orderItem.Product.Discount) / 100
            //        };
            //        orderItems.Add(item);
            //    }

            //    newOrder.OrderItems = orderItems;
            //}
            //else
            //{
            //    return View("EmptyCart");
            //}
            Order myOrder = ordersService.GetByUserAndNotCompleted(this.userManager.GetUserId(User));

            if (myOrder == null && myOrder.OrderItems.Count == 0)
            {
                return View("EmptyCart");
            }
            else
            {
                
            }

            myOrder.ShippingTax = Constants.ShippingTax;

            foreach (var item in myOrder.OrderItems)
            {
                myOrder.TotalAmountInclTax += item.SubTotal;
            }

            myOrder.UserId = this.userManager.GetUserId(User);

            myOrder.OrderDate = DateTime.UtcNow;
            myOrder.OrderStatus = OrderStatus.Confirmed;
            myOrder.PaymentMethod = PaymentMethod.PayToCourier;
            myOrder.ShippingMethod = ShippingMethod.ToAddress;

            myOrder.TotalAmountExclTax = Math.Round(myOrder.TotalAmountInclTax / Constants.TaxAmount, 2);
            myOrder.TaxAmount = myOrder.TotalAmountInclTax - myOrder.TotalAmountExclTax;
            //myOrder.TotalAmountInclTax += myOrder.ShippingTax;
            
            FullContactInfo info = new FullContactInfo()
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


            fullContactInfosService.Add(info);

            info.Orders.Add(myOrder);

            ordersService.Update(myOrder);

            return View("OrderCompleted");
        }
    }
}
