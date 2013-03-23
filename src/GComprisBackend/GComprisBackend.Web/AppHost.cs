using Funq;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using ServiceStack.WebHost.Endpoints;

namespace GComprisBackend.Web
{
    public class AppHost : AppHostBase
    {	
        public AppHost() : base("GCompris-BackendService", typeof(AppHost).Assembly) { }

        public override void Configure(Container container)
        {
            //TODO: change this
            LogManager.LogFactory = new ConsoleLogFactory();

            SetConfig(new EndpointHostConfig
            {
                DebugMode = true, 
            });

            //configure ormlite
            ConfigureOrmLite();
        }

        /// <summary>
        /// Configures the database connection string for use with ETL database
        /// </summary>
        public static void ConfigureOrmLite()
        {
            OrmLiteConfig.DialectProvider = PostgreSQLDialectProvider.Instance;
        }
    }
}
