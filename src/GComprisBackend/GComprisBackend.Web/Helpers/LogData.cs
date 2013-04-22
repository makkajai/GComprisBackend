using System;
using System.Collections.Generic;
using System.Linq;
using GComprisBackend.ServiceModel;
using GComprisBackend.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace GComprisBackend.Web.Helpers
{
    public class LogData
    {
        public static void Save(LogResource logResource)
        {
            using (var db = DbHelper.GetConnection())
            {
                var user = db.First<Student>(p => p.Login == logResource.Login);
                var board = db.First<Board>(p => p.Name == logResource.BoardName);
                var log = new Log
                    {
                        BoardId = board.Id,
                        StudentId = user.Id,
                        Date = logResource.Date,
                        Duration = logResource.Duration,
                        Level = logResource.Level,
                        Status = logResource.Status,
                        SubLevel = logResource.SubLevel
                    };

                db.Save(log);
            }
        }

        public static void Save(LogResources logResources)
        {
            //do not process further
            if (logResources.Count == 0) return;
            //instead of saving one by one, do batch queries on user and board, and then bulk save logs
            using (var db = DbHelper.GetConnection())
            {
                //using String.Format to hand-craft where condition since Select() will handle only parameterized values - 
                // and Sql.In is not working for some reason
                var distinctLogins = String.Join(",", logResources.Select(q => "'" + q.Login + "'").Distinct());
                var users = db.Select<Student>(string.Format("login in ({0})", distinctLogins));

                var distinctBoardNames = String.Join(",", logResources.Select(q => "'" + q.BoardName + "'").Distinct());
                var boards = db.Select<Board>(string.Format("name in ({0})", distinctBoardNames));

                var logs = logResources.Select(p => new Log
                    {
                        BoardId = boards.First(q => q.Name == p.BoardName).Id,
                        StudentId = users.First(q=>q.Login == p.Login).Id,
                        Date = p.Date,
                        Duration = p.Duration,
                        Level = p.Level,
                        Status = p.Status,
                        SubLevel = p.SubLevel
                    });

                db.SaveAll(logs);
            }
        }

        private const string LogsQuery =
            "select Date, Duration, Level, Status, SubLevel, s.Login, b.Name as BoardName " +
            "from logs l " +
            "inner join students s on l.student_id = s.student_id " +
            "inner join boards b on l.board_id = b.board_id " +
            "where s.login = {0} " +
            "and l.date > {1}" +
            "and not exists (select 1 from logs lin " +
            "               where lin.student_id = l.student_id and " +
            "               lin.board_id = l.board_id and lin.date > l.date) " +
            "order by l.date desc";

        public static List<LogResource> GetByStudent(string login, DateTime date)
        {
            using (var db = DbHelper.GetConnection())
            {
                return db.Select<LogResource>(LogsQuery, login, date);
            }
        }

    }
}