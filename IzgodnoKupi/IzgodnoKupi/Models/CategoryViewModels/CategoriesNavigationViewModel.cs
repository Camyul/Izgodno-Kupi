using IzgodnoKupi.Data.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IzgodnoKupi.Web.Models.CategoryViewModels
{
    public class CategoriesNavigationViewModel
    {
        public CategoriesNavigationViewModel()
        {
        }

        public CategoriesNavigationViewModel(Category category)
        {
            if (category != null)
            {
                this.Id = category.Id;
                this.Name = category.Name;
            }
        }

        [HiddenInput]
        public Guid? Id { get; set; }

        public string Name { get; set; }
    }
}
