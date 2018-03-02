
_see also_:
* **[SparkyTools.AutoMapper](https://www.nuget.org/packages/SparkyTools.AutoMapper)**: AutoMapper syntax helpers
* **[SparkyTestHelpers.Mapping](https://www.nuget.org/packages/SparkyTestHelpers.Mapping)**: Helpers for testing properties "mapped" from one type to another
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
This package extends **[SparkyTestHelpers.Mapping](https://www.nuget.org/packages/SparkyTestHelpers.Mapping)** with some additional extension methods specifically for use with **[AutoMapper](http://automapper.org/)**. 

---
## MapTester<TSource, TDestination>
*class SparkyTestHelpers.Mapping.MapTester<TSource, TDestination>*

This class is for testing that properties were successfully "mapped" from one type to another.

**Methods**
* *MapTester<TSource, TDestination>* **WithLogging** *(Action<*String*> action)*    
   (optional) "Callback" action to log property values when asserting. If called without an action, defaults to Console.WriteLine. 
* *MapTester<TSource, TDestination>* **IgnoringMember**(*Expression<Func<TDestination, Object>> destExpression*)  
* *MapMemberTester<TSource, TDestination>* **WhereMember**(*Expression<Func<TDestination, Object>> destExpression*)  
* *void* **AssertMappedValues**(*TSource source, TDestination dest*)     
   throws exception if any source/destination map validation specifications fail, or if any destination properties aren’t either tested or **IgnoreMember**’d. 

**Extension Methods for AutoMapper**
* *TDestination* **AssertAutoMappedValues**(*TSource source*)   
   calls **Mapper.Map**<*TSource, TDestination*>(source), then **AssertMappedValues**()
* *TDestination* **AssertAutoMappedRandomValues**()   
   uses **RandomValuesHelper** to create a new TSource instance, calls **Mapper.Map**<*TSource, TDestination*>(source), then **AssertMappedValues**()
    
**Static Methods**
* *MapTester<TSource, TDestination>* **ForMap**( 

**Examples**

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
***AutoMapper-specific methods:***

```csharp
using SparkyTestHelpers.AutoMapper;
```
```csharp
    Baz baz = CreateAndPopulateTestBax();
    Qux qux = MapTester.ForMap<Baz, Qux>.AssertAutoMappedValues(baz);
```
```csharp
    // Create & populate Baz, map to Qux, assert success all in one statement!
    MapTester.ForMap<Baz, Qux>.AssertAutoMappedRandomValues();
```
As with AutoMapper, you don't have to configure anything for properties with the same name/type in the source and destination instances.
**AssertMappedValues** considers those successful if the source/destination values matxh.

Use **IgnoringMember** for destination properties that are not mapped or which you need to test in another way.

Use **WhereMember** to "dot" to the **ShouldEqual**, **ShouldEqualValue** and **IsTestedBy** functions.

---
## MapMemberTester<TSource, TDestination>
*class SparkyTestHelpers.Mapping.MapMemberTester<TSource, TDestination>*

This class is for testing that a property was successfully "mapped" from one type to another.

**Methods**
* *MapTester<TSource, TDestination>* **ShouldEqual**(*Expression<Func<TSource, Object>> sourceExpression*)   
   use to verify destination property mapped from differently named source property(s)
* *MapTester<TSource, TDestination>* **ShouldEqualValue**(*Object value*)   
   use to verify destination property using a constant or some other value not derived from the source 
* *MapTester<TSource, TDestination>* **IsTestedBy** (*Action<TSource, TDestination> customTest*)   
   use for custom complex validation that doesn't fit one of the other verification methods

---
## RandomValuesHelper
*class SparkyTestHelpers.Mapping.RandomValuesHelper*

Test helper for updating class instance properties with random values (so you
can fill a "source" instance without writing a lot of code).

**Methods**
* *T* **CreateInstanceWithRandomValues**()  
* *T* **UpdatePropertiesWithRandomValues**(*T instance*)  
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
