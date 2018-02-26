using IzgodnoKupi.Data.Model;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Areas.Admin.Models.Product
{
    public class ProductSolytronViewModel
    {
        public string Name { get; set; }

        public IzgodnoKupi.Data.Model.Category Category { get; set; }

        public string FullDescription { get; set; }

        public ICollection<Picture> Pictures { get; set; }

        public decimal Price { get; set; }
    }
}
