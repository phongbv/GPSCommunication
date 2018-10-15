<?php

class ClientInformation{
    const BUFFER_SIZE = 128;

    private $isConnected = false;

    private $clientSocket;

    private $clientInfo ;

    private $currentRequest;

    public $isRunning;

    public function __construct($clientSocket)
    {
        # code...
        $this->clientSocket = $clientSocket;

    }


    public function ProcessingRequest()
    {
        $requestContent = array();
        while(!$this->isConnected){
           $requestContent = $this-> ConvertStreamToArray(socket_read($this->clientSocket, 128, PHP_BINARY_READ));
           $requestType = $requestContent[3];

           if($requestType == 1){
                echo 'Client is connected<br/>';
                $this->isConnected = true;
                $this->clientInfo = new LoginInfo($requestContent);
           }
           
        }
        $response = array( 0x78, 0x78, 0x05, 0x01, 0x00, 0x05, 0x9F, 0xF8, 0x0D, 0x0A);
        socket_write($this->clientSocket, implode("",$response),10);
        
        $this->isRunning = false;
        $dateTime = new DateTime();
        while($this->isConnected){
            $requestContent =  $this->ConvertStreamToArray(socket_read($this->clientSocket, BUFFER_SIZE));
            $this->InitRequest($requestContent);
            $this->currentRequest.DoProcessRequest();
        }
        
        # code...
    }

    private function InitRequest($requestContent)
    {

        switch ($requestContent[3]){
            case 0x22: 
                $this->currentRequest = new GPSLocationPacket($this->clientInfo, $requestContent);
                break;
            default:
                break;
        }

    }

    private function ConvertStreamToArray($content){
        $result =  array_slice(unpack("C*", "\0".$content), 1);
        // for($i=0;$i<count($result);$i++)
        // {
        //     printf("0x%02x ", $result[$i]);
        // }

        return $result;
    }
}

?>