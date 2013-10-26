using GComprisBackend.ServiceModel;
using ServiceStack.ServiceInterface;

namespace GComprisBackend.Web
{
    public class StudentService : Service
    {
        private const string StudentsUrl = "students/{0}";

        /// <summary>
        ///     Responds to the GET call on the /students/{login} url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(StudentRequest request)
        {
            return MakkajaiGateway.DoGet(string.Format(StudentsUrl, request.Login));
        }
    }
}