using System;
using System.Data;
using ServiceStack.OrmLite;

namespace GComprisBackend.Web.Helpers
{
    public class DbHelper
    {
        public const string ConnStringVariable = "GComprisBackend_DB";

        public static IDbConnection GetConnection()
        {
            var connString = Environment.GetEnvironmentVariable(ConnStringVariable);
            if(string.IsNullOrEmpty(connString))
                throw new Exception("GCompris Backend connection string not found - ensure that the GComprisBackend_DB env variable has been setup (and that you have restarted the system after setting it up)");
            return new OrmLiteConnectionFactory(connString).OpenDbConnection();
        }
    }
}
