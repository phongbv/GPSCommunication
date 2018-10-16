using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        private const int BUFFER_SIZE = 1024;
        private const int PORT_NUMBER = 3000;
        private const string SERVER_ADDRESS = "127.0.0.1";
        //private const string SERVER_ADDRESS = "112.78.11.14";
        private static readonly byte[] LOGIN_RESPONSE = new byte[] { 0x78, 0x78, 0x05, 0x01, 0x00, 0x05, 0x9F, 0xF8, 0x0D, 0x0A };
        private const int MAX_TRY = 5;
        private static int TRY_COUNT = 0;
        private static bool isRunning = true;
        static void Main(string[] args)
        {
            while (isRunning)
            {
                TcpClient client = new TcpClient();
                try
                {
                    client.Connect(SERVER_ADDRESS, PORT_NUMBER);
                }
                catch (Exception)
                {

                    Thread.Sleep(10000);
                    continue;
                }

                bool isConnected = false;

                var stream = client.GetStream();
                var loginRequest = File.ReadAllBytes("Data/LoginInformation.dat");
                while (!isConnected)
                {
                    Console.WriteLine("Sending login packet...");
                    // Sending login request
                    stream.Write(loginRequest, 0, loginRequest.Length);

                    var data = new byte[BUFFER_SIZE];
                    stream.Read(data, 0, 1);

                    //if (data == LOGIN_RESPONSE)
                    //{
                    Console.WriteLine("Client is connected to the server");
                    isConnected = true;
                    break;
                    //}
                    if (TRY_COUNT > MAX_TRY)
                    {
                        Console.WriteLine("Cannot login the server");
                        return;
                    }
                    Thread.Sleep(1000);
                }


                DoSendLocation(stream);
                client.Close();
            }
        }

        private static void DoSendLocation(NetworkStream stream)
        {
            try
            {
                Console.WriteLine("Send location information");
                var heartBeatRequest = File.ReadAllBytes("Data/GPSPositionData.dat");
                stream.Write(heartBeatRequest, 0, heartBeatRequest.Length);

            }
            catch (Exception)
            {

            }
            finally
            {
                Thread.Sleep(10000);
            }
        }

        static void DoSendHeartBeat(Stream stream)
        {
            Console.WriteLine("Send heartbeat connection");
            var heartBeatRequest = File.ReadAllBytes("Data/HeartBeatPacket.dat");
            stream.Write(heartBeatRequest, 0, heartBeatRequest.Length);
            Thread.Sleep(5000);
        }
    }
}
