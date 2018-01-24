using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IzgodnoKupi.Services
{
    public class ProductsService : IProductsService
    {
        private readonly ISaveContext context;
        private readonly IEfRepository<Product> productsRepo;

        public ProductsService(IEfRepository<Product> productsRepo, ISaveContext context)
        {
            Guard.WhenArgument(productsRepo, "productsRepo").IsNull().Throw();
            Guard.WhenArgument(context, "context").IsNull().Throw();

            this.productsRepo = productsRepo;
            this.context = context;
        }

        public IQueryable<Product> GetAll()
        {
            return this.productsRepo.All
                .Include(i => i.Pictures);
        }

        public IQueryable<Product> GetByName(string searchName)
        {
            return string.IsNullOrEmpty(searchName) ? this.productsRepo.All
                                                    : this.productsRepo.All
                                                    .Where(p => p.Name.Contains(searchName))
                                                    .Include(i => i.Pictures);
        }

        public IQueryable<Product> GetByCategory(Guid? id)
        {
            return this.productsRepo.All
                        .Where(c => c.CategoryId == id && c.IsPublished == true)
                        .Include(i => i.Pictures)
                        .OrderBy(c => c.Name);
        }

        public Product GetById(Guid? id)
        {
            Product result = null;
            if (id.HasValue)
            {
                Product product = this.productsRepo.All
                    .Where(x => x.Id == id.Value)
                    .Include(i => i.Pictures)    //For include pictures
                    .SingleOrDefault();

                if (product != null)
                {
                    result = product;
                }
            }

            return result;
        }

        public void AddProduct(Product product)
        {
            this.productsRepo.Add(product);
            this.context.Commit();
        }

        public void AddListOfProducts(IList<Product> products)
        {
            foreach (var product in products)
            {
                this.productsRepo.Add(product);
            }

            this.context.Commit();
        }

        public void Update(Product product)
        {
            this.productsRepo.Update(product);
            this.context.Commit();
        }

        public void Delete(Product product)
        {
            this.productsRepo.Delete(product);
            this.context.Commit();
        }
    }
}
