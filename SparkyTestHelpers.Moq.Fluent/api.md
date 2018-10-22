## [Moq](https://github.com/moq) [Fluent Assertions](https://fluentassertions.com/) syntax helpers ##

_see also_:
* [SparkyTestHelpers.Moq](https://www.nuget.org/packages/SparkyTestHelpers.Moq/)
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---

This package provides [Fluent Assertions](https://fluentassertions.com/) extension methods for [Moq, “the most popular and friendly mocking framework for .NET”](https://github.com/moq/moq4). 

The "out of the box" syntax: 

```csharp
_mock.Verify(x => x.Foo());
_mock.Verify(x => x.Foo(), Times.Once);
_mock.Verify(x => x.Foo(), Times.AtLeastOnce);
_mock.Verify(x => x.Foo(), Times.AtMostOnce);
_mock.Verify(x => x.Foo(), Times.Exactly(3));
_mock.Verify(x => x.Foo(), Times.Between(2, 5));
_mock.Verify(x => x.Foo(), Times.AtLeast(2));
_mock.Verify(x => x.Foo(), Times.AtMost(5));
_mock.Verify(x => x.Foo(), Times.Never);
```

...can be coded as:

```csharp
_mock.Method(x => x.Foo()).Should().HaveBeenCalled();
_mock.Method(x => x.Foo()).Should().HaveBeenCalledOnce();
_mock.Method(x => x.Foo()).Should().HaveBeenCalledAtLeastOnce();
_mock.Method(x => x.Foo()).Should().HaveBeenCalledAtMostOnce();
_mock.Method(x => x.Foo()).Should().HaveCallCount(3);
_mock.Method(x => x.Foo()).Should().HaveCallCountBetween(2, 5);
_mock.Method(x => x.Foo()).Should().HaveCallCountOfAtLeast(2);
_mock.Method(x => x.Foo()).Should().HaveCallCountOfAtMost(5);
_mock.Method(x => x.Foo()).Should().NotHaveBeenCalled();
```
There are property **Get** and **Set** equivalents for all of the ".Method ... .Should... .HaveBeen" methods listed above:

```csharp
_mock.Get(x => x.Bar).Should().HaveBeenCalledOnce();
_mock.Set(x => x.Bar = "Baz").Should().HaveBeenCalledOnce();
```

### “Any” - Syntax alternative to “It.IsAny<*T*>”

This package incorporates [SparkyTestHelpers.Moq.Core](https://www.nuget.org/packages/SparkyTestHelpers.Moq.core), which enables simplified "Any" syntax:

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

### Reduce code duplication ###

The **Method**, **Get** and **Set** extensions return their input expressions/actions, which can be used to reduce code duplication.

For example, this test:

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

...where you have to code the same “x => x.Foo(Any.String, Any.Int, Any.InstanceOf<*Bar*>()” expression for both the .Setup and .Verify calls  -  can be simplified to:

```csharp
// Arrange:
var fooCall = _mock.Method(x => x.Foo(Any.String, Any.Int, Any.InstanceOf<Bar>())).Expression;

_mock.Setup(fooCall).Returns(true);

// Act:
subjectUnderTest.Foo("yo", 5, myBar);

// Assert:
_mock.Method(fooCall).Should().HaveBeenCalledOnce();
```