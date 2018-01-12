using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using Microsoft.EntityFrameworkCore;
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

        public void Delete(Order order)
        {
            order.IsDeleted = true;
            this.ordersRepo.Update(order);
            this.context.Commit();
        }

        public IQueryable<Order> GetAll()
        {
            return this.ordersRepo
                .All
                .Where(c => c.IsDeleted == false)
                .Include(x => x.OrderItems)
                .Include(x => x.FullContactInfo);
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

        public Order GetByUserAndNotCompleted(string id)
        {
            return this.ordersRepo.All
                        .Where(c => c.UserId == id)
                        .Include(x => x.OrderItems)
                        .FirstOrDefault(c => c.OrderStatus == OrderStatus.NotCompleted);
        }

        public void Update(Order order)
        {
            this.ordersRepo.Update(order);

            this.context.Commit();
        }
    }
}
