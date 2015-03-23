package no.bouvet.solid.srpdip;

import java.util.HashMap;
import java.util.Map;

import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;

public class Deduplicator {

	private Map<Long, ResponseMessage> processedMessages = new HashMap<>();

	public ResponseMessage getExistingResponseMessage(long requestId) {
		return processedMessages.get(requestId);
	}

	public void storeResponseToMessage(long requestId, ResponseMessage response) {
		processedMessages.put(requestId, response);
	}
}
