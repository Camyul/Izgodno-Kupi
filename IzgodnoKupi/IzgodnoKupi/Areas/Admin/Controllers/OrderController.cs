using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.OrderItemViewModel;
using IzgodnoKupi.Web.Areas.Admin.Models.OrderViewModel;
using IzgodnoKupi.Web.Models.ContactInfoViewModels;
using IzgodnoKupi.Web.Models.OrderItemViewModels;
using IzgodnoKupi.Web.Models.ProductViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class OrderController : Controller
    {
        private readonly IOrdersService ordersService;
        private readonly IFullContactInfosService fullContactInfosService;
        private readonly IProductsService productService;

        public OrderController(IOrdersService ordersService, IFullContactInfosService fullContactInfosService, IProductsService productService)
        {
            Guard.WhenArgument(ordersService, "ordersService").IsNull().Throw();
            Guard.WhenArgument(fullContactInfosService, "fullContactInfoService").IsNull().Throw();
            Guard.WhenArgument(productService, "productService").IsNull().Throw();

            this.ordersService = ordersService;
            this.fullContactInfosService = fullContactInfosService;
            this.productService = productService;
        }

        public IActionResult Index()
        {
            IList<OrderListViewModel> modelListView = ordersService
                .GetAll()
                .Where(r => r.OrderStatus != OrderStatus.NotCompleted)
                .Select(x => new OrderListViewModel(x))
                .ToList();

            return View(modelListView);
        }

        [HttpGet]
        public IActionResult Edit(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = ordersService
                .GetAll()
                .Where(x => x.Id == id)
                .FirstOrDefault();
                //.GetById(id);

            if (order == null)
            {
                return NotFound();
            }

            OrderViewModel viewModelOrder = new OrderViewModel(order);
            foreach (var item in order.OrderItems)
            {
                OrderItemAdminViewModel newItem = new OrderItemAdminViewModel(item);
                newItem.Product = new ProductViewModel(this.productService.GetById(item.ProductId));
                viewModelOrder.OrderItems.Add(newItem);
            }

            return View(viewModelOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Guid id, OrderViewModel orderViewModel)
        {
            if (id != orderViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    FullContactInfo contactInfo = this.fullContactInfosService.GetDefaultByUser(orderViewModel.UserId);

                    contactInfo.FirstName = orderViewModel.FullContactInfo.FirstName;
                    contactInfo.LastName = orderViewModel.FullContactInfo.LastName;
                    contactInfo.PhoneNumber = orderViewModel.FullContactInfo.PhoneNumber;
                    contactInfo.Address = orderViewModel.FullContactInfo.Address;
                    contactInfo.City = orderViewModel.FullContactInfo.City;
                    contactInfo.Area = orderViewModel.FullContactInfo.Area;
                    contactInfo.PostCode = orderViewModel.FullContactInfo.PostCode;
                    contactInfo.CompanyName = orderViewModel.FullContactInfo.CompanyName;
                    contactInfo.EIK = orderViewModel.FullContactInfo.EIK;
                    contactInfo.BGEIK = orderViewModel.FullContactInfo.BGEIK;
                    contactInfo.CompanyCity = orderViewModel.FullContactInfo.CompanyCity;
                    contactInfo.CompanyAddress = orderViewModel.FullContactInfo.CompanyAddress;
                    contactInfo.MOL = orderViewModel.FullContactInfo.MOL;
                    contactInfo.Note = orderViewModel.FullContactInfo.Note;

                    this.fullContactInfosService.Update(contactInfo);

                    Order order = ordersService.GetById(id);

                    //order.OrderItems = orderViewModel.OrderItems;
                    order.OrderStatus = orderViewModel.OrderStatus;
                    //order.PaymentMethod = orderViewModel.PaymentMethod;
                    //order.ShippingMethod = orderViewModel.ShippingMethod;
                    order.ShippingTax = orderViewModel.ShippingTax;
                    order.TaxAmount = orderViewModel.TaxAmount;
                    order.TotalAmountExclTax = orderViewModel.TotalAmountExclTax;
                    order.TotalAmountInclTax = orderViewModel.TotalAmountInclTax;
                    order.TotalDiscount = orderViewModel.TotalDiscount;


                    ordersService.Update(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(orderViewModel.Id))
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
            return View(orderViewModel);
        }

        private bool OrderExists(Guid id)
        {
            return ordersService.GetById(id) == null ? false : true;
        }
    }
}
