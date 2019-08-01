using Common.MongoDBClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    [Collection("ALBLOG_Introduction")]
    public class Introduction : DomainModel
    {
        public string Profile { get; set; }

        public string CV { get; set; }

        public string About { get; set; }
    }
}
