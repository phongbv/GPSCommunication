<?php

class sql{
    public $connect;
    function __construct($server, $user, $pass, $dbname){
        $this->connect = sqlsrv_connect( $server, array( "Database"=> $dbname, "UID"=>$user, "PWD"=>$pass, "CharacterSet" => "UTF-8"));
        if( !$this->connect) {
            echo 'Connect fail';
            exit();
        }
    }

    public function db_query($query){
        if (!$query) {
            return false;
        }
        $query  = sqlsrv_query($this->connect, $query);
        // Debug
        if(!$query){
            foreach (sqlsrv_errors($this->connect) AS $error){
                echo "SQLSTATE: ".$error['SQLSTATE']."<br/>";
                echo "Code: ".$error['code']."<br/>";
                echo "Message: ".$error['message']."<br/>";
            }
        }
    }

}

/*$db = new sql('WIN-T2JRC8V71J9\SQL2008','sa','citypost@2018@*#','qbit');
$query = 'INSERT INTO qbit_detail(detail_imei,detail_lat,detail_lng,detail_speed,detail_time,detail_last) VALUES ("11111", "102", "105", "67", "2018-10-15 08:14:02.000", "2018-10-15 08:14:02.000")';
if($db->db_insert('qbit_detail', $query)){
    echo "Insert OK";
}else{
    echo "Insert fail";
}*/


