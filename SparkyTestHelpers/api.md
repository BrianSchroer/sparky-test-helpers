This package contains:

* **SparkyTestHelpers.Exceptions**: helpers for testing that an expected exception is thrown, with the expected message
* **SparkyTestHelpers.Scenarios**: helpers for testing a method with a variety of different input scenarios

_see also_: [SparkyTestHelpers.Scenarios.MsTest](https://www.nuget.org/packages/SparkyTestHelpers.Scenarios.MsTest/): MS Test / VisualStudio.TestTools implementation for properly handling `Assert.Inconclusive()` with the Visual Studio test runner

---

## AssertExceptionNotThrown

_class SparkyTestHelpers.Exceptions.AssertExceptionNotThrown_

Assert that an exception is not thrown when an action is executed. This class/method doesn't do much,
but it clarifies the intent of Unit Tests that wish to show that an action works correctly.

**Static Methods**

* _void_ **WhenExecuting** _(Action action)_  
  Asserts that an exception was not thrown when executing an Action.

**Example**

```csharp
using SparkyTestHelpers.Exceptions;
```

```csharp
AssertExceptionNotThrown.WhenExecuting(() => foo.Bar(baz));
```

---

## AssertExceptionThrown

_class SparkyTestHelpers.Exceptions.AssertExceptionThrown_

This class is used to assert than an expected exception is thrown when a test action is executed.

Why would you want to use this class instead of something like the VisualStudio TestTools ExpectedExceptionAttribute?

* This class allows you to check the exception message.
* This class allows you to assert than exception is thrown for a specific
  statement, not just anywhere in the test method.

There is no public constructor for this class. It is constructed using the "fluent" static factory method **AssertExceptionThrown.OfType<TException>()**.

**Example**

```csharp
using SparkyTestHelpers.Exceptions;
```

```csharp
AssertExceptionThrown
    .OfType<ArgumentOutOfRangeException>()
    .WithMessage("Limit cannot be greater than 10.")
    .WhenExecuting(() => { var foo = new Foo(limit: 11); });
```

**Methods**

* _AssertExceptionThrown_ **WithMessage** _(String expected)_  
  Set up to test that the action under test throws an exception where the message exactly matches the message.

* _AssertExceptionThrown_ **WithMessageStartingWith** _(String expected)_  
  Set up to test that the action under test throws an exception where the message starts with the message.

* _AssertExceptionThrown_ **WithMessageContaining** _(String expected)_  
  Set up to test that the action under testthrows an exception where the message contains themessage.

* _AssertExceptionThrown_ **WithMessageMatching** _(String regExPattern)_  
  Set up to test that the action under test throws an exception where the message matches the specified regular expression pattern.

* _Exception_ **WhenExecuting** _(Action action)_  
  Call the action that should throw an exception, and assert that the exception was thrown.

**Static Methods**

* _AssertExceptionThrown_.**OfType<TException>\***()\*

  * Set up to test that the action under test throws an exception of this specific type.

* _AssertExceptionThrown_.**OfTypeOrSubclassOfType<TException>\***()\*
  * Set up to test that the action under test throws an exception of this type of a subclass of this type.

---

## ScenarioTester<TScenario>

_class SparkyTestHelpers.Scenarios.ScenarioTester<TScenario>_

VisualStudio.TestTools doesn't have "RowTest" or "TestCase" attributes like NUnit or other .NET testing frameworks. (It does have a way to do data-driven tests, but it's pretty cumbersome.) This class provides the ability to execute the same test code for multiple test cases and, after all test cases have been executed, failing the unit test if any of the test cases failed.

Even if you're not testing with MSTest / VisualStudio.TestTools, these helpers provide an alternative syntax for "row testing".

This class is rarely used directly. It is more often used via the IEnumerable<TScenario>.**TestEach** extension method (see below).

When one or more of the test scenarios fails, the failure exception shows which were unsuccessful, for example, this scenario test:

```csharp
ForTest.Scenarios
(
    new { DateString = "1/31/2023", ShouldBeValid = true },
    new { DateString = "2/31/2023", ShouldBeValid = true },
    new { DateString = "3/31/2023", ShouldBeValid = true },
    new { DateString = "4/31/2023", ShouldBeValid = true },
    new { DateString = "5/31/2023", ShouldBeValid = true },
    new { DateString = "6/31/2023", ShouldBeValid = false }
)
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.ShouldBeValid, DateTime.TryParse(scenario.DateString, out dt));
});
```

...throws this exception:

```
Test method SparkyTestHelpers.UnitTests.DateTests threw exception:
SparkyTestHelpers.Scenarios.ScenarioTestFailureException: Scenario[1] (2 of 6) - Assert.AreEqual failed. Expected:<True>. Actual:<False>.

Scenario data - anonymousType: {"DateString":"2/31/2023","ShouldBeValid":true}

Scenario[3] (4 of 6) - Assert.AreEqual failed. Expected:<True>. Actual:<False>.

Scenario data - anonymousType: {"DateString":"4/31/2023","ShouldBeValid":true}
```

---

## ScenarioTesterExtension

_class SparkyTestHelpers.Scenarios.ScenarioTesterExtension_

**ScenarioTester<TScenario>** extension methods.

**Static Methods**

* _ScenarioTester<TScenario>_ **TestEach** _(IEnumerable<TScenario> enumerable, Action<TScenario> test)_

**Example**

```csharp
using SparkyTestHelpers.Scenarios;
```

```csharp
new []
{
    new { DateString = "1/31/2023", ShouldBeValid = true },  
    new { DateString = "2/31/2023", ShouldBeValid = false },  
    new { DateString = "3/31/2023", ShouldBeValid = true },  
    new { DateString = "4/31/2023", ShouldBeValid = false },  
    new { DateString = "5/31/2023", ShouldBeValid = true },  
    new { DateString = "6/31/2023", ShouldBeValid = false }
}
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.ShouldBeValid, DateTime.TryParse(scenario.DateString, out dt));  
});  
```

---

## ForTest

_class SparkyTestHelpers.Scenarios.ForTest_

"Syntactic sugar" methods for working with **ScenarioTester<TScenario>**

**Static Methods**

* _TScenario[]_ **Scenarios** _(TScenario[] scenarios)_ - creates array of scenarios that can be "dotted" to the **TestEach** extension method:

**Example**

```csharp
using SparkyTestHelpers.Scenarios;
```

```csharp
ForTest.Scenarios
(
    new { DateString = "1/31/2023", ShouldBeValid = true },  
    new { DateString = "2/31/2023", ShouldBeValid = false },  
    new { DateString = "3/31/2023", ShouldBeValid = true },  
    new { DateString = "4/31/2023", ShouldBeValid = false },  
    new { DateString = "5/31/2023", ShouldBeValid = true },  
    new { DateString = "6/31/2023", ShouldBeValid = false }
)
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.ShouldBeValid, DateTime.TryParse(scenario.DateString, out dt));  
});  
```

* _IEnumerable<TEnum>_ **EnumValues** _()_ - tests each value in an enum.

**_Example_**

```csharp
using SparkyTestHelpers.Scenarios;
```

```csharp
ForTest.EnumValues<OrderStatus>()
    .TestEach(orderStatus => foo.Bar(orderStatus));
```

* _IEnumerable<TEnum>_ **ExceptFor** _(IEnumerable<TEnum> values, TEnum[] valuesToExclude)_ - Exclude enum values from test scenarios.

**_Example_**

```csharp
using SparkyTestHelpers.Scenarios;
```

```csharp
ForTest.EnumValues<OrderStatus>()
    .ExceptFor(OrderStatus.Cancelled)
    .TestEach(orderStatus => foo.Bar(orderStatus));
```
