using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using IzgodnoKupi.Web.Areas.Admin.Models.Category;
using IzgodnoKupi.Web.Areas.Admin.Models.Product;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace IzgodnoKupi.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Administrator")]
    public class SolytronXmlController : Controller
    {
        private readonly ICategoriesService categiriesService;

        public SolytronXmlController(ICategoriesService categiriesService)
        {
            this.categiriesService = categiriesService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProductsFromSoftware()
        {
            Category category = this.categiriesService.GetByName("Софтуер");

            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Софтуер");

            ICollection<ProductSolytronViewModel> products = GetProductsFromSubCategories(subCategories[0], category);

            //Add products to DB    

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromComputers()
        {
            Category category = this.categiriesService.GetByName("Настолни Компютри");

            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Компютри");

            ICollection<ProductSolytronViewModel> products = GetProductsFromSubCategories(subCategories[1], category);

            //Add products to DB    

            return RedirectToAction("Index");
        }

        private ICollection<ProductSolytronViewModel> GetProductsFromSubCategories(CategorySolytronViewModel subCategory, Category category)
        {
            XDocument doc = XDocument.Load(subCategory.CategoryLink);

            ICollection<ProductSolytronViewModel> products = new List<ProductSolytronViewModel>();

            IEnumerable<XElement> productElement = doc.Descendants("productSet")
                                                      .Elements("product");

            foreach (var item in productElement)
            {
                //Check if product is availible
                string availability = string.Empty;
                if (item.Descendants("stockInfoValue").Any())
                {
                    availability = item.Element("stockInfoValue").Value;
                }
                if (availability == "OnOrder" || availability == string.Empty)
                {
                    continue;
                }

                string codeId = item.Attribute("codeId").Value;
                string groupId = item.Attribute("groupId").Value;

                string fullInfoUrl = "https://solytron.bg/products/xml/product.xml?codeId=" +
                                      codeId +
                                      "&groupId=" +
                                      groupId +
                                      "&j_u=cavescomputers&j_p=Magurata2000";

                ProductSolytronViewModel newProduct = GetFullInfo(fullInfoUrl);

                //Get Name
                string name = item.Element("name").Value;
                string vendor = item.Element("vendor").Value;
                string nameAndVendor = vendor + " " + name;

                if (nameAndVendor.Length > ValidationConstants.StandartMaxLength)
                {
                    nameAndVendor = nameAndVendor.Substring(0, ValidationConstants.StandartMaxLength);
                }

                newProduct.Name = nameAndVendor;

                //Get Price
                string currency = item.Element("priceEndUser").Attribute("currency").Value;
                decimal priceValue = decimal.Parse(item.Element("priceEndUser").Value);
                if (currency == "BGN")
                {
                    newProduct.Price = priceValue;
                }
                else if (currency == "EUR")
                {
                    decimal price = priceValue * Constants.EURValue;
                    newProduct.Price = Math.Round(price, 2);
                }
                else
                {
                    decimal price = priceValue * Constants.USDValue;
                    newProduct.Price = Math.Round(price, 2);
                }

                //Get warranty
                //if (item.Descendants("warrantyUnit").Any())
                //{
                //    string warrantyType = item.Element("warrantyUnit").Value;
                //    string warrantyValue = item.Element("warrantyQty").Value;
                //    if (warrantyType == "2")
                //    {
                //        string warranty = "<p><b>Гаранция: </b>" + warrantyValue + " месеца</p>";
                //        newProduct.FullDescription = newProduct.FullDescription + warranty;
                //    }
                //}

                newProduct.Category = category;

                //Add Id???

                products.Add(newProduct);
            }

            return products;
        }

        private ProductSolytronViewModel GetFullInfo(string fullInfoUrl)
        {
            XDocument doc = XDocument.Load(fullInfoUrl);

            var elementRoot = doc.Root;


            Guid id = Guid.Parse(elementRoot.Attribute("productId").Value);
            //var name = elementRoot.Element("name").Value;

            var descriptionProperties = elementRoot.Elements("propertyGroup")
                                         .LastOrDefault()
                                         .Elements("property");

            StringBuilder description = new StringBuilder();
            foreach (var property in descriptionProperties)
            {
                if (property.Attribute("propertyId").Value != "55" 
                    && property.Attribute("propertyId").Value != "56" 
                    && property.Attribute("propertyId").Value != "57"
                   )
                {
                    string propertyName = property.Attribute("name").Value;
                    string propertyValue = property.Element("value").Value;

                    description.Append("<p><b>" + propertyName + ": </b>" + propertyValue + "</p>");
                }
            }

            var imagesElements = elementRoot.Elements("image");
            ICollection<Picture> images = new List<Picture>();

            foreach (var img in imagesElements)
            {
                Picture pic = new Picture
                {
                    ImageUrl = img.Value
                };

                images.Add(pic);
            }

            ProductSolytronViewModel product = new ProductSolytronViewModel
            {
                //Name = name,
                FullDescription = description.ToString(),
                Pictures = images
            };

            return product;
        }

        private IList<CategorySolytronViewModel> GetSubCategories(string name)
        {
            XDocument doc = XDocument.Load("https://solytron.bg/products/xml/catalog-category.xml?j_u=cavescomputers&j_p=Magurata2000");

            IEnumerable<XElement> categories = doc.Descendants("productCategory")
                                                .Where(x => (string)x.Attribute("name") == name)
                                                .Elements("productGroup");

            IList<CategorySolytronViewModel> subCategories = new List<CategorySolytronViewModel>();

            foreach (var cat in categories)
            {
                var categoryName = cat.Attribute("name").Value;
                var link = (string)cat.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");

                CategorySolytronViewModel newCategory = new CategorySolytronViewModel
                {
                    Name = categoryName,
                    CategoryLink = link
                };

                subCategories.Add(newCategory);
            }

            return subCategories;
        }

        //public IActionResult GetMainCategories()
        //{
        //    XDocument doc = XDocument.Load("https://solytron.bg/products/xml/catalog-category.xml?j_u=cavescomputers&j_p=Magurata2000");

        //    IEnumerable<XElement> elements = doc.Elements();

        //    ICollection<CategoriesNavigationViewModel> mainCategories = new List<CategoriesNavigationViewModel>();


        //    foreach (var groups in elements)
        //    {
        //        var categoriesGroup = groups.Elements("productCategory").ToList();

        //        foreach (var categoryGroup in categoriesGroup)
        //        {
        //            var group = categoryGroup.Elements("productGroup").ToList();

        //            var name = categoryGroup.Attribute("name").Value;
        //            CategoriesNavigationViewModel mainCategory = new CategoriesNavigationViewModel
        //            {
        //                Name = name
        //            };
        //            mainCategories.Add(mainCategory);

        //           // var link = (string)categoryGroup.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");
        //            //categoriesLinks.Add(link);

        //            //foreach (var cat in group)
        //            //{
        //            //    var name = cat.Attribute("name").Value;
        //            //    var link = (string)cat.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");
        //            //    categoriesNames.Add(name);
        //            //    categoriesLinks.Add(link);
        //            //}
        //        }
        //    }

        //    return View(mainCategories);
        //}

        //public IActionResult GetProductsFromMainCategory(string name)
        //{
        //    XDocument doc = XDocument.Load("https://solytron.bg/products/xml/catalog-category.xml?j_u=cavescomputers&j_p=Magurata2000");

        //    IEnumerable<XElement> categories = doc.Descendants("productCategory")
        //                                        .Where(x => (string)x.Attribute("name") == name)
        //                                        .Elements("productGroup");

        //    ICollection<CategorySolytronViewModel> subCategories = new List<CategorySolytronViewModel>();

        //    foreach (var cat in categories)
        //    {
        //        var categoryName = cat.Attribute("name").Value;
        //        var link = (string)cat.Element(XName.Get("link", "https://www.w3.org/2005/Atom")).Attribute("href");

        //        CategorySolytronViewModel newCategory = new CategorySolytronViewModel
        //        {
        //            Name = categoryName,
        //            CategoryLink = link
        //        };
        //    }

        //    ICollection<ProductSolytronViewModel> products = GetProductsFromSubCategories(subCategories);

        //    //return View();
        //    return RedirectToAction("Index");
        //}
    }
}
