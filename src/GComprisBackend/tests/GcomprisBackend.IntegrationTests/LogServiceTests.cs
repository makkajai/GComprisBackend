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
    ///     This tests the Logs Service interface end-to-end -
    ///     1. Checks whether logs PUT for test login, is sent back when GET is called
    ///     2. Checks whether the latestlogs interface returns the expected data
    ///     The test also sets up the test user and then cleans up after itself so that there is no manual mucking
    ///     around to be done
    ///     Similar to the Web service, this test project depends on the environment variable GComprisBackend_DB being
    ///     setup and pointed to the postgresql database that serves as the datastore for this application.
    ///     For setting up the database, use the DBSetupScripts "GCompris-Service-Data-Model.sql" in the src/DbScript folder
    /// </summary>
    [TestFixture]
    public class LogServiceTests : BaseTests
    {
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

        private const string LogResource = "logs";
        private const string TestStudentId = "teststudent";
        private string _logResourceUrl;

        private const string CreateTestUser =
            "insert into students(student_id, login, lastname, firstname) values (1000000, '" + TestStudentId +
            "', 'testlast', 'testfirst')";

        private const string DeleteLogs =
            "delete from logs where student_id in (select student_id from students where login='" + TestStudentId + "')";

        private const string DeleteTestUser = "delete from students where login = 'teststudent'";

        private static readonly LogResource SingleLogRecord
            = new LogResource
            {
                Date = DateTime.Parse("2012-11-09 21:21:42"),
                Duration = 74,
                Login = TestStudentId,
                BoardName = "algebra_by",
                Level = 1,
                SubLevel = 1,
                Status = 0
            };

        private static readonly List<LogResource> MultiLogRecords
            = new List<LogResource>
            {
                new LogResource
                {
                    Date = DateTime.Parse("2012-11-09 21:21:42"),
                    Duration = 74,
                    Login = TestStudentId,
                    BoardName = "algebra_by",
                    Level = 1,
                    SubLevel = 1,
                    Status = 0
                },
                new LogResource
                {
                    Date = DateTime.Parse("2012-11-10 21:21:42"),
                    Duration = 75,
                    Login = TestStudentId,
                    BoardName = "algebra_plus",
                    Level = 1,
                    SubLevel = 1,
                    Status = 0
                },
            };

        private static readonly List<LogResource> MultiLogRecordsWithSameBoard
            = new List<LogResource>
            {
                new LogResource
                {
                    Date = DateTime.Parse("2012-11-09 21:21:42"),
                    Duration = 74,
                    Login = TestStudentId,
                    BoardName = "algebra_by",
                    Level = 1,
                    SubLevel = 1,
                    Status = 0
                },
                new LogResource
                {
                    Date = DateTime.Parse("2012-11-09 21:22:42"),
                    Duration = 74,
                    Login = TestStudentId,
                    BoardName = "algebra_by",
                    Level = 1,
                    SubLevel = 1,
                    Status = 0
                },
                new LogResource
                {
                    Date = DateTime.Parse("2012-11-10 21:21:42"),
                    Duration = 75,
                    Login = TestStudentId,
                    BoardName = "algebra_plus",
                    Level = 1,
                    SubLevel = 1,
                    Status = 0
                },
            };

        private static readonly List<LogResource> ExpectedLatestLogsOnly = new List<LogResource>
        {
            new LogResource
            {
                Date = DateTime.Parse("2012-11-09 21:22:42"),
                Duration = 74,
                Login = TestStudentId,
                BoardName = "algebra_by",
                Level = 1,
                SubLevel = 1,
                Status = 0
            },
            new LogResource
            {
                Date = DateTime.Parse("2012-11-10 21:21:42"),
                Duration = 75,
                Login = TestStudentId,
                BoardName = "algebra_plus",
                Level = 1,
                SubLevel = 1,
                Status = 0
            },
        };

        private static readonly List<LogResource> ExpectedLatestLogsOnlyWithDate = new List<LogResource>
        {
            new LogResource
            {
                Date = DateTime.Parse("2012-11-10 21:21:42"),
                Duration = 75,
                Login = TestStudentId,
                BoardName = "algebra_plus",
                Level = 1,
                SubLevel = 1,
                Status = 0
            },
        };

        private string _url;

        [Test]
        public void DoesGETReturnOnlyLatestRecordForEachBoard()
        {
            //Given that a collection of logs is PUT with at least two logs for a single board, 
            // when we do GET for this user, do we receive only the latest logs back.
            var client = new JsonServiceClient();
            Console.WriteLine("Json data being sent: {0}", MultiLogRecordsWithSameBoard.ToJson());

            //1. POST new log collection
            var response = client.Post<LogResponse>(_logResourceUrl, MultiLogRecordsWithSameBoard);
            Assert.IsTrue(response.Success);

            //GET all the logs for the test user
            var logResponse = client.Get<List<LogResource>>(string.Format("{0}/{1}", _logResourceUrl, TestStudentId));

            //check whether the list has two records as sent
            Assert.AreEqual(2, logResponse.Count);

            //check whether the list is as sent, but only the latest record for each activity - 
            // note that the expected result is that logs are sorted
            //date-wise desc, so compare that way
            Assert.AreEqual(ExpectedLatestLogsOnly.OrderByDescending(p => p.Date).ToList().ToJson(),
                logResponse.ToJson());
        }

        [Test]
        public void DoesGETWithFromDateReturnProperData()
        {
            //Given that a collection of logs is PUT with at least two logs for a single board, 
            // when we do GET for this user, do we receive only the latest logs back.
            var client = new JsonServiceClient();
            Console.WriteLine("Json data being sent: {0}", MultiLogRecordsWithSameBoard.ToJson());

            //1. POST new log collection
            var response = client.Post<LogResponse>(_logResourceUrl, MultiLogRecordsWithSameBoard);
            Assert.IsTrue(response.Success);

            //GET all the logs for the test user
            _url = string.Format("{0}/{1}?FromDate={2}", _logResourceUrl, TestStudentId,
                DateTime.Parse("2012-11-10 20:21:42"));
            Console.WriteLine("Url called: {0}", _url);
            var logResponse = client.Get<List<LogResource>>(_url);

            //check whether the list has two records as sent
            Assert.AreEqual(1, logResponse.Count);

            //check whether the list is as sent, but only the latest record for each activity - 
            // note that the expected result is that logs are sorted
            //date-wise desc, so compare that way
            Assert.AreEqual(ExpectedLatestLogsOnlyWithDate.ToJson(), logResponse.ToJson());
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
            var logResponse = client.Get<List<LogResource>>(string.Format("{0}/{1}", _logResourceUrl, TestStudentId));

            //check whether the list has two records as sent
            Assert.AreEqual(9, logResponse.Count);

            //check whether the list is as sent - note that the expected result is that logs are sorted
            //date-wise desc, so compare that way
            Assert.AreEqual(MultiLogRecords.OrderByDescending(p => p.Date).ToList().ToJson(), logResponse.ToJson());
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
            var logResponse = client.Get<List<LogResource>>(string.Format("{0}/{1}", _logResourceUrl, TestStudentId));

            //Check whether the list has one record
            Assert.AreEqual(1, logResponse.Count);

            //Check whether the received record matches the sent record in Json format
            Assert.AreEqual(SingleLogRecord.ToJson(), logResponse.First().ToJson());
        }
    }
}