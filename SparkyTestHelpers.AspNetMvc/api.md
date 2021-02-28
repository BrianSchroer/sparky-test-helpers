_see also_:
* **[SparkyTestHelpers.AspNetMvc.Core](https://www.nuget.org/packages/SparkyTestHelpers.AspNetMvc.Core)** - the .NET Core version of this package
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---

## Controller Testers
**ControllerTester**<*T*> and **ApiControllerTester**<*T*> test action methods of controllers that inherit from System.Web.Mvc.**Controller** and System.Web.Http.**ApiController** respectively.

The general syntax is:

*tester*<br/>
&nbsp;&nbsp;&nbsp;&nbsp;.**Action**(*action selection expression*)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;.**When**(*optional code to "arrange" test conditions*)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;.**Expecting**...(*optional code to set up assert expectations*)<br/>
&nbsp;&nbsp;&nbsp;&nbsp;.**Test**...()

### Examples

```csharp
//ControllerTester:

    moviesControllerTester
        .Action(x => x.Index)
        .TestView();

    moviesControllerTester
        .Action(x => () => x.Details(3))
        .ExpectingViewName("Details")
        .ExpectingModel<Movle>(movie => Assert.AreEqual("Office Space", movie.Title))
        .TestView();

    moviesControllerTester
        .Action(x => x.Edit(testInvalidModel))
        .WhenModelStateIsValidEquals(false)
        .TestRedirectToAction("Errors");

    moviesControllerTester
        .Action(x => () => x.Edit(testValidModel))
        .WhenModelStateIsValidEquals(true)
        .ExpectingViewName("UpdateSuccessful")
        .TestRedirectToRoute("Home/UpdateSuccessful");

//ApiControllerTester:

   moviesApiControllerTester 
        .Action<IEnumerable<Movie>>(x => x.GetAllMovies)
        .Test(movies => Assert.AreEqual(100, movies.Count());

   moviesApiControllerTester 
        .Action(x => () => x.Get)
        .WhenRequestHasQueryStringParameters(new QueryStringParameter("id", 3))
        .TestOkNegotiatedContentResult<Movie>(movie => Assert.AreEqual("Office Space", movie.Title));

    moviesApiControllerTester
        .Action(x => () => x.Update(updateModel))
        .WhenModelStateIsValidEquals(false)
        .TestBadRequestResult();

    moviesApiControllerTester
        .Action(x => () => x.Update(updateModel))
        .WhenModelStateIsValidEquals(true)
        .TestOkResult();
```

### Instantiation
**ControllerTester**<*T*> and **ApiControllerTester**<*T*> can be created either via their constructors:
```csharp
var homeControllerTester = new ControllerTester<HomeController>(testHomeController);
var moviesApiControllerTester = new ApiControllerTester<MoviesApiController>(testMoviesController);
```
...or via **CreateTester** extension methods:
```csharp
var homeControllerTester = testHomeController.CreateTester();
var moviesApiControllerTester = testMoviesController.CreateTester();
```
### "Action" Methods

*tester*.**Action** methods use lambda expressions to specify the controller action to be tested. The expressions enable Intellisense completion when you "dot on" to the controller argument.

The syntax for parameterless actions is "*.Action(controller => controller*.**actionName**)", e.g. ```.Action(x => x.Index)```.

For actions with parameters, the syntax is "*.Action(controller => () => controller*.**actionName**(*arguments*))", e.g. ```.Action(x => () => x.Get(3))```.

### "When" methods
* **WhenRequestHasQueryStringParameters**(*string siteUrlPrefix, NameValueCollection queryStringParameters*)
* **WhenRequestHasQueryStringParameters**(*NameValueCollection queryStringParameters*)
* **WhenRequestHasQueryStringParameters**(*string siteUrlPrefix, params QueryStringParameter[] queryStringParameters*)
* **WhenRequestHasQueryStringParameters**(*params QueryStringParameter[] queryStringParameters*)

The **WhenRequestHasQueryStringParameters** methods set up the controller's **Request** property with a **RequestUri** containing the specified parameters. If the **siteUrlPrefix** isn't specified (it usually won't matter in unit tests), the value "http://localhost" is used.
 
* **WhenModelStateIsValidEquals**(*bool isValid*)

This method sets up the controller's **ModelState** for testing.

* **When**(*Action action*)

This method can be used to "arrange" any conditions necessary for the test, e.g. setting up a mocked interface method.

### "Expecting" methods (**ControllerTester** only):
* **ExpectingViewName**(string expectedViewName)

**ExpectingViewName** sets up automatic validation when followed by **TestView** or **TestPartialView**

* **ExpectingModel<*TModel*>**(*Action<*TModel*> validate*) 

**ExpectingModel** sets up automatic model type and possibly content (the **validate** callback action is optional) validation when followed by **TestView** or **TestJson**.

### "Test" methods - ControllerTester:

**ControllerTester**<*TController*> has several .Test... methods used to assert that the controller action returns the expected ActionResult implementation (The method name suffixes correspond to ...Result types (e.g. **TestView** tests **ViewResult**). There are methods for all of the standard result types, plus the generic TestResult<*TActionResultType*> method.

The *validate* "callback" actions, which can be used to validate the result contents, are optional:

* **TestContent**(*Action<*ContentResult*> validate*)
* **TestEmpty**(*Action<*EmptyResult*> validate*)
* **TestFile**(*Action<*FileResult*> validate*)
* **TestJson**(*Action<*JsonResult*> validate*)
* **TestPartialView**(*Action<*PartialViewResult*> validate*)
* **TestRedirect**(*string expectedUrl, Action<*RedirectResult*> validate*)
* **TestRedirectToAction**(*string expectedActionName, Action<*RedirectToRouteResult*> validate*)
* **TestRedirectToRoute**(*string expectedRoute, Action<*RedirectToRouteResult*> validate*)
* **TestView**(*Action<*ViewResult*> validate*)
* **TestResult<*TActionResultType*>**(*Action<*TActionResultType*> validate*)

#### "Test" methods - ApiControllerTester - for actions that return an IHttpActionResult:

There several **.Test**... methods used to assert that API controller actions return the expected **IHttpActionResult** implementation. There are methods for all of the standard result types, plus the generic **TestResult<*THttpActionResultType*>** method:

The *validate* "callback" actions, which can be used to validate the result contents, are optional:

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

#### "Test" methods - ApiControllerTester - for actions that return an **HttpResponseMessage**:

The *validate* "callback" actions, which can be used to validate the result contents, are optional:

* **Test**(*Action<*HttpResponseMessage*> validate*)

...calls controller action, validates *HttpResponseMessage.StatusCode* (if **ExpectingHttpStatusCode(*HttpStatusCode*)** has been called) and returns the **HttpResponseMessage** returned from the action.

* **TestContentString**(*Action<*string*> validate*)

...calls controller action, validates *HttpResponseMessage.StatusCode* (if **ExpectingHttpStatusCode(*HttpStatusCode*)** has been called) and returns the **HttpResponseMessage.Content** string.

* **TestContentJsonDeserialization<*TContent*>**(*Action<*TContent*> validate*)

...calls controller action, validates *HttpResponseMessage.StatusCode* (if **ExpectingHttpStatusCode(*HttpStatusCode*)** has been called) and returns the **HttpResponseMessage.Content**'s JSON string deserialized to a *TContent* instance.


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

* **AssertMapTo**(IDictionary<string, object> *expectedValues*)
* **AssertMapTo**(object *routeValues*)
* **AssertMapTo**(string *controller*, string *action*, (object *id*)) - id defaults to null
* **AssertMapTo<*TController*>**(Expression<Func<TController, Func<*ActionResult*>>> *actionExpression*)
* **AssertRedirectTo**(string **expectedUrl**, (HttpStatusCode *expectedStatusCode)) - expectedStatusCode defaults to HttpStatusCode.Redirect (302)

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