using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Abstracts;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzgodnoKupi.Data.Model
{
    public class OrderItem : DataModel
    {
        public OrderItem()
        {
        }

        [ForeignKey("Order")]
        public Guid? OrderId { get; set; }

        public virtual Order Order { get; set; }

        [ForeignKey("Product")]
        public Guid? ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        [Range(
           ValidationConstants.QuantityMinValue,
           ValidationConstants.QuantityMaxValue,
           ErrorMessage = ValidationConstants.QuаntityOutOfRangeErrorMessage)]
        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal SubTotal { get; set; }

        public DateTime OrderedDate { get; set; }

        public double ItemWeight { get; set; }

        public double ItemDiscount { get; set; }
    }
}
