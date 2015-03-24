package no.bouvet.solid.srpdip.messageinterface;

import java.util.List;

import javax.xml.bind.annotation.XmlRootElement;

import no.bouvet.solid.srpdip.domain.Address;
import no.bouvet.solid.srpdip.domain.ContactInformation;
import no.bouvet.solid.srpdip.domain.OrderItem;

@XmlRootElement
public class RequestMessage {
	private long requestId;
	private Operation operation;
	private long accountId;
	private long orderId;
	private List<OrderItem> orderItems;
	private Address shippingAddress;
	private Address billingAddress;
	private ContactInformation contactInformation;
	private String contactEmail;

	public long getRequestId() {
		return requestId;
	}

	public void setRequestId(long requestId) {
		this.requestId = requestId;
	}

	public Operation getOperation() {
		return operation;
	}

	public void setOperation(Operation operation) {
		this.operation = operation;
	}

	public long getAccountId() {
		return accountId;
	}

	public void setAccountId(long accountId) {
		this.accountId = accountId;
	}

	public long getOrderId() {
		return orderId;
	}

	public void setOrderId(long orderId) {
		this.orderId = orderId;
	}

	public List<OrderItem> getOrderItems() {
		return orderItems;
	}

	public void setOrderItems(List<OrderItem> orderItems) {
		this.orderItems = orderItems;
	}

	public Address getShippingAddress() {
		return shippingAddress;
	}

	public void setShippingAddress(Address shippingAddress) {
		this.shippingAddress = shippingAddress;
	}

	public Address getBillingAddress() {
		return billingAddress;
	}

	public void setBillingAddress(Address billingAddress) {
		this.billingAddress = billingAddress;
	}

	public ContactInformation getContactInformation() {
		return contactInformation;
	}

	public void setContactInformation(ContactInformation contactInformation) {
		this.contactInformation = contactInformation;
	}

	public String getContactEmail() {
		return contactEmail;
	}

	public void setContactEmail(String contactEmail) {
		this.contactEmail = contactEmail;
	}
}
