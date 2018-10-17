using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    using System;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.IO;
    using ReadingRequest;
    using ReadingRequest.Model;

    public class Y2Server
    {


        private const int BUFFER_SIZE = 128;
        private const int PORT_NUMBER = 24292;
        private const string SERVER_ADDRESS = "127.0.0.1";
        //private const string SERVER_ADDRESS = "112.78.11.14";

        static ASCIIEncoding encoding = new ASCIIEncoding();
        private static Dictionary<RequestType, int> requestFile = new Dictionary<RequestType, int>();
        public static void Main()
        {
            new Y2Server().StartServer();
            Console.Read();
        }

        public void StartServer()
        {
            try
            {
                IPAddress address = IPAddress.Parse(SERVER_ADDRESS);

                TcpListener listener = new TcpListener(address, PORT_NUMBER);

                listener.Start();
                Console.WriteLine("Server started on " + listener.LocalEndpoint);
                while (true)
                {
                    Console.WriteLine("Waiting for a connection...");

                    Socket socket = listener.AcceptSocket();
                    Console.WriteLine("Connection received from " + socket.RemoteEndPoint);
                    ClientInformation clientInfo = new ClientInformation(socket);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex);
            }
        }
    }
}
