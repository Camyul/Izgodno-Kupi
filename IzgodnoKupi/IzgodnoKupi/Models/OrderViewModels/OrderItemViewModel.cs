using IzgodnoKupi.Web.Models.ProductViewModels;

namespace IzgodnoKupi.Web.Models.OrderViewModels
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

        public ProductViewModel Product { get; private set; }

        public int Quantity { get; set; }
    }
}
