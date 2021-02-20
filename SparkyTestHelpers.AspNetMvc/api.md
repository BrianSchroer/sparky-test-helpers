_see also_:
* **[SparkyTestHelpers.AspNetMvc.Core](https://www.nuget.org/packages/SparkyTestHelpers.AspNetMvc.Core)** - the .NET Core version of this package
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
## ControllerTester<*TController*>
...tests action methods of controllers that inherit from System.Web.Mvc.**Controller**:

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
The *.Action* method argument is an expression for either a synchronous or async controller action method.

Use the syntax `.Action(x => x.ActionName)` for parameterless methods, or `.Action(x => () x.ActionName(param))` for methods with parameters.  

ControllerActionTester has several **.Test**... methods used to assert that the controller action returns the expected **ActionResult** implementation. There are methods for all of the standard result types, plus the generic **TestResult<*TActionResultType*>** method:

* **TestContent**(*Action<*ContentResult*> validate*)
* **TestEmpty**(*Action<*EmptyResult*> validate*)
* **TestFile**(*Action<*FileResult*> validate*)
* **TestJson**(*Action<*JsonResult*> validate*)
* **TestPartialView**(*Action<*PartialViewResult*> validate*)
* **TestRedirect**(*string expectedUrl, Action<*RedirectResult*> validate*)
* **TestRedirectToAction**(*string expecteActionName, Action<*RedirectToRouteResult*> validate*)
* **TestRedirectToRoute**(*string expectedRoute, Action<*RedirectToRouteResult*> validate*)
* **TestView**(*Action<*ViewResult*> validate*)
* **TestResult<*TActionResultType*>**(*Action<*TActionResultType*> validate*)

**Additional methods:**
* **ExpectingViewName**(*string expectedViewName*) - used with **.TestView** and **.TestPartialView**
* **ExpectingModel<*TModelType*>**(*Action<*TModelType*> validate*) - using with **.TestView** and **.TestJson**
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
## ApiControllerTester<*TController*>
...tests action methods of controllers that inherit from System.Web.Http.**ApiController**:

Instantiation:
```csharp
using SparkyTestHelpers.AspNetMvc;
```

```csharp
    var thingController = new ThingController(/* with test dependencies */);
    var controllerTester = new ApiControllerTester<ThingController>(thingController);
```

It doesn't do anything on its own - just provides **Action**(*actionDefinitionExpression*) methods used to create action testers:

Use the syntax `.Action(x => x.ActionName)` for parameterless methods, or `.Action(x => () x.ActionName(param))` for methods with parameters.  
 
**Methods for testing actions that return an **IHttpActionResult**:**

There several **.Test**... methods used to assert that the API controller action returns the expected **IHttpActionResult** implementation. There are methods for all of the standard result types, plus the generic **TestResult<*THttpActionResultType*>** method:

* **TestBadRequestErrorMessageResult**(*Action<*BadRequestErrorMessageResult*> validate*)
* **TestBadRequestResult**(*Action<*BadRequestResult*> validate*)
* **TestConflictResult**(*Action<*ConflictResult*> validate*)
* **TestCreatedAtRouteNegotiatedContentResult<*T*>**(*Action<*CreatedAtRouteNegotiatedContentResult<*T*>*> validate*)
* **TestCreatedNegotiatedContentResult<*T*>**(*Action<*CreatedNegotiatedContentResult<*T*>*> validate*)
* **TestExceptionResult**(*Action<*ExceptionResult*> validate*)
* **TestFormattedContentResult<*T*>**(*Action<*FormattedContentResult<*T*>*> validate*)
* **TestInternalServerErrorResult**(*Action<*InternalServerErrorResult*> validate*)
* **TestInvalidModelStateResult**(*Action<*InvalidModelStateResult*> validate*)
* **TestJsonResult<*T*>**(*Action<*JsonResult<*T*>*> validate*)
* **TestNegotiatedContentResult<*T*>**(*Action<*NegotiatedContentResult<*T*>*> validate*)
* **TestNotFoundResult**(*Action<*NotFoundResult*> validate*)
* **TestOkNegotiatedContentResult<*T*>**(*Action<*OkNegotiatedContentResult<*T*>*> validate*)
* **TestOkResult**(*Action<*OkResult*> validate*)
* **TestRedirectResult**(*Action<*RedirectResult*> validate*)
* **TestRedirectToRouteResult**(*Action<*RedirectToRouteResult*> validate*)
* **TestResponseMessageResult**(*Action<*ResponseMessageResult*> validate*)
* **TestStatusCodeResult**(*Action<*StatusCodeResult*> validate*)
* **TestUnauthorizedResult**(*Action<*UnauthorizedResult*> validate*)
* **TestResult<*THttpActionResultType*>**(*Action<*THttpActionResultType*> validate*)

All *validate* "callback" actions are optional.

**Methods for testing actions that return an **HttpResponseMessage**:**

* **Test**(*Action<*HttpResponseMessage*> validate*) - Calls controller action, validates *HttpResponseMessage.StatusCode* (if **ExpectingHttpStatusCode(*HttpStatusCode*)** has been called) and returns the **HttpResponseMessage** returned from the action.

* **TestContentString**(*Action<*string*> validate*) - Calls controller action, validates *HttpResponseMessage.StatusCode* (if **ExpectingHttpStatusCode(*HttpStatusCode*)** has been called) and returns the **HttpResponseMessage.Content** string.

* **TestContentJsonDeserialization<*TContent*>**(*Action<*TContent*> validate*)  - Calls controller action, validates *HttpResponseMessage.StatusCode* (if **ExpectingHttpStatusCode(*HttpStatusCode*)** has been called) and returns the **HttpResponseMessage.Content**'s JSON string deserialized to a *TContent* instance.

**Additional methods:**
* **ExpectingHttpStatusCode**(*HttpStatusCode expectedStatusCode*) - used with HttpResponseMessage action **.Test** method
* **WhenModelStateIsValidEquals**(*bool isValid*) - used to test conditional logic based on ModelState.IsValid


## RouteTester
**RouteTester** and **RoutingAsserter** provide methods to assert that a given relative URL maps to the expected [RouteData.Values](https://docs.microsoft.com/en-us/dotnet/api/system.web.routing.routedata.values?view=netframework-4.7#System_Web_Routing_RouteData_Values). The **RoutingAsserter.AssertMapTo** overloads provide multiple ways to specify the expected values...

**Constructors**

* public **RouteTester**(Action<*RouteCollection*> *routeRegistrationMethod*)
* public **RouteTester**(AreaRegistration *areaRegistration*)

```csharp
using SparkyTestHelpers.AspNetMvc.Routing;
. . .
    var routeTester = new RouteTester(RouteConfig.RegisterRoutes);
    var areaRouteTester = new RouteTester(new FooAreaRegistration());
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
routeTester.ForUrl("Default.aspx")
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