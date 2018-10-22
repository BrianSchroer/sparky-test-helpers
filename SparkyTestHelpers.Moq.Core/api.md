## [Moq](https://github.com/moq) syntax helpers ##

_see also_:
* [SparkyTestHelpers.Moq](https://www.nuget.org/packages/SparkyTestHelpers.Moq/)
* [SparkyTestHelpers.Moq.Fluent](https://www.nuget.org/packages/SparkyTestHelpers.Moq.Fluent/)
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---

This package provides core extension methods intended for use with **SparkyTestHelpers.Moq** and **SparkyTestHelpers.Moq.Fluent**, but it can be used on its own...

[Moq, “the most popular and friendly mocking framework for .NET”](https://github.com/moq/moq4) is great, but some of the syntax is a bit unwieldy.

This NuGet package provides extension methods that allow you to use Moq with “wieldier” (Is that a word?) syntax:

### “Any” - Syntax alternative to “It.IsAny<*T*>”

```csharp
_mock.Setup(x => x.DoSomething(
    It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<int>>())
    .Returns(true);
```
...can be simplified to:
```csharp
using SparkyTestHelpers.Moq;
. . .
_mock.Setup(x => x.DoSomething(
    Any.String, Any.Int, Any.IEnumerable<int>()) 
    .Returns(true);
```

#### "Any" members:
* Any.Action
* Any.Action<*T*>
* Any.Action<*T1, T2*>
* Any.Action<*T1, T2, T3*>
* Any.Array<*T*>
* Any.Boolean
* Any.Dictionary<TKey, TValue>
* Any.DateTime
* Any.Decimal
* Any.Double
* Any.Func<*T*>
* Any.Func<*T1, T2*>
* Any.Func<*T1, T2, T3*>
* Any.Guid
* Any.IEnumerable<*T*>
* Any.InstanceOf<*T*> (*Any.One<*T*> is a "synonym" for Any.InstanceOf<*T*>*)
* Any.IList<*T*>
* Any.Int
* Any.IQueryable<*T*?
* Any.KeyValuePair<TKey, TValue>
* Any.Lazy<*T*>
* Any.List<*T*>
* Any.Long
* Any.Nullable<*T*>
* Any.Object
* Any.Short
* Any.Single
* Any.String
* Any.TimeSpan
* Any.Tuple<T1, T2>
* Any.Type
* Any.UInt
* Any.ULong
* Any.UShort

### *mock*.Where extension method
...provides an alternate syntax for "It.Is":
```csharp
using SparkyTestHelpers.Moq;
. . .
// sad:
_mock.Setup(x => x.Foo(It.Is<int>(i => i % 2 == 0))).Returns(true);
// rad!:
_mock.Setup(x => x.Foo(Any.Int.Where(i => i % 2 == 0))).Returns(true);
```
### *mock*.Expression extension method
...makes it easy to create a reusable expression so you don't duplicate code in ".Setup" and ".Verify" calls. This test:
```csharp
// Arrange:
_mock.Setup(x => x.Foo(
    Any.String, Any.Int, Any.InstanceOf<Bar>())
    ).Returns(true);

// Act:
subjectUnderTest.Foo("yo", 5, myBar);

//Assert:
_mock.VerifyOneCallTo(x => x.Foo(
    Any.String, Any.Int, Any.InstanceOf<Bar>()));
```
...where you have to code the same “*x => x.Foo(
 Any.String, Any.Int, Any.InstanceOf<*Bar*>()*” expression for both the .Setup and .Verify calls  -  can be simplified to:
```csharp
using SparkyTestHelpers.Moq;
. . .
// Arrange:
var fooExp = _mock.Expression(x => 
    x.Foo(Any.String, Any.Int, Any.InstanceOf<Bar>()));
_mock.Setup(fooExp).Returns(true);

// Act:
subjectUnderTest.Foo("yo", 5, myBar);

// Assert:
_mock.VerifyOneCallTo(fooExp);
```
...so you only have to code the expression once, reducing finger fatigue and the possibility of the Setup and Verify expressions not matching because of a typo!