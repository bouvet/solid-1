package no.bouvet.solid.srpdip;

import javax.ws.rs.Consumes;
import javax.ws.rs.POST;
import javax.ws.rs.Path;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;

@Path("/")
public class MessageProcessor {

	@POST
	@Consumes(MediaType.APPLICATION_JSON)
	@Produces(MediaType.APPLICATION_JSON)
	public ResponseMessage processMessage(RequestMessage message) {
		try {
			return new OrderProcessor().processMessage(message);
		} catch (Exception e) {
			e.printStackTrace();
			return null;
		}
	}
}
