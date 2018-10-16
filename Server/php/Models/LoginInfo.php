<?php

class LoginInfo extends BaseRequest{


    public $deviceId;

    function __construct($data){
        parent::__construct(null, $data);
        $this->deviceId =  implode($this->ElementBetween($this->informationContent, 0, 7));
    }

    public function InitContent()
    {
        $this->informationContent = $this->ElementBetween($this->requestContent, 4, 24);
    }
    public function GetResponse()
    {
       return array( 0x78, 0x78, 0x05, 0x01, 0x00, 0x05, 0x9F, 0xF8, 0x0D, 0x0A);
    }

}

?>