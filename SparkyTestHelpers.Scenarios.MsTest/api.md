This package contains an implementation of **[SparkyTestHelpers](https://www.nuget.org/packages/SparkyTestHelpers/).Scenarios** for "MsTest" / VisualStudio.TestTools.

The only differences are:
* If you use MsTest's `Assert.Inconclusive()` in a scenario test, your scenario "suite" will be recognized as inconclusive and not as a failure by the Visual Studio test runner.
* The "using" statement is `using SparkyTestHelpers.Scenarios.MsTest.MsTest;` instead of `using SparkyTestHelpers.Scenarios.MsTest;` 
---
_see also_:
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## MsTestScenarioTester<*TScenario*>
VisualStudio.TestTools now has ["DataMemberTest" and "DataRow" attributes](http://pmichaels.net/2016/07/23/using-mstest-datarow-as-a-substitute-for-nunit-testcase/),  but they didn't when I started using the framework, so I wrote my own "scenerio testing" tool based my experience with NUnit "TestCase" attributes. This class provides the ability to execute the same test code for multiple test cases and, after all test cases have been executed, fail the unit test if any of the test cases failed.

Even if your test framework has attribute-based scenario testing, these helpers provide an alternative syntax that you might find useful.

This class is rarely used directly. It's easier to use with IEnumerable<*TScenario*>.**TestEach** or **ForTest.Scenarios** (see below).

When one or more of the scenarios fails, the failure exception shows which were unsuccessful, for example, this scenario test:

```csharp
ForTest.Scenarios
(
    new { DateString = "1/31/2023", IsGoodDate = true },
    new { DateString = "2/31/2023", IsGoodDate = true },
    new { DateString = "6/31/2023", IsGoodDate = false }
)
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.IsGoodDate, DateTime.TryParse(scenario.DateString, out dt));
});
```

...throws this exception:

```
Test method SparkyTestHelpers.UnitTests.DateTests threw exception:
SparkyTestHelpers.Scenarios.ScenarioTestFailureException: Scenario[1] (2 of 3) - Assert.AreEqual failed. Expected:<True>. Actual:<False>.

Scenario data - anonymousType: {"DateString":"2/31/2023","IsGoodDate":true}

Scenario[2] (3 of 3) - Assert.AreEqual failed. Expected:<True>. Actual:<False>.

Scenario data - anonymousType: {"DateString":"4/31/2023","IsGoodDate":true}
```

**Public Methods**

* *ScenarioTester* **BeforeEachTest**(Action<*TScenario*>)
 
    Defines action to called before each scenario is tested.

**Example**
```csharp
ForTest.Scenarios(*array*)
.BeforeEachTest(scenario => 
{
    // do something, e.g. reset mocks, log scenario details
});
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.IsGoodDate, DateTime.TryParse(scenario.DateString, out dt));
});
```
* *ScenarioTester* **AfterEachTest**(*Action<*TScenario*>)
 
    Defines function called after each scenario is tested.
    The function receives the scenario and the exception (if any) caught by the test. 
    If the function returns true, the scenario test is "passed". 
    If false, exception is thrown to fail the test

**Example**
```csharp
ForTest.Scenarios(*array*)
.AfterEachTest((scenario, ex) => 
{
    // do something, e.g. log scenario details, 
    // decide if scenario with exception should be passed
    return ex == null;
});
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.IsGoodDate, DateTime.TryParse(scenario.DateString, out dt));
});
```
 ---

## ScenarioTesterExtension
**ScenarioTester<*TScenario*>** extension methods.

**Static Methods**

* _ScenarioTester<*TScenario*>_ **TestEach**_(IEnumerable<*TScenario*> enumerable, Action<*TScenario*> test)_

**Example**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
. . .
new []
{
    new { DateString = "1/31/2023", IsGoodDate = true },  
    new { DateString = "2/31/2023", IsGoodDate = false } 
}
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.IsGoodDate, DateTime.TryParse(scenario.DateString, out dt));  
});  
```
---

## ForTest
"Syntactic sugar" methods for working with **ScenarioTester<*TScenario*>**

**Static Methods**

* **ForTest.Scenarios**_(TScenario[] scenarios)_ - creates array of scenarios that can be "dotted" to the **TestEach** extension method:

**Example**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
. . .
ForTest.Scenarios
(
    new { DateString = "1/31/2023", IsGoodDate = true },  
    new { DateString = "2/31/2023", IsGoodDate = false }
)
.TestEach(scenario =>
{
    DateTime dt;
    Assert.AreEqual(scenario.IsGoodDate, DateTime.TryParse(scenario.DateString, out dt));  
});  
```

* _IEnumerable<*TEnum*>_ **EnumValues**_()_ - tests each value in an enum.

**_Example_**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
. . .
ForTest.EnumValues<OrderStatus>()
    .TestEach(orderStatus => foo.Bar(orderStatus));
```

* _IEnumerable<*TEnum*>_ **ExceptFor**_(IEnumerable<*TEnum*> values, TEnum[] valuesToExclude)_ - Exclude enum values from test scenarios.

**_Example_**

```csharp
using SparkyTestHelpers.Scenarios.MsTest;
. . .
ForTest.EnumValues<OrderStatus>()
    .ExceptFor(OrderStatus.Cancelled)
    .TestEach(orderStatus => foo.Bar(orderStatus));
```