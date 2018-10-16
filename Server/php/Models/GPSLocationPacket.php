<?php

class GPSLocationPacket extends BaseRequest{
    

    public $dateTime;

    public $speed;

    public $latitus;

    public $longtitus;


    function __construct($logonInfo, $data){
        parent::__construct($logonInfo, $data);
        
        
        $this->dateTime = new DateTime();
        $this->dateTime->setDate($this->informationContent[0] + 2000, $this->informationContent[1], $this->informationContent[2]);
        $this->dateTime->setTime($this->informationContent[3], $this->informationContent[4], $this->informationContent[5]);

        $this->speed = $this->informationContent[15];
        
       $this->latitus = hexdec ( $this->ToHexString($this->ElementBetween($this->informationContent, 7, 10))) / 1800000;
       $this->longtitus = hexdec ( $this->ToHexString($this->ElementBetween($this->informationContent, 11, 14)))/ 1800000;

    }

    
    public function InitContent()
    {
        $this->informationContent = $this->ElementBetween($this->requestContent, 4, 24);
    }

    public function DoProcessRequest()
    {
        parent::DoProcessRequest();
        echo $this->GetInsertScript();
    }

    public function ToString()
    {
        return "IMEI " .$this->logonInfo->deviceId.' has speed '.$this->speed.'<br/>';
    }

    public function GetInsertScript()
    {
       return ' INSERT INTO [qbit].[dbo].[qbit_detail]
           ([detail_imei]
           ,[detail_lat]
           ,[detail_lng]
           ,[detail_speed]
           ,[detail_time]
           ,[detail_last])
     VALUES
           (\''. $this->logonInfo->deviceId .'\'
           ,'. $this->latitus .'
           ,'. $this->longtitus .'
           ,'. $this->speed .'
           ,\''. $this->dateTime->format('Y-m-d H:i:s'). '\'
           ,\''. $this->dateTime->format('Y-m-d H:i:s'). '\')';


    }

}
?>