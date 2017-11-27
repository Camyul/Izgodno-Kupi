using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model;
using System;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Web.Models.CategoryViewModels
{
    public class CategoryViewModel
    {
        public CategoryViewModel()
        {

        }

        public CategoryViewModel(Category category)
        {
            this.Id = category.Id;
            this.Name = category.Name;
            //this.Products = category.Products;
        }

        public Guid? Id { get; set; }

        [Required]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [RegularExpression(ValidationConstants.EnBgDigitSpaceMinus, ErrorMessage = ValidationConstants.NotAllowedSymbolsErrorMessage)]
        public string Name { get; set; }

        //public ICollection<Product> Products { get; set; }
    }
}
