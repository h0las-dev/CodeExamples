package parser;

public class CityStatistics {
    public String city;
    private int[] houses;

    public CityStatistics(String city) {
        this.city = city;
        this.houses = new int[] { 0, 0, 0, 0, 0 };
    }

    public int[] getHouses() {
        return this.houses;
    }

    public void incHouses(int floor) {
        houses[floor - 1]++;
    }

    @Override
    public String toString() {
        var res = new StringBuilder(this.city + " :\n");

        for (var floor = 0; floor < this.houses.length; floor++) {
            res.append(String.format(" %d этаж: %d шт.\n", floor + 1, this.houses[floor]));
        }

        return res.toString();
    }
}
