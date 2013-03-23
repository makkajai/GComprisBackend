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
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Get(LogRequest log)
        {
            return LogData.GetByUser(log.UserName);
        }
    }
}