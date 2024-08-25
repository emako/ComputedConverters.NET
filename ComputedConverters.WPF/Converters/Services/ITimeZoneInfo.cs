using System;

namespace ComputedConverters.Services;

internal interface ITimeZoneInfo
{
    public TimeZoneInfo Utc { get; }

    public TimeZoneInfo Local { get; }
}
