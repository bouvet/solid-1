package no.bouvet.solid.srpdip;

import java.io.File;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.OutputStream;
import java.util.Map;

import no.bouvet.solid.srpdip.domain.InventoryItem;

import com.thoughtworks.xstream.XStream;

public class InventoryRepository {

	public static Factory<InventoryRepository> factory = new Factory<>(InventoryRepository.class);

	private static final String INVENTORY_FILE_NAME = "data/inventoryFile.xml";

	private Map<String, InventoryItem> inventory;

	public InventoryRepository() {
		readInventoryFromFile();
	}
	
	public InventoryItem getInventoryItem(String code) {
		return inventory.get(code);
	}

	public void updateInventory() {
		writeInventoryToFile();
	}

	@SuppressWarnings("unchecked")
	private void readInventoryFromFile() {
		inventory = (Map<String, InventoryItem>) new XStream().fromXML(new File(INVENTORY_FILE_NAME));
	}

	private void writeInventoryToFile() {
		try (OutputStream stream = new FileOutputStream(INVENTORY_FILE_NAME)) {
			new XStream().toXML(inventory, stream);
		} catch (IOException e) {
			throw new RuntimeException("Failed to persist inventory: " + e.getMessage());
		}
	}
}
