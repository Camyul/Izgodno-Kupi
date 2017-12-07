using IzgodnoKupi.Data.Model;
using System;

namespace IzgodnoKupi.Services.Contracts
{
    public interface IOrderItemsService
    {
        // IEnumerable<OrderItem> GetAll();

        OrderItem GetById(Guid? id);

        Guid Update(OrderItem orderItem);

        Guid Delete(OrderItem orderItem);

        Guid Create(OrderItem orderItem);
    }
}
