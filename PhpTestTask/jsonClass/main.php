<?php

require_once('Person.php');
require_once('JsonWorker.php');

try {
    $person = new Person("Nikita", "Kuzmin", "Aleksandrovich", 22, "Student");
    $file = "file.json";

    $jsonWorker = new JsonWorker($file);
    echo count($jsonWorker->getAll());
    // or $jsonWorker = new JsonWorker();
    //$jsonWorker->setFile($file);

    /* $jsonWorker->add($person);

    $jsonWorker->saveAll();

    $person = new Person("Ivan", "Ivanov", "Ivanovich", 30, "PHP developer");

    $jsonWorker->add($person);

    $jsonWorker->saveAll();

    $jsonWorker->showAll();

    $removeOptions = array(
        "name" => "Nikita"
    );

    $jsonWorker->removeByFilter($removeOptions);

    $jsonWorker->showAll();*/
}
catch (Exception $e) {
    echo "Error: " . $e->getMessage();
}