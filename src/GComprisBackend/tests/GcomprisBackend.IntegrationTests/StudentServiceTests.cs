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
    /// This tests the Users Service interface end-to-end - 
    ///   - Checks whether given a user, whether GET with the login gives the user details
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
    public class StudentServiceTests: BaseTests
    {
        #region variable and constant declarations
        private const string StudentsResource= "students";
        private string _studentsResourceUrl;

        private const string CreateTestUser =
            "insert into students(student_id, login, lastname, firstname) values (1000000, 'test', 'testlast', 'testfirst')";

        private static readonly StudentResource ExpectedStudent = new StudentResource
                {
                    Login = "test",
                    FirstName = "testfirst",
                    LastName = "testlast"
                };

        private const string DeleteTestUser = "delete from students where login = 'test'";

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

            _studentsResourceUrl = ListeningOn + StudentsResource;
        }

        [Test]
        public void DoesGETUserReturnData()
        {
            var client = new JsonServiceClient();
            var student = client.Get<StudentResource>(string.Format("{0}/{1}", _studentsResourceUrl, "test"));
            Assert.AreEqual(ExpectedStudent.ToJson(), student.ToJson()); 
        }

        [TearDown]
        public void CleanUp()
        {
            //clean up the data that is created for this test - don't leave stray data hanging!
            using (var db = DbHelper.GetConnection())
            {
                db.ExecuteSql(DeleteTestUser);
            }
        }
    }
}