﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop
{
    public class OrderRepository
    {
        private const string OrderFileName = "data\\orderFile.xml";
        private long _lastOrderId;

        private readonly LoggerService _loggerService = new LoggerService();

        public Dictionary<long, Order> Orders;

        public OrderRepository()
        {
            Order[] orders = ReadOrdersFromFile();

            Orders = orders.ToDictionary(i => i.OrderId, i => i);

            _lastOrderId = Orders.Max(o => o.Value.OrderId);
        }

        public Order CreateOrder()
        {
            return new Order { OrderId = ++_lastOrderId };
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
                _loggerService.WriteLine("Exception while trying to read orders from file: " + ex, "OrderError");
                throw;
            }

            return orders;
        }

        public void UpdateOrders()
        {
            WriteOrdersToFile(Orders.Select(o => o.Value).ToArray());
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
                _loggerService.WriteLine("Exception while trying to write orders to file: " + ex, "OrdersError");
                throw;
            }
        }
    }
}
