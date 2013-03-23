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
                throw new Exception("GCompris Backend connection string not found - ensure that the GComprisBackend_DB env variable has been setup");
            return new OrmLiteConnectionFactory(connString).OpenDbConnection();
        }
    }
}