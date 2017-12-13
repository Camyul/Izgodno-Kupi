using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using System;
using System.Linq;

namespace IzgodnoKupi.Services
{
    public class OrdersService : IOrdersService
    {
        private readonly ISaveContext context;
        private readonly IEfRepository<Order> ordersRepo;

        public OrdersService(IEfRepository<Order> ordersRepo, ISaveContext context)
        {
            Guard.WhenArgument(ordersRepo, "ordersRepo").IsNull().Throw();
            Guard.WhenArgument(context, "context").IsNull().Throw();

            this.ordersRepo = ordersRepo;
            this.context = context;
        }

        public void AddOrder(Order order)
        {
            this.ordersRepo.Add(order);
            this.context.Commit();
        }

        public void Delete(Guid? id)
        {
            Guard.WhenArgument(id, nameof(id)).IsNull().Throw();

            var entity = this.GetById(id.Value);
            entity.IsDeleted = true;
            this.ordersRepo.Update(entity);
            this.context.Commit();
        }

        public IQueryable<Order> GetAll()
        {
            return this.ordersRepo
                .All
                .Where(c => c.IsDeleted == false);
        }

        public Order GetById(Guid? id)
        {
            return id.HasValue ? this.ordersRepo.GetById(id) : null;
        }

        public IQueryable<Order> GetByUser(string id)
        {
            return this.ordersRepo.All
                        .Where(c => c.UserId == id)
                        .OrderBy(c => c.OrderDate);
        }

        public void Update(Guid id)
        {
            Order orderToUpdate = this.ordersRepo.GetById(id);
            this.ordersRepo.Update(orderToUpdate);

            this.context.Commit();
        }
    }
}
