using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using System;

namespace IzgodnoKupi.Web.Areas.Admin.Models.OrderViewModel
{
    public class OrderListViewModel
    {
        public OrderListViewModel()
        {

        }
        public OrderListViewModel(Order order)
        {
            this.Id = order.Id;
            this.OrderDate = order.OrderDate;
            this.TotalAmountInclTax = order.TotalAmountInclTax;
            this.OrderStatus = order.OrderStatus;
            //this.PaymentMethod = order.PaymentMethod;
            this.TotalDiscount = order.TotalDiscount;
            this.TaxAmount = order.TaxAmount;
            //this.ShippingMethod = order.ShippingMethod;
            this.FullContactInfo = order.FullContactInfo;
            this.ShortContactInfo = order.ShortContactInfo;
            this.ShippingTax = order.ShippingTax;
        }

        public Guid Id { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmountInclTax { get; set; }

        public OrderStatus OrderStatus { get; set; }

        //public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TaxAmount { get; set; }

        //public ShippingMethod ShippingMethod { get; set; }

        public FullContactInfo FullContactInfo { get; set; }

        public ShortContactInfo ShortContactInfo { get; set; }

        public decimal ShippingTax { get; set; }
    }
}
