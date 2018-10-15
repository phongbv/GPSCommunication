using ReadingRequest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest
{
    class Program
    {
        static void Main(string[] args)
        {
            // Convert the hex string back to the number
            byte[] data = File.ReadAllBytes("GPSPositionData.dat");
        }
    }

}
