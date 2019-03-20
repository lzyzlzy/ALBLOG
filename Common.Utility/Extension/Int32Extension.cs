// ReSharper disable CheckNamespace

namespace System
// ReSharper restore CheckNamespace
{
    /// <Summary>
    ///     Extended the System.Int32 class
    /// </Summary>
    public static class Int32Extension
    {
        /// <summary>
        ///     Convert a int value to a specific enum value
        /// </summary>
        /// <typeparam name="T">the enum type</typeparam>
        /// <param name="value">the underline int value</param>
        /// <returns>the result of enum value</returns>
        public static T ToEnum<T>(this Int32 value)
        {
            return Enum.GetName(typeof(T), value).ToEnum<T>();
        }

        public static string ToJobNumberString(this Int32 value)
        {
            var str = value.ToString();
            if (value.ToString().Length < 5)
            {
                for (int i = 0; i < 5 - str.Length; i++)
                {
                    str = str.Insert(0, "0");
                }
            }
            return str;
        }

       
    }
}