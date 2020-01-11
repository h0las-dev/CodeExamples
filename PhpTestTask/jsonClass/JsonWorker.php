<?php

require_once('IJsonWorker.php');

class JsonWorker implements IJsonWorker
{
    /**
     * Stores unsaved records
     * @var Person[]
     */
    private $persons = [];

    /**
     * Filename
     * @var string
     */
    private $fileName = "";

    /**
     * Options for records filtering
     * @var array
     */
    private $filterOptions = [];

    /**
     * Constructor
     * @param string $fileName
     */
    public function __construct($fileName="")
    {
        $this->fileName = $fileName;

        if ($fileName !== "")
        {
            $this->setFile($fileName);
        }
    }

    /**
     * Return list of a persons
     * @return Person[]
     * @throws Exception
     */
    public function getAll() : array
    {
        $personsFromFile = $this->getFileData();
        $unsavedPersons = $this->persons;

        $data = array_merge($personsFromFile, $unsavedPersons);
        return $data;
    }

    /**
     * Add Person to unsaved persons list
     * @param Person $person
     */
    public function add(Person $person) : void
    {
        array_push($this->persons, $person);
    }

    /**
     * Remove Person by options from json file and unsaved persons list
     * @param array $options
     * @throws Exception
     */
    public function removeByFilter($options) : void
    {
        $this->filterOptions = $options;

        $personsFromFile = $this->getFileData();
        $unsavedPersons = $this->persons;

        $data = array_merge($personsFromFile, $unsavedPersons);
        $filterData = array_filter($data, array($this, "filterCallback"));
        $this->clearAll();
        $this->persons = $filterData;
        $this->saveAll();
    }

    /**
     * Delete all records from file and unsaved persons list
     * @throws Exception
     */
    public function clearAll() : void
    {
        if ($this->fileNotSet())
        {
            throw new Exception("file not set");
        }

        file_put_contents($this->fileName, "");
        $this->persons = array();
    }

    /**
     * Save all records from unsaved persons list to file
     * @throws Exception
     */
    public function saveAll() : void
    {
        if ($this->fileNotSet())
        {
            throw new Exception("file not set");
        }

        $personsFromFile = $this->getFileData();
        $data = array_merge($personsFromFile, $this->persons);
        $jsonData = json_encode($data);
        file_put_contents($this->fileName, $jsonData);

        $this->persons = array();
    }

    /**
     * Set filename for json file directly
     * @param string $fileName
     */
    public function setFile($fileName) : void
    {
        $this->fileName = $fileName;

        if (!file_exists($fileName) || (filesize($fileName) == 0))
        {
            file_put_contents($fileName, "");
        }
    }

    /**
     * Echo all records from file and unsaved list to console
     * @throws Exception
     */
    public function showAll() : void
    {
        $personsFromFile = $this->getFileData();
        echo "persons from file:\n\n";
        $this->showPersons($personsFromFile);

        echo "unsaved persons:\n\n";
        if(empty($this->persons))
        {
            echo "- empty - \n\n";
            return;
        }

        $this->showPersons($this->persons);
    }

    /**
     * Callback for records filtering
     * @param Person $person
     * @return bool
     */
    private function filterCallback($person) : bool
    {
        $filter = true;
        if (array_key_exists( "name", $this->filterOptions))
        {
            $name = $this->filterOptions["name"];
            $filter = $filter && $person->name === $name;
        }
        if (array_key_exists( "surname", $this->filterOptions))
        {
            $surname = $this->filterOptions["surname"];
            $filter = $filter && $person->surname === $surname;
        }
        if (array_key_exists( "patronymic", $this->filterOptions))
        {
            $patronymic = $this->filterOptions["patronymic"];
            $filter = $filter && $person->patronymic === $patronymic;
        }
        if (array_key_exists( "age", $this->filterOptions))
        {
            $age = $this->filterOptions["age"];
            $filter = $filter && $person->age === $age;
        }
        if (array_key_exists( "profession", $this->filterOptions))
        {
            $profession = $this->filterOptions["profession"];
            $filter = $filter && $person->profession === $profession;
        }

        return !$filter;
    }

    /**
     * Check if file not set in the class
     * @return bool
     */
    private function fileNotSet() : bool
    {
        if ($this->fileName === "")
        {
            return true;
        }

        return false;
    }

    /**
     * Get data from json file
     * @return Person[]
     * @throws Exception
     */
    private function getFileData() : array
    {
        if ($this->fileNotSet())
        {
            throw new Exception("file not set");
        }

        $jsonString = file_get_contents($this->fileName);

        if ($jsonString === false)
        {
            throw new Exception("can't get a file content from current fileName: " . $this->fileName);
        }

        $jsonArray = json_decode($jsonString, true);

        $personsFromFile = [];

        if ($jsonArray === null)
        {
            // if $jsonString == "" then file is empty and it may be correct
            if ($jsonString != "")
            {
                throw new Exception("can't convert a json string from file: " . $this->fileName);
            }
        }
        else
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

    /**
     * Echo persons list
     * @param Person[] $persons
     */
    private function showPersons($persons) : void
    {
        foreach ($persons as $key => $value)
        {
            echo $value . "\n";
        }
    }
}