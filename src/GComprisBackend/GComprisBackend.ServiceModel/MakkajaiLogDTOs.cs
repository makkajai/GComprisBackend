using System;
using System.Collections.Generic;

namespace GComprisBackend.ServiceModel
{
    public class MakkajaiLogResource
    {
        public string date { get; set; }

        public int duration { get; set; }

        public string login { get; set; }

        public string boardname { get; set; }

        public int level { get; set; }

        public int sublevel { get; set; }

        public int status { get; set; }
    }

    public static class LogDTOX
    {
        private const string UTCDateFormat = "yyyy-MM-ddTHH:mm:ssZ";

        public static MakkajaiLogResource ToMakkajaiLog(this LogResource log)
        {
            return new MakkajaiLogResource
            {
                date = log.Date.ToFormattedUTC(),
                boardname = log.BoardName,
                duration = log.Duration,
                login = log.Login,
                status = log.Status,
                level = log.Level,
                sublevel = log.SubLevel
            };
        }

        public static List<MakkajaiLogResource> ToMakkajaiLogs(this LogResources logs)
        {
            var makkajaiLogResources = new List<MakkajaiLogResource>();
            logs.ForEach(x => makkajaiLogResources.Add(x.ToMakkajaiLog()));
            return makkajaiLogResources;
        }

        public static string ToFormattedUTC(this DateTime date)
        {
            return date.ToString(UTCDateFormat);
        }
    }
}