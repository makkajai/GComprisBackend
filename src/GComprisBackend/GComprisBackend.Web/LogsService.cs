using System;
using GComprisBackend.ServiceModel;
using ServiceStack.ServiceInterface;

namespace GComprisBackend.Web
{
    public class LogsService : Service
    {
        private const string LogsUrlPostfix = "logs";
        private const string GetLogsUrlPostfix = "logs/{0}";
        private const string FromDateParamFormat = "?fromDate={0}";

        /// <summary>
        ///     Respond to the PUT request
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Put(LogResource log)
        {
            MakkajaiGateway.DoPut(LogsUrlPostfix, log.ToMakkajaiLog());
            return new LogResponse {Success = true};
        }


        /// <summary>
        ///     Respond to the POST request on /logs - post is expected only with multiple records
        /// </summary>
        /// <param name="logs">List of logs </param>
        /// <returns></returns>
        public object Post(LogResources logs)
        {
            MakkajaiGateway.DoPost(LogsUrlPostfix, logs.ToMakkajaiLogs());
            return new LogResponse {Success = true};
        }

        /// <summary>
        ///     Respond to the GET request on /logs/{Login} - get is expected to return multiple records
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Get(LogRequest log)
        {
            return
                MakkajaiGateway.DoGet(string.Format("{0}{1}", string.Format(GetLogsUrlPostfix, log.Login),
                    GetFromDateFormat(log)));
        }

        private string GetFromDateFormat(LogRequest log)
        {
            if (log.FromDate == new DateTime()) return string.Empty;
            return string.Format(FromDateParamFormat, log.FromDate.ToFormattedUTC());
        }
    }
}