using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public class ClientInformation
    {
        private const int BUFFER_SIZE = 128;
        private Socket clientSocket;
        public bool IsConnected => _loginInfo != null;
        private LoginPacket _loginInfo;
        public ClientInformation(Socket socket)
        {
            clientSocket = socket;
        }


        public BaseRequest CurrentRequest { get; set; }


        public void ProcessingRequest()
        {
            byte[] data = new byte[BUFFER_SIZE];
            // Waiting login request
            while (!IsConnected)
            {
                clientSocket.Receive(data);
                if (data[3] == 0x01)
                {
                    DoLogin(data);
                    clientSocket.Send(_loginInfo.Response);
                    Console.WriteLine("Logon device: " + _loginInfo.TerminalId);
                }
            }
            Console.WriteLine("Getting request from: " + _loginInfo.TerminalId);
            // Starting reading position
            while (true)
            {
                data = new byte[BUFFER_SIZE];
                clientSocket.Receive(data);
                var response = ProcessRequest(data);
                if (response != null)
                {
                    clientSocket.Send(data);
                }
            }
        }

        private byte[] ProcessRequest(byte[] requestData)
        {
            InitRequest(requestData);
            CurrentRequest?.DoProcessRequest();
            return CurrentRequest?.Response;
        }

        private void InitRequest(byte[] requestData)
        {
            CurrentRequest = null;
            var requestType = (RequestType)requestData[3];
            Console.WriteLine("Request Type: " + requestType.ToString());
            switch (requestType)
            {
                case RequestType.LoginInformation:
                    CurrentRequest = new LoginPacket(requestData);
                    break;
                case RequestType.GPSPositionData:
                    CurrentRequest = new GPSLocationPacket(_loginInfo, requestData);
                    break;
                case RequestType.HeartBeatPacket:
                    CurrentRequest = new HeartBeat(_loginInfo, requestData);
                    break;
                case RequestType.WifiCommunicationProtocol:
                    break;
                case RequestType.TimeCheckPacket:
                    break;
                default:
                    break;
            }
        }

        public void DoLogin(byte[] requestData)
        {
            _loginInfo = new LoginPacket(requestData);
        }

    }
}
