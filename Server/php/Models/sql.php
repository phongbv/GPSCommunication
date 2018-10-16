 <?php

define('_DB_SERVER', 'WIN-T2JRC8V71J9\SQL2008');
define('_DB_USER', 'sa');
define('_DB_PASS', 'citypost@2018@*#');
define('_DB_NAME', 'qbit');
$connection = sqlsrv_connect( _DB_SERVER, array( "Database"=> _DB_NAME, "UID"=>_DB_USER, "PWD"=>_DB_PASS, "CharacterSet" => "UTF-8"));
if( !$connection ) {
    echo "<center>Connection fail.</center>";
    exit();
}

function insertSqlserver($table, $colum, $data){
    global $connection;
    if (!$table || !$data || !$colum) {
        return false;
    } else {
        $colums = implode(',', $colum);
        $datas  = implode(',', $data);
        $query  = 'INSERT INTO '.$table.'('. $colums .') VALUES('. $datas .')';
        $query  = sqlsrv_query($connection, $query);
        // Debug
        if($query == FALSE){
            die('error: '.FormatErrors(sqlsrv_errors($connection)));
        }else{
            return true;
        }
    }
}

function getGlobalAll($table, $data = '', $option = ''){
    global $connection;
    if($data){
        foreach ($data as $key => $value) {
            $colums[] = '['.$key .'] = '. "N'". $value ."'";
        }
        $colums_list = implode(' AND ', $colums);
    }

    $extra = '';

    if($option['order_by'] && $option['order_by_soft']){
        $extra .= ' ORDER BY '.$option['order_by'].' '. $option['order_by_soft'].' ';
    }

    if($option['limit'] && $option['limit_offset']){
        $extra .= ' LIMIT '.$option['limit'].' OFFSET '.$option['limit_offset'].' ';
    }else if($option['limit'] && !$option['limit_offset']){
        $extra .= ' LIMIT '. $option['limit'] .' ';
    }

    if($option['query']){
        $query = $option['query'];
    }else{
        $query = 'SELECT '. ($option['select'] ? $option['select'] : '*') .' FROM '. $table .' '. (($data) ? 'WHERE '.$colums_list : '') .' '.$extra;
    }

    if($option['onecolum'] && ($option['onecolum'] != 'limit')){
        $q = sqlsrv_query($connection, $query);
        if($q == FALSE){
            die(FormatErrors( sqlsrv_errors()));
        }
        $r = sqlsrv_fetch_array($q,SQLSRV_FETCH_ASSOC);
        return $r[$option['onecolum']];
    }else if($option['onecolum'] == 'limit'){
        $q = sqlsrv_query($connection, $query);
        if($q == FALSE){
            die(FormatErrors( sqlsrv_errors()));
        }
        $r = sqlsrv_fetch_array($q,SQLSRV_FETCH_ASSOC);
        return $r;
    }else{
        $q = sqlsrv_query($connection, $query);
        if($q == FALSE){
            die(FormatErrors( sqlsrv_errors()));
        }
        while($r = sqlsrv_fetch_array($q, SQLSRV_FETCH_ASSOC)){
            $n[] = $r;
        }
        return $n;
    }
}

function updateGlobal($table, $data='', $where = ''){
    global $connection;

    foreach($data as $key => $value){
        $colums[] =  '['.$key ."] = N'". checkInsert($value) ."'";
    }
    $colums_list = implode(',', $colums);

    if($where) {
        foreach ($where as $key_w => $value_w) {
            $colums_w[] =  $key_w . " = '" . checkInsert($value_w) . "'";
        }
        $colums_list_w = ' WHERE '.implode(' AND ', $colums_w);
    }
    if(sqlsrv_query($connection, 'UPDATE '. $table .' SET '.$colums_list.' '.$colums_list_w)){
        return true;
    }else{
        return false;
    }
}

function deleteGlobal($table, $data = ''){
    global $connection;
    foreach ($data as $key => $value) {
        $colums[] = $key . " = " . checkInsert($value);
    }
    $colums_list = implode(' AND ', $colums);
    $q = sqlsrv_query($connection,'DELETE FROM '. $table .' WHERE '.$colums_list);
    if($q){
        return true;
    }else{
        return false;
    }
}

function checkGlobal($table, $data = '', $option = ''){
    global $connection;
    foreach ($data as $key => $value) {
        $colums[] = '['. $key . '] = ' . "'". checkInsert($value) ."'";
    }
    $colums_list = implode(' AND ', $colums);
    if($option['query']){
        $q = sqlsrv_query($connection, $option['query'], array(), array( "Scrollable" => SQLSRV_CURSOR_KEYSET ));
    }else{
        $q = sqlsrv_query($connection,'SELECT * FROM '. $table .' WHERE '.$colums_list,array(), array( "Scrollable" => SQLSRV_CURSOR_KEYSET ));
    }
    $n = sqlsrv_num_rows( $q);
    if($n > 0){
        return $n;
    }else{
        return 0;
    }
}


function FormatErrors( $errors )
{
    /* Display errors. */
    echo "Error information: <br/>";

    foreach ( $errors as $error )
    {
        echo "SQLSTATE: ".$error['SQLSTATE']."<br/>";
        echo "Code: ".$error['code']."<br/>";
        echo "Message: ".$error['message']."<br/>";
    }
}

function checkInsert($text){
    return stripslashes($text);
}


/*
 * Code Insert Demo
 $colum  = array('[detail_imei]','[detail_lat]','[detail_lng]','[detail_speed]','[detail_time]','[detail_last]');
$data   = array("'11111'", "'102'", "'105'", "'67'", "'2018-10-15 08:14:02.000'", "N'2018-10-15 08:14:02.000'");
if(insertSqlserver('qbit_detail', $colum, $data)){
    echo 'Insert Ok';
}
*/
