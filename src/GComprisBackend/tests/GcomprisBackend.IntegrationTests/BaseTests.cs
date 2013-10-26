using GcomprisBackend.IntegrationTests.Support;
using NUnit.Framework;
using ServiceStack.Logging;
using ServiceStack.Logging.Support.Logging;
using ServiceStack.Service;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

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

            JsConfig.DateHandler = JsonDateHandler.ISO8601;
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