package parser;

import junit.framework.TestCase;
import org.junit.Test;
import org.xml.sax.InputSource;
import org.xml.sax.SAXException;

import javax.xml.parsers.SAXParser;
import javax.xml.parsers.SAXParserFactory;

import java.io.IOException;
import java.io.StringReader;

import static org.junit.Assert.*;

public class AddressParserTest extends TestCase {
    private SAXParserFactory parserFactory;
    private SAXParser parser;
    private AddressHandler addressHandler;

    @Override
    protected void setUp() throws Exception {
        super.setUp();
        addressHandler = new AddressHandler();
        this.parserFactory = SAXParserFactory.newInstance();
        this.parser = this.parserFactory.newSAXParser();
    }

    @Test
    public void testEmptyXmlString() {
        var xmlString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        xmlString.append("<root>");
        xmlString.append("</root>");

        try {
            parser.parse(new InputSource(new StringReader(xmlString.toString())), addressHandler);
        } catch (SAXException | IOException e) {
            e.printStackTrace();
        }

        assertEquals(0, addressHandler.getDuplicateEntries().size());
        assertEquals(0, addressHandler.getCities().size());
    }

    @Test
    public void testOnlyUniqueElementsXmlString() {
        var xmlString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        xmlString.append("<root>");
        xmlString.append("<item city=\"Буденновск\" street=\"Северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("<item city=\"Буденновск\" street=\"Южная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("</root>");

        try {
            parser.parse(new InputSource(new StringReader(xmlString.toString())), addressHandler);
        } catch (SAXException | IOException e) {
            e.printStackTrace();
        }

        assertEquals(0, addressHandler.getDuplicateEntries().size());
        assertEquals(1, addressHandler.getCities().size());
        assertArrayEquals(new int [] {2,0,0,0,0}, addressHandler.getCities().get("буденновск").getHouses());
    }

    @Test
    public void testCompletelyMatchingElementsXmlString() {
        var xmlString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        xmlString.append("<root>");
        xmlString.append("<item city=\"Буденновск\" street=\"Северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("<item city=\"Буденновск\" street=\"Северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("</root>");

        try {
            parser.parse(new InputSource(new StringReader(xmlString.toString())), addressHandler);
        } catch (SAXException | IOException e) {
            e.printStackTrace();
        }

        assertEquals(1, addressHandler.getDuplicateEntries().size());
        assertEquals(1, addressHandler.getCities().size());
        assertArrayEquals(new int [] {1,0,0,0,0}, addressHandler.getCities().get("буденновск").getHouses());
    }

    @Test
    public void testDuplicateButDifferentCaseElementsXmlString() {
        var xmlString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        xmlString.append("<root>");
        xmlString.append("<item city=\"Буденновск\" street=\"северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("<item city=\"Буденновск\" street=\"Северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("</root>");

        try {
            parser.parse(new InputSource(new StringReader(xmlString.toString())), addressHandler);
        } catch (SAXException | IOException e) {
            e.printStackTrace();
        }

        assertEquals(1, addressHandler.getDuplicateEntries().size());
        assertEquals(1, addressHandler.getCities().size());
        assertArrayEquals(new int [] {1,0,0,0,0}, addressHandler.getCities().get("буденновск").getHouses());
    }

    @Test
    public void testUniqueAndDuplicateElementsXmlString() {
        var xmlString = new StringBuilder("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
        xmlString.append("<root>");
        xmlString.append("<item city=\"Буденновск\" street=\"северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("<item city=\"Буденновск\" street=\"Северная улица\" house=\"21\" floor=\"1\" />");
        xmlString.append("<item city=\"Саранск\" street=\"Профсоюзная улица\" house=\"26\" floor=\"1\" />");
        xmlString.append("</root>");

        try {
            parser.parse(new InputSource(new StringReader(xmlString.toString())), addressHandler);
        } catch (SAXException | IOException e) {
            e.printStackTrace();
        }

        assertEquals(1, addressHandler.getDuplicateEntries().size());
        assertEquals(2, addressHandler.getCities().size());
        assertArrayEquals(new int [] {1,0,0,0,0}, addressHandler.getCities().get("буденновск").getHouses());
        assertArrayEquals(new int [] {1,0,0,0,0}, addressHandler.getCities().get("саранск").getHouses());
    }
}
