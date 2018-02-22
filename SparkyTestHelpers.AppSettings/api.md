_see also_: 
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)

## AppSettingsHelper

If you need to test code that directly uses **ConfigurationManager.AppSettings** to retrieve .config file values, your test code can say:

```csharp
    ConfigurationManager.AppSettings["key"] = "test value";
```
...but that leaves your test value in the static ConfigurationManager.AppSettings collection, which can cause problems with tests of other code using the same app setting.

This class lets you test with one or more AppSettings overrides (```WithAppSetting``` is required. You can use zero or as many ```AndAppSetting``` clauses as you need), and automatically undo the overrides when the test is complete:

```csharp
using SparkyTestHelpers.AppSettings
...
    AppSettingsHelper
        .WithAppSetting("key1", "test value 1")
        .AndAppSetting("key2", "test value 2")
        .Test(() =>
        {
            // Code to test method that uses "key1"/"key2" 
            // ConfigurationManager.AppSettings values
        });
```

*But there's a better way...*

## AppSettings.DependencyProvider

Code with static dependencies can be made more testable by using a dependency-injectable wrapper around the static code. [**SparkyTools.DependencyProvider**](https://www.nuget.org/packages/SparkyTools.DependencyProvider/) provides one way to do this.

[**SparkyTools.XmlConfig**](https://www.nuget.org/packages/SparkyTools.XmlConfig/) has an **AppSettings.DependencyProvider()** method that creates a DependencyProvider wrapping ConfigurationManager.AppSettings, and this package has an **AppSettings.TestDependencyProvider()** method that creates a provider that serves from a key/value dictionary:

*Class using **DependencyProvider**:*
```csharp
using SparkyTools.DependencyProvider;

public class Foo
{
    private readonly Func<string, string> _getAppSetting;

    public Foo(DependencyProvider<Func<string, string> appSettingsProvider)
    {
        _getAppSetting = appSettingsProvider.GetValue();
    }

    public void MethodUsingAppSettings()
    {
        string valueFromAppSettings = _getAppSetting("bar");
    }
}
```
*Production code using ConfigurationManager.AppSettings DependencyProvider:*
```csharp
using SparkyTools.XmlConfig;
...
    var foo = new Foo(AppSettings.DependencyProvider());
```
*Unit test code using dictionary DependencyProvider:*
```csharp
using SparkyTestHelpers.AppSettings;
...
    var foo = new Foo(AppSettings.DependencyProvider(
        new Dictionary<string, string>{ { "bar", "test bar value" } });
```

