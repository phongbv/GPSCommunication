using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public class LoginPacket : BaseRequest
    {
        public override byte[] Response => new byte[] { 0x78, 0x78, 0x05, 0x01, 0x00, 0x05, 0x9F, 0xF8, 0x0D, 0x0A };
        public LoginPacket(byte[] data) : base(data)
        {
            _informationContent = data.ElementBetween(4, 15);
        }
        public string TerminalId => _informationContent.ElementBetween(0, 7).JoinIntoString();
        public byte[] TypeIdeniticationCode => _informationContent.ElementBetween(8, 9);
        public byte[] TimeZone => _informationContent.ElementBetween(10, 11);

        protected override byte ContentLength
        {
            get
            {
                return 12;
            }
        }
    }

}
