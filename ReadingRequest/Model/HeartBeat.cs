using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public class HeartBeat : BaseRequest
    {
        public override byte[] Response => new byte[] { 0x78, 0x78, 0x05, 0x23, 0x01, 0x00, 0x67, 0x0E, 0x0D, 0x0A };

        /// <summary>
        /// Data length(6): from 4 to 9
        /// </summary>
        /// <param name="data"></param>
        public HeartBeat(LoginPacket connectionInfo, byte[] data) : base(connectionInfo, data)
        {
            _informationContent = data.ElementBetween(4, 9);
        }

        public byte[] VoltageLevel { get; set; }
        public GSMSignal GSMSignal => (GSMSignal)_rawData[3];

        protected override byte ContentLength
        {
            get
            {
                return 6;
            }
        }
    }

    public enum GSMSignal
    {
        NoSignal = 0x00,
        ExtremelyWeakSignal = 0x01,
        VeryWeakSignal = 0x02,
        GoodSignal = 0x03,
        StrongSignal = 0x04
    }
}
