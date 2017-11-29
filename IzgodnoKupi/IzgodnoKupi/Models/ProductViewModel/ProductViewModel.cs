using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.ProductViewModel
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {

        }

        public ProductViewModel(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.ShortDescription = product.ShortDescription;
            this.FullDescription = product.FullDescription;
            this.CategoryId = product.Category.Id;
            this.Quantity = product.Quantity;
            this.PictureUrl = product.PictureUrl;
            this.Price = product.Price;
            this.IsPublished = product.IsPublished;
            this.ProductAvailability = product.ProductAvailability;
            this.IsFreeShipping = product.IsFreeShipping;
            this.Weight = product.Weight;

        }

        public Guid? Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgDigitSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.DescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthDescriptionErrorMessage)]
        [RegularExpression(ValidationConstants.DescriptionRegex, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string ShortDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.LongDescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthLongDescriptionErrorMessage)]
        [RegularExpression(ValidationConstants.DescriptionRegex, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string FullDescription { get; set; }
        public Guid? CategoryId { get; set; }
        //public TelerikAcademy.FinalProject.Data.Model.Category Category { get; set; }

        [Required]
        [Range(
                ValidationConstants.QuantityMinValue,
                   ValidationConstants.QuantityMaxValue,
                    ErrorMessage = ValidationConstants.QuаntityOutOfRangeErrorMessage)]
        public int Quantity { get; set; }

        [MinLength(ValidationConstants.ImageUrlMinLength, ErrorMessage = ValidationConstants.MinLengthUrlErrorMessage)]
        [MaxLength(ValidationConstants.ImageUrlMaxLength, ErrorMessage = ValidationConstants.MaxLengthUrlErrorMessage)]
        public string PictureUrl { get; set; }

        [Required]
        public decimal Price { get; set; }

        public bool IsPublished { get; set; }

        public ProductAvailability ProductAvailability { get; set; }

        public bool IsFreeShipping { get; set; }

        public double Weight { get; set; }
    }
}
