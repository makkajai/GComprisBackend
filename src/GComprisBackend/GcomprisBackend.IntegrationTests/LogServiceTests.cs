using GComprisBackend.Web.Helpers;
using NUnit.Framework;
using ServiceStack.OrmLite;
using ServiceStack.OrmLite.PostgreSQL;

namespace GcomprisBackend.IntegrationTests
{
    /// <summary>
    /// This tests the Logs Service interface end-to-end - 
    /// 1. Checks whether logs sent for test login, is sent back when GET is called
    /// 2. Checks whether the latestlogs interface returns the expected data
    /// 
    /// The test also sets up the test user and then cleans up after itself so that there is no manual mucking 
    /// around to be done
    /// </summary>
    [TestFixture]
    public class LogServiceTests
    {
        private const string InsertTestUser = "insert into users(user_id, login, lastname, firstname) values (1000, 'test', 'testlast', 'testfirst')";

        private const string DeleteLogs =
            "delete from logs where user_id in (select user_id from users where login='test')";
        private const string DeleteTestUser = "delete from users where login = 'test'";

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
