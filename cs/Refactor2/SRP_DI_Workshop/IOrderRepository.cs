using System.Collections.Generic;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop
{
    public interface IOrderRepository
    {
        IDictionary<long, Order> Orders { get; }

        Order CreateOrder();
        void UpdateOrders();
    }
}