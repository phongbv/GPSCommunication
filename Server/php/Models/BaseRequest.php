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
        if($this->logonInfo != null){
            echo 'Update last online'.PHP_EOL;
            file_get_contents('http://114.78.11.14/api/?act=update&imei='.$this->logonInfo->deviceId);
        }
    }

    function ElementBetween($input, $from, $to){
        $arr = array();
        for($i = $from; $i <= $to; $i++){
            $arr[$i-$from] = $input[$i];
        }
        return $arr;
    }

    function ToHexString($byte_array)
    {
        $result = '';
        
        for($i=0;$i<count($byte_array);$i++)
        {
            $result= $result.''.dechex($byte_array[$i]);
        }
        return $result ;
    }


}


?>