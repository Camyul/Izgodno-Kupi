using IzgodnoKupi.Data.Model;
using System;
using System.Linq;

namespace IzgodnoKupi.Services.Contracts
{
    public interface IOrdersService
    {
        IQueryable<Order> GetAll();

        Order GetById(Guid? id);

        IQueryable<Order> GetByUser(string id);

        Order GetByUserAndNotCompleted(string id);

        void AddOrder(Order order);

        void Update(Order order);

        void Delete(Order order);
    }
}
