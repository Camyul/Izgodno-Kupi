using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.OrderItemViewModel;
using IzgodnoKupi.Web.Areas.Admin.Models.OrderViewModel;
using IzgodnoKupi.Web.Extensions;
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

        public IActionResult Index(int? page)
        {
            IList<OrderListViewModel> modelOrderListView = ordersService
                                                                .GetAll()
                                                                .Where(r => r.OrderStatus != OrderStatus.NotCompleted)
                                                                .OrderByDescending(o => o.OrderDate)
                                                                .Select(x => new OrderListViewModel(x))
                                                                .ToList();

            Pager pager = new Pager(modelOrderListView.Count(), page);

            IndexOrderViewModel viewPageIndexModel = new IndexOrderViewModel
            {
                Items = modelOrderListView.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };


            return View(viewPageIndexModel);
        }

        [HttpGet]
        public IActionResult SearchOrder(DateTime? startDate, DateTime? endDate, OrderStatus orderStatus, int? page)
        {
            IList<OrderListViewModel> modelOrderListView = ordersService
                                                                .GetAll()
                                                                .Where(r => r.OrderStatus == orderStatus && 
                                                                       r.OrderDate >= startDate && 
                                                                       r.OrderDate <= endDate)
                                                                .OrderByDescending(o => o.OrderDate)
                                                                .Select(x => new OrderListViewModel(x))
                                                                .ToList();

            Pager pager = new Pager(modelOrderListView.Count(), page);

            IndexOrderViewModel viewPageIndexModel = new IndexOrderViewModel
            {
                Items = modelOrderListView.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize).ToList(),
                Pager = pager
            };

            ViewData["startDate"] = startDate;
            ViewData["endDate"] = endDate;
            ViewData["orderStatus"] = orderStatus;

            return View(viewPageIndexModel);
        }

        [HttpGet]
        public IActionResult Details(Guid id)
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

            ViewBag.TotalSum = viewModelOrder.TotalAmountInclTax + viewModelOrder.ShippingTax;

            return View(viewModelOrder);
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

            ViewBag.TotalSum = viewModelOrder.TotalAmountInclTax + viewModelOrder.ShippingTax;

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
            
            try
            {
                FullContactInfo contactInfo = new FullContactInfo();

                if (orderViewModel.UserId != null)
                {
                     contactInfo = this.fullContactInfosService.GetDefaultByUser(orderViewModel.UserId);
                }

                Order order = ordersService.GetById(id);

                if (contactInfo.UserID != null)
                {
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
                }
                else
                {
                    if (order.FullContactInfoId != null)
                    {
                        order.FullContactInfo.FirstName = orderViewModel.FullContactInfo.FirstName;
                        order.FullContactInfo.LastName = orderViewModel.FullContactInfo.LastName;
                        order.FullContactInfo.PhoneNumber = orderViewModel.FullContactInfo.PhoneNumber;
                        order.FullContactInfo.Address = orderViewModel.FullContactInfo.Address;
                        order.FullContactInfo.City = orderViewModel.FullContactInfo.City;
                        order.FullContactInfo.Area = orderViewModel.FullContactInfo.Area;
                        order.FullContactInfo.PostCode = orderViewModel.FullContactInfo.PostCode;
                        order.FullContactInfo.CompanyName = orderViewModel.FullContactInfo.CompanyName;
                        order.FullContactInfo.EIK = orderViewModel.FullContactInfo.EIK;
                        order.FullContactInfo.BGEIK = orderViewModel.FullContactInfo.BGEIK;
                        order.FullContactInfo.CompanyCity = orderViewModel.FullContactInfo.CompanyCity;
                        order.FullContactInfo.CompanyAddress = orderViewModel.FullContactInfo.CompanyAddress;
                        order.FullContactInfo.MOL = orderViewModel.FullContactInfo.MOL;
                        order.FullContactInfo.Note = orderViewModel.FullContactInfo.Note;
                    }
                    //Check all fields for values not only Address
                    else if (!string.IsNullOrEmpty(orderViewModel.FullContactInfo.Address))
                    {
                        FullContactInfo newContactInfo = new FullContactInfo()
                        {
                            FirstName = orderViewModel.ShortContactInfo.FirstName,
                            LastName = orderViewModel.ShortContactInfo.LastName,
                            PhoneNumber = orderViewModel.ShortContactInfo.PhoneNumber,
                            Address = orderViewModel.FullContactInfo.Address,
                            City = orderViewModel.FullContactInfo.City,
                            Area = orderViewModel.FullContactInfo.Area,
                            PostCode = orderViewModel.FullContactInfo.PostCode,
                            CompanyName = orderViewModel.FullContactInfo.CompanyName,
                            EIK = orderViewModel.FullContactInfo.EIK,
                            BGEIK = orderViewModel.FullContactInfo.BGEIK,
                            CompanyCity = orderViewModel.FullContactInfo.CompanyCity,
                            CompanyAddress = orderViewModel.FullContactInfo.CompanyAddress,
                            MOL = orderViewModel.FullContactInfo.MOL,
                            Note = orderViewModel.FullContactInfo.Note
                        };

                        newContactInfo.Orders.Add(order);
                        fullContactInfosService.Add(newContactInfo);
                    }
                    else
                    {
                        order.ShortContactInfo.FirstName = orderViewModel.ShortContactInfo.FirstName;
                        order.ShortContactInfo.LastName = orderViewModel.ShortContactInfo.LastName;
                        order.ShortContactInfo.PhoneNumber = orderViewModel.ShortContactInfo.PhoneNumber;
                    }
                }


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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = ordersService
                                .GetAll()
                                .Where(x => x.Id == id)
                                .FirstOrDefault();

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

            ViewBag.TotalSum = viewModelOrder.TotalAmountInclTax + viewModelOrder.ShippingTax;

            return View(viewModelOrder);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(Guid id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Order order = ordersService.GetById(id);

            if (order == null)
            {
                return NotFound();
            }

            order.DeletedOn = DateTime.Now;

            ordersService.Delete(order);

            return RedirectToAction("Index");
        }

        private bool OrderExists(Guid id)
        {
            return ordersService.GetById(id) == null ? false : true;
        }
    }
}
