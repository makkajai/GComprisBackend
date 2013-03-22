using GComprisBackend.Web;
using ServiceStack.WebHost.Endpoints;
using Container = Funq.Container;

namespace GcomprisBackend.IntegrationTests.Support
{
        /// <summary>
        /// This is a test-specific class used with self-hosting to avoid any dependencies on IIS - can be used to test everything except the final setup. 
        /// </summary>
        public class TestAppHostHttpListener : AppHostHttpListenerBase
        {
            public TestAppHostHttpListener():base("GComprisBackendService", typeof(AppHost).Assembly) {}

            public override void Configure(Container container)
            {
                SetConfig(new EndpointHostConfig {DebugMode = true});
                AppHost.ConfigureOrmLite();
            }
        }
}
