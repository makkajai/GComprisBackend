using GComprisBackend.ServiceModel;
using GComprisBackend.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace GComprisBackend.Web.Helpers
{
    public class StudentData
    {
        public static StudentResource GetFromLogin(string login)
        {
            using (var db = DbHelper.GetConnection())
            {
                var student = db.FirstOrDefault<Student>("login = {0}", login);
                
                return new StudentResource
                    {
                        Login = student.Login,
                        FirstName = student.FirstName,
                        LastName = student.LastName
                    };
            }
        }
    }
}