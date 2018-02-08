
## AssertExceptionNotThrown
*class SparkyTestHelpers.Core.Exceptions.AssertExceptionNotThrown*

Assert that an exception is not thrown when an action is executed. This class/method doesn't do much,
but it clarifies the intent of Unit Tests that which to show that an action works correctly.

**Static Methods**

* *void* **WhenExecuting** *(Action action)*  
  Asserts that an exception was not thrown when executing an Action.  

**Example**

```csharp
AssertExceptionNotThrown.WhenExecuting(() => foo.Bar(baz));
```
---

## AssertExceptionThrown
*class SparkyTestHelpers.Core.Exceptions.AssertExceptionThrown*

This class is used to assert than an expected exception is thrown when a
test action is executed.

Why would you want to use this class instead of something like the
VisualStudio TestTools ExpectedExceptionAttribute? 
* This class allows you to check the exception message.
* This class allows you to assert than exception is thrown for a specific
statement, not just anywhere in the test method.

There is no public constructor for this class. It is constructed using the
"fluent" static factory method `AssertExceptionThrown.OfType<TException>()`.

**Example**

```csharp
AssertExceptionThrown
    .OfType<ArgumentOutOfRangeException>()
    .WithMessage("Limit cannot be greater than 10.")
    .WhenExecuting(() => { var foo = new Foo(limit: 11); });
```

**Methods**

* *AssertExceptionThrown* **WithMessage** *(String expected)*  
  Set up to test that the action under test
throws an exception where the message exactly matches the message.  

* *AssertExceptionThrown* **WithMessageStartingWith** *(String expected)*  
  Set up to test that the action under test
throws an exception where the message starts with the message.  

* *AssertExceptionThrown* **WithMessageContaining** *(String expected)*  
  Set up to test that the action under test
throws an exception where the message contains themessage.  


* *AssertExceptionThrown* **WithMessageMatching** *(String regExPattern)*  
  Set up to test that the action under test 
throws an exception where the message matches the specified retular expressionpattern.  


* *Exception* **WhenExecuting** *(Action action)*  
  Call the action that should throw an exception, and assert that the exception was thrown.  


**Static Methods**


* *AssertExceptionThrown*.**OfType&lt;TException&gt;***()*
  - Set up to test that the action under test throws an exception of this specific type.


* *AssertExceptionThrown*.**OfTypeOrSubclassOfType&&lt;TException&gt;***()*
  - Set up to test that the action under test throws an exception of this type of a subclass of this type.

---

## ScenarioTester&lt;TScenario&gt;
*class SparkyTestHelpers.Core.Scenarios.ScenarioTester&lt;TScenario&gt;*

VisualStudio.TestTools doesn't have "RowTest" or "TestCase" attributes like
NUnit or other .NET testing frameworks. (It does have a way to do data-driven
tests, but it's pretty cumbersome.) This class provides the ability to execute the same test code for multiple test
cases and, after all test cases have been executed, failing the unit test if 
any of the test cases failed.

This class is rarely used directly. It is more often used via the 
IEnumerable&lt;TScenario&gt;.**TestEach** extension method...

---

## ScenarioTesterExtension
*class SparkyTestHelpers.Core.Scenarios.ScenarioTesterExtension*

**ScenarioTester&lt;TScenario&gt;** extension methods.

**Static Methods**

* *ScenarioTester&lt;TScenario&gt;* **TestEach** *(IEnumerable&lt;TScenario&gt; enumerable, Action&lt;TScenario&gt; test)*  

**Example**

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
*class SparkyTestHelpers.Core.Scenarios.ForTest*

"Syntactic sugar" methods for working with **ScenarioTester&lt;TScenario&gt;**

**Static Methods**

* *TScenario[]* **Scenarios** *(TScenario[] scenarios)*  - creates array of scenarios that can be "dotted" to the **TestEach** extension method:

**Example**

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

* *IEnumerable&lt;TEnum&gt;* **EnumValues** *()* - tests each value in an enum.

***Example***

```csharp
 ForTest.EnumValues<OrderStatus>()
    .TestEach(orderStatus => foo.Bar(orderStatus));
```

* *IEnumerable&lt;TEnum&gt;* **ExceptFor** *(IEnumerable&lt;TEnum&gt; values, TEnum[] valuesToExclude)* - Exclude enum values from test scenarios.

***Example***

```csharp
 ForTest.EnumValues<OrderStatus>()
    .ExceptFor(OrderStatus.Cancelled)
    .TestEach(orderStatus => foo.Bar(orderStatus));
```


