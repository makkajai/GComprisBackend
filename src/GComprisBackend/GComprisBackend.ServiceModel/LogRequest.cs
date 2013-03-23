using System;
using ServiceStack.ServiceHost;

namespace GComprisBackend.ServiceModel
{
    [Route("/logs", "PUT")]
    public class LogResource
    {
        public DateTime Date { get; set; }

        public int Duration { get; set; }

        public string Login { get; set; }

        public string BoardName { get; set; }

        public int Level { get; set; }

        public int SubLevel { get; set; }

        public int Status { get; set; }
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
