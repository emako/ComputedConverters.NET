using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;
using System.Windows.Data;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xaml;
using static ComputedConverters.Interop;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class EmbeddedResourceExtension(string key) : I18nExtension(key)
{
}

[MarkupExtensionReturnType(typeof(object))]
public class I18nExtension(string key) : MarkupExtension
{
    [ConstructorArgument(nameof(Key))]
    public string Key { get; set; } = key;

    public override object? ProvideValue(IServiceProvider serviceProvider)
    {
        if (Key == null)
        {
            throw new NullReferenceException($"{nameof(Key)} cannot be null at the same time.");
        }

        if (serviceProvider.GetService(typeof(IProvideValueTarget)) is not IProvideValueTarget provideValueTarget)
        {
            throw new ArgumentException($"The {nameof(serviceProvider)} must implement {nameof(IProvideValueTarget)} interface.");
        }

        if (provideValueTarget.TargetObject?.GetType().FullName == "System.Windows.SharedDp")
        {
            return this;
        }

        if (serviceProvider.GetService(typeof(IRootObjectProvider)) is not IRootObjectProvider rootObjectProvider)
        {
            throw new ArgumentException($"The {nameof(serviceProvider)} must implement {nameof(IRootObjectProvider)} interface.");
        }

        Type typeInTargetAssembly = rootObjectProvider.RootObject.GetType().Assembly.DefinedTypes.Where(t => t.FullName?.EndsWith(".Properties.Resources") ?? false).FirstOrDefault()
            ?? throw new InvalidOperationException($"File `Resources.Designer.cs` can not be found from requested assembly.");

        if (!I18nManager.Instance.Contains(typeInTargetAssembly.FullName!))
        {
            // Store ResourceManager from `Properties.Resources.ResourceManager`.
            PropertyInfo resourceManagerProperty = typeInTargetAssembly.GetProperty("ResourceManager", BindingFlags.Static | BindingFlags.Public)
                 ?? throw new InvalidOperationException($"Property `ResourceManager` can not be found from file `Resources.Designer.cs`.");
            ResourceManager resourceManager = (ResourceManager)resourceManagerProperty.GetValue(null)!;

            I18nManager.Instance.Add(resourceManager);
        }

        return new Binding(nameof(I18nSource.Value))
        {
            Source = new I18nSource(new ComponentResourceKey(typeInTargetAssembly, Key), provideValueTarget.TargetObject),
            Mode = BindingMode.OneWay,
            Converter = I18nResourceConverter.Instance,
        }.ProvideValue(serviceProvider);
    }

    private class I18nResourceConverter : IValueConverter
    {
        private static readonly Lazy<I18nResourceConverter> _instance = new(() => new I18nResourceConverter(), LazyThreadSafetyMode.PublicationOnly);

        public static I18nResourceConverter Instance => _instance.Value;

        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            if (value.GetType().FullName == "System.Drawing.Bitmap")
            {
                dynamic bitmap = value;
                nint hBitmap = bitmap.GetHbitmap();
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions()
                );

                _ = Gdi32.DeleteObject(hBitmap);
                return imageSource;
            }
            else if (value.GetType().FullName == "System.Drawing.Image")
            {
                dynamic image = value;
                using MemoryStream memoryStream = new();
                dynamic imageFormatPng = value.GetType().Assembly
                    .GetType("System.Drawing.Imaging.ImageFormat")!
                    .GetField("Png", BindingFlags.Public | BindingFlags.Static)!
                    .GetValue(null)!;

                image.Save(memoryStream, imageFormatPng);

                BitmapImage imageSource = new();

                imageSource.BeginInit();
                imageSource.StreamSource = memoryStream;
                imageSource.CacheOption = BitmapCacheOption.OnLoad;
                imageSource.EndInit();
                imageSource.Freeze();

                return imageSource;
            }
            else if (value.GetType().FullName == "System.Drawing.Icon")
            {
                dynamic icon = value;
                ImageSource imageSource = Imaging.CreateBitmapSourceFromHIcon(
                    icon.Handle,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());

                return imageSource;
            }

            return value;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}

public class I18nSource : INotifyPropertyChanged
{
    private readonly ComponentResourceKey _key;

    public event PropertyChangedEventHandler? PropertyChanged;

    public I18nSource(ComponentResourceKey key, object? owner = null)
    {
        _key = key;

        switch (owner)
        {
            case FrameworkElement frameworkElement:
                frameworkElement.Loaded += OnLoaded;
                frameworkElement.Unloaded += OnUnloaded;
                break;

            case FrameworkContentElement frameworkContentElement:
                frameworkContentElement.Loaded += OnLoaded;
                frameworkContentElement.Unloaded += OnUnloaded;
                break;

            default:
                OnLoaded(null!, null!);
                break;
        }
    }

    public object Value => I18nManager.Instance.Get(_key);

    private void OnLoaded(object sender, RoutedEventArgs e)
    {
        OnCurrentUICultureChanged(sender, null!);
        I18nManager.Instance.CurrentUICultureChanged += OnCurrentUICultureChanged;
    }

    private void OnUnloaded(object sender, RoutedEventArgs e)
    {
        I18nManager.Instance.CurrentUICultureChanged -= OnCurrentUICultureChanged;
    }

    private void OnCurrentUICultureChanged(object? sender, CurrentUICultureChangedEventArgs e)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
    }

    public override string ToString() => Value?.ToString() ?? string.Empty;

    public static implicit operator I18nSource(ComponentResourceKey resourceKey) => new(resourceKey);
}

public class I18nManager : INotifyPropertyChanged
{
    public static I18nManager Instance { get; } = new();

    private readonly ConcurrentDictionary<string, ResourceManager> _resourceManagerStorage = new();
    private CultureInfo _currentUICulture = null!;

    public event EventHandler<CurrentUICultureChangedEventArgs>? CurrentUICultureChanged;

    public event PropertyChangedEventHandler? PropertyChanged;

    private I18nManager()
    {
    }

    public CultureInfo CurrentUICulture
    {
        get => _currentUICulture;
        set
        {
            if (EqualityComparer<CultureInfo>.Default.Equals(_currentUICulture, value)) return;

            OnCurrentUICultureChanged(_currentUICulture, _currentUICulture = value);
            OnPropertyChanged(nameof(CurrentUICulture));
        }
    }

    public void Add(ResourceManager resourceManager)
    {
        if (_resourceManagerStorage.ContainsKey(resourceManager.BaseName))
        {
            throw new ArgumentException($"The ResourceManager named {resourceManager.BaseName} already exists, cannot be added repeatedly. ", nameof(resourceManager));
        }

        _resourceManagerStorage[resourceManager.BaseName] = resourceManager;
    }

    public object Get(ComponentResourceKey key)
    {
        return GetCurrentResourceManager(key.TypeInTargetAssembly.FullName!)?
            .GetObject(key.ResourceId.ToString()!, CurrentUICulture) ?? $"<MISSING: {key}>";
    }

    public bool Contains(string key)
    {
        return _resourceManagerStorage.ContainsKey(key);
    }

    private ResourceManager GetCurrentResourceManager(string key)
    {
        return _resourceManagerStorage.TryGetValue(key, out var value) ? value : null!;
    }

    protected virtual void OnCurrentUICultureChanged(CultureInfo oldCulture, CultureInfo newCulture)
    {
        CurrentUICultureChanged?.Invoke(this, new CurrentUICultureChangedEventArgs(oldCulture, newCulture));
    }

    protected virtual void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

public sealed class CurrentUICultureChangedEventArgs(CultureInfo oldUiCulture, CultureInfo newUiCulture) : EventArgs
{
    public CultureInfo OldUICulture { get; } = oldUiCulture;

    public CultureInfo NewUICulture { get; } = newUiCulture;
}
