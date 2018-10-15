using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public class GPSLocationPacket : BaseRequest
    {
        /// <summary>
        /// Content length is 21. Start from 4 to 24
        /// </summary>
        /// <param name="data"></param>
        public GPSLocationPacket(LoginPacket connectionInfo, byte[] data) : base(connectionInfo, data)
        {
            _informationContent = _rawData.ElementBetween(4, 24);
        }

        public DateTime Time => new DateTime(_informationContent[0] + 2000, _informationContent[1], _informationContent[2], _informationContent[3], _informationContent[4], _informationContent[5]);

        public decimal Latitude => _informationContent.ElementBetween(7, 10).ToDecimal() / 1800000;
        public decimal Longitude => _informationContent.ElementBetween(11, 14).ToDecimal() / 1800000;
        public byte Speed => _informationContent[15];

        public decimal MCC => _informationContent.ElementBetween(18, 19).ToDecimal();

        public decimal MNC => _informationContent[20];

        protected override byte ContentLength => 21;

        public override void DoProcessRequest()
        {
            base.DoProcessRequest();
            using (var dbContext = new DataContext())
            {
                dbContext.qbit_detail.Add(new qbit_detail()
                {
                    detail_imei = clientConnectionInfor.TerminalId,
                    detail_last = Time,
                    detail_lat = Latitude.ToString(),
                    detail_lng = Longitude.ToString(),
                    detail_time = Time,
                    detail_speed = Speed.ToString()
                });
                dbContext.SaveChanges();
            }
        }
    }
}
