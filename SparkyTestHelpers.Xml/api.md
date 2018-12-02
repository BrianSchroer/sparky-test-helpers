_see also_: 
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)

---
This NuGet package contains helpers to perform .config file XML transformations, and to test the resulting transformed XML (or actually any XML, whether it's .config format or not), e.g.:

```csharp
using SparkyTestHelpers.Xml;
using SparkyTestHelpers.Xml.Config;
using SparkyTestHelpers.Xml.Transformation;
``` 
```csharp
// In test setup/initialize method:
    TransformResults transformResults = 
        XmlTransformer
        .ForXmlFile("../../web.config")
        .TransformedByFile("web.release.config")
        .Transform();

    if (!transformResults.Successful)
    {
        Assert.Fail(transformResults.ErrorMessage);
    }

    _xmlTester = new XmlTester(transformResults.XDocument);

. . .

// In unit test method:
    _xmlTester.AssertAppSettingsValue("testKey", "expectedValue");
```

See https://github.com/BrianSchroer/sparky-test-helpers/blob/master/SparkyTestHelpers.Xml/api.md for complete API documentation for this package.
