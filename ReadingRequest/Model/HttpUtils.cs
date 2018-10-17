using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ReadingRequest.Model
{
    public class HttpUtils
    {
        public static void SendPost(string url, string formData)
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = "POST";
            byte[] requestData = Encoding.UTF8.GetBytes(formData);
            request.ContentLength = requestData.Length;
            request.ContentType = "application/x-www-form-urlencoded";
            Stream dataStream = request.GetRequestStream();
            dataStream.Write(requestData, 0, requestData.Length);
            dataStream.Close();

        }
    }
}
