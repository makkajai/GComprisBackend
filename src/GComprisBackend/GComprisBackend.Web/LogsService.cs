using System.Collections.Generic;
using System.Diagnostics;
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
        public object Put(LogResource log)
        {
            return new LogResponse {Success = true};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Get(LogRequest log)
        {
            return new List<LogResource>
                {
                    new LogResource()
                };
        }
    }
}