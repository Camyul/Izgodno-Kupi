using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Abstracts;
using IzgodnoKupi.Data.Model.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IzgodnoKupi.Data.Model
{
    public class Product : DataModel
    {
        private ICollection<OrderItem> orderItems;
        private ICollection<Picture> pictures;

        public Product()
        {
            this.OrderItems = new HashSet<OrderItem>();
            this.Pictures = new HashSet<Picture>();
        }

        [Required]
        //[Index(IsUnique = true)]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        public string Name { get; set; }

        [Column(TypeName = "ntext")]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.DescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthDescriptionErrorMessage)]
        public string ShortDescription { get; set; }

        [Column(TypeName = "ntext")]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.LongDescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthLongDescriptionErrorMessage)]
        public string FullDescription { get; set; }

        [Required]
        [Range(
            ValidationConstants.QuantityMinValue,
            ValidationConstants.QuantityMaxValue,
            ErrorMessage = ValidationConstants.QuаntityOutOfRangeErrorMessage)]
        public int Quantity { get; set; }

        [ForeignKey("Category")]
        public Guid CategoryId { get; set; }

        public virtual Category Category { get; set; }

        [JsonIgnore]
        public virtual ICollection<Picture> Pictures
        {
            get
            {
                return this.pictures;
            }

            set
            {
                this.pictures = value;
            }
        }
        //public string PictureUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        public decimal OldPrice { get; set; }

        public double Discount { get; set; }

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

        public ProductAvailability ProductAvailability { get; set; }

        public bool IsFreeShipping { get; set; }

        public bool MarkAsNew { get; set; }

        public bool IsPublished { get; set; }

        public double Weight { get; set; }
    }
}
