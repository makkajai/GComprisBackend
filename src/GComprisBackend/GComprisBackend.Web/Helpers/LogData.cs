using System.Collections.Generic;
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
                var user = db.First<User>(p => p.Login == logResource.Login);
                var board = db.First<Board>(p => p.Name == logResource.BoardName);
                var log = new Log
                    {
                        BoardId = board.Id,
                        UserId = user.Id,
                        Date = logResource.Date,
                        Duration = logResource.Duration,
                        Level = logResource.Level,
                        Status = logResource.Status,
                        SubLevel = logResource.SubLevel
                    };

                db.Save(log);
            }
        }

        private const string LogsQuery =
            "select Date, Duration, Level, Status, SubLevel, u.Login, b.Name as BoardName " +
            "from logs l " +
            "inner join users u on l.user_id = u.user_id " +
            "inner join boards b on l.board_id = b.board_id " +
            "where u.login = {0}";

        public static List<LogResource> GetByUser(string login)
        {
            using (var db = DbHelper.GetConnection())
            {
                return db.Select<LogResource>(LogsQuery, login);
            }
        }
    }
}