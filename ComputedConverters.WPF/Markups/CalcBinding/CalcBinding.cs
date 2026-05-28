using ComputedConverters.CalcBinding.ExpressionParsers;
using ComputedConverters.CalcBinding.PathAnalysis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace ComputedConverters.CalcBinding;

/// <summary>
/// Binding with expression support
/// </summary>
public class Binding : MarkupExtension
{
    // We cannot use PropertyPath instead of string (such as standard Binding) because transformation from xaml value string to PropertyPath
    // is done automatically by PropertyPathConverter and the result PropertyPath object could have a form that cannot be retranslated to a normal string.
    // e.g.: (local:MyStaticVM.Prop) -> PropertyPath.Path = (0), Converted to string = MyStaticVM.Prop (but we need to analyze static class with xaml namespace prefix)
    public string Path { get; set; }

    /// <summary>
    /// False to visibility. Default: False = Collapsed
    /// </summary>
    public FalseToVisibility FalseToVisibility { get; set; } = FalseToVisibility.Collapsed;

    /// <summary>
    /// If true then single quotes and double quotes are considered as single quotes, otherwise - both are considered as double quotes
    /// </summary>
    /// <remarks>
    /// Use this flag if you need to use char in path expression
    /// </remarks>
    public bool SingleQuotes { get; set; } = false;

    /// <summary>Value to use when source cannot provide a value</summary>
    /// <remarks>
    ///     Initialized to DependencyProperty.UnsetValue; if FallbackValue is not set, BindingExpression
    ///     will return target property's default when Binding cannot get a real value.
    /// </remarks>
    public object FallbackValue { get; set; } = DependencyProperty.UnsetValue;

    public Binding()
    {
        Mode = BindingMode.Default;
    }

    public Binding(string path)
        : this()
    {
        Path = path;
    }

    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        Type targetPropertyType = GetPropertyType(serviceProvider);
        IXamlTypeResolver typeResolver = (IXamlTypeResolver)serviceProvider.GetService(typeof(IXamlTypeResolver));
        ITypeDescriptorContext typeDescriptor = serviceProvider as ITypeDescriptorContext;

        string normalizedPath = NormalizePath(Path);
        List<PathAppearances> pathes = GetSourcePathes(normalizedPath, typeResolver);

        string expressionTemplate = GetExpressionTemplate(normalizedPath, pathes, out Dictionary<string, Type> enumParameters);

        CalcConverter mathConverter = new CalcConverter(_parser.Value, FallbackValue, enumParameters)
        {
            FalseToVisibility = FalseToVisibility,
            StringFormatDefined = StringFormat != null,
        };

        List<PathAppearances> bindingPathes = pathes
            .Where(p => p.PathId.PathType == PathTokenType.Property ||
                        p.PathId.PathType == PathTokenType.StaticProperty).ToList();

        BindingBase resBinding;

        if (bindingPathes.Count == 1)
        {
            System.Windows.Data.Binding binding = new System.Windows.Data.Binding()
            {
                Mode = Mode,
                NotifyOnSourceUpdated = NotifyOnSourceUpdated,
                NotifyOnTargetUpdated = NotifyOnTargetUpdated,
                NotifyOnValidationError = NotifyOnValidationError,
                UpdateSourceExceptionFilter = UpdateSourceExceptionFilter,
                UpdateSourceTrigger = UpdateSourceTrigger,
                ValidatesOnDataErrors = ValidatesOnDataErrors,
                ValidatesOnExceptions = ValidatesOnExceptions,
                ValidatesOnNotifyDataErrors = ValidatesOnNotifyDataErrors,
                FallbackValue = FallbackValue,
            };

            PathTokenId pathId = bindingPathes.Single().PathId;

            // we need to use convert from string for support of static properties
            string pathValue = pathId.Value;

            if (pathId.PathType == PathTokenType.StaticProperty)
            {
                pathValue = string.Format("({0})", pathValue);  // need to use brackets for Static property recognition in standard binding
            }
            PropertyPath resPath = (PropertyPath)new PropertyPathConverter().ConvertFromString(typeDescriptor, pathValue);
            binding.Path = resPath;

            if (Source != null)
                binding.Source = Source;

            if (ElementName != null)
                binding.ElementName = ElementName;

            if (RelativeSource != null)
                binding.RelativeSource = RelativeSource;

            if (StringFormat != null)
                binding.StringFormat = StringFormat;

            // we don't use converter if binding is trivial - {0}, except type conversion from bool to visibility
            if ((expressionTemplate != "{0}" && expressionTemplate != "({0})") || targetPropertyType == typeof(Visibility))
            {
                binding.Converter = mathConverter;
                binding.ConverterParameter = expressionTemplate;
                binding.ConverterCulture = ConverterCulture;
            }
            resBinding = binding;
        }
        else
        {
            MultiBinding mBinding = new MultiBinding
            {
                Converter = mathConverter,
                ConverterParameter = expressionTemplate,
                ConverterCulture = ConverterCulture,
                Mode = BindingMode.OneWay,
                NotifyOnSourceUpdated = NotifyOnSourceUpdated,
                NotifyOnTargetUpdated = NotifyOnTargetUpdated,
                NotifyOnValidationError = NotifyOnValidationError,
                UpdateSourceExceptionFilter = UpdateSourceExceptionFilter,
                UpdateSourceTrigger = UpdateSourceTrigger,
                ValidatesOnDataErrors = ValidatesOnDataErrors,
                ValidatesOnExceptions = ValidatesOnExceptions,
                FallbackValue = FallbackValue,
            };

            if (StringFormat != null)
                mBinding.StringFormat = StringFormat;

            foreach (PathAppearances path in bindingPathes)
            {
                System.Windows.Data.Binding binding = new System.Windows.Data.Binding();

                // we need to use convert from string for support of static properties
                string pathValue = path.PathId.Value;

                if (path.PathId.PathType == PathTokenType.StaticProperty)
                {
                    pathValue = string.Format("({0})", pathValue);
                }

                PropertyPath resPath = (PropertyPath)new PropertyPathConverter().ConvertFromString(typeDescriptor, pathValue);

                binding.Path = resPath;

                if (Source != null)
                    binding.Source = Source;

                if (ElementName != null)
                    binding.ElementName = ElementName;

                if (RelativeSource != null)
                    binding.RelativeSource = RelativeSource;

                mBinding.Bindings.Add(binding);
            }

            resBinding = mBinding;
        }

        return resBinding.ProvideValue(serviceProvider);
    }

    [EditorBrowsable(EditorBrowsableState.Never)]
    public static void ReplaceExpressionParser(IExpressionParser expressionParser)
    {
        if (expressionParser == null)
            throw new ArgumentNullException(nameof(expressionParser));

        _parser = new Lazy<IExpressionParser>(() => expressionParser);
    }

    private Type GetPropertyType(IServiceProvider serviceProvider)
    {
        IProvideValueTarget targetProvider = (IProvideValueTarget)serviceProvider
            .GetService(typeof(IProvideValueTarget));

        if (targetProvider.TargetProperty is DependencyProperty)
        {
            return ((DependencyProperty)targetProvider.TargetProperty).PropertyType;
        }

        return targetProvider.TargetProperty.GetType();
    }

    /// <summary>
    /// Replace source properties pathes by its numbers
    /// </summary>
    private string GetExpressionTemplate(string path, List<PathAppearances> properties, out Dictionary<string, Type> enumParameters)
    {
        string result = "";
        int sourceIndex = 0;

        Dictionary<PathTokenId, string> passedProps = new Dictionary<PathTokenId, string>();
        Dictionary<PathTokenId, string> enumNames = new Dictionary<PathTokenId, string>();

        enumParameters = new Dictionary<string, Type>();

        while (sourceIndex < path.Length)
        {
            bool replaced = false;
            for (int index = 0; index < properties.Count(); index++)
            {
                PathAppearances propGroup = properties[index];
                PathTokenId propId = propGroup.PathId;
                PathToken targetProp = propGroup.Pathes.FirstOrDefault(token => token.Start == sourceIndex);

                if (targetProp != null)
                {
                    string propPath = propId.Value;

                    if (propId.PathType == PathTokenType.Property || propId.PathType == PathTokenType.StaticProperty)
                    {
                        string replace = null;
                        if (passedProps.ContainsKey(propId))
                        {
                            replace = passedProps[propId];
                        }
                        else
                        {
                            replace = (passedProps.Count).ToString("{0}");
                            passedProps.Add(propId, replace);
                        }

                        result += replace;
                        sourceIndex += propPath.Length;
                        replaced = true;
                    }
                    else if (propId.PathType == PathTokenType.Enum)
                    {
                        EnumToken? enumPath = propGroup.Pathes.First() as EnumToken;

                        string? enumTypeName = null;
                        if (enumNames.TryGetValue(propId, out string? value))
                        {
                            enumTypeName = value;
                        }
                        else
                        {
                            enumTypeName = GetEnumName(enumNames.Count);
                            enumNames.Add(propId, enumTypeName);
                            enumParameters.Add(enumTypeName, enumPath!.Enum);
                        }

                        string replace = string.Join(".", enumTypeName, enumPath!.EnumMember);

                        result += replace;
                        sourceIndex += propPath.Length;
                        replaced = true;
                    }
                    if (replaced)
                        break;
                }
            }

            if (!replaced)
            {
                result += path[sourceIndex];
                sourceIndex++;
            }
        }

        return result;
    }

    private string GetEnumName(int i)
    {
        return string.Format("Enum{0}", ++i);
    }

    /// <summary>
    /// Find all sourceProperties pathes in Path string
    /// </summary>
    private List<PathAppearances> GetSourcePathes(string normPath, IXamlTypeResolver typeResolver)
    {
        PropertyPathAnalyzer propertyPathAnalyzer = new PropertyPathAnalyzer();

        List<PathToken> pathes = propertyPathAnalyzer.GetPathes(normPath, typeResolver);

        List<PathAppearances> propertiesGroups = pathes.GroupBy(p => p.Id).Select(p => new PathAppearances(p.Key, p.ToList())).ToList();

        return propertiesGroups;
    }

    /// <summary>
    /// Replace operators labels to operators names (ex. and -> &amp;&amp;), remove excess spaces
    /// </summary>
    private string NormalizePath(string path)
    {
        Dictionary<string, string> replaceDict = new Dictionary<string, string>
        {
            {" and ",     " && "},
            {")and ",     ")&& "},
            {" and(",     " &&("},
            {")and(",     ")&&("},

            {" or ",      " || "},
            {")or ",      ")|| "},
            {" or(",      " ||("},
            {")or(",      ")||("},

            {" less ",    " < "},
            {")less ",    ")< "},
            {" less(",    " <("},
            {")less(",    ")<("},

            {" less=",   " <="},
            {")less=",   ")<="},

            {"not ",    "!"}
        };

        if (!SingleQuotes)
            replaceDict.Add("\'", "\"");
        else
            replaceDict.Add("\"", "\'");

        string normPath = path;
        foreach (KeyValuePair<string, string> pair in replaceDict)
            normPath = normPath.Replace(pair.Key, pair.Value);

        return normPath;
    }

    #region Binding Properties

    [DefaultValue("")]
    public IMultiValueConverter Converter { get; set; }

    [DefaultValue("")]
    [TypeConverter(typeof(CultureInfoIetfLanguageTagConverter))]
    public CultureInfo ConverterCulture { get; set; }

    [DefaultValue("")]
    public object ConverterParameter { get; set; }

    [DefaultValue(BindingMode.Default)]
    public BindingMode Mode { get; set; }

    [DefaultValue(false)]
    public bool NotifyOnSourceUpdated { get; set; }

    [DefaultValue(false)]
    public bool NotifyOnTargetUpdated { get; set; }

    [DefaultValue(false)]
    public bool NotifyOnValidationError { get; set; }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public UpdateSourceExceptionFilterCallback UpdateSourceExceptionFilter { get; set; }

    public UpdateSourceTrigger UpdateSourceTrigger { get; set; }

    [DefaultValue(false)]
    public bool ValidatesOnDataErrors { get; set; }

    [DefaultValue(false)]
    public bool ValidatesOnExceptions { get; set; }

    [DefaultValue(true)]
    public bool ValidatesOnNotifyDataErrors { get; set; }

    [DefaultValue("")]
    public RelativeSource RelativeSource { get; set; }

    public object Source { get; set; }

    [DefaultValue("")]
    public string ElementName { get; set; }

    [DefaultValue("")]
    public string StringFormat { get; set; }

    #endregion Binding Properties

    private static Lazy<IExpressionParser> _parser = new Lazy<IExpressionParser>(() => new ParserFactory().CreateCachedParser());

    private class PathAppearances
    {
        public PathTokenId PathId { get; private set; }

        public IEnumerable<PathToken> Pathes { get; private set; }

        public PathAppearances(PathTokenId id, List<PathToken> pathes)
        {
            PathId = id;
            Pathes = pathes;
        }
    }
}
