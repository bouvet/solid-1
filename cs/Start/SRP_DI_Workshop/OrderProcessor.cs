using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using SRP_DI_Workshop.Domain;
using SRP_DI_Workshop.ServiceInterface;

namespace SRP_DI_Workshop
{
    public class OrderProcessor
    {
        private readonly Dictionary<long, ResponseMessage> _processedMessages = new Dictionary<long, ResponseMessage>();
        private readonly Dictionary<string, InventoryItem> _inventory;
        private readonly Dictionary<long, Order> _orders;

        private const string InventoryFileName = "data\\inventoryFile.xml";
        private const string OrderFileName = "data\\orderFile.xml";
        private long _lastOrderId;

        public OrderProcessor()
        {
            InventoryItem[] inventoryItems = ReadInventoryFromFile();
            _inventory = inventoryItems.ToDictionary(i => i.ItemCode, i => i);

            Order[] orders = ReadOrdersFromFile();
            _orders = orders.ToDictionary(i => i.OrderId, i => i);

            _lastOrderId = _orders.Max(o => o.Value.OrderId);
        }

        public ResponseMessage ProcessOrder(RequestMessage reqMsg)
        {
            ResponseMessage responseToPreviousMessage;

            if (_processedMessages.TryGetValue(reqMsg.RequestId, out responseToPreviousMessage))
                return responseToPreviousMessage;

            ResponseMessage response;

            switch (reqMsg.Operation)
            {
                case Operation.Submit_Order:
                    response = CreateOrder(reqMsg);
                    break;
                case Operation.Cancel_Order:
                    response = CancelOrder(reqMsg);
                    break;
                case Operation.Get_Order_Details:
                    response = GetOrder(reqMsg);
                    break;
                default:
                    Trace.WriteLine("Received bad message: " + reqMsg.RequestId, "ProcessOrderError");
                    response = CreateBadRequestResponseMessage(reqMsg);
                    break;
            }

            _processedMessages[reqMsg.RequestId] = response;

            return response;
        }

        private ResponseMessage CreateBadRequestResponseMessage(RequestMessage reqMsg)
        {
            return new ResponseMessage {StatusCode = HttpStatusCode.BadRequest, ResponseBody = reqMsg};
        }

        private ResponseMessage GetOrder(RequestMessage reqMsg)
        {
            try
            {
                Order orderToGet = _orders[reqMsg.OrderId];

                return CreateOrderQueryResponseMessage(reqMsg, orderToGet);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception during operation GetOrderDetails: " + ex, "GetOrderDetailsError");
                return CreateErrorResponseMessage(ex);
            }
        }

        private ResponseMessage CreateOrderQueryResponseMessage(RequestMessage reqMsg, Order orderToGet)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new OrderQueryResponseMessage
                {
                    RequestId = reqMsg.RequestId,
                    Order = orderToGet
                }
            };
        }

        private ResponseMessage CancelOrder(RequestMessage reqMsg)
        {
            try
            {
                Order orderToCancel = _orders[reqMsg.OrderId];

                switch (orderToCancel.State)
                {
                    case OrderState.Shipped:
                        // msg cannot cancel shipped order
                        return CreateCancellationResponseMessage(reqMsg, orderToCancel,
                            "Cannot cancel an order that has been shipped.");
                    case OrderState.Cancelled:
                        return CreateCancellationResponseMessage(reqMsg, orderToCancel,
                            "Order has already been cancelled.");
                    default:
                        foreach (OrderItem item in orderToCancel.OrderItems)
                        {
                            InventoryItem inventoryItem = _inventory[item.ItemCode];

                            if (item.State == OrderItemState.Filled)
                                inventoryItem.QuantityOnHand += item.Quantity;

                            item.State = OrderItemState.Cancelled;
                        }

                        orderToCancel.State = OrderState.Cancelled;

                        // save inventory
                        WriteInventoryToFile(_inventory.Select(kvp => kvp.Value).ToArray());

                        // save order
                        WriteOrdersToFile(_orders.Select(o => o.Value).ToArray());

                        return CreateCancellationResponseMessage(reqMsg, orderToCancel,
                            "Order has been cancelled and reserved inventory has been released.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception during operation CancelOrder: " + ex, "CancelOrderError");
                return CreateErrorResponseMessage(ex);
            }
        }

        private ResponseMessage CreateCancellationResponseMessage(RequestMessage reqMsg, Order orderToCancel, string message)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new OrderCancellationResponse
                {
                    RequestId = reqMsg.RequestId,
                    OrderId = reqMsg.OrderId,
                    OrderState = orderToCancel.State.ToString(),
                    Message = message
                }
            };
        }

        private ResponseMessage CreateOrder(RequestMessage reqMsg)
        {
            try
            {
                Order order = new Order { OrderId = ++_lastOrderId };

                order.OrderItems.AddRange(reqMsg.OrderItems);

                foreach (OrderItem item in order.OrderItems)
                {
                    InventoryItem inventoryItem = _inventory[item.ItemCode];

                    if (item.Quantity <= inventoryItem.QuantityOnHand)
                    {
                        inventoryItem.QuantityOnHand -= item.Quantity;
                        item.Weight = item.WeightPerUnit * (float)item.Quantity;

                        CalculatePrice(item, inventoryItem);

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

                _orders.Add(order.OrderId, order);

                // save inventory
                WriteInventoryToFile(_inventory.Select(kvp => kvp.Value).ToArray());

                // save order
                WriteOrdersToFile(_orders.Select(o => o.Value).ToArray());

                return CreateOrderSubmissionResponseMessage(reqMsg, order);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception during operation SubmitOrder: " + ex, "SubmitOrderError");
                return CreateErrorResponseMessage(ex);
            }
        }

        private ResponseMessage CreateErrorResponseMessage(Exception ex)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                ResponseBody = ex.ToString()
            };
        }

        private ResponseMessage CreateOrderSubmissionResponseMessage(RequestMessage reqMsg, Order order)
        {
            return new ResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                ResponseBody = new OrderSubmissionResponse
                {
                    RequestId = reqMsg.RequestId,
                    OrderId = order.OrderId,
                    OrderState = order.State.ToString()
                }
            };
        }

        private void CalculatePrice(OrderItem item, InventoryItem inventoryItem)
        {
            switch (inventoryItem.PricingCalculation)
            {
                case "FULLQUANTITY":
                    item.WeightPerUnit = inventoryItem.WeightPerUnit;
                    item.PricePerUnit = inventoryItem.PricePerUnit;
                    item.RebatePerUnit = inventoryItem.RebatePerUnit;

                    item.Weight = item.WeightPerUnit*(float)item.Quantity;
                    item.Rebate = item.RebatePerUnit * item.Quantity;
                    item.Price = item.PricePerUnit * item.Quantity - item.Rebate;
                    item.Vat = item.Price * inventoryItem.VatRate;
                    break;
                case "BUY2GET1FREE":
                    item.WeightPerUnit = inventoryItem.WeightPerUnit;
                    item.PricePerUnit = inventoryItem.PricePerUnit;
                    item.RebatePerUnit = 0m;

                    int multiplesOfThree = (int)item.Quantity / 3;
                    decimal quantityTocharge = item.Quantity - multiplesOfThree;

                    item.Weight = item.WeightPerUnit*(float)item.Quantity;
                    item.Rebate = 0m;
                    item.Price = item.PricePerUnit * quantityTocharge;
                    item.Vat = item.Price * inventoryItem.VatRate;
                    break;
                default:
                    throw new InvalidOperationException("Invalid pricing calculation type: " + inventoryItem.PricingCalculation);
            }
        }

        private InventoryItem[] ReadInventoryFromFile()
        {
            InventoryItem[] inventoryItems;

            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(InventoryItem[]));

                using (FileStream fstr = File.OpenRead(InventoryFileName))
                using (StreamReader sr = new StreamReader(fstr, Encoding.UTF8))
                {
                    inventoryItems = xmlSer.Deserialize(sr) as InventoryItem[] ?? new InventoryItem[0];

                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception while trying to read inventory from file: " + ex, "InventoryError");
                throw;
            }

            return inventoryItems;
        }

        private void WriteInventoryToFile(InventoryItem[] inventoryItems)
        {
            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(InventoryItem[]));

                using (FileStream fstr = File.Create(InventoryFileName))
                using (StreamWriter sw = new StreamWriter(fstr, Encoding.UTF8))
                {
                    xmlSer.Serialize(sw, inventoryItems);

                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception while trying to write inventory to file: " + ex, "InventoryError");
                throw;
            }
        }

        private Order[] ReadOrdersFromFile()
        {
            Order[] orders;

            try
            {
                XmlSerializer xmlSer = new XmlSerializer(typeof(Order[]));

                using (FileStream fstr = File.OpenRead(OrderFileName))
                using (StreamReader sr = new StreamReader(fstr, Encoding.UTF8))
                {
                    orders = xmlSer.Deserialize(sr) as Order[] ?? new Order[0];


                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception while trying to read orders from file: " + ex, "OrderError");
                throw;
            }

            return orders;
        }

        private void WriteOrdersToFile(Order[] orders)
        {
            try
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(Order[]));

                using (FileStream fs = File.Create(OrderFileName))
                using (StreamWriter sw = new StreamWriter(fs, Encoding.UTF8))
                {
                    xmlser.Serialize(sw, orders);

                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Exception while trying to write orders to file: " + ex, "OrdersError");
                throw;
            }
        }
    }
}
