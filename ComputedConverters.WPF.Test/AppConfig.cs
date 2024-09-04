using System.Reflection;

namespace ComputedConverters.Test;

public static class AppConfig
{
    public static string Version => Assembly.GetExecutingAssembly().GetName().Version!.ToString(3);
}
