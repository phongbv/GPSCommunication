<?php

class BaseRequest{

    protected $requestContent;

    protected $informationContent;

    protected $logonInfo;

    function __construct($logonInfo, $data ){
        $this->requestContent = $data;
        $this->logonInfo = $logonInfo;
        $this-> InitContent();
       
    }

    function InitContent(){

    }

    public function GetResponse()
    {
        return null;
    }

    public function DoProcessRequest()
    {
        echo 'Do Process Request base class<br/>';
    }

    function ElementBetween($input, $from, $to){
        $arr = array();
        for($i = $from; $i <= $to; $i++){
            $arr[$i-$from] = $input[$i];
        }
        return $arr;
    }

}

class LoginInfo extends BaseRequest{

    function __construct($data){
        parent::__construct(null, $data);
    }


    public function GetResponse()
    {
       return array( 0x78, 0x78, 0x05, 0x01, 0x00, 0x05, 0x9F, 0xF8, 0x0D, 0x0A);
    }

}


class GPSLocationPacket extends BaseRequest{
    

    public $dateTime;

    public $speed;


    function __construct($logonInfo, $data){
        parent::__construct($logonInfo, $data);
        echo 'Initialize GPSLocationPacket<br/>';
        $this->dateTime = new DateTime();
        $this->dateTime->setDate($this->informationContent[0] + 2000, $this->informationContent[1], $this->informationContent[2]);
        $this->dateTime->setTime($this->informationContent[3], $this->informationContent[4], $this->informationContent[5]);

        
    }

    public function InitContent()
    {
        echo 'GPSLocationPacket InitContent<br/>';
        $this->informationContent = $this->ElementBetween($this->requestContent, 4, 24);
    }

    public function DoProcessRequest()
    {
        echo 'GPSLocationPacket: DoProcessRequest<br/>';
        parent::DoProcessRequest();
        echo 'GPSLocationPacket: DoProcessRequest<br/>';
    }

}


?>