<?php


interface IJsonWorker
{
    public function getAll();
    public function add(Person $person);
    public function removeByFilter($options);
    public function clearAll();
    public function saveAll();
    public function showAll();
}