using System;
using System.Collections.Generic;
using ServiceStack.ServiceHost;

namespace GComprisBackend.ServiceModel
{
    [Route("/logs", "PUT")]
    public class LogResource
    {
        public DateTime Date { get; set; }

        public String FormattedDate
        {
            get { return Date.ToString("yyyy-MM-dd HH:mm:ss"); }
        }

        public int Duration { get; set; }

        public string Login { get; set; }

        public string BoardName { get; set; }

        public int Level { get; set; }

        public int SubLevel { get; set; }

        public int Status { get; set; }
    }

    [Route("/logs", "POST")]
    public class LogResources : List<LogResource>
    {
    }

    /// <summary>
    /// Represents a message for getting Log resources
    /// </summary>
    [Route("/logs/{Login}", "GET")]
    public class LogRequest
    {
        public String Login { get; set; }

        public DateTime FromDate { get; set; }
    }

    public class LogResponse
    {
        public bool Success { get; set; }
    }
}
