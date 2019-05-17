using Common.MongoDBClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    [Collection("ALBLOG_Users")]
    public class User : DomainModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
