using System;
using System.Linq;
using SRP_DI_Workshop.Domain.Revenue;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop.Domain.OrderOperations
{
    public class CreateOrderOperation : IOrderOperation
    {
        public InventoryRepository InventoryRepository;
        public OrderRepository OrderRepository;
        private readonly PriceCalculator _priceCalculator = new PriceCalculator();
        private readonly LoggerService _loggerService = new LoggerService();
        private readonly ResponseMessageFactory _responseMessageFactory = new ResponseMessageFactory();

        public const Operation Id = Operation.SubmitOrder;

        public ResponseMessage ExecuteOperation(RequestMessage request)
        {
            try
            {
                Order order = OrderRepository.CreateOrder();

                order.OrderItems.AddRange(request.OrderItems);

                foreach (OrderItem item in order.OrderItems)
                {
                    InventoryItem inventoryItem = InventoryRepository.Inventory[item.ItemCode];

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

                OrderRepository.Orders.Add(order.OrderId, order);

                // save inventory
                InventoryRepository.UpdateInventory();

                // save order
                OrderRepository.UpdateOrders();

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