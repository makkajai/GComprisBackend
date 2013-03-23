﻿using System.Collections.Generic;
using GComprisBackend.ServiceModel;
using ServiceStack.ServiceInterface;

namespace GComprisBackend.Web
{
    public class LogsService : Service
    {
        private static List<LogResource> data = new List<LogResource>(); 

        /// <summary>
        /// Respond to the PUT request
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Put(LogResource log)
        {
            data.Add(log);
            return new LogResponse {Success = true};
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public object Get(LogRequest log)
        {
            return data;
        }
    }
}