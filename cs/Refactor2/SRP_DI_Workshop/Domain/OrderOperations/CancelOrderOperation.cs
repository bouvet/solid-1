using System;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class CancelOrderOperation : IOrderOperation
    {
        private readonly IInventoryRepository _inventoryService;
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerService _loggerService;
        private readonly IResponseMessageFactory _responseMessageFactory;

        public CancelOrderOperation(
            IResponseMessageFactory responseMessageFactory,
            ILoggerService loggerService,
            IOrderRepository orderRepository,
            IInventoryRepository inventoryService)
        {
            _responseMessageFactory = responseMessageFactory;
            _loggerService = loggerService;
            _orderRepository = orderRepository;
            _inventoryService = inventoryService;

            Operation = Operation.CancelOrder;
        }

        public Operation Operation { get; private set; }

        public ResponseMessage ExecuteOperation(RequestMessage request)
        {
            try
            {
                Order orderToCancel = _orderRepository.GetOrder(request.OrderId);

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
                            InventoryItem inventoryItem = _inventoryService.GetInventoryItem(item.ItemCode);

                            if (item.State == OrderItemState.Filled)
                                inventoryItem.QuantityOnHand += item.Quantity;

                            item.State = OrderItemState.Cancelled;
                        }

                        orderToCancel.State = OrderState.Cancelled;

                        // save inventory
                        _inventoryService.UpdateInventory();

                        // save order
                        _orderRepository.UpdateOrders();

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