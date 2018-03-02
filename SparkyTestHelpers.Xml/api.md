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

---

### SparkyTestHelpers.Xml.XmlTester
Helper class for testing [XDocument](https://msdn.microsoft.com/en-us/library/system.xml.linq.xdocument) values (from .config file or any XML source).

**Constructors**
* *XmlTester* **XmlTester**(*String xml*)
* *XmlTester* **XmlTester**(*XDocument xDocument*)


**Methods**

* *void* **AssertAttributeValue** *(String elementExpression, String attributeName, String expectedValue)*  
  Assert the element attribute has the expected value.  

* *void* **AssertAttributeValues** *(String elementExpression, IEnumerable<KeyValuePair<String, String>> attributeNamesAndExpectedValues)*  
  Assert that attributes have expected values.

* *void* **AssertAttributeValueMatch** *(String elementExpression, String attributeName, String pattern)*  
  Assert the element attribute value matches regular expression pattern.  

* *void* **AssertElementDoesNotExist** *(String elementExpression)*  
  Assert element does not exist for specified XPath expresion.  

* *void* **AssertElementDoesNotHaveAttribute** *(String elementExpression, String attributeName)*  
  Assert that element exists, but does not have the specified attribute.  

* *XElement* **AssertElementExists** *(String elementExpression)*  
  Assert element exists for specified XPath expression.  

* *void* **AssertElementText** *(String elementExpression, String expectedText)*  
  Assert the element contains an expected text value.  
* *void* **AssertAttributeValueIsWellFormedUrl** *(String elementExpression, String attributeName)*  
  Assert element/attribute value is a well-formed URI string.  
* *void* **AssertNoDuplicateElements** *(String elementExpression, String keyAttributeName, String[] ignoreKeys)*  
  Assert no duplicate elements found for element / key attribute combination.  

* *String* **GetAttributeValue** *(XElement elem, String attributeName)*  
  Get XElement attribute value.  

* *XElement* **GetElement** *(String elementExpression)*  
  Get first element matching XPath expression.  

* *IEnumerable<*XElement*>* **GetElements** *(String expression)*  
  Get elements matching XPath expression.  

**Properties and Fields**

* *XDocument* **XDocument**  
  The XDocument being tested.  

### SparkyTestHelpers.Xml.Config.XmlTesterConfigExtensions
**XmlTester** extension methods for testing app.config / web.config XML files:

* *void* **AssertAppSettingsKeyDoesNotExist** *(String key)*  
  Assert that specified configuration/appSettings key doesn't exist.  
* *void* **AssertAppSettingsValue** *(String key, String expectedValue)*  
  Assert expected value for configuration/appSettings key.  
* *void* **AssertAppSettingsValueMatch** *(String key, String regExPattern)*  
  Assert that value for configuration/appSettings key matches RegEx pattern.  
* *void* **AssertAppSettingsValues** *(IEnumerable<KeyValuePair<String, String>> keysAndValues)*  
  For specified dictionary of keys / expected values, verify that the actual appSetting values match. 
* *void* **AssertClientEndpointAddressIsWellFormedUrl** *(String endpointName)*  
  Assert that ServiceModel client endpoint address is a well-formed URL.  
* *void* **AssertClientEndpointAddressesAreWellFormedUrls** *()*  
  Assert that all ServiceModel client endpoint addresses are well-formed URLs.  
* *void* **AssertCompilationDebugFalse** *()*  
  Assert that *confirmation/system.web/compilation* "debug" attribute has been removed or set to false.  
* *XElement* **GetAppSettingsElement** *(String key)*  
  Get XElement for "appSettings" key.  
* *IEnumerable<*XElement*>* **GetAppSettingsElements** *()*  
  Get appSettings XElements.  
* *String* **GetAppSettingsValue** *(String key)*  
  Get value for "appSettings" key.  
* *String* **GetClientEndpointAddress** *(String endpointName)*  
  Get "address" value for ServiceModel client endpoint.  
* *XElement* **GetClientEndpointElement** *(String endpointName)*  
  Get XElement for ServiceModel client endpoint.  
* *IEnumerable<*XElement*>* **GetClientEndpointElements** *()*  
  Get service client endpoint XElements.  
* *String* **GetConnectionString** *(String name)*  
  Get connection string for name key.  
* *XElement* **GetConnectionStringElement** *(String name)*  
  Get connection string XElement for name key.  
* *IEnumerable<*XElement*>* **GetConnectionStringElements** *()*  
  Get connection string XElements.  

### SparkyTestHelpers.Xml.Config.ConfigXPath

XPath string provider for **XmlTester** testing of .config XML files:

**Static Fields**

* *String* **AnonymousAuthentication**  
  XPath string for anonymous authentication element:
  ```configuration/system.webServer/security/authentication/anonymousAuthentication``` 

* *String* **AppSettings**  
  XPath string for appSettings elements:  
  ```configuration/appSettings/add```

* *String* **ConnectionStrings**  
  XPath string for connection string elements:   
  ```configuration/connectionStrings/add``` 

* *String* **ClientEndpoints**  
  XPath string for service client endpoint elements:  
  ```configuration/system.serviceModel/client/endpoint``` 

* *String* **SystemWebCompilation**  
  XPath string for system.web compilation element:   
  ```configuration/system.web/compilation```

* *String* **WindowsAuthentication**  
  XPath string for Windows authentication element:
  ```configuration/system.webServer/security/authentication/windowsAuthentication```
 
**Static Methods**

* *String* **AppSettingForKey** *(String key)*  
  Build XPath string for AppSettings key.  

* *String* **ClientEndpointForName** *(String endpointName)*  
  Build XPath string for service client endpoint.  

* *String* **ConnectionStringForName** *(String name)*  
  Build XPath string for connection string name key.  

---
### SparkyTestHelpers.Xml.Transformation.XmlTransformer

XML .config file transformation test helper.

**Methods**

* *TransformResults* **Transform** *()*  
  Perform XML transformation(s). 

    (To help speed up your unit testing, this method caches the TransformResults keyed by the specified
    ForXmlFile and TransformedByFile(s) combination, so the time-consuming actual transformation only happens once per file combination.)

**Static Methods**

An **XmlTransformer** instance is created starting with the static "fluent" **ForXmlFile** method:
```csharp
    TransformResults transformResults = 
        XmlTransformer
        .ForXmlFile("../../web.config")
        .TransformedByFile("web.release.config")
        .Transform();
```

**ForXmlFile** takes a "params" array of relative file paths (relative to the unit test assembly file when tests are run), e.g. 

```.ForXmlFile("../../web.config", "../../../web.config")```

It takes an array because the relative file locaion can differ depending on the test runner (Visual Studio / ReSharper / build on server, etc.). The Transform method resolves each specified possible path and uses the first one where a file is found.

**TransformedByFile** also takes a [params string array[(https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/params), but one path will probably suffice because these paths are resolved relative to the file found by **ForXmlFile**, and it's likely that your base .config and transform .config file(s) are in the same folder. (You can "dot together" multiple **TransformedByFile** clauses for multi-stage transformations.)

---
### SparkyTestHelpers.Xml.Transformation.TransformResults
XML transformation results returned by the **XmlTransformer.Transform** method.

**Properties and Fields**

* *Boolean* **Successful**  
  Was the transformation successful?  

* *XDocument* **XDocument**  
  The transformed XDocument (if **Successful**; otherwise, null)  

* *String* **TransformedXml**  
  The transformed XML string (if **Successful**; otherwise, null)   

* *String* **ErrorMessage**  
  Error message (if **Successful** is false)

* *String* **Log**  
  Details about the files and transformation steps involved. Look here for details if not **Successful**.  
