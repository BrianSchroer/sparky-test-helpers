_see also_:
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
How do you unit test private (or internal or protected) methods, properties and fields?

The short answer, which you'll find if you do a web search for this question, is **DON'T**.

You can usually indirectly test the behavior of non-public members by testing the public members that use them. If you find yourself wanting to access non-public members from unit tests, it's a "code smell" that the class you're testing should be refactored to be testable.

...Sometimes, though - maybe just temporarily so you can get tests wrapped around "legacy code" that you'll eventually be refactoring out, accessing non-public members from unit tests is a pragmatic choice. Here's how to do it:

If you're using the "MSTest" (Microsoft.VisualStudio.TestTools.UnitTesting) framework, it has a **[PrivateObject](https://docs.microsoft.com/en-us/dotnet/api/microsoft.visualstudio.testtools.unittesting.privatetype)** helper that can be used to access non-public members.

If you're using a different test framework, or if you want to use easier syntax (to do this thing that I'm telling you not to do :) ), you can the ".NonPublic()" extension method fluent syntax included in this package:

```csharp
// methods
subjectUnderTest.NonPublic().Method("PrivateMethod").Invoke();
subjectUnderTest.NonPublic().Method("PrivateMethodWithArgs").Invoke(3, "test", DateTime.Now);

object value1 = subjectUnderTest.NonPublic().Method("PrivateFunction").Invoke();
bool typedValue1 = subjectUnderTest.NonPublic().Method("PrivateFunction").Invoke();

object value2 = subjectUnderTest.NonPublic().Method("PrivateFunctionWithArgs").Invoke("test", 3, true);
bool typedValue2 = subjectUnderTest.NonPublic().Method("PrivateFunctionWIthArgs").Invoke("test", 3, true);
```
```csharp
// properties
subjectUnderTest.NonPublic().Property("PrivateDateProperty").Set(DateTime.Now);
object value = subjectUnderTest.NonPublic().Property("PrivateDateProperty").Get();
DateTime typedValue = subjectUnderTest.NonPublic().Property("PrivateDateProperty").Get<DateTime>();
```
```csharp
// fields:
subjectUnderTest.NonPublic().Field("_stringField").Set("test");
object value = subjectUnderTest.NonPublic().Field("_stringField").Get();
string typedValue = subjectUnderTest.NonPublic().Field("_stringField").Get<string>();
```

The examples above are for "instance" members. For accessing static members, use **.StaticMethod**, **.StaticProperty** and **.StaticField**:
```csharp
DateTime dt = subjectUnderTest.NonPublic().StaticMethod("PrivateStaticFunction").Invoke<DateTime>("test", 5);
bool boolValue = subjectUnderTest.NonPublic().StaticProperty("StaticProperty").Get<bool>();
string stringValue = subjectUnderTest.NonPublic().StaticField("_staticField").Get<string>();
```

MsTest's **PrivateObject** has a lot of methods with binding flag, type array, CultureInfo, etc. arguments. I honestly don't know when those would be needed, but if you're unable to accomplish what you need to with the fluent syntax described above, you can use this package's SparkyTestHelpers.NonPublic.**NonPublicMembers** class. It's a "clone" of **PrivateObject**, and uses the same syntax.
