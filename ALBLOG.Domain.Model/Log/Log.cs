using Common.MongoDBClient.Model;
using System;

namespace ALBLOG.Domain.Model
{
    [Collection("ALBLOG_Log")]
    public class Log : DomainModel
    {
        public string LogId { get; set; }

        public DateTime Date { get; set; }

        public LogType Type { get; set; }

        public bool IsAdmin { get; set; }

        public string SessionId { get; set; }

        public string ControllerName { get; set; }

        public string ActionName { get; set; }

        public string IPAddress { get; set; }

        public string Content { get; set; }

    }
}
