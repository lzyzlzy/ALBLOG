using Common.MongoDBClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    [Collection("ALBlOG_Posts")]
    public class Post : DomainModel
    {
        public string Title { get; set; }

        public string Context { get; set; }

        public DateTime Date { get; set; }

        public List<string> Tags { get; set; }
    }
}
