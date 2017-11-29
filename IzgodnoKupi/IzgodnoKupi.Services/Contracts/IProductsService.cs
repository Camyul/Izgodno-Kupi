using IzgodnoKupi.Data.Model;
using System;
using System.Linq;

namespace IzgodnoKupi.Services.Contracts
{
    public interface IProductsService
    {
        IQueryable<Product> GetAll();

        IQueryable<Product> GetByName(string searchName);

        Product GetById(Guid? id);

        IQueryable<Product> GetByCategory(Guid? id);

        void AddProduct(Product product);

        void Update(Product product);

        void Delete(Product product);
    }
}
