namespace Common.Utility.Datetime
{
    using Newtonsoft.Json.Linq;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Collections.Generic;
    using System.Globalization;

    public class DateTimeHelper
    {
        public static DateTime GetNextWorkday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Friday)
            {
                return date.AddDays(3);
            }
            if (date.DayOfWeek == DayOfWeek.Saturday)
            {
                return date.AddDays(2);
            }
            return date.AddDays(1);
        }

        public static DateTime GetPreviousWorkday(DateTime date)
        {
            if (date.DayOfWeek == DayOfWeek.Monday)
            {
                return date.AddDays(-3);
            }
            if (date.DayOfWeek == DayOfWeek.Sunday)
            {
                return date.AddDays(-2);
            }
            return date.AddDays(-1);
        }

        public static DateTime GetSpringFestivalDateByYear(int year)
        {
            return new DateTime(year, 1, 1, new ChineseLunisolarCalendar());
        }
    }
}