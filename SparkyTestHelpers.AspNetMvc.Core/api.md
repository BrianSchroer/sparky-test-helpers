_see also_:
* **[SparkyTestHelpers.AspNetMvc](https://www.nuget.org/packages/SparkyTestHelpers.AspNetMvc)** - the .NET Framework version of this package
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## Controller Action test helpers
### ControllerTester<*TController*>

Instantiation:
```csharp
using SparkyTestHelpers.AspNetMvc
```

```csharp
    var homeController = new HomeController(/* with test dependencies */);
    var controllerTester = new ControllerTester<HomeController>(homeController);
```

It doesn't do anything on its own - just provides an **Action**(*actionDefinitionExpression*) method that's used to create a... 

### ControllerActionTester
```csharp
    var controllerActionTester = 
        new ControllerTester<HomeController>(homeController).Action(x => x.Index);
```

The *.Action* method argument is an expression for either a synchronous or async controller action method.

ControllerActionTester has several **.Test**... methods used to assert that the controller action returns the expected **IActionResult** implementation. There are methods for all of the standard result types, plus the generic **TestResult<*TActionResultType*>** method:

* **.TestContent**(*Action<*ContentResult*> validate*)
* **.TestEmpty**(*Action<*EmptyResult*> validate*)
* **.TestFile**(*Action<*FileResult*> validate*)
* **.TestJson**(*Action<*JsonResult*> validate*)
* **.TestOkObject**(*Action<*OkObjectResult*> validate*)
* **.TestPartialView**(*Action<*PartialViewResult*> validate*)
* **.TestRedirectToAction**(*string expecteActionName, string expectedControllerName, object expectedRouteValues, Action<*RedirectToRouteResult*> validate*)
* **.TestRedirectToPage**(*string expectedPageName, Action<*RedirectToPageResult*> validate*)
* **.TestRedirectToRoute**(*string expectedRoute, Action<*RedirectToRouteResult*> validate*)
* **.TestView**(*Action<*ViewResult*> validate*)
* **.TestResult<*TActionResultType*>**(*Action<*TActionResultType*> validate*)

**Additional methods:**
* **.ExpectingViewName**(*string expectedViewName*) - used with **.TestView** and **.TestPartialView**
* **.ExpectingModel<*TModelType*>**(*Action<*TModelType*> validate*) - used with **.TestView** and **.TestJson**
* **WhenModelStateIsValidEquals**(*bool isValid*) - used to test conditional logic based on ModelState.IsValid

All *validate* "callback" actions shown above are optional.

#### Examples

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
## [Razor Page](https://docs.microsoft.com/en-us/aspnet/core/mvc/razor-pages/?tabs=visual-studio) test helpers

### PageModelTester<*TPageModel*>

ASP.NET MVC Razor Page **PageModel**s have a lot in common with Controllers (they kind of combine the "C" and "M" of MVC), so the tester classes are pretty similar to **ControllerTester** and **ControllerActionTester**...

PageModelTester Instantiation:
```csharp
using SparkyTestHelpers.AspNetMvc
```

```csharp
    var homeModel = new HomeModel(/* with test dependencies */);
    var pageTester = new PageModelTester<HomeModel>(homeModel);
```

It doesn't do anything on its own - just provides an **Action**(*actionDefinitionExpression*) method that's used to create a... 

### PageModelActionTester<*TPageModel*>
```csharp
    var actionTester = 
        new PageModelTester<HomeModel>(homeModel).Action(x => x.OnGet);
```

The *.Action* method argument is an expression for either a synchronous or async PageModel action method.

PageModelActionTester has several **.Test**... methods used to assert that the PageModel action returns the expected **IActionResult** implementation. There are methods for many standard result types, plus the generic **TestResult<*TActionResultType*>** method:

* **.TestContent**((*optional*) *Action<*ContentResult*> validate*)
* **.TestFile**((*optional*) *Action<*FileResult*> validate*)
* **.TestJsonResult**((*optional*) *Action<*JsonResult*> validate*)
* **.TestPage**((*optional*) *Action<*PageResult*> validate*)
* **.TestRedirectToAction**(*string expecteActionName, string expectedControllerName, object expectedRouteValues, (*optional*)  Action<*RedirectToRouteResult*> validate*)
* **.TestRedirectToPage**(*string expectedPageName, (*optional*) Action<*RedirectToPageResult*> validate*)
* **.TestRedirectToRoute**(*string expectedRoute, (*optional*) Action<*RedirectToRouteResult*> validate*)
* **.TestResult<*TActionResultType*>**((*optional*) *Action<*TActionResultType*> validate*)

**Additional methods:**
* **.ExpectingModel<*TModelType*>**((*optional*) *Action<*TModelType*> validate*) - used with **.TestJson**. Can also be used with **.TestPage**, but it's more efficient to use...
* **.ExpectingModel**(*Action<*TPageModel*> validate*) - used with **.TestPage** to perform validation PageModel property validation
* **WhenModelStateIsValidEquals**(*bool isValid*) - used to test conditional logic based on ModelState.IsValid

#### Examples
```csharp
    var homeModel = new HomeModel(/* with test dependencies */);
    var pageTester = new PageModelTester<HomeModel>(homeModel);

    pageTester
        .Action(x => x.OnGet)
        .ExpectingModel(model => Assert.IsTrue(model.Foo))
        .TestPage();

    pageTester
        .Action(x => x.OnPost)
        .WhenModelStateIsValidEquals(false)
        .ExpectingModel(model => Assert.AreEqual(expectedErrorMessage, model.ErrorMessage))
        .TestPage();

    pageTester
        .Action(x => x.Post)
        .WhenModelStateIsValidEquals(true)
        .TestRedirectToPage("UpdateSuccessful");
```
## [ViewComponent](https://docs.microsoft.com/en-us/aspnet/core/mvc/views/view-components) test helpers
**ViewComponentTester<*TViewComponent*>** and **ViewComponentInvocationTester** are very similar to their Controller... and PageModel... counterparts.

### ViewComponentTester methods
* **.Invocation**(*synchronous Invoke method expression*)
* **.Invocation**(*async InvokeAsync method expression*)

### ViewComponentInvocationTester methods
* **.ExpectingViewName**(*string expectedViewName*) - used with **.TestView** 
* **.ExpectingModel<*TModelType*>**(*Action<*TModelType*> validate*) - used with **.TestView**
* **.TestContent**(*Action<*ContentViewComponentResult*> validate*)
* **.TestHtmlContent**(*Action<*HtmlContentViewComponentResult*> validate*)
* **.TestView**(*Action<*ViewViewComponentResult*> validate*)
* **.TestResult<*TViewComponentResultType*>**(*Action<*TViewComponentResultType*> validate*)
* **WhenModelStateIsValidEquals**(*bool isValid*) - used to test conditional logic based on ModelState.IsValid

All *validate* "callback" actions shown above are optional.

#### Examples
```csharp
using SparkyTestHelpers.AspNetMvc
```
```csharp
new ViewComponentTester<FooViewComponent>()
    .Invocation(x => x.Invoke)
    .ExpectingViewName("Default")
    .ExpectingModel(model => Assert.IsTrue(model.Baz))
    .TestView();

new ViewComponentTester<BarViewComponent>()
    .Invocation(x => x.InvokeAsnyc)
    .TestView();
```