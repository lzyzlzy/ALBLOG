using Common.MongoDBClient.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    [Collection("ALBLOG_Posts")]
    public class Post : DomainModel
    {
        public string Title { get; set; }

        public string Context { get; set; }

        public string MarkDown { get; set; }

        public DateTime Date { get; set; }

        public DateTime? EditDate { get; set; }

        public List<string> Tags { get; set; }

        public string UserName { get; set; }

        public bool IsDraft { get; set; }

        public int? PageViews { get; set; }

        public static Post CreateEmptyPost()
        {
            return new Post
            {
                Title = "",
                Context = "",
                MarkDown="",
                Tags = new List<string> { }
            };
        }
    }
}
