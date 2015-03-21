using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using SRP_DI_Workshop.Domain;

namespace SRP_DI_Workshop
{
    public class InventoryRepository : IInventoryRepository
    {
        private const string InventoryFileName = "data\\inventoryFile.xml";
        private readonly ILoggerService _loggerService;

        public IDictionary<string, InventoryItem> Inventory { get; private set; }

        public InventoryRepository(ILoggerService loggerService)
        {
            _loggerService = loggerService;
            InventoryItem[] inventoryItems = ReadInventoryFromFile();
            Inventory = inventoryItems.ToDictionary(i => i.ItemCode, i => i);

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
                _loggerService.WriteLine("Exception while trying to read inventory from file: " + ex, "InventoryError");
                throw;
            }

            return inventoryItems;
        }

        public void UpdateInventory()
        {
            WriteInventoryToFile(Inventory.Select(kvp => kvp.Value).ToArray());
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
                _loggerService.WriteLine("Exception while trying to write inventory to file: " + ex, "InventoryError");
                throw;
            }
        }
    }
}

