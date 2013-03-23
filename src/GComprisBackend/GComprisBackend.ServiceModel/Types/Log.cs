using System;
using ServiceStack.DataAnnotations;

namespace GComprisBackend.ServiceModel.Types
{
    public class Log
    {
        [Alias("date"), PrimaryKey]
        public DateTime Date { get; set; }

        [Alias("duration")]
        public int Duration { get; set; }

        [Alias("user_id")]
        public int UserId { get; set; }

        [Alias("board_id")]
        public int BoardId { get; set; }

        [Alias("level")]
        public int Level { get; set; }

        [Alias("sublevel")]
        public int SubLevel { get; set; }

        [Alias("status")]
        public int Status { get; set; }
    }
}
