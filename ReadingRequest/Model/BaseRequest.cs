using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public abstract class BaseRequest
    {
        protected const string URL = "http://112.78.11.14/api";
        protected byte[] _rawData;
        protected byte[] _informationContent;
        protected abstract byte ContentLength { get; }
        public virtual byte[] Response { get; }
        protected LoginPacket clientConnectionInfor;
        public BaseRequest(byte[] data)
        {
            _rawData = data;
        }
        public BaseRequest(LoginPacket connectionInfo, byte[] data)
        {
            _rawData = data;
            clientConnectionInfor = connectionInfo;
        }

        protected byte[] _startBit => _rawData.ElementBetween(0, 1);
        public string StartBit
        {
            get
            {
                return _startBit.ToHexString();
            }
        }

        public byte PackageLength
        {
            get
            {
                return _rawData[2];
            }
        }

        public IInformationContent InformationContent { get; private set; }

        public byte[] InformationSerialNumber { get { return _rawData.ElementBetween(4 + ContentLength, 4 + ContentLength + 1); } }
        public byte[] ErrorCheck { get { return _rawData.ElementBetween(4 + ContentLength + 2, 4 + ContentLength + 3); } }
        public byte[] StopBit { get { return _rawData.ElementBetween(4 + ContentLength + 4, 4 + ContentLength + 5); } }


        public virtual void DoProcessRequest()
        {
            HttpUtils.SendPost(URL, "imei=" + clientConnectionInfor.TerminalId);
        }
    }
}
