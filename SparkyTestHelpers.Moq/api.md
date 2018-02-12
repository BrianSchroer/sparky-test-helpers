## [Moq](https://github.com/moq) syntax helpers ##

_see also_:
* **[SparkyTestHelpers](https://www.nuget.org/packages/SparkyTestHelpers/)**: exception expectation and "scenario test" (row test) helpers

---

### "Any" syntax

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
* Any.Boolean
* Any.DateTime
* Any.Decimal
* Any.Double
* Any.Int
* Any.Object
* Any.String
* Any.InstanceOf&lt;T&gt;
* Any.Array&lt;T&gt;
* Any.IEnumerable&lt;T&gt;
* Any.List&lt;T&gt;
* Any.Dictionary&lt;TKey, TValue&gt;
* Any.KeyValuePair&lt;TKey, TValue&gt;
* Any.Tuple&lt;T1, T2&gt;

### Alternate "Verify" syntax

```csharp
_mock.Verify(x => x.Foo("bar", 3), Times.Once);
```
...can be coded as:
```csharp
using SparkyTestHelpers.Moq;
. . .
_mock.VerifyOneCallTo(x => x.Foo("bar", 3));
```

#### "Verify" extension methods:
* VerifyCallCount(*int count, expresssion*)
* VerifyOneCallTo(*expression*)
* VerifyAtLeastOneCallTo(*expression*)
* VerifyAtMostOneCallTo(*expression*)
* VerifyNoCallsTo(*expression*)
* VerifyGetCount(*int count, expresssion*)
* VerifyOneGet(*expression*)
* VerifyAtLeastOneGet(*expression*)
* VerifyAtMostOneGet(*expression*)
* VerifyNoGets(*expression*)
* VerifySetCount(*int count, expresssion*)
* VerifyOneSet(*expression*)
* VerifyAtLeastOneSet(*expression*)
* VerifyAtMostOneSet(*expression*)
* VerifyNoSets(*expression*)

### *mock*.Where extension method
...provides an alternate syntax for "It.Is":
```csharp
using SparkyTestHelpers.Moq;
. . .
// sad:
_mock.Setup(x => x.Foo(It.Is<int>(i => i % 2 == 0))).Returns(true);
// rad!:
_mock.Setup(x => x.Foo(Any.Int.Where(i => i % 2 == 0))).Returns(true);_
```
### *mock*.Expression extension method
...makes it easy to create a reusable expression so you don't duplicate code in ".Setup" and ".Verify" calls:
```csharp
_mock.Setup(x => x.Foo(Any.String, Any.Int, Any.InstanceOf<Bar>())).Returns(true);
subjectUnderTest.Fooify("yo", 5, myBar);
_mock.VerifyOneCallTo(x => x.Foo(Any.String, Any.Int, Any.InstanceOf<Bar>()));
```
...can be coded as:
```csharp
using SparkyTestHelpers.Moq;
. . .
var fooExp = _mock.Expression(x => x.Foo(Any.String, Any.Int, Any.InstanceOf<Bar>()));
_mock.Setup(fooExp).Returns(true);
subjectUnderTest.Fooify("yo", 5, myBar);
_mock.VerifyOneCallTo(fooExp);
```