using System;
using ServiceStack.ServiceHost;

namespace GComprisBackend.ServiceModel
{
    [Route("/logs", "PUT")]
    public class LogResource
    {
        public string Date { get; set; }

        public string Duration { get; set; }

        public string Login { get; set; }

        public string BoardName { get; set; }

        public string Level { get; set; }

        public string SubLevel { get; set; }

        public string Status { get; set; }
    }

    /// <summary>
    /// Represents a message for getting Log resources
    /// </summary>
    [Route("/logs/{UserName}", "GET")]
    public class LogRequest
    {
        public String UserName { get; set; }

        public DateTime FromDate { get; set; }

        public Boolean Latest { get; set; }
    }

    public class LogResponse
    {
        public bool Success { get; set; }
    }
}
