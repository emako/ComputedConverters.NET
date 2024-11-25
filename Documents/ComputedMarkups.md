
### 3. Computed Markups

#### 3.1 Setup

Add the namespaces that it will need to know about (before any xaml loaded).

```c#
public partial class App : Application
{
    public App() : base()
    {
        // Add the System namespace so we can use primitive types (i.e. int, etc.).
        EquationTokenizer.AddNamespace(typeof(object));
        // Add the System.Windows namespace so we can use Visibility.Collapsed, etc.
        EquationTokenizer.AddNamespace(typeof(System.Windows.Visibility));
    }
}
```

Other using cases

```c#
EquationTokenizer.AddNamespace("System", Assembly.GetAssembly(typeof(object)));
EquationTokenizer.AddAssembly(typeof(object).Assembly);
EquationTokenizer.AddExtensionMethods(typeof(Enumerable));
```


#### 3.2 Computed

Here is a binding with a Boolean to System.Visibility converter written with ComputedConverter:

```xaml
<Control Visibility="{c:Computed '$P ? Visibility.Visible : Visibility.Collapsed', P={Binding ShowElement}}" />
```

Following are two more examples of valid converter code:

```xaml
'$P != null ? $P.GetType() == typeof(int) : false'
```

```xaml
'(Double.Parse($P) + 123.0).ToString(\\’0.00\\’)'
```

Constructors and object initializers are also valid:

```xaml
<Control FontSize="{c:Computed 'new Dictionary\[string, int\]() { { \\'Sml\\', 16 }, { \\'Lrg\\', 32 } }\[$P\]', P={Binding TestIndex}}" />
```

```xaml
<Control Content="{c:Computed 'new TestObject(1,2,3) { FieldOne = \\'Hello\\', FieldTwo = \\'World\\' }}" />
```

The following shows how to write a two-way binding:

```xaml
<Control Height="{c:Computed '$P * 10', ConvertBack='$value * 0.1', P={Binding TestWidth, Mode=TwoWay}}" />
```

#### 3.3 MultiComputed

Multibinding work in a very similar way.

The following demonstrates an inline multibinding:

```xaml
<Control Angle="{c:MultiComputed 'Math.Atan2($P0, $P1) * 180 / 3.14159', P0={Binding ActualHeight, ElementName=rootElement}, P1={Binding ActualWidth, ElementName=rootElement}}" />
```

#### 3.4 ComputedConverter

Converters can also be created independently of the ComputedConverter binding extensions. This allows an extra level of flexibility. The following is an example of this:

```xaml
<Control Width="{Binding Data, Converter={c:ComputedConverter '$P * 10', ConvertBack='$value * 0.1'}}" />
```

**Local Variables**

Local variables can be used through a lambda expression like syntax. Local variables have precedence over binding variables and are only valid with the scope of the lambda expression in which they are declared. The following shows this:

```xaml
<Control IsEnabled="{c:Computed '(Loc = $P.Value, A = $P.Show) => $Loc != null ## $A', P={Binding Obj}}" />
```

\* Note that the "&&" operator must be written as "&amp;&amp;" in XML.

** Due to a bug with the designer, using "&amp;" in a markup extension breaks Intellisense. Instead of two ampersands, use the alternate syntax "##". "#" also works for bitwise and operations.

**Lambda Expressions**

Support for lambda expressions is limited, but its support is sufficient to allow LINQ to be used. They look quite similar to conventional C# lambda expressions, but there are a few important differences. First off, block expressions are not supported. Only single, inline statements are allowed. Also, the expression must return a value. Additionally, the function will default to object for all generic parameters (eg. Func<object, object>). This can be overridden with typecast operators. The following shows this:

```xaml
<Control ItemsSource="{c:Computed '$P.Where(( (int)i ) => (bool)($i % 2 == 0))', P={Binding Source}}" />
```

*Note: The parameters must always be enclosed by parenthesis.

**Null Propagation Operator**

The null propagation operator allows safe property/field, method, and indexer access on objects. When used, if the target object is null, instead of throwing an exception null is returned. The operator is implemented by "?". 

Instead of this:

```xaml
'$P != null ? $P.Value : null'
```

You can write this:

```xaml
'$P?.Value'
```

This can be combined with the null coalescing operator to change this:

```xaml
'$P != null ? $P.GetType() == typeof(int) : false'
```

Into this:

```xaml
'($P?.GetType() == typeof(int)) ?? false'
```

This operator is particularly useful in long statements where there are multiple accessors that could throw null exceptions. In this example, we assume Data can never be null when Value is not null.

```xaml
'$P?.Value?.Data.ArrayData?\[4\]'
```

#### 3.5 ComputedEvent

This markup extension allows you to create event handlers inline. Aside from allowing void functions, the code is identical to ComputedConverters. However, ComputedEvent exposes a number of variables by default.

```xaml
$sender - The sender of the event.

$eventArgs - The arguments object of the event.

$dataContext - The data context of the sender.

$V0-$V9 - The values set on the ComputedEvent Vx properties.

$P0-$P4 - The values of the ComputedEvent.P0-ComputedEvent.P4 inherited attached properties on sender.

${name} - Any element within the name scope where {name} is the value of x:Name on that element.
```

An example:

```xaml
<StackPanel c:ComputedEvent.P0="{Binding SomeValue}">
   <TextBlock x:Name="textField" />
   <Button Content="Click Me" Click="{c:ComputedEvent '$textField.Text = $dataContext.Transform($P0.Value)'}" />
</StackPanel>
```

The P0-P4 values are useful for crossing name scope boundaries like DataTemplates. Typically, when set outside a DataTemplate they will be inherited by the contents inside the DataTemplate. This allows you to easily make external controls and values accessible from inside DataTemplates.

#### 3.6 ComputedValue

This markup extension evaluates exactly like a ComputedConverter except there are no P0-P9 variables, and it is evaluated at load. The markup extension returns the result of the expression.
