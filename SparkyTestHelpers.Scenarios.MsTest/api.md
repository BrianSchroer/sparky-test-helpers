This package contains an implementation of **[SparkyTestHelpers](https://www.nuget.org/packages/SparkyTestHelpers/).Scenarios** for "MsTest" / VisualStudio.TestTools.

The only differences are:
* If you use MsTest's `Assert.Inconclusive()` in a scenario test, your scenario "suite" will be recognized as inconclusive and not as a failure by the Visual Studio test runner.
* The "using" statement is `using SparkyTestHelpers.Scenarios.MsTest;` instead of `using SparkyTestHelpers.Scenarios;` 
---
_see also_:
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## MsTestScenarioTester<TScenario>

_class SparkyTestHelpers.Scenarios.MsTest.MsTestScenarioTester<TScenario>_

VisualStudio.TestTools doesn't have "RowTest" or "TestCase" attributes like NUnit or other .NET testing frameworks. (It does have a way to do data-driven tests, but it's pretty cumbersome.) This class provides the ability to execute the same test code for multiple test cases and, after all test cases have been executed, failing the unit test if any of the test cases failed.

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

## MsTestScenarioTesterExtension

_class SparkyTestHelpers.Scenarios.MsTest.MsTestScenarioTesterExtension_

**MsTestScenarioTester<TScenario>** extension methods.

**Static Methods**

* _MsTestScenarioTester<TScenario>_ **TestEach** _(IEnumerable<TScenario> enumerable, Action<TScenario> test)_

**Example**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
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

_class SparkyTestHelpers.Scenarios.MsTest.ForTest_

"Syntactic sugar" methods for working with **MsTestScenarioTester<TScenario>**

**Static Methods**

* _TScenario[]_ **Scenarios** _(TScenario[] scenarios)_ - creates array of scenarios that can be "dotted" to the **TestEach** extension method:

**Example**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
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
using SparkyTestHelpers.Scenarios.MsTest;
```

```csharp
ForTest.EnumValues<OrderStatus>()
    .TestEach(orderStatus => foo.Bar(orderStatus));
```

* _IEnumerable<TEnum>_ **ExceptFor** _(IEnumerable<TEnum> values, TEnum[] valuesToExclude)_ - Exclude enum values from test scenarios.

**_Example_**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
```

```csharp
ForTest.EnumValues<OrderStatus>()
    .ExceptFor(OrderStatus.Cancelled)
    .TestEach(orderStatus => foo.Bar(orderStatus));
```
