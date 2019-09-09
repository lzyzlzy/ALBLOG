using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    public class ReturnModel
    {
        public bool IsSuccess { get; set; }

        public string Message { get; set; }

        public object Data { get; set; }
    }
}
