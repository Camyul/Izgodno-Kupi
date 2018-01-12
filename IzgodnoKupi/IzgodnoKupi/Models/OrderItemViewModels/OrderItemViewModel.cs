using IzgodnoKupi.Web.Models.ProductViewModels;
using Newtonsoft.Json;

namespace IzgodnoKupi.Web.Models.OrderItemViewModels
{
    public class OrderItemViewModel
    {
        public OrderItemViewModel()
        {
        }

        public OrderItemViewModel(ProductViewModel product, int quantity)
        {
            this.Product = product;
            this.Quantity = quantity;
        }

        [JsonProperty("Product")]
        public ProductViewModel Product { get; private set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }
}
