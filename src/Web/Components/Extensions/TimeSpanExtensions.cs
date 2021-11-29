namespace System;

public static class TimeSpanExtensions
{
    public static string ToDurationString(this TimeSpan value)
    {
        var format = value.TotalHours >= 1 ? "h\\:mm\\:ss" : "mm\\:ss";
        return value.ToString(format);
    }
}