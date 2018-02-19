using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class SolytronXmlController : Controller
    {



        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProducts()
        {
            XDocument doc = XDocument.Load("https://solytron.bg/products/xml/catalog-category.xml?j_u=cavescomputers&j_p=Magurata2000");

            IEnumerable<XElement> elements = doc.Elements();

            ICollection<string> categoriesNames = new List<string>();
            ICollection<string> categoriesLinks = new List<string>();

            foreach (var groups in elements)
            {
                var categoriesGroup = groups.Elements("productCategory").ToList();

                foreach (var categoryGroup in categoriesGroup)
                {
                    var group = categoryGroup.Elements("productGroup").ToList();

                    foreach (var cat in group)
                    {
                        var name = cat.Attribute("name").Value;
                        var link = (string)cat.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");
                        categoriesNames.Add(name);
                        categoriesLinks.Add(link);
                    }
                }
            }

            //var categores = doc.Root
            //                .Elements("productCatalog")
            //                .Select(node =>
            //                {
            //                    var name = node.Descendants("productGroup").Elements("propertyGroupId").ToList();

            //                    return new CategoriesNavigationViewModel
            //                    {
            //                        Name = name[0].Value
            //                    };
            //                })
            //                .ToList();
            //.ForEach(x => categores.Add(x));


            return RedirectToAction("Index");
        }
    }
}
