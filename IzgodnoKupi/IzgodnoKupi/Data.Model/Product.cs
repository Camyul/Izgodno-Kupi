using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Abstracts;
using IzgodnoKupi.Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzgodnoKupi.Data.Model
{
    public class Product : DataModel
    {
        private ICollection<OrderItem> orderItems;

        public Product()
        {
            this.OrderItems = new HashSet<OrderItem>();
        }

        [Required]
        //[Index(IsUnique = true)]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgDigitSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.DescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthDescriptionErrorMessage)]
        [RegularExpression(ValidationConstants.DescriptionRegex, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string ShortDescription { get; set; }

        [Column(TypeName = "ntext")]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.LongDescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthLongDescriptionErrorMessage)]
        [RegularExpression(ValidationConstants.DescriptionRegex, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string FullDescription { get; set; }

        [Required]
        [Range(
            ValidationConstants.QuantityMinValue,
            ValidationConstants.QuantityMaxValue,
            ErrorMessage = ValidationConstants.QuаntityOutOfRangeErrorMessage)]
        public int Quantity { get; set; }

        [ForeignKey("Category")]
        public Guid? CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [MinLength(ValidationConstants.ImageUrlMinLength, ErrorMessage = ValidationConstants.MinLengthUrlErrorMessage)]
        [MaxLength(ValidationConstants.ImageUrlMaxLength, ErrorMessage = ValidationConstants.MaxLengthUrlErrorMessage)]
        public string PictureUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public int Discount { get; set; }

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

        public Availability StockAvailability { get; set; }

        public bool IsFreeShipping { get; set; }

        public bool MarkAsNew { get; set; }

        public bool IsPublished { get; set; }

        public double Weight { get; set; }
    }
}
