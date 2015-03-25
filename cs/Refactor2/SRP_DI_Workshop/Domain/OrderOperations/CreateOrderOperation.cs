using System;
using System.Linq;
using SRP_DI_Workshop.Domain.Revenue;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class CreateOrderOperation : IOrderOperation
    {
        private readonly IPriceCalculator _priceCalculator;
        private readonly IInventoryRepository _inventoryService;
        private readonly IOrderRepository _orderRepository;
        private readonly ILoggerService _loggerService;
        private readonly IResponseMessageFactory _responseMessageFactory;

        public CreateOrderOperation(
            IPriceCalculator priceCalculator, 
            IInventoryRepository inventoryService, 
            IOrderRepository orderRepository, 
            ILoggerService loggerService, 
            IResponseMessageFactory responseMessageFactory)
        {
            _priceCalculator = priceCalculator;
            _inventoryService = inventoryService;
            _orderRepository = orderRepository;
            _loggerService = loggerService;
            _responseMessageFactory = responseMessageFactory;

            Operation = Operation.SubmitOrder;
        }

        public Operation Operation { get; private set; }

        public ResponseMessage ExecuteOperation(RequestMessage request)
        {
            try
            {
                Order order = _orderRepository.CreateOrder();

                order.OrderItems.AddRange(request.OrderItems);

                foreach (OrderItem item in order.OrderItems)
                {
                    InventoryItem inventoryItem = _inventoryService.GetInventoryItem(item.ItemCode);

                    if (item.Quantity <= inventoryItem.QuantityOnHand)
                    {
                        inventoryItem.QuantityOnHand -= item.Quantity;
                        item.Weight = item.WeightPerUnit * (float)item.Quantity;

                        _priceCalculator.CalculatePrice(item, inventoryItem);

                        item.State = OrderItemState.Filled;
                    }
                    else
                    {
                        item.State = OrderItemState.NotEnoughQuantityOnHand;
                    }
                }

                order.State = order.OrderItems.All(o => o.State == OrderItemState.Filled)
                    ? OrderState.Filled
                    : OrderState.Processing;

                _orderRepository.AddOrder(order);

                // save inventory
                _inventoryService.UpdateInventory();

                // save order
                _orderRepository.UpdateOrders();

                return _responseMessageFactory.CreateOrderSubmissionResponseMessage(request, order);
            }
            catch (Exception ex)
            {
                _loggerService.WriteLine("Exception during operation SubmitOrder: " + ex, "SubmitOrderError");
                return _responseMessageFactory.CreateErrorResponseMessage(ex);
            }
        }
    }
}