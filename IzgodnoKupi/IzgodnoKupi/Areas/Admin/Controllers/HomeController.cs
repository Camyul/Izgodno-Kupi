using Bytes2you.Validation;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.OrderViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class HomeController : Controller
    {
        private readonly IOrdersService ordersService;

        public HomeController(IOrdersService ordersService)
        {
            Guard.WhenArgument(ordersService, "ordersService").IsNull().Throw();

            this.ordersService = ordersService;
        }

        public IActionResult Index()
        {
            // Here display 1 or 2 charts
            
            IList<OrderListViewModel> modelListView = ordersService
                .GetAll()
                .Where(r => r.OrderStatus != OrderStatus.NotCompleted)
                .OrderByDescending(o => o.OrderDate)
                .Take(5)
                .Select(x => new OrderListViewModel(x))
                .ToList();

            return View(modelListView);
        }
    }
}
