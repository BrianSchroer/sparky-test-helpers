
_see also_:
* **[SparkyTestHelpers](https://www.nuget.org/packages/SparkyTestHelpers/)**: exception expectation and "scenario test" (row test) helpers
* **[SparkyTestHelpers.Moq](https://www.nuget.org/packages/SparkyTestHelpers.Moq)**: syntax helpers for testing with [Moq](https://github.com/moq)

---
## MapTester<TSource, TDestination>
*class SparkyTestHelpers.Mapping.MapTester<TSource, TDestination>*

This class is for testing that properties were successfully "mapped" from one type to another.

**Methods**
* *MapTester&lt;TSource, TDestination&gt;* **WithLogging** *([Action&lt;String&gt; action])* 
    - (optional) log property values when asserting. If called without an action, defaults to Console.WriteLine. 
* *MapTester&lt;TSource, TDestination&gt;* **IgnoringMember** *(Expression&lt;Func&lt;TDestination, Object&gt;&gt; destExpression)*  
* *MapMemberTester&lt;TSource, TDestination&gt;* **WhereMember** *(Expression&lt;Func&lt;TDestination, Object&gt;&gt; destExpression)*  
* *void* **AssertMappedValues** *(TSource source, TDestination dest)*  

**Static Methods**
* *MapTester&lt;TSource, TDestination&gt;* **ForMap** *()* 

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
As with AutoMapper, you don't have to configure anything for properties with the same name/type in the source and destination instances.
**AssertMappedValues** considers those successful if the source/destination values matter.

Use **IgnoringMember** for destination properties that are not mapped or which you need to test in another way.

Use **WhereMember** to "dot" to the **ShouldEqual**, **ShouldEqualValue** and **IsTestedBy** functions.

---
## MapMemberTester<TSource, TDestination>
*class SparkyTestHelpers.Mapping.MapMemberTester<TSource, TDestination>*

This class is for testing that a property was successfully "mapped" from one type to another.

**Methods**
* *MapTester&lt;TSource, TDestination&gt;* **ShouldEqual** *(Expression&lt;Func&lt;TSource, Object&gt;&gt; sourceExpression)*  
    - use to verify destination property mapped from differently named source property(s)
* *MapTester&lt;TSource, TDestination&gt;* **ShouldEqualValue** *(Object value)* 
    - use to verify destination property using a constant or some other value not derived from the source 
* *MapTester&lt;TSource, TDestination&gt;* **IsTestedBy** *(Action&lt;TSource, TDestination&gt; customTest)* 
    - use for custom complex validation that doesn't fit one of the other verification methods

---
## RandomValuesHelper
*class SparkyTestHelpers.Mapping.RandomValuesHelper*

Test helper for updating class instance properties with random values (so you
can fill a "source" instance without writing a lot of code).

**Methods**
* *T* **CreateInstanceWithRandomValues** *()*  
* *T* **UpdatePropertiesWithRandomValues** *(T instance)*  
---
**Example**

```csharp
using SparkyTestHelpers.Mapping;
```
```csharp
    Foo foo = new RandomValuesHelper().CreateInstanceWithRandomValues<Foo>();

    var bar = new Bar();
    new RandomValuesHelper().UpdatePropertiesWithRandomValues(bar);
```
