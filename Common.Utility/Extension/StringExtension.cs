// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
{
    #region using directives

    using CodeDom.Compiler;
    using Collections.Generic;
    using Diagnostics.Contracts;
    using Microsoft.CSharp;
    using Runtime.InteropServices;
    using Security;
    using Text;
    using Text.RegularExpressions;

    #endregion using directives

#pragma warning disable 1587
    /// <Summary>
    ///     extension of the System.String class
    /// </Summary>
#pragma warning restore 1587

    public static class StringExtension
    {
        public static SecureString StringToSecureString(this string value)
        {
            var password = new SecureString();
            var pass = value.ToCharArray();
            foreach (var t in pass)
            {
                password.AppendChar(t);
            }
            return password;
        }

        public static string SecureStringToString(this SecureString value)
        {
            var bstr = Marshal.SecureStringToBSTR(value);
            try
            {
                return Marshal.PtrToStringBSTR(bstr);
            }
            finally
            {
                Marshal.FreeBSTR(bstr);
            }
        }




        /// <summary>
        ///     To test if the string is null or empty at instance level
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the test result</returns>
        public static Boolean IsNullOrEmpty(this String value)
        {
            return String.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     To test if the string is not null or empty at instance level
        /// </summary>
        /// <param name="value">the string value</param>
        /// <returns>the test result</returns>
        [Pure]
        public static Boolean IsNotNullOrEmpty(this String value)
        {
            return !String.IsNullOrEmpty(value);
        }

        /// <summary>
        ///     java long string time to dot net time in ticks
        /// </summary>
        /// <param name="javaTimeInLongString"></param>
        /// <returns></returns>
        public static Int64 JavaToDotNetTimeInLong(this String javaTimeInLongString)
        {
            var time = Int64.Parse(javaTimeInLongString);
            return time.JavaToDotNetTimeInLong();
        }

        /// <summary>
        ///     convert dot net date time ticks to java long time
        /// </summary>
        /// <param name="timeInLong"></param>
        /// <returns></returns>
        public static Int64 DotNetToJavaTime(this String timeInLong)
        {
            var time = Int64.Parse(timeInLong);
            return time.DotNetToJavaTime();
        }

        /// <summary>
        ///     format string with instance
        /// </summary>
        /// <param name="format"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static String FormatWith(this String format, params Object[] args)
        {
            return String.Format(format, args);
        }

        /// <summary>
        ///     wrap the is match method of the regex
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static Boolean IsMatch(this String s, String pattern)
        {
            return s != null && Regex.IsMatch(s, pattern, RegexOptions.IgnoreCase);
        }

        /// <summary>
        ///     wrap the regex class match method
        /// </summary>
        /// <param name="s"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static String Match(this String s, String pattern)
        {
            return s == null ? String.Empty : Regex.Match(s, pattern).Value;
        }

        /// <summary>
        ///     the string class if extension
        /// </summary>
        /// <param name="value"></param>
        /// <param name="predicate"></param>
        /// <param name="function"></param>
        /// <returns></returns>
        public static String If(this String value, Predicate<String> predicate, Func<String, String> function)
        {
            return predicate(value) ? function(value) : value;
        }

        /// <summary>
        ///     convert string to int32
        /// </summary>
        /// <param name="value">string value</param>
        /// <returns>the converted int value</returns>
        public static Int32 ToInt32(this String value)
        {
            return Convert.ToInt32(value);
        }

        /// <summary>
        ///     Convert String to Enum
        /// </summary>
        /// <typeparam name="T">the enum type</typeparam>
        /// <param name="value">the enum constant string</param>
        /// <returns>converted enum value</returns>
        public static T ToEnum<T>(this String value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        /// <summary>
        ///     equals method with ignore case
        /// </summary>
        /// <param name="currentValue">current string value</param>
        /// <param name="compareValue">compare string value</param>
        /// <returns>the equals result</returns>
        [Pure]
        public static Boolean EqualsIgnoreCase(this String currentValue, String compareValue)
        {
            return currentValue.Equals(compareValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     index of method with ignore case
        /// </summary>
        /// <param name="currentValue">current string value</param>
        /// <param name="compareValue">compare string value</param>
        /// <returns>the index of result</returns>
        public static Int32 IndexOfIgnoreCase(this String currentValue, String compareValue)
        {
            return currentValue.IndexOf(compareValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     last index of method with ignore case
        /// </summary>
        /// <param name="currentValue">current string value</param>
        /// <param name="compareValue">compare string value</param>
        /// <returns>the last index of result</returns>
        public static Int32 LastIndexOfIgnoreCase(this String currentValue, String compareValue)
        {
            return currentValue.LastIndexOf(compareValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     equals method with ignore case
        /// </summary>
        /// <param name="currentValue">current string value</param>
        /// <param name="endValue">end string value</param>
        /// <returns>the end with string result</returns>
        public static Boolean EndWithIgnoreCase(this String currentValue, String endValue)
        {
            return currentValue.EndsWith(endValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Compare two string
        /// </summary>
        /// <param name="currentValue">first string</param>
        /// <param name="compareValue">compare string</param>
        /// <returns>compare result</returns>
        public static Int32 CompareToIngnoreCase(this String currentValue, String compareValue)
        {
            return String.Compare(currentValue, compareValue, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        ///     Replaces the first occurrence of a specified System.String in this instance, with another specified System.String.
        /// </summary>
        /// <param name="currentValue">current string value</param>
        /// <param name="oldValue">old value</param>
        /// <param name="newValue">new value</param>
        /// <returns>replace result</returns>
        public static String ReplaceFirst(this String currentValue, String oldValue, String newValue)
        {
            var offset = currentValue.IndexOf(oldValue, StringComparison.OrdinalIgnoreCase);
            var temp = currentValue.Remove(offset, oldValue.Length);
            return temp.Insert(offset, newValue);
        }
     
        public static string ToJobNumberString(this string value)
        {
            var str = value.ToString();
            if (value.Length < 5)
            {
                for (int i = 0; i < 5 - str.Length; i++)
                {
                    str = str.Insert(0, "0");
                }
            }
            return str;
        }
        public static DateTime ToDateTime(this string value)
        {
            return DateTime.Parse(value);
        }
    }
}