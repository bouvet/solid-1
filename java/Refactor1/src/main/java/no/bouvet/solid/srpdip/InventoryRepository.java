package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.util.Map;

import no.bouvet.solid.srpdip.domain.InventoryItem;

import com.thoughtworks.xstream.XStream;

public class InventoryRepository {

	private static final String INVENTORY_FILE_NAME = "inventoryFile.xml";

	public Map<String, InventoryItem> inventory;

	public InventoryRepository() {
		readInventoryFromFile();
	}
	
	public void updateInventory() {
		writeInventoryToFile();
	}
	
	@SuppressWarnings("unchecked")
	private void readInventoryFromFile()
	{
		try {
			inventory = (Map<String, InventoryItem>) new XStream().fromXML(getClass().getClassLoader().getResourceAsStream(
					INVENTORY_FILE_NAME));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to read inventory from file: "
			// + ex, "InventoryError");
			//throw ex;
		}
	}

	private void writeInventoryToFile()
	{
		try {
			new XStream().toXML(inventory, new FileOutputStream(new File(INVENTORY_FILE_NAME)));
		} catch (Exception ex) {
			// Trace.WriteLine("Exception while trying to write inventory to file: "
			// + ex, "InventoryError");
			//throw ex;
		}
	}
}
