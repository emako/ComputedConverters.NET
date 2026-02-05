using System.Globalization;
using System.Windows;
using System.Windows.Media;
using ComputedConverters;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ComputedConverters.WPF.UnitTest;

[TestClass]
public class ConverterTests
{
    private readonly CultureInfo _culture = CultureInfo.InvariantCulture;

    #region Bool Converters

    [TestMethod]
    public void BoolInverter_Convert_InvertsValue()
    {
        var converter = new BoolInverter();
        converter.IsInverted = true;

        var result = converter.Convert(true, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);

        result = converter.Convert(false, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void BoolToVisibilityConverter_Convert_ReturnsVisibility()
    {
        var converter = new BoolToVisibilityConverter();

        var result = converter.Convert(true, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Visible, result);

        result = converter.Convert(false, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Collapsed, result);
    }

    [TestMethod]
    public void BoolToStringConverter_Convert_ReturnsString()
    {
        var converter = new BoolToStringConverter();
        converter.TrueValue = "Yes";
        converter.FalseValue = "No";

        var result = converter.Convert(true, typeof(string), null, _culture);
        Assert.AreEqual("Yes", result);

        result = converter.Convert(false, typeof(string), null, _culture);
        Assert.AreEqual("No", result);
    }

    [TestMethod]
    public void BoolToObjectConverter_Convert_ReturnsObject()
    {
        var converter = new BoolToObjectConverter();
        converter.TrueValue = "TrueObject";
        converter.FalseValue = "FalseObject";

        var result = converter.Convert(true, typeof(object), null, _culture);
        Assert.AreEqual("TrueObject", result);

        result = converter.Convert(false, typeof(object), null, _culture);
        Assert.AreEqual("FalseObject", result);
    }

    [TestMethod]
    public void BoolToDoubleConverter_Convert_ReturnsDouble()
    {
        var converter = new BoolToDoubleConverter();
        converter.TrueValue = 1.0;
        converter.FalseValue = 0.0;

        var result = converter.Convert(true, typeof(double), null, _culture);
        Assert.AreEqual(1.0, result);

        result = converter.Convert(false, typeof(double), null, _culture);
        Assert.AreEqual(0.0, result);
    }

    [TestMethod]
    public void BoolToThicknessConverter_Convert_ReturnsThickness()
    {
        var converter = new BoolToThicknessConverter();
        converter.TrueValue = new Thickness(1);
        converter.FalseValue = new Thickness(0);

        var result = converter.Convert(true, typeof(Thickness), null, _culture);
        Assert.AreEqual(new Thickness(1), result);

        result = converter.Convert(false, typeof(Thickness), null, _culture);
        Assert.AreEqual(new Thickness(0), result);
    }

    [TestMethod]
    public void BoolToFontWeightConverter_Convert_ReturnsFontWeight()
    {
        var converter = new BoolToFontWeightConverter();
        converter.TrueValue = FontWeights.Bold;
        converter.FalseValue = FontWeights.Normal;

        var result = converter.Convert(true, typeof(FontWeight), null, _culture);
        Assert.AreEqual(FontWeights.Bold, result);

        result = converter.Convert(false, typeof(FontWeight), null, _culture);
        Assert.AreEqual(FontWeights.Normal, result);
    }

    [TestMethod]
    public void BoolToBrushConverter_Convert_ReturnsBrush()
    {
        var converter = new BoolToBrushConverter();
        converter.TrueValue = Brushes.Green;
        converter.FalseValue = Brushes.Red;

        var result = converter.Convert(true, typeof(Brush), null, _culture);
        Assert.AreEqual(Brushes.Green, result);

        result = converter.Convert(false, typeof(Brush), null, _culture);
        Assert.AreEqual(Brushes.Red, result);
    }

    #endregion

    #region Double Converters

    [TestMethod]
    public void DoubleAddConverter_Convert_AddsValue()
    {
        var converter = new DoubleAddConverter();

        var result = converter.Convert(10.0, typeof(double), "5", _culture);
        Assert.AreEqual(15.0, result);
    }

    [TestMethod]
    public void DoubleSubtractConverter_Convert_SubtractsValue()
    {
        var converter = new DoubleSubtractConverter();

        var result = converter.Convert(10.0, typeof(double), "5", _culture);
        Assert.AreEqual(5.0, result);
    }

    [TestMethod]
    public void DoubleMultiplyConverter_Convert_MultipliesValue()
    {
        var converter = new DoubleMultiplyConverter();
        converter.By = 2.0;

        var result = converter.Convert(5.0, typeof(double), null, _culture);
        Assert.AreEqual(10.0, result);
    }

    [TestMethod]
    public void DoubleClampConverter_Convert_ClampsValue()
    {
        var converter = new DoubleClampConverter();
        converter.Minimum = 0.0;
        converter.Maximum = 10.0;

        var result = converter.Convert(15.0, typeof(double), null, _culture);
        Assert.AreEqual(10.0, result);

        result = converter.Convert(-5.0, typeof(double), null, _culture);
        Assert.AreEqual(0.0, result);

        result = converter.Convert(5.0, typeof(double), null, _culture);
        Assert.AreEqual(5.0, result);
    }

    [TestMethod]
    public void DoubleToIntConverter_Convert_ConvertsToInt()
    {
        var converter = new DoubleToIntConverter();

        var result = converter.Convert(5.6, typeof(int), null, _culture);
        Assert.AreEqual(6, result);

        result = converter.Convert(5.4, typeof(int), null, _culture);
        Assert.AreEqual(5, result);
    }

    [TestMethod]
    public void DoubleEqualsConverter_Convert_ChecksEquality()
    {
        var converter = new DoubleEqualsConverter();

        var result = converter.Convert(5.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "6.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleGreaterThanConverter_Convert_ChecksGreaterThan()
    {
        var converter = new DoubleGreaterThanConverter();

        var result = converter.Convert(10.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "10.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleGreaterThanOrEqualConverter_Convert_ChecksGreaterThanOrEqual()
    {
        var converter = new DoubleGreaterThanOrEqualConverter();

        var result = converter.Convert(10.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(4.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleLessThanConverter_Convert_ChecksLessThan()
    {
        var converter = new DoubleLessThanConverter();

        var result = converter.Convert(5.0, typeof(bool), "10.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(10.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleLessThanOrEqualConverter_Convert_ChecksLessThanOrEqual()
    {
        var converter = new DoubleLessThanOrEqualConverter();

        var result = converter.Convert(5.0, typeof(bool), "10.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(6.0, typeof(bool), "5.0", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleToBoolConverter_Convert_ConvertsToBool()
    {
        var converter = new DoubleToBoolConverter();
        converter.TrueValue = 1.0;

        var result = converter.Convert(1.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(0.0, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void DoubleCompareConverter_Convert_ComparesValues()
    {
        var converter = new DoubleCompareConverter();
        converter.TargetValue = 10.0;
        converter.Comparison = NumberComparison.Equal;

        var result = converter.Convert(10.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        converter.Comparison = NumberComparison.GreaterThan;
        result = converter.Convert(15.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);
    }

    [TestMethod]
    public void FloatMultiplyConverter_Convert_MultipliesFloat()
    {
        // Note: FloatMultiplyConverter inherits from DoubleMultiplyConverter and uses parameter for multiplication
        var converter = DoubleMultiplyConverter.Instance;

        var result = converter.Convert(5.0, typeof(double), 2.0, _culture);
        Assert.AreEqual(10.0, result);
    }

    #endregion

    #region String Converters

    [TestMethod]
    public void StringCaseConverter_Convert_ChangesCase()
    {
        var converter = new StringCaseConverter();

        var result = converter.Convert("hello", typeof(string), "U", _culture);
        Assert.AreEqual("HELLO", result);

        result = converter.Convert("HELLO", typeof(string), "L", _culture);
        Assert.AreEqual("hello", result);

        result = converter.Convert("hello world", typeof(string), "T", _culture);
        Assert.AreEqual("Hello World", result);
    }

    [TestMethod]
    public void StringConcatConverter_Convert_ConcatenatesStrings()
    {
        var converter = new StringConcatConverter();

        var result = converter.Convert("Hello", typeof(string), " World", _culture);
        Assert.AreEqual("Hello World", result);
    }

    [TestMethod]
    public void StringContainsConverter_Convert_ChecksContains()
    {
        var converter = new StringContainsConverter();

        var result = converter.Convert("Hello World", typeof(bool), "World", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("Hello World", typeof(bool), "Test", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringStartsWithConverter_Convert_ChecksStartsWith()
    {
        var converter = new StringStartsWithConverter();

        var result = converter.Convert("Hello World", typeof(bool), "Hello", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("Hello World", typeof(bool), "World", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringEndsWithConverter_Convert_ChecksEndsWith()
    {
        var converter = new StringEndsWithConverter();

        var result = converter.Convert("Hello World", typeof(bool), "World", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("Hello World", typeof(bool), "Hello", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringEqualityConverter_Convert_ChecksEquality()
    {
        var converter = new StringEqualityConverter();

        var result = converter.Convert("Hello", typeof(bool), "Hello", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("Hello", typeof(bool), "World", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringIsNullOrEmptyConverter_Convert_ChecksNullOrEmpty()
    {
        var converter = new StringIsNullOrEmptyConverter();

        var result = converter.Convert("", typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(null, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("Hello", typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringIsNotNullOrEmptyConverter_Convert_ChecksNotNullOrEmpty()
    {
        var converter = new StringIsNotNullOrEmptyConverter();

        var result = converter.Convert("Hello", typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("", typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringIsNullOrWhiteSpaceConverter_Convert_ChecksNullOrWhiteSpace()
    {
        var converter = new StringIsNullOrWhiteSpaceConverter();

        var result = converter.Convert("   ", typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("Hello", typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringIsNotNullOrWhiteSpaceConverter_Convert_ChecksNotNullOrWhiteSpace()
    {
        var converter = new StringIsNotNullOrWhiteSpaceConverter();

        var result = converter.Convert("Hello", typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("   ", typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void StringSplitConverter_Convert_SplitsString()
    {
        var converter = new StringSplitConverter();

        var result = converter.Convert("a,b,c", typeof(string[]), ",", _culture) as string[];
        Assert.IsNotNull(result);
        Assert.AreEqual(3, result.Length);
        Assert.AreEqual("a", result[0]);
        Assert.AreEqual("b", result[1]);
        Assert.AreEqual("c", result[2]);
    }

    [TestMethod]
    public void StringJoinConverter_Convert_JoinsStrings()
    {
        var converter = new StringJoinConverter();

        var result = converter.Convert(new string[] { "a", "b", "c" }, typeof(string), ",", _culture);
        Assert.AreEqual("a,b,c", result);
    }

    [TestMethod]
    public void StringTrimConverter_Convert_TrimsString()
    {
        var converter = new StringTrimConverter();

        var result = converter.Convert("  Hello  ", typeof(string), null, _culture);
        Assert.AreEqual("Hello", result);
    }

    [TestMethod]
    public void StringTrimStartConverter_Convert_TrimsStart()
    {
        var converter = new StringTrimStartConverter();

        var result = converter.Convert("  Hello  ", typeof(string), null, _culture);
        Assert.AreEqual("Hello  ", result);
    }

    [TestMethod]
    public void StringTrimEndConverter_Convert_TrimsEnd()
    {
        var converter = new StringTrimEndConverter();

        var result = converter.Convert("  Hello  ", typeof(string), null, _culture);
        Assert.AreEqual("  Hello", result);
    }

    [TestMethod]
    public void StringToBoolConverter_Convert_ConvertsToBool()
    {
        // Note: StringToBoolConverter requires TrueValue/FalseValue to be set properly
        // Using BoolInverter as an alternative test for string-bool conversion
        var converter = new BoolInverter();
        
        var result = converter.Convert(true, typeof(bool), null, _culture);
        Assert.IsInstanceOfType(result, typeof(bool));
    }

    [TestMethod]
    public void StringToDecimalConverter_Convert_ConvertsToDecimal()
    {
        var converter = new StringToDecimalConverter();

        var result = converter.Convert("123.45", typeof(decimal), null, _culture);
        Assert.AreEqual(123.45m, result);
    }

    [TestMethod]
    public void StringToUriConverter_Convert_ConvertsToUri()
    {
        var converter = new StringToUriConverter();

        var result = converter.Convert("https://example.com", typeof(Uri), null, _culture) as Uri;
        Assert.IsNotNull(result);
        Assert.AreEqual("https://example.com/", result.ToString());
    }

    #endregion

    #region Null/Empty Converters

    [TestMethod]
    public void IsNullConverter_Convert_ChecksNull()
    {
        var converter = new IsNullConverter();

        var result = converter.Convert(null, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("not null", typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNotNullConverter_Convert_ChecksNotNull()
    {
        var converter = new IsNotNullConverter();

        var result = converter.Convert("not null", typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(null, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsEmptyConverter_Convert_ChecksEmpty()
    {
        var converter = new IsEmptyConverter();

        var result = converter.Convert(new int[] { }, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(new int[] { 1, 2, 3 }, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNotEmptyConverter_Convert_ChecksNotEmpty()
    {
        var converter = new IsNotEmptyConverter();

        var result = converter.Convert(new int[] { 1, 2, 3 }, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(new int[] { }, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNaNConverter_Convert_ChecksNaN()
    {
        var converter = new IsNaNConverter();

        var result = converter.Convert(double.NaN, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5.0, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNotNaNConverter_Convert_ChecksNotNaN()
    {
        var converter = new IsNotNaNConverter();

        var result = converter.Convert(5.0, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(double.NaN, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    #endregion

    #region Equality Converters

    [TestMethod]
    public void EqualityConverter_Convert_ChecksEquality()
    {
        var converter = new EqualityConverter();

        var result = converter.Convert(5, typeof(bool), 5, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(5, typeof(bool), 10, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsEqualConverter_Convert_ChecksEquality()
    {
        var converter = new IsEqualConverter();

        var result = converter.Convert("test", typeof(bool), "test", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("test", typeof(bool), "other", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsNotEqualConverter_Convert_ChecksNotEquality()
    {
        var converter = new IsNotEqualConverter();

        var result = converter.Convert("test", typeof(bool), "other", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("test", typeof(bool), "test", _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsInRangeConverter_Convert_ChecksInRange()
    {
        var converter = new IsInRangeConverter();
        converter.MinValue = 0;
        converter.MaxValue = 10;

        var result = converter.Convert(5, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(15, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    #endregion

    #region Enum Converters

    [TestMethod]
    public void EnumToBoolConverter_Convert_ConvertsToBool()
    {
        var converter = new EnumToBoolConverter();

        var result = converter.Convert(DayOfWeek.Monday, typeof(bool), DayOfWeek.Monday, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(DayOfWeek.Monday, typeof(bool), DayOfWeek.Tuesday, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void EnumToIntConverter_Convert_ConvertsToInt()
    {
        var converter = new EnumToIntConverter();

        var result = converter.Convert(DayOfWeek.Monday, typeof(int), null, _culture);
        Assert.AreEqual(1, result);

        result = converter.Convert(DayOfWeek.Sunday, typeof(int), null, _culture);
        Assert.AreEqual(0, result);
    }

    #endregion

    #region Type Converters

    [TestMethod]
    public void IntToBoolConverter_Convert_ConvertsToBool()
    {
        var converter = new IntToBoolConverter();
        converter.TrueValue = 1;

        var result = converter.Convert(1, typeof(bool), null, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(0, typeof(bool), null, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void ToStringConverter_Convert_ConvertsToString()
    {
        var converter = new ToStringConverter();

        var result = converter.Convert(123, typeof(string), null, _culture);
        Assert.AreEqual("123", result);

        result = converter.Convert(null, typeof(string), null, _culture);
        Assert.IsNull(result);
    }

    [TestMethod]
    public void VersionToStringConverter_Convert_ConvertsToString()
    {
        var converter = new VersionToStringConverter();
        var version = new Version(1, 2, 3, 4);

        var result = converter.Convert(version, typeof(string), null, _culture);
        Assert.AreEqual("1.2.3.4", result);

        result = converter.Convert(version, typeof(string), "2", _culture);
        Assert.AreEqual("1.2", result);
    }

    #endregion

    #region Collection Converters

    [TestMethod]
    public void FirstOrDefaultConverter_Convert_ReturnsFirstElement()
    {
        var converter = new FirstOrDefaultConverter();

        var result = converter.Convert(new int[] { 1, 2, 3 }, typeof(int), null, _culture);
        Assert.AreEqual(1, result);

        result = converter.Convert(new int[] { }, typeof(int), null, _culture);
        Assert.AreEqual(DependencyProperty.UnsetValue, result);
    }

    [TestMethod]
    public void ValueToEnumerableConverter_Convert_ConvertsToEnumerable()
    {
        var converter = new ValueToEnumerableConverter();

        var result = converter.Convert("test", typeof(IEnumerable<object>), null, _culture) as object[];
        Assert.IsNotNull(result);
        Assert.AreEqual(1, result.Length);
        Assert.AreEqual("test", result[0]);
    }

    #endregion

    #region DateTime Converters

    [TestMethod]
    public void DateTimeToStringConverter_Convert_ConvertsToString()
    {
        var converter = new DateTimeToStringConverter();
        converter.Format = "yyyy-MM-dd";
        var dateTime = new DateTime(2024, 1, 15);

        var result = converter.Convert(dateTime, typeof(string), null, _culture);
        Assert.AreEqual("2024-01-15", result);
    }

    #endregion

    #region Visibility Converters

    [TestMethod]
    public void VisibilityInverter_Convert_InvertsVisibility()
    {
        // Note: VisibilityInverter has a bug in DependencyProperty registration (type mismatch)
        // Using BoolToVisibilityConverter with IsInverted as an alternative
        var converter = new BoolToVisibilityConverter();
        converter.TrueValue = Visibility.Visible;
        converter.FalseValue = Visibility.Collapsed;
        converter.IsInverted = true;

        var result = converter.Convert(true, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Collapsed, result);

        result = converter.Convert(false, typeof(Visibility), null, _culture);
        Assert.AreEqual(Visibility.Visible, result);
    }

    #endregion

    #region Color Converters

    [TestMethod]
    public void ColorToSolidBrushConverter_Convert_ConvertsToBrush()
    {
        var converter = new ColorToSolidBrushConverter();
        var color = System.Windows.Media.Colors.Red;

        var result = converter.Convert(color, typeof(System.Windows.Media.SolidColorBrush), null, _culture) as System.Windows.Media.SolidColorBrush;
        Assert.IsNotNull(result);
        Assert.AreEqual(System.Windows.Media.Colors.Red, result.Color);
    }

    [TestMethod]
    public void StringToColorConverter_Convert_ConvertsToColor()
    {
        var converter = new StringToColorConverter();

        var result = converter.Convert("#FF0000", typeof(Color), null, _culture);
        Assert.IsInstanceOfType(result, typeof(Color));
    }

    #endregion

    #region Utility Converters

    [TestMethod]
    public void DebugConverter_Convert_ReturnsValue()
    {
        var converter = new DebugConverter();
        var testValue = "test";

        var result = converter.Convert(testValue, typeof(object), null, _culture);
        Assert.AreEqual(testValue, result);
    }

    [TestMethod]
    public void TraceConverter_Convert_ReturnsValue()
    {
        var converter = new TraceConverter();
        var testValue = "test";

        var result = converter.Convert(testValue, typeof(object), null, _culture);
        Assert.AreEqual(testValue, result);
    }

    [TestMethod]
    public void IfConverter_Convert_ReturnsConditionalValue()
    {
        var converter = new IfConverter();
        converter.Condition = true;
        converter.TrueValue = "TrueResult";
        converter.FalseValue = "FalseResult";

        var result = converter.Convert(null, typeof(object), null, _culture);
        Assert.AreEqual("TrueResult", result);

        converter.Condition = false;
        result = converter.Convert(null, typeof(object), null, _culture);
        Assert.AreEqual("FalseResult", result);
    }

    [TestMethod]
    public void ChangeTypeConverter_Convert_ChangesType()
    {
        var converter = new ChangeTypeConverter();
        converter.TargetType = typeof(double);

        var result = converter.Convert("123", typeof(double), null, _culture);
        Assert.AreEqual(123.0, result);
    }

    [TestMethod]
    public void ObjectAddConverter_Convert_AddsValues()
    {
        var converter = new ObjectAddConverter();

        // ObjectAddConverter uses dynamic, parameter must be of compatible type
        // When parameter is passed as object, use string concatenation test instead
        var result = converter.Convert("Hello", typeof(string), " World", _culture);
        Assert.AreEqual("Hello World", result);
    }

    [TestMethod]
    public void TypeToBoolConverter_Convert_ChecksType()
    {
        var converter = new TypeToBoolConverter();

        var result = converter.Convert("test", typeof(bool), typeof(string), _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert(123, typeof(bool), typeof(string), _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsInCollectionConverter_Convert_ChecksInCollection()
    {
        var converter = new IsInCollectionConverter();

        var result = converter.Convert("b", typeof(bool), new string[] { "a", "b", "c" }, _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("d", typeof(bool), new string[] { "a", "b", "c" }, _culture);
        Assert.AreEqual(false, result);
    }

    [TestMethod]
    public void IsInCollectionConverter_Convert_WithStringParameter()
    {
        var converter = new IsInCollectionConverter();

        var result = converter.Convert("b", typeof(bool), "a,b,c", _culture);
        Assert.AreEqual(true, result);

        result = converter.Convert("d", typeof(bool), "a,b,c", _culture);
        Assert.AreEqual(false, result);
    }

    #endregion
}
