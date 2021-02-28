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
---

Complete API documentation:

[https://github.com/BrianSchroer/sparky-test-helpers/blob/master/SparkyTestHelpers.AspNetMvc/api.md](https://github.com/BrianSchroer/sparky-test-helpers/blob/master/SparkyTestHelpers.AspNetMvc/api.md)