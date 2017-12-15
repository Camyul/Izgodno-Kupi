using IzgodnoKupi.Data.Model.Abstracts;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzgodnoKupi.Data.Model
{
    public class Order : DataModel
    {
        private ICollection<OrderItem> orderItems;

        public Order()
        {
            this.orderItems = new HashSet<OrderItem>();
            this.ShippingMethod = ShippingMethod.ToOffice;
            // this.OrderDate = DateTime.UtcNow.Date;
            // this.OrderStatus = OrderStatus.Confirmed;
        }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<OrderItem> OrderItems
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

        public PaymentMethod PaymentMethod { get; set; }

        public decimal TotalDiscount { get; set; }

        public decimal TaxAmount { get; set; }

        public ShippingMethod ShippingMethod { get; set; }

        [ForeignKey("FullContactInfo")]
        public Guid? FullContactInfoId { get; set; }

        public virtual FullContactInfo FullContactInfo { get; set; }

        [ForeignKey("ShortContactInfo")]
        public Guid? ShortContactInfoId { get; set; }

        public virtual ShortContactInfo ShortContactInfo { get; set; }

        public decimal ShippingTax { get; set; }
    }
}
