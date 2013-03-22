using GComprisBackend.ServiceModel;
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
        public object Put(LogRequest log)
        {
            return new LogResponse {Success = true};
        }
    }
}
