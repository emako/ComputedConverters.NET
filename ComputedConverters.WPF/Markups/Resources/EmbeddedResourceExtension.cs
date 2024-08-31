﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Resources;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters;

[MarkupExtensionReturnType(typeof(object))]
public sealed class EmbeddedResourceExtension(ComponentResourceKey key) : I18nExtension(key)
{
}

[MarkupExtensionReturnType(typeof(object))]
public class I18nExtension(ComponentResourceKey key) : MarkupExtension
{
    private static readonly I18nResourceConverter I18NResourceConverter = new();

    [ConstructorArgument(nameof(Key))]
    public ComponentResourceKey Key { get; set; } = key;

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

        return new Binding(nameof(I18nSource.Value))
        {
            Source = new I18nSource(Key, provideValueTarget.TargetObject),
            Mode = BindingMode.OneWay,
            Converter = I18NResourceConverter
        }.ProvideValue(serviceProvider);
    }

    private class I18nResourceConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
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
            throw new ArgumentException($"The ResourceManager named {resourceManager.BaseName} already exists, cannot be added repeatedly. ", nameof(resourceManager));

        _resourceManagerStorage[resourceManager.BaseName] = resourceManager;
    }

    public object Get(ComponentResourceKey key)
    {
        return GetCurrentResourceManager(key.TypeInTargetAssembly.FullName!)?
            .GetObject(key.ResourceId.ToString()!, CurrentUICulture) ?? $"<MISSING: {key}>";
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
