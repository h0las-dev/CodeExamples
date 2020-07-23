package parser;

import org.xml.sax.SAXException;

import javax.xml.parsers.ParserConfigurationException;
import javax.xml.parsers.SAXParserFactory;
import java.io.FileWriter;
import java.io.IOException;

public class Main {
    public static void main(String[] args) {
        long m = System.currentTimeMillis();
        var saxParserFactory = SAXParserFactory.newInstance();
        try {
            var saxParser = saxParserFactory.newSAXParser();
            var addressHandler = new AddressHandler();
            ClassLoader classLoader = Main.class.getClassLoader();
            saxParser.parse(classLoader.getResourceAsStream("address.xml"), addressHandler);

            var duplicateEntries = addressHandler.getDuplicateEntries();
            var cities = addressHandler.getCities();

            try(var writer = new FileWriter("duplicates.txt", false))
            {
                for (var entry : duplicateEntries.entrySet()) {
                    writer.write(entry.getValue().toString() + "\n");
                }

                writer.flush();
            }
            try(var writer = new FileWriter("cities.txt", false))
            {
                for (var entry : cities.entrySet()) {
                    writer.write(entry.getValue().toString() + "\n");
                }

                writer.flush();
            }
        } catch (ParserConfigurationException | SAXException | IOException e) {
            e.printStackTrace();
        }

        System.out.println("TASK COMPLETED.");
        System.out.println(((double) (System.currentTimeMillis() - m)) / 1000);
    }
}
