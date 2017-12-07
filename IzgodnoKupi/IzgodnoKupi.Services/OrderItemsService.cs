using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Services.Contracts;
using System;

namespace IzgodnoKupi.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IEfRepository<OrderItem> orderItemsRepo;
        private readonly ISaveContext context;

        public OrderItemsService(IEfRepository<OrderItem> orderItemsRepo, ISaveContext context)
        {
            Guard.WhenArgument(orderItemsRepo, "OrderItemsRepo").IsNull().Throw();
            Guard.WhenArgument(context, "OrderItemscontext").IsNull().Throw();

            this.orderItemsRepo = orderItemsRepo;
            this.context = context;
        }

        public Guid Create(OrderItem orderItem)
        {
            this.orderItemsRepo.Add(orderItem);
            context.Commit();

            return orderItem.Id;
        }

        public Guid Delete(OrderItem orderItem)
        {
            OrderItem orderItemToDelete = this.orderItemsRepo.GetById(orderItem.Id);//???????????????
            this.orderItemsRepo.Delete(orderItemToDelete);
            this.context.Commit();

            return orderItem.Id;
        }

        public OrderItem GetById(Guid? id)
        {
            return id.HasValue ? this.orderItemsRepo.GetById(id) : null;
        }

        public Guid Update(OrderItem orderItem)
        {
            OrderItem orderItemToUpdate = this.orderItemsRepo.GetById(orderItem.Id);
            this.orderItemsRepo.Delete(orderItemToUpdate);
            this.context.Commit();

            return orderItem.Id;
        }
    }
}
