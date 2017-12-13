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
            this.ShowOnHomePage = category.ShowOnHomePage;
            //this.Products = category.Products;
        }

        public Guid? Id { get; set; }

        [Required(ErrorMessage = "Полето {0} е задължително!")]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        [Display(Name = "Име")]
        public string Name { get; set; }

        public bool ShowOnHomePage { get; set; }

        //public ICollection<Product> Products { get; set; }
    }
}
