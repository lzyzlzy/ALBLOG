using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Dto
{
    public class ReturnDto
    {
        public string State { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
