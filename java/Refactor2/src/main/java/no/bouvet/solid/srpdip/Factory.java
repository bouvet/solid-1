package no.bouvet.solid.srpdip;

public class Factory<TYPE> {

	protected TYPE instance;
	protected Class<TYPE> clazz;

	public Factory(TYPE instance) {
		this.instance = instance;
	}

	public Factory(Class<TYPE> clazz) {
		this.clazz = clazz;
	}

	public void setInstance(TYPE instance) {
		this.instance = instance;
	}

	public TYPE getInstance() {
		if (instance != null) {
			return instance;
		}
		try {
			return clazz.newInstance();
		} catch (InstantiationException | IllegalAccessException e) {
			throw new RuntimeException(e.getMessage(), e);
		}
	}
}