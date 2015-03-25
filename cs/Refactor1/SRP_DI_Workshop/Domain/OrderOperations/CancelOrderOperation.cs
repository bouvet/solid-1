using System;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class CancelOrderOperation : IOrderOperation
    {
        public InventoryRepository InventoryRepository;
        public OrderRepository OrderRepository;
        private readonly LoggerService _loggerService = new LoggerService();
        private readonly ResponseMessageFactory _responseMessageFactory = new ResponseMessageFactory();

        public const Operation Id = Operation.Cancel_Order;

        public ResponseMessage ExecuteOperation(RequestMessage request)
        {
            try
            {
                Order orderToCancel = OrderRepository.GetOrder(request.OrderId);

                switch (orderToCancel.State)
                {
                    case OrderState.Shipped:
                        // msg cannot cancel shipped order
                        return _responseMessageFactory.CreateCancellationResponseMessage(request, orderToCancel,
                            "Cannot cancel an order that has been shipped.");
                    case OrderState.Cancelled:
                        return _responseMessageFactory.CreateCancellationResponseMessage(request, orderToCancel,
                            "Order has already been cancelled.");
                    default:
                        foreach (OrderItem item in orderToCancel.OrderItems)
                        {
                            InventoryItem inventoryItem = InventoryRepository.GetInventoryItem(item.ItemCode);

                            if (item.State == OrderItemState.Filled)
                                inventoryItem.QuantityOnHand += item.Quantity;

                            item.State = OrderItemState.Cancelled;
                        }

                        orderToCancel.State = OrderState.Cancelled;

                        // save inventory
                        InventoryRepository.UpdateInventory();

                        // save order
                        OrderRepository.UpdateOrders();

                        return _responseMessageFactory.CreateCancellationResponseMessage(request, orderToCancel,
                            "Order has been cancelled and reserved inventory has been released.");
                }
            }
            catch (Exception ex)
            {
                _loggerService.WriteLine("Exception during operation CancelOrder: " + ex, "CancelOrderError");
                return _responseMessageFactory.CreateErrorResponseMessage(ex);
            }
        }
    }
}