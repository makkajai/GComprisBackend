using ServiceStack.ServiceHost;

namespace GComprisBackend.ServiceModel
{
    [Route("/logs")]
    public class LogRequest
    {
        public string Date { get; set; }

        public string Duration { get; set; }

        public string Login { get; set; }

        public string BoardName { get; set; }

        public string Level { get; set; }

        public string SubLevel { get; set; }

        public string Status { get; set; }
    }
}
