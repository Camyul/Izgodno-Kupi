using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Web.Models.ContactInfoViewModels;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Models.OrderItemViewModels
{
    public class MyCartViewModel
    {
        private ICollection<OrderItemViewModel> orderItems;

        public MyCartViewModel()
        {
            this.orderItems = new HashSet<OrderItemViewModel>();
        }

        public MyCartViewModel(Order order)
        {
            this.UserId = order.UserId;
            this.FullContactInfo = new FullContactInfoViewModel(order.FullContactInfo);
            this.OrderItems = new HashSet<OrderItemViewModel>();
        }
        
        public string UserId { get; set; }

        public FullContactInfoViewModel FullContactInfo { get; set; }

        public virtual ICollection<OrderItemViewModel> OrderItems
        {
            get
            {
                return this.orderItems;
            }

            set
            {
                this.orderItems = value;
            }
        }

        public decimal TotalAmountExclTax { get; set; }

        public decimal TotalAmountInclTax { get; set; }

        public decimal TaxAmount { get; set; }

        public decimal TotalAmount { get; set; }

        public decimal ShippingTax { get; set; }
    }
}
