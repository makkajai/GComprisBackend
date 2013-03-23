using GComprisBackend.ServiceModel;
using GComprisBackend.Web.Helpers;
using ServiceStack.ServiceInterface;

namespace GComprisBackend.Web
{
    public class StudentService : Service
    {
        /// <summary>
        /// Responds to the GET call on the /students/{login} url
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public object Get(StudentRequest request)
        {
            return StudentData.GetFromLogin(request.Login);
        }
    }
}
