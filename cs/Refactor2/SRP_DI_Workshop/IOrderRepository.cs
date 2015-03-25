using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop
{
    public interface IOrderRepository
    {
        Order CreateOrder();
        Order GetOrder(long orderId);
        void AddOrder(Order order);

        void UpdateOrders();
    }
}