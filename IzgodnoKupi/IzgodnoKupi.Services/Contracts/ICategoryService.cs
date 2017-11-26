using IzgodnoKupi.Data.Model;
using System;
using System.Linq;

namespace IzgodnoKupi.Services.Contracts
{
    public interface ICategoryService
    {
        IQueryable<Category> GetAllCategoriesSortedByName();

        Category GetById(Guid? id);

        void AddCategory(Category category);
    }
}
