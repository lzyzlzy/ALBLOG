using System;
using System.Collections.Generic;
using System.Text;

namespace ALBLOG.Domain.Model
{
    public enum LogType
    {
        Log = 1,

        Error = 2,

        Assert = 4,

        Warning = 8,

        Exception = 16
    }
}
