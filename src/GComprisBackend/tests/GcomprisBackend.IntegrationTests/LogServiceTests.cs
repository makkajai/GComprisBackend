using System;
using System.Collections.Generic;
using System.Linq;
using GComprisBackend.ServiceModel;
using GComprisBackend.Web.Helpers;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;
using ServiceStack.ServiceClient.Web;
using ServiceStack.Text;

namespace GcomprisBackend.IntegrationTests
{
    /// <summary>
    /// This tests the Logs Service interface end-to-end - 
    /// 1. Checks whether logs PUT for test login, is sent back when GET is called
    /// 2. Checks whether the latestlogs interface returns the expected data
    /// 
    /// The test also sets up the test user and then cleans up after itself so that there is no manual mucking 
    /// around to be done
    /// 
    /// Similar to the Web service, this test project depends on the environment variable GComprisBackend_DB being 
    /// setup and pointed to the postgresql database that serves as the datastore for this application.
    /// 
    /// For setting up the database, use the DBSetupScripts "GCompris-Service-Data-Model.sql" in the src/DbScript folder
    /// </summary>
    [TestFixture]
    public class LogServiceTests: BaseTests
    {
        #region variable and constant declarations
        private const string LogResource= "logs";
        private string _logResourceUrl;

        private const string CreateTestUser =
            "insert into users(user_id, login, lastname, firstname) values (1000000, 'test', 'testlast', 'testfirst')";

        private const string DeleteLogs =
            "delete from logs where user_id in (select user_id from users where login='test')";

        private const string DeleteTestUser = "delete from users where login = 'test'";

        private static readonly LogResource SingleLogRecord 
            = new LogResource{Date = DateTime.Parse("2012-11-09 21:21:42"), Duration = 74, Login = "test", BoardName = "algebra_by", Level = 1, SubLevel = 1, Status = 0};

        private static readonly List<LogResource> MultiLogRecords
            = new List<LogResource>
                {
                    new LogResource
                        {
                            Date = DateTime.Parse("2012-11-09 21:21:42"),
                            Duration = 74,
                            Login = "test",
                            BoardName = "algebra_by",
                            Level = 1,
                            SubLevel = 1,
                            Status = 0
                        },
                    new LogResource
                        {
                            Date = DateTime.Parse("2012-11-10 21:21:42"),
                            Duration = 75,
                            Login = "test",
                            BoardName = "algebra_plus",
                            Level = 1,
                            SubLevel = 1,
                            Status = 0
                        },
                };

        #endregion

        [SetUp]
        public void SetUp()
        {
            //setup Ormlite
            OrmLiteConfig.DialectProvider = PostgreSQLDialectProvider.Instance;

            //setup the test user
            using (var db = DbHelper.GetConnection())
            {
                db.ExecuteSql(CreateTestUser);
            }

            _logResourceUrl = ListeningOn + LogResource;
        }

        [Test]
        public void DoesSendingSingleRecordWork()
        {
            //Given that a log is PUT, when we do GET for this user, do we receive the log back.
            var client = new JsonServiceClient();
            Console.WriteLine("Json data being sent: {0}", SingleLogRecord.ToJson());

            //1. PUT a new log record
            var response = client.Put<LogResponse>(_logResourceUrl, SingleLogRecord);
            Assert.IsTrue(response.Success);

            //GET all the logs for the test user
            var logResponse = client.Get<List<LogResource>>(string.Format("{0}/{1}", _logResourceUrl, "test"));

            //Check whether the list has one record
            Assert.AreEqual(1, logResponse.Count);

            //Check whether the received record matches the sent record in Json format
            Assert.AreEqual(SingleLogRecord.ToJson(), logResponse.First().ToJson());
        }

        [Test]
        public void DoesSendingMultipleRecordsWork()
        {
            //Given that a collection of logs is PUT, when we do GET for this user, do we receive the logs back.
            var client = new JsonServiceClient();
            Console.WriteLine("Json data being sent: {0}", MultiLogRecords.ToJson());
            
            //1. POST new log collection
            var response = client.Post<LogResponse>(_logResourceUrl, MultiLogRecords);
            Assert.IsTrue(response.Success);

            //GET all the logs for the test user
            var logResponse = client.Get<List<LogResource>>(string.Format("{0}/{1}", _logResourceUrl, "test"));

            //check whether the list has two records as sent
            Assert.AreEqual(2, logResponse.Count);

            //check whether the list is as sent - note that the expected result is that logs are sorted
            //date-wise desc, so compare that way
            Assert.AreEqual(MultiLogRecords.ToJson(), logResponse.ToJson());

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