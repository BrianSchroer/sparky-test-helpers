_see also_:
* **[SparkyTestHelpers.AspNetMvc.Core](https://www.nuget.org/packages/SparkyTestHelpers.AspNetMvc.Core)** - the .NET Core version of this package
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## ControllerTester<*TController*>

Instantiation:
```csharp
using SparkyTestHelpers.AspNetMvc
```

```csharp
    var homeController = new HomeController(/* with test dependencies */);
    var controllerTester = new ControllerTester<HomeController>(homeController);
```

It doesn't do anything on its own - just provides an **Action**(*actionDefinitionExpression*) method that's used to create a... 

## ControllerActionTester
```csharp
    var controllerActionTester = 
        new ControllerTester<HomeController>(homeController).Action(x => x.Index);
```

ControllerActionTester has several **.Test**... methods used to assert that the controller action returns the expected **ActionResult** implementation. There are methods for all of the standard result types, plus the generic **TestResult<*TActionResultType*>** method:

* **.TestContent**(*Action<*ContentResult*> validate*)
* **.TestEmpty**(*Action<*EmptyResult*> validate*)
* **.TestFile**(*Action<*FileResult*> validate*)
* **.TestJson**(*Action<*JsonResult*> validate*)
* **.TestPartialView**(*Action<*PartialViewResult*> validate*)
* **.TestRedirect**(*string expectedUrl, Action<*RedirectResult*> validate*)
* **.TestRedirectToAction**(*string expecteActionName, Action<*RedirectToRouteResult*> validate*)
* **.TestRedirectToRoute**(*string expectedRoute, Action<*RedirectToRouteResult*> validate*)
* **.TestView**(*Action<*ViewResult*> validate*)
* **.TestResult<*TActionResultType*>**(*Action<*TActionResultType*> validate*)

**Additional methods:**
* **.ExpectingViewName**(*string expectedViewName*) - used with **.TestView** and **.TestPartialView**
* **.ExpectingModel<*TModelType*>**(*Action<*TModelType*> validate*) - using with **.TestView** and **.TestJson**
* **WhenModelStateIsValidEquals**(*bool isValid*) - used to test conditional logic based on ModelState.IsValid

All *validate* "callback" actions shown above are optional.

### Examples

```csharp
    var homeController = new HomeController(/* with test dependencies */);
    var controllerTester = new ControllerTester<HomeController>(homeController);

    controllerTester.Action(x => x.Index).TestView();

    controllerTester
        .Action(x => () => x.Details(3))
        .ExpectingViewName("Details")
        .ExpectingModel<Foo>(foo => Assert.IsTrue(foo.IsValid))
        .TestView();

    controllerTester
        .Action(x => () => x.Edit(updateModel))
        .WhenModelStateIsValidEquals(false)
        .TestRedirectToAction("Errors");

    controllerTester
        .Action(x => () => x.Edit(updateModel))
        .WhenModelStateIsValidEquals(true)
        .ExpectingViewName("UpdateSuccessful")
        .TestRedirectToRoute("Home/UpdateSuccessful");
```
