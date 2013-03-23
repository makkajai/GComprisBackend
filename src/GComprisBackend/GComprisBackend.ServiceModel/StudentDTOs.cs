using ServiceStack.ServiceHost;

namespace GComprisBackend.ServiceModel
{
    /// <summary>
    /// This is to separate the resource object from the DTO - can potentially add more properties
    /// on either of them without having to do so for the other one
    /// </summary>
    public class StudentResource
    {
        public string Login { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }

    [Route("/students/{Login}", "GET")]
    public class StudentRequest
    {
        public string Login { get; set; }
    }
}
