using Bytes2you.Validation;
using IzgodnoKupi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class ExportXmlController : Controller
    {
        private readonly IProductsService productsService;

        public ExportXmlController(IProductsService productsService)
        {
            Guard.WhenArgument(productsService, "productsService");

            this.productsService = productsService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ExportToXml()
        {
            var url = "./wwwroot/xml/export.xml";

            var doc = new XDocument();

            var products = this.productsService
                               .GetAll()
                               .Where(x => x.Category.Name == "SSD")
                               .ToList();

            doc.Add(new XElement("product",
                                    new XElement("name", products[0].Name)
                         )
                );

            doc.Save(url);

            return RedirectToAction("Index");
        }
    }
}
