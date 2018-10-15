<?php
error_reporting(0);
set_time_limit(0);
ob_implicit_flush();

// Define Config Database
define('_DB_SERVER', 'localhost:3306');
define('_DB_USER', 'root');
define('_DB_PASS', 'citypost@2018@*#');
define('_DB_NAME', 'qbit');
define('_IP', '127.0.0.1');
define('_PORT', '3000');

$LoginResponse = array( 0x78, 0x78, 0x05, 0x01, 0x00, 0x05, 0x9F, 0xF8, 0x0D, 0x0A);

function byteStr2byteArray($s) {
    return array_slice(unpack("C*", "\0".$s), 1);
}

//$db_connect = mysqli_connect(_DB_SERVER, _DB_USER, _DB_PASS, _DB_NAME) or die('Cant Connect To Database');
//mysqli_query($db_connect,"SET NAMES 'utf8'");
echo "Site is running\r\n";

if (($sock = socket_create(AF_INET, SOCK_STREAM, SOL_TCP)) === false) {
    echo "Error socket_create(): " . socket_strerror(socket_last_error()).PHP_EOL;
    exit();
}

if (socket_bind($sock, _IP, _PORT) === false) {
    echo "Error socket_bind(): " . socket_strerror(socket_last_error($sock)) . PHP_EOL;
    exit();
}

if (socket_listen($sock, 5) === false) {
    echo "Error socket_listen(): " . socket_strerror(socket_last_error($sock)) . PHP_EOL;
    exit();
}
$IsRunning = true;
//clients array
$clients = array();
//echo "Welcome To Socket".PHP_EOL;
//echo "Socket listening on ". _IP .":". _PORT ."".PHP_EOL;
do {
    $read = array();
    $read[] = $sock;

    $read = array_merge($read,$clients);

    // Set up a blocking call to socket_select
    if(socket_select($read,$write = NULL, $except = NULL, $tv_sec = 5) < 1)
    {
        //    SocketServer::debug("Problem blocking socket_select?");
        continue;
    }

    // Handle new Connections
    if (in_array($sock, $read)) {
        if (($msgsock = socket_accept($sock)) === false) {
            echo "Error socket_accept(): " . socket_strerror(socket_last_error($sock)) . "\n";
            break;
        }
        $clients[] = $msgsock;
        $key = array_keys($clients, $msgsock);
        // /* Enviar instrucciones. */
        // $msg = "Hello You \r\n".PHP_EOL;
        // @socket_write($msgsock, $msg,strlen($msg));
        echo 'New connection'.PHP_EOL;
    }
  
    // Handle Input
    foreach ($clients as $key => $client) { // for each client
        Print_r($client);
        if (in_array($client, $read)) {
            if (false === ($buf = @socket_read($client, 32, PHP_BINARY_READ))) {
                echo "Error socket_read(): " . socket_strerror(socket_last_error($client)) . "\n".$buf;
                break 2;
            }
            $talkback = "Client {$key}: Said '$buf'".PHP_EOL;
            //socket_write($client, $talkback, strlen($talkback));
            // Sent to All Connect ....5
            foreach ($clients as $key_c => $client_c) {
                echo $client_c;
                socket_write($client_c, $talkback, strlen($talkback));
            }
            
            $byte_array = byteStr2byteArray($buf);
            $requestType = $byte_array[3];
            echo "Connection Type: ".$requestType;
            // for($i=0;$i<count($byte_array);$i++)
            // {
            //     printf("0x%02x ", $byte_array[$i]);
            // }
            $IsRunning = false;
        }
    }
} while ($IsRunning);

socket_close($sock);
?>