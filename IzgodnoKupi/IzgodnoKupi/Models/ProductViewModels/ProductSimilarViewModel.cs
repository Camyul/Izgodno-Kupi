using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IzgodnoKupi.Web.Models.ProductViewModels
{
    public class ProductSimilarViewModel
    {
        private string name;

        public ProductSimilarViewModel()
        {
        }

        public ProductSimilarViewModel(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Picture = product.Pictures.FirstOrDefault();
            this.Price = product.Price;
        }

        public Guid? Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        public string Name
        {
            get
            {
                return this.name;
            }
            set
            {
                if (value.Length <= 50)
                {
                    this.name = value;
                }
                else
                {
                    this.name = value.Substring(0, 50);
                }
            }
        }

        [MinLength(ValidationConstants.ImageUrlMinLength, ErrorMessage = ValidationConstants.MinLengthUrlErrorMessage)]
        [MaxLength(ValidationConstants.ImageUrlMaxLength, ErrorMessage = ValidationConstants.MaxLengthUrlErrorMessage)]
        public Picture Picture { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
