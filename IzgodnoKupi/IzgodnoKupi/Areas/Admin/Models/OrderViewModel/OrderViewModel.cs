using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Web.Areas.Admin.Models.OrderItemViewModel;
using System;
using System.Collections.Generic;

namespace IzgodnoKupi.Web.Areas.Admin.Models.OrderViewModel
{
    public class OrderViewModel
    {
        private ICollection<OrderItemAdminViewModel> orderItems;

        public OrderViewModel()
        {
            this.orderItems = new HashSet<OrderItemAdminViewModel>();
        }
        public OrderViewModel(Order order)
        {
            this.Id = order.Id;
            this.OrderDate = order.OrderDate;
            this.UserId = order.UserId;
            this.OrderItems = new HashSet<OrderItemAdminViewModel>();
            this.TotalAmountInclTax = order.TotalAmountInclTax;
            this.TotalAmountExclTax = order.TotalAmountExclTax;
            this.OrderStatus = order.OrderStatus;
            //this.PaymentMethod = order.PaymentMethod;
            this.TotalDiscount = order.TotalDiscount;
            this.TaxAmount = order.TaxAmount;
            //this.ShippingMethod = order.ShippingMethod;
            this.FullContactInfo = order.FullContactInfo;
            this.FullContactInfoId = order.FullContactInfoId;
            this.ShortContactInfo = order.ShortContactInfo;
            this.ShippingTax = order.ShippingTax;


        }

        public Guid Id { get; set; }

        public string UserId { get; set; }

        public virtual ICollection<OrderItemAdminViewModel> OrderItems
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

        public DateTime OrderDate { get; set; }

        public decimal TotalAmountExclTax { get; set; }

        public decimal TotalAmountInclTax { get; set; }

        public OrderStatus OrderStatus { get; set; }

        //public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TaxAmount { get; set; }

        //public ShippingMethod ShippingMethod { get; set; }

        public Guid? FullContactInfoId { get; set; }
        public FullContactInfo FullContactInfo { get; set; }

        public ShortContactInfo ShortContactInfo { get; set; }

        public decimal ShippingTax { get; set; }
    }
}
