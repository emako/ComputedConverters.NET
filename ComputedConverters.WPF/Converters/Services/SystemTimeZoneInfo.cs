using System;
using System.Threading;

namespace ComputedConverters.Services;

internal sealed class SystemTimeZoneInfo : ITimeZoneInfo
{
    private static readonly Lazy<ITimeZoneInfo> Implementation = new(() => new SystemTimeZoneInfo(), LazyThreadSafetyMode.PublicationOnly);

    public static ITimeZoneInfo Current => Implementation.Value;

    private SystemTimeZoneInfo()
    {
    }

    public TimeZoneInfo Utc => TimeZoneInfo.Utc;

    public TimeZoneInfo Local => TimeZoneInfo.Local;
}
