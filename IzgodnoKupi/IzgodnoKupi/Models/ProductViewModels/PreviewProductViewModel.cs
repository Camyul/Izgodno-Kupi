﻿using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IzgodnoKupi.Web.Models.ProductViewModels
{
    public class PreviewProductViewModel
    {
        public PreviewProductViewModel()
        {
        }

        public PreviewProductViewModel(Product product)
        {
            this.Id = product.Id;
            this.Name = HttpUtility.HtmlDecode(product.Name);
            this.Quantity = product.Quantity;
            //this.PictureUrl = product.PictureUrls.OfType<string>().FirstOrDefault();
            this.Picture = product.Pictures.FirstOrDefault();
            this.Price = product.Price;
            this.OldPrice = product.OldPrice;
            this.Discount = product.Discount;
        }

        public Guid? Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        public string Name { get; set; }

        [Required]
        [Range(
                ValidationConstants.QuantityMinValue,
                   ValidationConstants.QuantityMaxValue,
                    ErrorMessage = ValidationConstants.QuаntityOutOfRangeErrorMessage)]
        public int Quantity { get; set; }

        [MinLength(ValidationConstants.ImageUrlMinLength, ErrorMessage = ValidationConstants.MinLengthUrlErrorMessage)]
        [MaxLength(ValidationConstants.ImageUrlMaxLength, ErrorMessage = ValidationConstants.MaxLengthUrlErrorMessage)]
        public Picture Picture { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal OldPrice { get; set; }
        public double Discount { get; set; }
    }
}
