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
        private readonly LoggerService _loggerService = new LoggerService();

        private readonly Dictionary<string, InventoryItem> _inventory;

        public InventoryRepository()
        {
            InventoryItem[] inventoryItems = ReadInventoryFromFile();
            _inventory = inventoryItems.ToDictionary(i => i.ItemCode, i => i);

        }

        public InventoryItem GetInventoryItem(string code)
        {
            return _inventory[code];
        }

        public void UpdateInventory()
        {
            WriteInventoryToFile(_inventory.Select(kvp => kvp.Value).ToArray());
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

