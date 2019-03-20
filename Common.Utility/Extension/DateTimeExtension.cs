namespace System
{
    using Common.Utility.Datetime;
    using System.Collections.Generic;
    using System.Globalization;

    /// <Summary>
    ///     Extended the System.DateTime structure
    /// </Summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Converts a DateTime to a javascript timestamp.
        /// http://stackoverflow.com/a/5117291/13932
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns>The javascript timestamp.</returns>
        public static long ToJavaScriptTimestamp(this DateTime input)
        {
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var time = input.Subtract(new TimeSpan(epoch.Ticks));
            return (long)(time.Ticks / 10000);
        }

        /// <summary>
        ///     convert a date time to java time in long
        /// </summary>
        /// <param name="dateTime">date time</param>
        /// <returns>the java date time in long type</returns>
        public static Int64 DotNetToJavaTime(this DateTime dateTime)
        {
            return dateTime.Ticks.DotNetToJavaTime();
        }

        /// <summary>
        ///     convert a date time to java time in long
        /// </summary>
        /// <param name="dateTime">date time</param>
        /// <returns>the java date time in long type</returns>
        public static Int64 ToJavaTime(this DateTime dateTime)
        {
            return dateTime.Ticks.DotNetToJavaTime();
        }

        /// <summary>
        ///     Unix time is offset second of 1970, 1, 1, 0, 0, 0
        /// </summary>
        /// <param name="dateTime" />
        /// <returns></returns>
        public static Int64 ToUnixTime(this DateTime dateTime)
        {
            return dateTime.Ticks.DotNetToJavaTime() / 1000L;
        }

        /// <summary>
        ///     Windows file  time is offset second of 1600, 1, 1, 0, 0, 0
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static Int64 ToWindowsFileTime(this DateTime dateTime)
        {
            return dateTime.ToFileTimeUtc();
        }

        public static String ToRfc822TimeFormatString(this DateTime datetime)
        {
            return new Rfc822DateTime(datetime.ToUniversalTime()).ToString(TimeZoneInfo.Utc);
        }

        public static bool IsWorkDay(this DateTime date)
        {
            int flag = 0;
            return (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday || date.ToString("M.d") == "7.4" || date.ToString("M.d") == "12.25" || date.IsInSpringFestivalHolidays()) && (!IsDayoff(date, out flag)) ? false : true;
        }

        /// <summary>
        /// 是否在春节七天假之内
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static bool IsInSpringFestivalHolidays(this DateTime time)
        {
            DateTime holiday = DateTimeHelper.GetSpringFestivalDateByYear(time.Year).AddDays(-1);
            return time.Year == holiday.Year && time.DayOfYear >= holiday.DayOfYear && time.DayOfYear <= holiday.AddDays(6).DayOfYear ;
        }

        public static bool IsSpringFestivalEve(this DateTime time)
        {
            DateTime holiday = DateTimeHelper.GetSpringFestivalDateByYear(time.Year).AddDays(-1);
            return holiday.DayOfYear == time.DayOfYear;
        }

        public static bool IsDayoff(DateTime date, out int flag)
        {
            flag = 0;
            //判断是否是周六与周日
            if (date.DayOfWeek == DayOfWeek.Sunday || date.DayOfWeek == DayOfWeek.Saturday)
            {
                int month = date.Month;
                //春节日期一般在公历的一月二十一日之后与二月二十日浮动
                if (month == 1 || month == 2)
                {
                    //获取春节假期的前两天
                    DateTime chuyi = new DateTime(date.Year, 1, 1, new ChineseLunisolarCalendar());
                    DateTime chuxi = chuyi.AddDays(-1);
                    //获取春节假期的后两天
                    DateTime chuwu = chuyi.AddDays(4);
                    DateTime chuliu = chuwu.AddDays(1);

                    if (chuxi.DayOfWeek == DayOfWeek.Saturday && chuyi.DayOfWeek == DayOfWeek.Sunday)
                    {
                        DateTime workday1 = chuliu.AddDays(1);
                        DateTime workday2 = workday1.AddDays(1);
                        return date.Date == workday1.Date || date.Date == workday2.Date;
                    }
                    else if (chuwu.DayOfWeek == DayOfWeek.Saturday && chuliu.DayOfWeek == DayOfWeek.Sunday)//会安排半天的工作日
                    {
                        DateTime workday1 = chuxi.AddDays(-2);
                        DateTime workday2 = chuxi.AddDays(-1);
                        if (date.Date == workday2.Date)
                        {
                            flag = 1;
                        }
                        return date.Date == workday1.Date || date.Date == workday2.Date;
                    }
                    else if (chuxi.DayOfWeek == DayOfWeek.Sunday && chuliu.DayOfWeek == DayOfWeek.Saturday)//会安排半天的工作日
                    {
                        DateTime workday1 = chuxi.AddDays(-1);
                        DateTime workday2 = chuliu.AddDays(1);
                        if (date.Date == workday1.Date)
                        {
                            flag = 1;
                        }
                        return date.Date == workday1.Date || date.Date == workday2.Date;
                    }
                    else
                    {
                        DateTime start = chuxi;
                        DateTime end = chuliu;
                        while (start.DayOfWeek != DayOfWeek.Sunday)
                        {
                            start = start.AddDays(-1);
                        }
                        while (end.DayOfWeek != DayOfWeek.Saturday)
                        {
                            end = end.AddDays(1);
                        }
                        return date.Date == start.Date || date.Date == end.Date;
                    }
                }
            }
            return false;
        }
    }
}