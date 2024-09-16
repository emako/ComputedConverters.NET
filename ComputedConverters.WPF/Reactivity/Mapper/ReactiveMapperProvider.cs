using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace ComputedConverters;

public static class ReactiveMapperProvider
{
    public static IReactiveMapper? Service { get; }

    static ReactiveMapperProvider()
    {
        try
        {
            ReactiveMapperConfiguration configuration = new(CreateMap);
#if DEBUG
            configuration.AssertConfigurationIsValid();
#endif
            Service = configuration.CreateMapper();
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
            Debugger.Break();
        }
    }

    private static void CreateMap(IReactiveMapperConfigurationExpression cfg)
    {
        foreach (Assembly assembly in ReactiveMapperAssemblyResolver.Resolve())
        {
            if (assembly == null)
            {
                continue;
            }

            var types = assembly.GetTypes()
                .Where(t => t.GetCustomAttribute<ReactiveMapperIndicatorAttribute>() != null
                        || typeof(IReactiveMapperIndicator).IsAssignableFrom(t));

            foreach (var type in types)
            {
                try
                {
                    var instance = (IReactiveMapperIndicator)Activator.CreateInstance(type)!;
                    instance.CreateMap(cfg);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }
        }
    }
}
