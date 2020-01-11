<?php


class Person
{
    public $name = null;
    public $surname = null;
    public $patronymic = null;
    public $age = null;
    public $profession = null;

    public function __construct($name, $surname, $patronymic, $age, $profession)
    {
        $this->name = $name;
        $this->surname = $surname;
        $this->patronymic = $patronymic;
        $this->age = $age;
        $this->profession = $profession;
    }

    /**
     * Convert Person object to string
     * @return string
     */
    public function __toString() : string
    {
        return "name: " . $this->name . "\n"
            . "surname: " . $this->surname . "\n"
            . "patronymic: " . $this->patronymic . "\n"
            . "age: " . $this->age . "\n"
            . "profession: " . $this->profession . "\n";
    }
}