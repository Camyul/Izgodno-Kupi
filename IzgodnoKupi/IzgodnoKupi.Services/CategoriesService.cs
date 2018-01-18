using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using System;
using System.Linq;

namespace IzgodnoKupi.Services
{
    public class CategoriesService : ICategoriesService
    {
        private readonly ISaveContext context;
        private readonly IEfRepository<Category> categoriesRepo;

        public CategoriesService(IEfRepository<Category> categoriesRepo, ISaveContext context)
        {
            Guard.WhenArgument(categoriesRepo, "categoriesRepo").IsNull().Throw();
            Guard.WhenArgument(context, "context").IsNull().Throw();

            this.categoriesRepo = categoriesRepo;
            this.context = context;
        }

        public IQueryable<Category> GetAll()
        {
            var result = this.categoriesRepo.All;

            return result;
        }

        public IQueryable<Category> GetAllCategoriesSortedByName()
        {
            var result = this.categoriesRepo.All
                                 .OrderBy(x => x.Name);

            return result;
        }

        public IQueryable<Category> GetAllCategoriesForHomePage()
        {
            var result = this.categoriesRepo.All
                                 .Where(x => x.ShowOnHomePage == true)
                                 .OrderBy(c => c.Name);

            return result;
        }

        public Category GetById(Guid? id)
        {
            Category result = null;
            if (id.HasValue)
            {
                Category product = this.categoriesRepo.All
                    .Where(x => x.Id == id.Value)
                    .SingleOrDefault();

                if (product != null)
                {
                    result = product;
                }
            }

            return result;
        }

        public Category GetByName(string name)
        {
            Category result = null;
            if (name != null || name != string.Empty)
            {
                Category product = this.categoriesRepo.All
                    .Where(x => x.Name == name)
                    .SingleOrDefault();

                if (product != null)
                {
                    result = product;
                }
            }

            return result;
        }

        public void AddCategory(Category category)
        {
            this.categoriesRepo.Add(category);
            this.context.Commit();
        }

        public void Update(Category category)
        {
            this.categoriesRepo.Update(category);
            this.context.Commit();
        }

        public void Delete(Category category)
        {
            this.categoriesRepo.Delete(category);
            this.context.Commit();
        }

    }
}
