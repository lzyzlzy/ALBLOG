namespace ALBLOG.Domain.Model
{
    using System.ComponentModel;

    public enum PlatformType
    {
        [Description("Unknown")]
        Unknown,

        [Description("Windows")]
        Windows,

        [Description("Apple")]
        Apple,

        [Description("Android")]
        Android
    }
}