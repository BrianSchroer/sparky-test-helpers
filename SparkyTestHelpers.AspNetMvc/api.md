_see also_:
* **[SparkyTestHelpers.AspNetMvc.Core](https://www.nuget.org/packages/SparkyTestHelpers.AspNetMvc.Core)** - the .NET Core version of this package
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## ControllerTester<*TController*>

Instantiation:
```csharp
using SparkyTestHelpers.AspNetMvc;
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
## RouteTester
**RouteTester** and **RoutingAsserter** provide methods to assert that a given relative URL maps to the expected [RouteData.Values](https://docs.microsoft.com/en-us/dotnet/api/system.web.routing.routedata.values?view=netframework-4.7#System_Web_Routing_RouteData_Values). The **RoutingAsserter.AssertMapTo** overloads provide multiple ways to specify the expected values...

**Constructor**

 public **RouteTester**(Action<*RouteCollection*> *routeRegistrationMethod*)

```csharp
using SparkyTestHelpers.AspNetMvc.Routing;
. . .
    var routeTester = new RouteTester(RouteConfig.RegisterRoutes);
```

### methods

* .**ForUrl**(string *relativeUrl*) - creates a new **RoutingAsserter** instance.

## RoutingAsserter

### methods

* .**AssertMapTo**(IDictionary<string, object> *expectedValues*)
* .**AssertMapTo**(object *routeValues*)
* .**AssertMapTo**(string *controller*, string *action*, (object *id*)) - id defaults to null
* .**AssertMapTo<*TController*>**(Expression<Func<TController, Func<*ActionResult*>>> *actionExpression*)
* .**AssertRedirectTo**(string **expectedUrl**, (HttpStatusCode *expectedStatusCode)) - expectedStatusCode defaults to HttpStatusCode.Redirect (302)

### examples

```csharp
routeTester.ForUrl("Default.aspx)
    .AssertRedirectTo("Home/LegacyRedirect");

// alternate syntaxes for asserting Home/Index routing:
routeTester.ForUrl("Home/Index")
    .AssertMapTo(new Dictionary<string, object> 
        { { "controller", "Home" }, { "action", "Index" }, { "id", null } );
routeTester.ForUrl("Home/Index")
    .AssertMapTo(new {controller = "Home", action = "Index"});
routeTester.ForUrl("Home/Index")
    .AssertMapTo("Home", "Index");
routeTester.ForUrl("Home/Index")
    .AssertMapTo<HomeController>(x => x.Index);

// alternate syntaxes for asserting Order/Details/3 routing:
routeTester.ForUrl("Order/Details/3")
    .AssertMapTo(new Dictionary<string, object> 
        { { "controller", "Order" }, { "action", "Details" }, { "id", 3 } );
routeTester.ForUrl("Order/Details/3")
    .AssertMapTo(new {controller = "Order", action = "Details", id = 3 });
routeTester.ForUrl("Order/Details/3")
    .AssertMapTo("Order", "Details", 3);
routeTester.ForUrl("Order/Details/3")
    .AssertMapTo<OrderController>(x => () => x.Details(3));
```