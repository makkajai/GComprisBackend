using System;
using ServiceStack.DataAnnotations;

namespace GComprisBackend.ServiceModel.Types
{
    [Alias("logs")]
    public class Log
    {
        [Alias("date"), PrimaryKey]
        public DateTime Date { get; set; }

        [Alias("duration")]
        public int Duration { get; set; }

        [Alias("student_id")]
        public int StudentId { get; set; }

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
