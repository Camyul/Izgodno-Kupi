using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Web.Models.ProductViewModels;
using System;

namespace IzgodnoKupi.Web.Areas.Admin.Models.OrderItemViewModel
{
    public class OrderItemAdminViewModel
    {
        public OrderItemAdminViewModel()
        {
        }

        public OrderItemAdminViewModel(OrderItem orderItem)
        {
            this.ProductId = orderItem.ProductId;
            this.Quantity = orderItem.Quantity;
            this.UnitPrice = orderItem.UnitPrice;
            this.SubTotal = orderItem.SubTotal;
            this.ItemWeight = orderItem.ItemWeight;
            this.ItemDiscount = orderItem.ItemDiscount;
        }
        
        public Guid ProductId { get; set; }

        public ProductViewModel Product { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }

        public double ItemWeight { get; set; }

        public double ItemDiscount { get; set; }
    }
}
