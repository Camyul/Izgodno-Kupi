using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace IzgodnoKupi.Web.Models.ProductViewModels
{
    public class ProductDetailsViewModel
    {
        public ProductDetailsViewModel()
        {
        }

        public ProductDetailsViewModel(Product product)
        {
            this.Id = product.Id;
            this.Name = HttpUtility.HtmlDecode(product.Name);
            this.ShortDescription = product.ShortDescription;
            this.FullDescription = product.FullDescription;
            this.CategoryId = product.CategoryId;
            this.Category = product.Category;
            this.Quantity = product.Quantity;
            this.Picture = product.Pictures;
            this.Price = product.Price;
            this.OldPrice = product.OldPrice;
            this.Discount = product.Discount;
            this.IsPublished = product.IsPublished;
            this.ProductAvailability = product.ProductAvailability;
            this.IsFreeShipping = product.IsFreeShipping;
            this.Weight = product.Weight;

        }

        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Име")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.DescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthDescriptionErrorMessage)]
        public string ShortDescription { get; set; }

        [DataType(DataType.MultilineText)]
        [MinLength(ValidationConstants.DescriptionMinLength, ErrorMessage = ValidationConstants.MinLengthDescriptionErrorMessage)]
        [MaxLength(ValidationConstants.LongDescriptionMaxLength, ErrorMessage = ValidationConstants.MaxLengthLongDescriptionErrorMessage)]
        public string FullDescription { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }

        [Required]
        [Range(
                ValidationConstants.QuantityMinValue,
                   ValidationConstants.QuantityMaxValue,
                    ErrorMessage = ValidationConstants.QuаntityOutOfRangeErrorMessage)]
        [Display(Name = "Количество")]
        public int Quantity { get; set; }

        public ICollection<Picture> Picture { get; set; }

        [Required]
        [Range(ValidationConstants.PriceMinValue, ValidationConstants.PriceMaxValue,
            ErrorMessage = ValidationConstants.PriceOutOfRangeErrorMessage)]
        [Display(Name = "Цена")]
        public decimal Price { get; set; }

        [Range(ValidationConstants.PriceMinValue, ValidationConstants.PriceMaxValue,
            ErrorMessage = ValidationConstants.PriceOutOfRangeErrorMessage)]
        public decimal OldPrice { get; set; }

        [Range(ValidationConstants.DiscountMinValue, ValidationConstants.DiscountMaxValue,
            ErrorMessage = ValidationConstants.DiscountOutOfRangeErrorMessage)]
        [Display(Name = "Отстъпка")]
        public double Discount { get; set; }

        [Required]
        [Display(Name = "Видима")]
        public bool IsPublished { get; set; }

        [Display(Name = "Наличност")]
        public ProductAvailability ProductAvailability { get; set; }

        [Display(Name = "Безплатна доставка")]
        public bool IsFreeShipping { get; set; }

        public double Weight { get; set; }


        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string FirstName { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgSpaceMinus)]
        public string LastName { get; set; }

        [Required]
        [RegularExpression(ValidationConstants.PhoneRegex)]
        public string PhoneNumber { get; set; }
    }
}
