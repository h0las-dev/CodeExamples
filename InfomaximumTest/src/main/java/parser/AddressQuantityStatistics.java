package parser;

public class AddressQuantityStatistics {
    private int count;
    public Address address;

    public AddressQuantityStatistics(Address address)
    {
        this.address = address;
        this.count = 1;
    }

    public int getCount() {
        return count;
    }    

    public void incCount() {
        this.count++;
    }

    @Override
    public String toString() {
        return address.toString() + " : " + this.count;
    }
}
