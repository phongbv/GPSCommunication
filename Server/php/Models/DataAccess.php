<?php


class DataAccess{

    private $connection;

    public function __construct()
    {
        $this-> $connection = sqlsrv_connect( "112.78.11.14", array( "Database"=> 'qbit', "UID"=> 'sa', "PWD"=>'citypost@2018@*#', "CharacterSet" => "UTF-8"));
    }


}


?>