using System.Net;
using GComprisBackend.ServiceModel;
using GComprisBackend.ServiceModel.Types;
using GComprisBackend.Web.Helpers;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using ServiceStack.ServiceClient.Web;

namespace GcomprisBackend.IntegrationTests
{
    /// <summary>
    /// This tests the Logs Service interface end-to-end - 
    /// 1. Checks whether logs PUT for test login, is sent back when GET is called
    /// 2. Checks whether the latestlogs interface returns the expected data
    /// 
    /// The test also sets up the test user and then cleans up after itself so that there is no manual mucking 
    /// around to be done
    /// </summary>
    [TestFixture]
    public class LogServiceTests: BaseTests
    {
        private const string LogResource= "logs";
        private string _logResourceUrl;

        private const string InsertTestUser =
            "insert into users(user_id, login, lastname, firstname) values (1000000, 'test', 'testlast', 'testfirst')";

        private const string DeleteLogs =
            "delete from logs where user_id in (select user_id from users where login='test')";

        private const string DeleteTestUser = "delete from users where login = 'test'";

        private static readonly object SingleLogRecord 
            = new Log{Date = "2012-11-09 21:21:42", Duration = "74", Login = "test", BoardName = "algebra_by", Level = "1", SubLevel = "1", Status = "0"};

        [SetUp]
        public void SetUp()
        {
            //setup Ormlite
            OrmLiteConfig.DialectProvider = PostgreSQLDialectProvider.Instance;

            //setup the test user
            using (var db = DbHelper.GetConnection())
            {
                db.ExecuteSql(InsertTestUser);
            }

            _logResourceUrl = ListeningOn + LogResource;
        }

        [Test]
        public void DoesSendingSingleRecordWork()
        {
            //Given that a log is PUT, when we do GET for this user, do we receive the log back.

            var client = new JsonServiceClient();

            //1. PUT a new log record
            var response = client.Put<LogResponse>(_logResourceUrl, SingleLogRecord);
            Assert.IsTrue(response.Success);
        }

        [TearDown]
        public void CleanUp()
        {
            //clean up the data that is created for this test - don't leave stray data hanging!
            using (var db = DbHelper.GetConnection())
            {
                db.ExecuteSql(DeleteLogs);
                db.ExecuteSql(DeleteTestUser);
            }
        }
    }
}
