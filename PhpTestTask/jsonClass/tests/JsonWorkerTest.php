<?php


require_once('Person.php');
require_once('JsonWorker.php');

use PHPUnit\Framework\TestCase;

class JsonWorkerTest extends TestCase
{
    public $file = "tests/test.json";

    protected function setUp(): void
    {
        file_put_contents($this->file, "");
    }

    public function testGetAll()
    {
        $jsonWorker = new JsonWorker($this->file);
        $jsonWorkerPersonsProp = $this->getPrivateProperty('JsonWorker', 'persons');

        // check only unsaved data
        $persons = [];
        $person1 = new Person("Nikita", "Kuzmin", "Aleksandrovich", 22, "Student");
        $person2 = new Person("Ivan", "Ivanov", "Ivanovich", 30, "PHP developer");
        array_push($persons, $person1, $person2);
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);

        $this->assertEquals(count($persons), count($jsonWorker->getAll()));

        // check unsaved data and data from file
        $this->generateTestDataFile();
        $persons = [];
        $person3 = new Person("Oleg", "Olegov", "Olegovich", 45, "Engineer");
        array_push($persons, $person3);
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);
        $resData = array_merge($persons, $this->getDataFromTestFile());

        $this->assertEquals(count($resData), count($jsonWorker->getAll()));

        // check if file data and unsaved data are empty
        $persons = [];
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);
        file_put_contents($this->file, "");

        $this->assertEquals(0, count($jsonWorker->getAll()));
    }

    public function testRemoveByFilter()
    {
        $jsonWorker = new JsonWorker($this->file);
        $jsonWorkerPersonsProp = $this->getPrivateProperty('JsonWorker', 'persons');

        // check only unsaved data
        $removeOptions = array(
            "name" => "Nikita",
            "age" => 22
        );

        $personForAdd = new Person("Nikita", "Kuzmin", "Aleksandrovich", 22, "Student");
        $persons = [];
        array_push($persons, $personForAdd);
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);
        $jsonWorker->removeByFilter($removeOptions);
        $this->assertEquals(0, count($jsonWorkerPersonsProp->getValue($jsonWorker)));

        // check unsaved data and test file data, only one value should be deleted(unsaved)
        $this->generateTestDataFile();
        $savedData = $this->getDataFromTestFile();
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);
        $jsonWorker->removeByFilter($removeOptions);
        $this->assertEquals($savedData, $this->getDataFromTestFile());
    }

    public function testSetFile()
    {
        $jsonWorker = new JsonWorker();
        $jsonWorkerFileNameProp = $this->getPrivateProperty('JsonWorker', 'fileName');
        $jsonWorker->setFile($this->file);
        $this->assertEquals($this->file, $jsonWorkerFileNameProp->getValue($jsonWorker));
    }

    public function testSaveAll()
    {
        $jsonWorker = new JsonWorker();
        $jsonWorkerPersonsProp = $this->getPrivateProperty('JsonWorker', 'persons');
        $jsonWorker->setFile($this->file);

        $personForAdd = new Person("Nikita", "Kuzmin", "Aleksandrovich", 22, "Student");
        $persons = [];
        array_push($persons, $personForAdd);
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);
        $jsonWorker->saveAll();

        $this->assertEquals($persons, $this->getDataFromTestFile());
    }

    public function testAdd()
    {
        file_put_contents($this->file, "");
        $jsonWorker = new JsonWorker($this->file);
        $jsonWorkerPersonsProp = $this->getPrivateProperty('JsonWorker', 'persons');

        $personForAdd = new Person("Nikita", "Kuzmin", "Aleksandrovich", 22, "Student");
        $persons = [];
        array_push($persons, $personForAdd);
        $jsonWorker->add($personForAdd);

        $this->assertEquals($persons, $jsonWorkerPersonsProp->getValue($jsonWorker));
    }

    public function testClearAll()
    {
        $jsonWorker = new JsonWorker($this->file);
        $jsonWorkerPersonsProp = $this->getPrivateProperty('JsonWorker', 'persons');

        $this->generateTestDataFile();
        $persons = [];
        $person = new Person("Oleg", "Olegov", "Olegovich", 45, "Engineer");
        array_push($persons, $person);
        $jsonWorkerPersonsProp->setValue($jsonWorker, $persons);

        $jsonWorker->clearAll();

        $personsFromFileShouldBeZero = $this->getDataFromTestFile();
        $data = array_merge($jsonWorkerPersonsProp->getValue($jsonWorker), $personsFromFileShouldBeZero);

        $this->assertEquals(0, count($data));
    }

    private function getPrivateProperty($className, $propertyName)
    {
        $reflector = new ReflectionClass($className);
        $property = $reflector->getProperty($propertyName);
        $property->setAccessible(true);

        return $property;
    }

    private function generateTestDataFile()
    {
        file_put_contents($this->file, "");

        $persons = [];
        $person1 = new Person("Nikita", "Kuzmin", "Aleksandrovich", 26, "DevOps");
        $person2 = new Person("Nikita", "Kuzmin", "Aleksandrovich", 25, "Manager");
        $person3 = new Person("Ivan", "Ivanov", "Ivanovich", 30, "PHP developer");
        array_push($persons, $person1, $person2, $person3);
        $jsonData = json_encode($persons);
        file_put_contents($this->file, $jsonData);
    }

    private function getDataFromTestFile()
    {
        $jsonString = file_get_contents($this->file);
        $jsonArray = json_decode($jsonString, true);

        $personsFromFile = [];

        if ($jsonArray !== null)
        {
            foreach ($jsonArray as $key => $value)
            {
                $person = new Person($value["name"], $value["surname"], $value["patronymic"],
                    $value["age"], $value["profession"]);

                array_push($personsFromFile, $person);
            }
        }

        return $personsFromFile;
    }
}
