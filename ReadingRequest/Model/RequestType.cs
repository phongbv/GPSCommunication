using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest
{
    public enum RequestType
    {
        LoginInformation = 0x01,
        GPSPositionData = 0x22,
        HeartBeatPacket = 0x23,
        WifiCommunicationProtocol = 0x2C,
        TimeCheckPacket = 0x8A
    }
}
