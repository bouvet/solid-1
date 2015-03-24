package no.bouvet.solid.srpdip.messageinterface;

import javax.ws.rs.core.MediaType;
import javax.ws.rs.core.Response;
import javax.ws.rs.ext.ExceptionMapper;
import javax.ws.rs.ext.Provider;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@Provider
public class OrderExceptionMapper implements ExceptionMapper<Exception> {

	private static final Logger LOG = LoggerFactory.getLogger(OrderExceptionMapper.class);

	@Override
	public Response toResponse(Exception e) {
		LOG.error("An error occurred: {}", e.getMessage());

		Response.Status status = Response.Status.INTERNAL_SERVER_ERROR;

		return Response.status(status).type(MediaType.TEXT_PLAIN).entity(e.getMessage()).build();
	}
}
