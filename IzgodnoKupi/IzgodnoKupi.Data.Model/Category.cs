using IzgodnoKupi.Common;
using IzgodnoKupi.Data.Model.Abstracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IzgodnoKupi.Data.Model
{
    public class Category : DataModel
    {
        private ICollection<Product> products;
        //private ICollection<Category> subCategories;

        public Category()
        {
            this.Products = new HashSet<Product>();
            //this.SubCategories = new HashSet<Category>();
        }


        [Required]
        //[Index(IsUnique = true)]
        [MinLength(ValidationConstants.StandardMinLength, ErrorMessage = ValidationConstants.MinLengthFieldErrorMessage)]
        [MaxLength(ValidationConstants.StandartMaxLength, ErrorMessage = ValidationConstants.MaxLengthFieldErrorMessage)]
        public string Name { get; set; }

        public virtual ICollection<Product> Products
        {
            get
            {
                return this.products;
            }

            set
            {
                this.products = value;
            }
        }

        public bool ShowOnHomePage { get; set; }

        //public virtual ICollection<Category> SubCategories
        //{
        //    get
        //    {
        //        return this.subCategories;
        //    }

        //    set
        //    {
        //        this.subCategories = value;
        //    }
        //}
    }
}
