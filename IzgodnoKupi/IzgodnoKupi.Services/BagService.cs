using Bytes2you.Validation;
using IzgodnoKupi.Data.Contracts;
using IzgodnoKupi.Data.Model;
using IzgodnoKupi.Data.Model.Enums;
using IzgodnoKupi.Services.Contracts;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace IzgodnoKupi.Services
{
    public class BagService : IBagService
    {
        private readonly ISaveContext context;
        private readonly IEfRepository<Order> ordersRepo;

        public BagService(IEfRepository<Order> ordersRepo, ISaveContext context)
        {
            Guard.WhenArgument(ordersRepo, "ordersRepo").IsNull().Throw();
            Guard.WhenArgument(context, "context").IsNull().Throw();

            this.ordersRepo = ordersRepo;
            this.context = context;
        }

        public decimal TotalAmount(string id)
        {
            var result = this.ordersRepo.All
                        .Where(c => c.UserId == id)
                        .Include(x => x.OrderItems)
                        .FirstOrDefault(c => c.OrderStatus == OrderStatus.NotCompleted);

            return result.TotalAmountInclTax;
        }

        public int Count(string id)
        {
            var result = this.ordersRepo.All
                        .Where(c => c.UserId == id)
                        .Include(x => x.OrderItems)
                        .FirstOrDefault(c => c.OrderStatus == OrderStatus.NotCompleted);

            return result.OrderItems.Count;
        }
    }
}
