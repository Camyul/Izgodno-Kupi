using Bytes2you.Validation;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Xml;
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

        public IActionResult ExportToLinqToXml()
        {
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();

            var url = "./wwwroot/xml/products.xml";

            string product_url = "http://izgodnokupi.com/product/details/";

            var products = this.productsService
                               .GetAll()
                               .Where(x => x.IsPublished == true
                                        && x.Category.Name != "LED осветление"
                                        && x.Category.Name != "Артикули втора у-ба"
                                        && x.Category.Name != "Кутии за Компютри"
                                        && x.Category.Name != "Софтуер"
                                        && x.Category.Name != "Стабилизатори и UPS"
                                        && x.Category.Name != "Телевизори"
                                        && x.Category.Name != "Резервни части"
                                        && x.Category.Name != "Смартфони"
                                        && x.Category.Name != "Стъклени протектори"
                                        && x.Category.Name != "Дисплей/Tъч за таблети"
                                        && x.Category.Name != "Дисплей/Tъч за смартфони")
                               .ToList();

            var doc = new XDocument();
            var root = new XElement("products",
                products.Select( product =>
                {
                    return new XElement("product",
                            new XElement("identifier", product.Id.ToString()),
                            //new XElement("manufacturer", string.Empty),
                            new XElement("manufacturer", GetManufacturer(product.Name)),
                            new XElement("category", product.Category.Name),
                            new XElement("name", product.Name),
                            new XElement("product_url", product_url + product.Id.ToString()),
                            new XElement("price", product.Price.ToString())

                        );
                }));

            //stopwatch.Stop();
            //var time = new XElement("time", stopwatch.Elapsed);
            //root.Add(time);

            doc.Add(root);

            doc.Save(url);


            return RedirectToAction("Index");
        }

        public IActionResult ExportToXml()
        {
            //var stopwatch = new Stopwatch();
            //stopwatch.Start();

            var url = "./wwwroot/xml/products.xml";

            var products = this.productsService
                               .GetAll()
                               .Where(x => x.IsPublished == true
                                        && x.Category.Name != "LED осветление"
                                        && x.Category.Name != "Артикули втора у-ба"
                                        && x.Category.Name != "Кутии за Компютри"
                                        && x.Category.Name != "Софтуер"
                                        && x.Category.Name != "Стабилизатори и UPS"
                                        && x.Category.Name != "Телевизори"
                                        && x.Category.Name != "Резервни части"
                                        && x.Category.Name != "Смартфони"
                                        && x.Category.Name != "Стъклени протектори"
                                        && x.Category.Name != "Дисплей/Tъч за таблети"
                                        && x.Category.Name != "Дисплей/Tъч за смартфони")
                               .ToList();

            using (var writer = XmlWriter.Create(url))
            {
                writer.WriteStartDocument();

                writer.WriteStartElement("products");

                foreach (var product in products)
                {
                    WriteNextProduct(writer, product);
                }

                //stopwatch.Stop();
                //writer.WriteStartElement("time");
                //writer.WriteValue(stopwatch.Elapsed);
                //writer.WriteEndElement();


                writer.WriteEndElement();

                writer.WriteEndDocument();
            }
           

            return RedirectToAction("Index");
        }

        private void WriteNextProduct(XmlWriter writer, Product product)
        {
            string product_url = "http://izgodnokupi.com/product/details/";

            writer.WriteStartElement("product");

            writer.WriteElementString("identifier", product.Id.ToString());
            //writer.WriteElementString("manufacturer", string.Empty);
            writer.WriteElementString("manufacturer", GetManufacturer(product.Name));
            writer.WriteElementString("category", product.Category.Name);
            writer.WriteElementString("name", product.Name);
            writer.WriteElementString("product_url", product_url + product.Id.ToString());
            writer.WriteElementString("price", product.Price.ToString());


            writer.WriteEndElement();
        }

        private string GetManufacturer(string name)
        {
            int spacePosition = name.IndexOf(' ');
            string manufacturer = string.Empty;

            if (spacePosition > 0)
            {
                manufacturer = name.Substring(0, spacePosition);
            }

            if (manufacturer.ToLower() == "western" ||
                manufacturer.ToLower() == "hewlett" ||
                manufacturer.ToLower() == "silicon" ||
                manufacturer.ToLower() == "it")
            {
                int secondSpacePosition = name.IndexOf(' ', spacePosition + 1);
                manufacturer = name.Substring(0, secondSpacePosition);
            }

            return manufacturer;
        }
    }
}
