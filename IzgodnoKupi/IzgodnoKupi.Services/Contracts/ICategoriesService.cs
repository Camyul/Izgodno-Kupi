using IzgodnoKupi.Data.Model;
using System;
using System.Linq;

namespace IzgodnoKupi.Services.Contracts
{
    public interface ICategoriesService
    {
        IQueryable<Category> GetAll();

        IQueryable<Category> GetAllCategoriesSortedByName();

        Category GetById(Guid? id);

        Category GetByName(string name);

        void AddCategory(Category category);

        void Update(Category category);

        void Delete(Category category);
    }
}
