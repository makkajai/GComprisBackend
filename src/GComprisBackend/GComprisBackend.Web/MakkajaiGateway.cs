using System;
using System.Configuration;
using System.IO;
using System.Net;
using ServiceStack.Text;

namespace GComprisBackend.Web
{
    public class MakkajaiGateway
    {
        private const string JsonAcceptHeader = "application/json";

        private static readonly string TargetMakkajaiBackendUrl =
            ConfigurationManager.AppSettings["targetMakkajaiBackendUrl"];

        public static object DoGet(string requestUriPostfix)
        {
            var httpWebRequest = GetHttpWebRequest(requestUriPostfix, WebRequestMethods.Http.Get);
            return ParseResponse(httpWebRequest);
        }

        public static object DoPut(string requestUriPostfix, object data)
        {
            return DoPush(requestUriPostfix, data, WebRequestMethods.Http.Put);
        }

        public static object DoPost(string requestUriPostfix, object data)
        {
            return DoPush(requestUriPostfix, data, WebRequestMethods.Http.Post);
        }

        private static object DoPush(string requestUriPostfix, object data, string method)
        {
            var httpWebRequest = GetHttpWebRequest(requestUriPostfix, method);
            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = data.ToJson();

                Console.WriteLine("The Data to post: " + json);

                streamWriter.Write(json);
                streamWriter.Flush();
                streamWriter.Close();

                return ParseResponse(httpWebRequest);
            }
        }

        private static HttpWebRequest GetHttpWebRequest(string requestUriPostfix, string method)
        {
            Console.WriteLine("The request url: " + requestUriPostfix);
            var httpWebRequest = (HttpWebRequest) WebRequest.Create(TargetMakkajaiBackendUrl + requestUriPostfix);
            httpWebRequest.Method = method;
            httpWebRequest.Accept = JsonAcceptHeader;
            return httpWebRequest;
        }

        private static object ParseResponse(HttpWebRequest httpWebRequest)
        {
            using (var response = (HttpWebResponse) httpWebRequest.GetResponse())
            {
                using (var dataStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(dataStream))
                    {
                        return reader.ReadToEnd();
                    }
                }
            }
        }
    }
}