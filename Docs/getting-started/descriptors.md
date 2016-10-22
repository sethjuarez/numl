# Descriptors

Descriptors are the primary means of converting standard .net types into their representative 
numerical forms. There are primarily two ways of doing this: through attributes or declaratively.

## Strong Descriptor Declaration

The first approach for preparing Descriptors is by adding attributes to properties of a C# class:

```csharp
public class Iris
{
    [Feature]
    public decimal SepalLength { get; set; }
    [Feature]
    public decimal SepalWidth { get; set; }
    [Feature]
    public decimal PetalLength { get; set; }
    [Feature]
    public decimal PetalWidth { get; set; }
    [StringLabel]
    public string Class { get; set; }
}
```

This class can then be used as a means to create a Descriptor:

```csharp
var description = Descriptor.Create<Iris>();
```

There are several feature and label attributes that can be applied to a class:

- @numl.Model.FeatureAttribute
- @numl.Model.StringFeatureAttribute
- @numl.Model.DateFeatureAttribute
- @numl.Model.GuidFeatureAttribute
- @numl.Model.EnumerableFeatureAttribute
- @numl.Model.LabelAttribute
- @numl.Model.StringLabelAttribute
- @numl.Model.GuidLabelAttribute

While this approach is the simplest, it creates a very strong dependency on this library.

## Descriptor Declaration

While the second approach is type dependent, it dispenses with the attribute requirement and 
instead uses a tpye-safe fluent API to delare features and labels:

```csharp
var d = Descriptor.For<Iris>() 
                    .With(i => i.SepalLength) 
                    .With(i => i.SepalWidth) 
                    .With(i => i.PetalLength) 
                    .With(i => i.PetalWidth) 
                    .Learn(i => i.Class);
```

This example creates a descriptor with 4 features and one label. There are several 
variants of the With method that allow further customization:

- @numl.Model.Descriptor`1.With(System.Linq.Expressions.Expression{System.Func{`0,System.Object}}) - Standard approach infers property name and type
- @numl.Model.Descriptor`1.WithString(System.Linq.Expressions.Expression{System.Func{`0,System.String}},numl.Model.StringSplitType,System.String,System.Boolean,System.String) - Assumes properties of type string and allows additional settings for expansion
- @numl.Model.Descriptor`1.WithDateTime(System.Linq.Expressions.Expression{System.Func{`0,System.DateTime}},numl.Model.DatePortion) - Assumes properties of type DateTime and allows additional settings for expansion
- @numl.Model.Descriptor`1.WithGuid(System.Linq.Expressions.Expression{System.Func{`0,System.Guid}}) - Assumes properties of type Guid
- @numl.Model.Descriptor`1.WithEnumerable(System.Linq.Expressions.Expression{System.Func{`0,System.Collections.IEnumerable}},System.Int32) - Assumes list properties and allows additional settings for expansion

This approach is less intrusive as it only relies on the structure of your already existing data types.

## Weak Descriptor Declaration  

The last approach is completely agnostic to the provided data type and only attempts to 
read properties of the given name and type. These type of descriptors work with a number 
of different data types (classes, DataTable's, Expando, etc) and allow for the greatest 
flexibility:

```csharp
var d = Descriptor.New()
            .With("SepalLength").As(typeof(decimal))
            .With("SepalWidth").As(typeof(double))
            .With("PetalLength").As(typeof(decimal))
            .With("PetalWidth").As(typeof(int))
            .Learn("Class").As(typeof(string));
```

This style of declaration creates an empty descriptor [New( )] and adds 4 features and 
a label. The general style of this fluent interface is the use of the With or Learn 
method (which describes the name of the property that will be accessed) and the 
As_ method (which describes the property type along with any additional information). 
The With method adds a Feature to the descriptor while the Learn method overwrites the 
Label of the descriptor.

This style of declaration creates an empty descriptor [New( )] and adds 4 features and a label. The general style of this fluent interface is the use of the With or Learn method (which describes the name of the property that will be accessed) and the As_ method (which describes the property type along with any additional information). The With method adds a Feature to the descriptor while the Learn method overwrites the Label of the descriptor.

There are sevaral As methods available:
- @numl.Model.DescriptorProperty.As(System.Type)
- @numl.Model.DescriptorProperty.AsString
- @numl.Model.DescriptorProperty.AsDateTime(numl.Model.DatePortion)
- @numl.Model.DescriptorProperty.AsGuid
- @numl.Model.DescriptorProperty.AsEnumerable(System.Int32)

## Properties

Utlimately the process of creating descriptors boils down to creating a collection 
of features as well as an optional label that describes the types that will participate 
in the learning algorithms. These all impement a conversion between the respective 
types to a double. Some of these properties when expanded also could potential become 
multivalued as in the case of Strings, Enumerables, and DateTimes. Here is a list of 
available properties:

- @numl.Model.Property (base)
- @numl.Model.StringProperty (multi- or single-valued depending on use)
- @numl.Model.DateTimeProperty (multi- or single-valued depending on use)
- @numl.Model.GuidProperty
- @numl.Model.EnumerableProperty (multi-valued)

One could also create their own property by deriving from the Property class and 
appending it as a feature of the desciptor.
