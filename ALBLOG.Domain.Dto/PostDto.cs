using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Dto
{
    public class PostDto
    {
        public string id { get; set; }

        public string title { get; set; }

        public string tags { get; set; }

        public string context { get; set; }

        public string MarkDown { get; set; }
    }
}
