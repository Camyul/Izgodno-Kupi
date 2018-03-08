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
using System.Net;
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

        public IActionResult GetProductsFromVideoCams()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Видеонаблюдение");

            //CheckCategoryExist("Видео камери");
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Видео камери"));
            //GetProductsFromSubCategory("Видео камери", subCategories[0]);

            CheckCategoryExist("Test");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Test"));
            GetProductsFromSubCategory("Test", subCategories[0]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromCommunication()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Комуникации");

            CheckCategoryExist("Мрежа, LAN, Wi-Fi");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Мрежа, LAN, Wi-Fi"));
            GetProductsFromSubCategory("Мрежа, LAN, Wi-Fi", subCategories[0]);
            GetProductsFromSubCategory("Мрежа, LAN, Wi-Fi", subCategories[1]);
            GetProductsFromSubCategory("Мрежа, LAN, Wi-Fi", subCategories[2]);
            GetProductsFromSubCategory("Мрежа, LAN, Wi-Fi", subCategories[4]);

            CheckCategoryExist("Шкафове");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Шкафове"));
            GetProductsFromSubCategory("Шкафове", subCategories[15]);//Rack
            GetProductsFromSubCategory("Шкафове", subCategories[16]);//Parts for Rack

            CheckCategoryExist("Телефонни апарати");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Телефонни апарати"));
            GetProductsFromSubCategory("Телефонни апарати", subCategories[7]);//IP Phones
            GetProductsFromSubCategory("Телефонни апарати", subCategories[8]);//Digital Phones
            GetProductsFromSubCategory("Телефонни апарати", subCategories[9]);//Dect Phones

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromAudioVideo()
        {

            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Аудио и Видео");

            CheckCategoryExist("Аудио и Видео");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Аудио и Видео"));
            GetProductsFromSubCategory("Аудио и Видео", subCategories[1]);
            GetProductsFromSubCategory("Аудио и Видео", subCategories[2]);
            GetProductsFromSubCategory("Аудио и Видео", subCategories[4]);
            GetProductsFromSubCategory("Аудио и Видео", subCategories[6]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromAccessoary()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Аксесоари");


            CheckCategoryExist("Мишки");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Мишки"));
            GetProductsFromSubCategory("Мишки", subCategories[1]);

            CheckCategoryExist("Клавиатури");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Клавиатури"));
            GetProductsFromSubCategory("Клавиатури", subCategories[2]);

            CheckCategoryExist("Чанти, раници за лаптоп");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Чанти, раници за лаптоп"));
            GetProductsFromSubCategory("Чанти, раници за лаптоп", subCategories[3]);

            CheckCategoryExist("Други...");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Други..."));
            GetProductsFromSubCategory("Други...", subCategories[4]);

            CheckCategoryExist("Кабели и преходници");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Кабели и преходници"));
            GetProductsFromSubCategory("Кабели и преходници", subCategories[5]);

            IList<CategorySolytronViewModel> secondSubCategories = GetSubCategories("Комуникации");
            GetProductsFromSubCategory("Кабели и преходници", secondSubCategories[17]);//Cable chinch

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromMultimedia()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Мултимедия");

            CheckCategoryExist("Слушалки и микрофони");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Слушалки и микрофони"));
            GetProductsFromSubCategory("Слушалки и микрофони", subCategories[2]);
            GetProductsFromSubCategory("Слушалки и микрофони", subCategories[3]);

            CheckCategoryExist("Тон колони");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Тон колони"));
            GetProductsFromSubCategory("Тон колони", subCategories[8]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromCartridges()
        {
            //To Long operation
            //IList<CategorySolytronViewModel> subCategories = GetSubCategories("Консумативи");

            //CheckCategoryExist("За принтери");
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За принтери"));
            //GetProductsFromSubCategory("За принтери", subCategories[0]);
            //GetProductsFromSubCategory("За принтери", subCategories[1]);
            //GetProductsFromSubCategory("За принтери", subCategories[2]);
            //GetProductsFromSubCategory("За принтери", subCategories[3]);
            //GetProductsFromSubCategory("За принтери", subCategories[4]);

            //IList<CategorySolytronViewModel> secondSubCategories = GetSubCategories("Принтери");

            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За принтери"));
            //GetProductsFromSubCategory("За принтери", secondSubCategories[12]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromScanners()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Скенери");

            CheckCategoryExist("Скенери");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Скенери"));
            GetProductsFromSubCategory("Скенери", subCategories[0]);
            GetProductsFromSubCategory("Скенери", subCategories[1]);
            GetProductsFromSubCategory("Скенери", subCategories[2]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromPrinters()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Принтери");

            CheckCategoryExist("Принтери");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Принтери"));
            GetProductsFromSubCategory("Принтери", subCategories[0]);
            GetProductsFromSubCategory("Принтери", subCategories[1]);

            IList<CategorySolytronViewModel> secondSubCategories = GetSubCategories("Мултифункционални");

            GetProductsFromSubCategory("Принтери", secondSubCategories[0]);
            GetProductsFromSubCategory("Принтери", secondSubCategories[1]);
            GetProductsFromSubCategory("Принтери", secondSubCategories[3]);
            GetProductsFromSubCategory("Принтери", secondSubCategories[6]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromTV()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Телевизори");

            CheckCategoryExist("Телевизори");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Телевизори"));
            GetProductsFromSubCategory("Телевизори", subCategories[0]);

            CheckCategoryExist("За телевизори");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За телевизори"));
            GetProductsFromSubCategory("За телевизори", subCategories[1]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromProjectors()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Видеопрожектори");

            CheckCategoryExist("Проектори");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Проектори"));
            GetProductsFromSubCategory("Проектори", subCategories[0]);

            CheckCategoryExist("За Проектори");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За Проектори"));
            GetProductsFromSubCategory("За Проектори", subCategories[1]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromDisplays()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Дисплеи");

            CheckCategoryExist("Монитори");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Монитори"));
            GetProductsFromSubCategory("Монитори", subCategories[0]);
            GetProductsFromSubCategory("Монитори", subCategories[1]);
            //Only one monitor stand
            GetProductsFromSubCategory("Монитори", subCategories[2]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromComponents()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Компоненти");

            CheckCategoryExist("Хард дискове HDD");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Хард дискове HDD"));
            GetProductsFromSubCategory("Хард дискове HDD", subCategories[0]);

            CheckCategoryExist("SSD");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("SSD"));
            GetProductsFromSubCategory("SSD", subCategories[2]);

            CheckCategoryExist("Видео карти");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Видео карти"));
            GetProductsFromSubCategory("Видео карти", subCategories[3]);

            //Have USB Hubs in this category
            CheckCategoryExist("CD и DVD записвачки");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("CD и DVD записвачки"));
            GetProductsFromSubCategory("CD и DVD записвачки", subCategories[4]);

            CheckCategoryExist("Флашки и USB HDD");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Флашки и USB HDD"));
            GetProductsFromSubCategory("Флашки и USB HDD", subCategories[1]);
            GetProductsFromSubCategory("Флашки и USB HDD", subCategories[5]);
            GetProductsFromSubCategory("Флашки и USB HDD", subCategories[10]);

            //Mix for Desktop and Laptop
            CheckCategoryExist("Оперативна памет RAM");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Оперативна памет RAM"));
            GetProductsFromSubCategory("Оперативна памет RAM", subCategories[6]);

            //Only 3, and lot of lincks
            //CheckCategoryExist("Дънни платки");
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Дънни платки"));
            //GetProductsFromSubCategory("Дънни платки", subCategories[7]);
            

            //Mix from adapters for laptops, gsm, and PC, only 8 pieces
            CheckCategoryExist("Захранване за компютри");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Захранване за компютри"));
            GetProductsFromSubCategory("Захранване за компютри", subCategories[8]);

            CheckCategoryExist("Охладители");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Охладители"));
            GetProductsFromSubCategory("Охладители", subCategories[11]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromStorages()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Storage");

            string categoryName = "NAS";

            CheckCategoryExist(categoryName);
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName(categoryName));
            GetProductsFromSubCategory(categoryName, subCategories[1]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromTabletsAndSmartphones()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Таблети и Смартфони");

            //Have in Stantek
            CheckCategoryExist("Таблети");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Таблети"));
            GetProductsFromSubCategory("Таблети", subCategories[0]);

            CheckCategoryExist("Смартфони");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Смартфони"));
            GetProductsFromSubCategory("Смартфони", subCategories[1]);

            CheckCategoryExist("За смартфони");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("За смартфони"));
            GetProductsFromSubCategory("За смартфони", subCategories[2]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromServers()
        {
            //Only 4 servers
            //IList<CategorySolytronViewModel> subCategories = GetSubCategories("Сървъри");

            //CheckCategoryExist("Test");
            //SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Test"));
            //GetProductsFromSubCategory("Test", subCategories[0]);

            return RedirectToAction("Index");
        }

        public IActionResult GetProductsFromComputers()
        {
            IList<CategorySolytronViewModel> subCategories = GetSubCategories("Компютри");

            CheckCategoryExist("Лаптопи");
            SetProductsFromCategoryNotPublished(this.categoriesService.GetByName("Лаптопи"));
            GetProductsFromSubCategory("Лаптопи", subCategories[0]);

            CheckCategoryExist("Настолни Компютри");
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
            ////Only six product
            //CheckCategoryExist("Софтуер");
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
                Product dbProduct = productsService.GetByName(product.Name)
                                                   .Where(x => x.Supplier == Supplier.Solytron)
                                                   .FirstOrDefault();
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

                //Get Name
                string name = item.Element("name").Value;
                //var isExist = this.productsService.GetByName(name.Substring(20))
                //                                  .Where(x => x.Supplier == Supplier.Stantek && x.IsPublished == true)
                //                                  .FirstOrDefault();
                //if (isExist != null)
                //{
                //    continue;
                //}

                string codeId = item.Attribute("codeId").Value;
                string groupId = item.Attribute("groupId").Value;

                string fullInfoUrl = "https://solytron.bg/products/xml/product.xml?codeId=" +
                                      codeId +
                                      "&groupId=" +
                                      groupId +
                                      "&j_u=cavescomputers&j_p=Magurata2000";

                ProductSolytronViewModel newProduct = GetFullInfo(fullInfoUrl);

                string vendor = item.Element("vendor").Value;
                string nameAndVendor = string.Empty;
                //Check name whether start with vendor
                if (vendor.Length > name.Length || vendor.ToLower() != name.Substring(0, vendor.Length).ToLower())
                {
                    nameAndVendor = vendor + " " + name;
                }
                else
                {
                    nameAndVendor = name;
                }

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
                //string fileSaveAddress = string.Empty;
                //using (WebClient client = new WebClient())
                //{
                //    //client.DownloadFile(new Uri(img.Value), @"c:\temp\image35.png");

                //    //OR 
                //    string url = img.Value;
                //    int start = url.IndexOf("resources/") + 10;
                //    int end = url.LastIndexOf("?");
                //    fileSaveAddress = "./wwwroot/productImages/" + url.Substring(start, end - start);

                //    client.DownloadFileAsync(new Uri(img.Value), fileSaveAddress);
                //}

                //Picture pic = new Picture
                //{
                //    ImageUrl = fileSaveAddress.Substring(9)
                //};

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

        private void CheckCategoryExist(string name)
        {
            bool isCategoryNotExist = this.categoriesService.GetByName(name) == null;

            if (isCategoryNotExist)
            {
                Category categoryToAdd = new Category()
                {
                    Name = name,
                    ShowOnHomePage = false,
                    CategoriesGroup = CategoriesGroup.Pc
                };

                this.categoriesService.AddCategory(categoryToAdd);
            }
        }
    }
}
