using GcomprisBackend.IntegrationTests.Support;
using NUnit.Framework;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;

namespace GcomprisBackend.IntegrationTests
{
    public class BaseTests
    {
        protected const string ListeningOn = "http://localhost:1337/";

        protected TestAppHostHttpListener AppHost;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            LogManager.LogFactory = new ConsoleLogFactory();
            AppHost = new TestAppHostHttpListener();
            AppHost.Init();
            AppHost.Start(ListeningOn);
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            AppHost.Dispose();
        }

        protected static IRestClient GetClient()
        {
            return new JsonServiceClient(ListeningOn);
        }
    }
}
