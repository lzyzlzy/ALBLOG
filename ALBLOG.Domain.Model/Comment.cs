using Common.MongoDBClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    [Collection("ALBLOG_Comment")]
    public class Comment : DomainModel
    {
        public string Content { get; set; }
        
        public string Name { get; set; }

        public string Date { get; set; }

        public string IPAddress { get; set; }
    }
}
