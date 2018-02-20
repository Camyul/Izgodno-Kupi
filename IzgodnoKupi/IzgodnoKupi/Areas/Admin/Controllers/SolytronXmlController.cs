using IzgodnoKupi.Web.Areas.Admin.Models.Category;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using IzgodnoKupi.Web.Models.CategoryViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System;

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

        public IActionResult GetMainCategories()
        {
            XDocument doc = XDocument.Load("https://solytron.bg/products/xml/catalog-category.xml?j_u=cavescomputers&j_p=Magurata2000");

            IEnumerable<XElement> elements = doc.Elements();

            ICollection<CategoriesNavigationViewModel> mainCategories = new List<CategoriesNavigationViewModel>();
            

            foreach (var groups in elements)
            {
                var categoriesGroup = groups.Elements("productCategory").ToList();

                foreach (var categoryGroup in categoriesGroup)
                {
                    var group = categoryGroup.Elements("productGroup").ToList();

                    var name = categoryGroup.Attribute("name").Value;
                    CategoriesNavigationViewModel mainCategory = new CategoriesNavigationViewModel
                    {
                        Name = name
                    };
                    mainCategories.Add(mainCategory);

                   // var link = (string)categoryGroup.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");
                    //categoriesLinks.Add(link);

                    //foreach (var cat in group)
                    //{
                    //    var name = cat.Attribute("name").Value;
                    //    var link = (string)cat.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");
                    //    categoriesNames.Add(name);
                    //    categoriesLinks.Add(link);
                    //}
                }
            }

            return View(mainCategories);
        }

        public IActionResult GetProductsFromMainCategory(string name)
        {
            XDocument doc = XDocument.Load("https://solytron.bg/products/xml/catalog-category.xml?j_u=cavescomputers&j_p=Magurata2000");

            IEnumerable<XElement> categories = doc.Descendants("productCategory")
                                                .Where(x => (string)x.Attribute("name") == name)
                                                .Elements("productGroup");
            
            ICollection<CategorySolytronViewModel> subCategories = new List<CategorySolytronViewModel>();

            foreach (var cat in categories)
            {
                var categoryName = cat.Attribute("name").Value;
                var link = (string)cat.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");

                CategorySolytronViewModel newCategory = new CategorySolytronViewModel
                {
                    Name = categoryName,
                    CategoryLink = link
                };
            }

            ICollection<ProductSolytronViewModel> products = GetProductsFromSubCategories(subCategories);

            //return View();
            return RedirectToAction("Index");
        }

        private ICollection<ProductSolytronViewModel> GetProductsFromSubCategories(ICollection<CategorySolytronViewModel> subCategories)
        {
            throw new NotImplementedException();
        }
    }
}
