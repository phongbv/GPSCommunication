using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public class ClientInformation
    {
        private const int BUFFER_SIZE = 64;
        private Socket clientSocket;
        public bool IsConnected => _loginInfo != null;
        private LoginPacket _loginInfo;
        private Thread thread;
        private DateTime _timeConnection = DateTime.Now;
        public ClientInformation(Socket socket)
        {
            clientSocket = socket;
            thread = new Thread(ProcessingRequest);
            thread.Start();
        }


        public BaseRequest CurrentRequest { get; set; }


        public void ProcessingRequest()
        {
            try
            {
                byte[] data = new byte[BUFFER_SIZE];
                // Waiting login request
                while (!IsConnected)
                {
                    _timeConnection = DateTime.Now;
                    data = new byte[BUFFER_SIZE];
                    clientSocket.Receive(data);
                    if (data[3] == 0x01)
                    {
                        DoLogin(data);
                        clientSocket.Send(_loginInfo.Response);
                        Console.WriteLine("Logon device: " + _loginInfo.TerminalId);
                    }
                }
                // Console.WriteLine("Getting request from: " + _loginInfo.TerminalId);
                // Starting reading position
                while (true)
                {
                    data = new byte[BUFFER_SIZE];
                    clientSocket.Receive(data);
                    _timeConnection = DateTime.Now;
                    var response = ProcessRequest(data);
                    if (response != null)
                    {
                        clientSocket.Send(data);
                    }


                }
            }
            catch (SocketException)
            {
                thread.Abort();
                clientSocket.Close();
                Console.WriteLine($"Client { clientSocket.RemoteEndPoint } is disconnected.");
            }
            catch (Exception ex)
            {
                File.WriteAllText(string.Format("Logs/{0}_{1}.log", _loginInfo?.TerminalId,
                    _timeConnection.ToString("yyyyMMddHHmmss")), ex.StackTrace);
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
            // Console.WriteLine("Request Type: " + requestType.ToString());
            if (IsConnected)
            {
                WriteRequest(requestType.ToString(), requestData);
            }

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



        private Task WriteRequest(string requestType, byte[] requestData)
        {
            string parentDirectory = "Dump/" + _loginInfo.TerminalId + "/" + requestType;
            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }

            string filePath = parentDirectory + "/" + _timeConnection.ToString("yyyyMMddHHmmss") + ".dat";
            return WriteAsync(filePath, requestData);
        }

        private async Task WriteAsync(string filePath, byte[] fileContent)
        {
            using (FileStream sourceStream = new FileStream(filePath,
                FileMode.Append, FileAccess.Write, FileShare.None,
                bufferSize: 1024, useAsync: true))
            {
                await sourceStream.WriteAsync(fileContent, 0, fileContent.Length);
            };
        }

    }
}
