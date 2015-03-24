package no.bouvet.solid.srpdip;

import no.bouvet.solid.srpdip.domain.orderoperations.OperationFactory;
import no.bouvet.solid.srpdip.domain.orderoperations.OrderOperation;
import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;

public class OrderProcessor {

	private Deduplicator deduplicator = Deduplicator.factory.getInstance();
	private OperationFactory operationFactory = OperationFactory.factory.getInstance();

	public ResponseMessage processMessage(RequestMessage reqMsg) {
		ResponseMessage responseToDuplicateMessage = deduplicator.getExistingResponseMessage(reqMsg.getRequestId());

		if (responseToDuplicateMessage != null) {
			return responseToDuplicateMessage;
		}

		OrderOperation operation = operationFactory.getInstance(reqMsg.getOperation());
		ResponseMessage response = operation.execute(reqMsg);

		deduplicator.storeResponseToMessage(reqMsg.getRequestId(), response);

		return response;
	}
}
