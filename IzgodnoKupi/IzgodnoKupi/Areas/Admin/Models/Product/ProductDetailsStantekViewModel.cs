namespace IzgodnoKupi.Web.Areas.Admin.Models.Product
{
    public class ProductDetailsStantekViewModel
    {
        //public Guid? Id { get; set; }
        
        public string Name { get; set; }

        public string FullDescription { get; set; }

        public string PictureUrl { get; set; }
        
        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }
        
        public double Discount { get; set; }
    }
}
