package no.bouvet.solid.srpdip.domain;

public class ContactInformation {
	private String givenName;
	private String surname;
	private String email;
	private String phoneNumber;
	private boolean allowNotificationBySms;

	public String getGivenName() {
		return givenName;
	}

	public void setGivenName(String givenName) {
		this.givenName = givenName;
	}

	public String getSurname() {
		return surname;
	}

	public void setSurname(String surname) {
		this.surname = surname;
	}

	public String getEmail() {
		return email;
	}

	public void setEmail(String email) {
		this.email = email;
	}

	public String getPhoneNumber() {
		return phoneNumber;
	}

	public void setPhoneNumber(String phoneNumber) {
		this.phoneNumber = phoneNumber;
	}

	public boolean getAllowNotificationBySms() {
		return allowNotificationBySms;
	}

	public void setAllowNotificationBySms(boolean allowNotificationBySms) {
		this.allowNotificationBySms = allowNotificationBySms;
	}
}
