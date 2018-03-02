using Bytes2you.Validation;
using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
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
        private readonly ICategoriesService categoriesService;
        private readonly IProductsService productsService;

        public SolytronXmlController(ICategoriesService categoriesService, IProductsService productsService)
        {
            Guard.WhenArgument(productsService, "productsService").IsNull().Throw();
            Guard.WhenArgument(categoriesService, "categiriesService").IsNull().Throw();

            this.categoriesService = categoriesService;
            this.productsService = productsService;
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GetProductsFromComponents()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Компоненти"); 

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Хард дискове HDD"));
            GetProductsFromSubCategory("Хард дискове HDD", subCategories[0]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("SSD"));
            GetProductsFromSubCategory("SSD", subCategories[2]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Видео карти"));
            GetProductsFromSubCategory("Видео карти", subCategories[3]);

            //Have USB Hubs in this category
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("CD и DVD записвачки"));
            //GetProductsFromSubCategory("CD и DVD записвачки", subCategories[4]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Флашки и USB HDD"));
            GetProductsFromSubCategory("Флашки и USB HDD", subCategories[1]);
            GetProductsFromSubCategory("Флашки и USB HDD", subCategories[5]);
            GetProductsFromSubCategory("Флашки и USB HDD", subCategories[10]);

            //Mix for Desctop and Laptop
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Оперативна памет RAM"));
            //GetProductsFromSubCategory("Оперативна памет RAM", subCategories[6]);

            //Only 3, and lot of lincks
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Дънни платки"));
            //GetProductsFromSubCategory("Дънни платки", subCategories[7]);

            //Mix from adapters for laptops, gsm, and PC, only 3-4 pieces
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Захранване за компютри"));
            //GetProductsFromSubCategory("Захранване за компютри", subCategories[8]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Охладители"));
            GetProductsFromSubCategory("Охладители", subCategories[11]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromStorages()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Storage");

            string categoryName = "NAS";
            bool isCategoryExist = this.categoriesService.GetByName(categoryName) == null;

            if (isCategoryExist)
            {
                Category categoryToAdd = new Category()
                {
                    Name = categoryName,
                    ShowOnHomePage = false,
                    CategoriesGroup = CategoriesGroup.Pc
                };

                this.categoriesService.AddCategory(categoryToAdd);
            }


            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName(categoryName));
            GetProductsFromSubCategory(categoryName, subCategories[1]);

            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Смартфони"));
            //GetProductsFromSubCategory("Смартфони", subCategories[1]);

            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За смартфони"));
            //GetProductsFromSubCategory("За смартфони", subCategories[2]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromTabletsAndSmartphones()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Таблети и Смартфони");

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Таблети"));
            GetProductsFromSubCategory("Таблети", subCategories[0]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Смартфони"));
            GetProductsFromSubCategory("Смартфони", subCategories[1]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За смартфони"));
            GetProductsFromSubCategory("За смартфони", subCategories[2]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromServers()
        {
            //IList<CategorySolytronViewModel> subCategories = GetSubCategories("Сървъри");

            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Test"));
            //GetProductsFromSubCategory("Test", subCategories[0]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromComputers()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Компютри");

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Лаптопи"));
            GetProductsFromSubCategory("Лаптопи", subCategories[0]);

            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Настолни Компютри"));
            GetProductsFromSubCategory("Настолни Компютри", subCategories[1]);
            GetProductsFromSubCategory("Настолни Компютри", subCategories[2]);
            GetProductsFromSubCategory("Настолни Компютри", subCategories[3]);

            //subCategories[4] - Гаранции
            //subCategories[5] - Опции

            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Test"));
            //GetProductsFromSubCategory("Настолни Компютри", subCategories[5]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromSoftware()
        {
            //Category category = this.categoriesService.GetByName("Софтуер");

            ////Make Solytron's products in category - Not published
            //SetProductsFromCategoryNotPublished(category);

            //IList<CategorySolytronViewModel> subCategories = GetSubCategories("Софтуер");

            //foreach (var subCategory in subCategories)
            //{
            //    ICollection<ProductSolytronViewModel> products = GetProductsFromSubCategories(subCategory, category);

            //    //Add products to DB    
            //    AddProductsToDb(products);
            //}

            return RedirectToAction("Index");
        }

        private void AddProductsToDb(ICollection<ProductSolytronViewModel> products)
        {
            foreach (var product in products)
            {
                Product dbProduct = productsService.GetByName(product.Name).FirstOrDefault();
                if (dbProduct != null)
                {
                    dbProduct.Category = product.Category;
                    dbProduct.Name = product.Name;
                    dbProduct.FullDescription = product.FullDescription;
                    dbProduct.IsPublished = true;
                    dbProduct.Pictures.Clear();
                    dbProduct.Pictures = product.Pictures;
                    dbProduct.Price = product.Price;
                    dbProduct.Supplier = Supplier.Solytron;

                    productsService.Update(dbProduct);
                }
                else
                {
                    Product newProduct = new Product();
                    newProduct.Name = product.Name;
                    newProduct.Category = product.Category;
                    newProduct.FullDescription = product.FullDescription;
                    newProduct.IsPublished = true;
                    newProduct.Pictures = product.Pictures;
                    newProduct.Price = product.Price;
                    newProduct.Supplier = Supplier.Solytron;

                    productsService.AddProduct(newProduct);
                }
            }
        }

        private void SetProductsFromCategoryNotPublished(Category category)
        {
            IList<Product> products = this.productsService
                                          .GetByCategory(category.Id)
                                          .Where(x => x.IsPublished == true && x.Supplier == Supplier.Solytron)
                                          .ToList();

            foreach (var product in products)
            {
                product.IsPublished = false;
                this.productsService.Update(product);
            }

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

                //Check for End User Price
                if (!item.Descendants("priceEndUser").Any())
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
                FullDescription = description.ToString(),
                Pictures = images,
                //Supplier = Supplier.Solytron
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

        private void GetProductsFromSubCategory(string stantekCategory, CategorySolytronViewModel subCategory)
        {
            Category category = this.categoriesService.GetByName(stantekCategory);

            ICollection<ProductSolytronViewModel> products = GetProductsFromSubCategories(subCategory, category);

            //Add products to DB   
            AddProductsToDb(products);
        }

    }
}
