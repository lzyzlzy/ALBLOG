using System.Collections.Generic;

namespace ALBLOG.Domain.Model
{
    public class Page
    {
        public IEnumerable<Post> Posts { get; set; }

        public bool HaveLast { get; set; }

        public bool HaveNext { get; set; }

        public int Index { get; set; }

        public int PageCount { get; set; }

        public int Size { get; set; }

    }
}
