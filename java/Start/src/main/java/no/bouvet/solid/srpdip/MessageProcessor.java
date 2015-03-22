package no.bouvet.solid.srpdip;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;

@Path("/")
public class MessageProcessor {

	@GET
	@Produces(MediaType.APPLICATION_JSON)
	public ResponseMessage processMessage(RequestMessage message) {
		return new OrderProcessor().processMessage(message);
	}
}
