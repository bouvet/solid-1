package no.bouvet.solid.srpdip;

public class PaymentInformation {
	private String cardholderName;
	private String creditCardNumber;
	private String expiryDate;
	private String cardVerificationCode;

	public String getCardholderName() {
		return cardholderName;
	}

	public void setCardholderName(String cardholderName) {
		this.cardholderName = cardholderName;
	}

	public String getCreditCardNumber() {
		return creditCardNumber;
	}

	public void setCreditCardNumber(String creditCardNumber) {
		this.creditCardNumber = creditCardNumber;
	}

	public String getExpiryDate() {
		return expiryDate;
	}

	public void setExpiryDate(String expiryDate) {
		this.expiryDate = expiryDate;
	}

	public String getCardVerificationCode() {
		return cardVerificationCode;
	}

	public void setCardVerificationCode(String cardVerificationCode) {
		this.cardVerificationCode = cardVerificationCode;
	}
}
