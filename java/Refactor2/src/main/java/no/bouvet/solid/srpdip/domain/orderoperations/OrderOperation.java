package no.bouvet.solid.srpdip.domain.orderoperations;

import no.bouvet.solid.srpdip.messageinterface.RequestMessage;
import no.bouvet.solid.srpdip.messageinterface.ResponseMessage;

public interface OrderOperation {

	public abstract ResponseMessage execute(RequestMessage request);

}