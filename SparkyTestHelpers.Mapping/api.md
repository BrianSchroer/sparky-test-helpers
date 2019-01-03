
_see also_:
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## MapTester<TSource, TDestination>
*class SparkyTestHelpers.Mapping.MapTester<TSource, TDestination>*

This class is for testing that properties were successfully "mapped" from one type to another, either via **AutoMapper** or another automatic mapping framework, or by hand-written "*destination.X = source.X*;" code.

See **[SparkyTestHelpers.AutoMapper](https://www.nuget.org/packages/SparkyTestHelpers.AutoMapper/)** for additional extension methods specifically for working with **AutoMapper**.

**Methods**
* *MapTester<TSource, TDestination>* **WithLogging**(*Action<*String*> action*)   
   (optional) "Callback" action to log property values when asserting. If called without an action, defaults to Console.WriteLine. 
* *MapTester<TSource, TDestination>* **IgnoringMember**(*Expression<Func<TDestination, Object>> destExpression*) 
* *MapTester<TSource, TDestination>* **IgnoringMembers**(*params Expression<Func<TDestination, Object>>[] destExpressions*) 
* *MapTester<TSource, TDestination>* **IgnoringMemberNamesStartingWith**(*string prefix*)   
* *MapMemberTester<TSource, TDestination>* **WhereMember**(*Expression<Func<TDestination, Object>> destExpression*) 
* *MapMemberTester<TSource, TDestination>* **WhereMember**(*Expression<Func<TDestination, Object>> destExpression*)  
* *void* **AssertMappedValues**(*TSource source, TDestination dest*)   
   throws exception if any source/destination map validation specifications fail, or if any destination properties aren’t either tested or **IgnoreMember**’d. 

**Static Methods**
* *MapTester<TSource, TDestination>* **ForMap**() 

**Example**

```csharp
using SparkyTestHelpers.Mapping;
```
```csharp
    Foo foo = CreateAndPopulateTestFoo();
    Bar bar = Mapper.Map<Foo, Bar>(foo); 

    MapTester.ForMap<Foo, Bar>()
        .WithLogging()
        .IgnoringMember(dest => dest.DestOnlyProperty)
        .WhereMember(dest => dest.StatusCode).ShouldEqual(src => src.Status)
        .WhereMember(dest => dest.IsValid).ShouldEqualValue(true)
        .WhereMember(dest => dest.Percent).IsTestedBy((src, dest) => 
            Assert.AreEqual(src.Rate / 100, dest.Percent))
        .AssertMappedValues(foo, bar);
```
You don't have to configure anything for properties with the same name/type in the source and destination instances. **AssertMappedValues**() considers those successful if the source/destination values match.

Use **IgnoringMember**, **IgnoringMembers** or **IgnoringMemberNamesStartingWith** for destination properties that are not mapped or which you need to test in another way

Use **WhereMember** to "dot" to the **ShouldEqual**, **ShouldEqualValue** and **IsTestedBy** functions.

---
## MapMemberTester<TSource, TDestination>
*class SparkyTestHelpers.Mapping.MapMemberTester<TSource, TDestination>*

This class is for testing that a property was successfully "mapped" from one type to another.

**Methods**
* *MapTester<TSource, TDestination>* **ShouldEqual**(*Expression<Func<TSource, Object>> sourceExpression*)   
   use to verify destination property mapped from differently named source property(s)
* *MapTester<TSource, TDestination>* **ShouldBeStringMatchFor**(*Expression<Func<TSource, Object>> sourceExpression*)   
   use to verify the ".ToString()" values of the source and destination properties (useful for testing Enum mapping where the source/destination Enum types have the same names)
* *MapTester<TSource, TDestination>* **ShouldEqualValue**(*Object value*)   
   use to verify destination property using a constant or some other value not derived from the source 
* *MapTester<TSource, TDestination>* **IsTestedBy**(*Action<TSource, TDestination> customTest*)   
   use for custom complex validation that doesn't fit one of the other verification methods
