using GComprisBackend.ServiceModel;
using GComprisBackend.Web.Helpers;
using ServiceStack.ServiceInterface;

namespace GComprisBackend.Web
{
    public class LogsService : Service
    {
        /// <summary>
        /// Respond to the PUT request
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Put(LogResource log)
        {
            LogData.Save(log);
            return new LogResponse {Success = true};
        }


        /// <summary>
        /// Respond to the POST request on /logs - post is expected only with multiple records
        /// </summary>
        /// <param name="logs">List of logs </param>
        /// <returns></returns>
        public object Post(LogResources logs)
        {
            LogData.Save(logs);
            return new LogResponse {Success = true};
        }

        /// <summary>
        /// Respond to the GET request on /logs/{Login} - get is expected to return multiple records
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Get(LogRequest log)
        {
            return LogData.GetByUser(log.Login);
        }
    }
}