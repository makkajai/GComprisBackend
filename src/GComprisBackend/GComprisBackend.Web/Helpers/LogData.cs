using System;
using GComprisBackend.ServiceModel;
using GComprisBackend.ServiceModel.Types;
using ServiceStack.OrmLite;

namespace GComprisBackend.Web.Helpers
{
    public static class LogData
    {
        public static void Save(this LogResource logResource)
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
    }
}