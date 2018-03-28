using SparkyTestHelpers.AspNetMvc.Routing.Mocks;
using System;
using System.Linq.Expressions;
using System.Net;
using System.Web.Mvc;
using System.Web.Routing;

namespace SparkyTestHelpers.AspNetMvc.Routing
{
    /// <summary>
    /// Routing tester.
    /// </summary>
    public class RouteTester
    {
        private MockHttpContextBase _context;
        private MockHttpRequestBase _request;
        private MockHttpResponseBase _response;
        private readonly Action<RouteCollection> _registerRoutes;

        private RouteCollection _routeCollection;

        private RouteCollection RouteCollection
        {
            get
            {
                if (_routeCollection == null)
                {
                    _routeCollection = new RouteCollection();
                    _registerRoutes(_routeCollection);
                }

                return _routeCollection;
            }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RouteTester"/> class.
        /// </summary>
        /// <param name="routeRegistrationMethod">Action that registers routes.</param>
        /// <example>
        /// <code><![CDATA[
        ///     var routeTester = new RouteTester(RouteConfig.RegisterRoutes);
        /// ]]></code>
        /// </example>
        public RouteTester(Action<RouteCollection> routeRegistrationMethod)
        {
            _registerRoutes = routeRegistrationMethod;

            _request = new MockHttpRequestBase();
            _response = new MockHttpResponseBase();
            _context = new MockHttpContextBase(_request, _response);
        }

        /// <summary>
        /// Creates a new instance of the <see cref="RouteTester"/> class, using <see cref="AreaRegistration"/>.
        /// </summary>
        /// <param name="areaRegistration"><see cref="AreaRegistration"/> instance.</param>
        public RouteTester(AreaRegistration areaRegistration)
            : this(routeCollection => RegisterArea(areaRegistration, routeCollection))
        {
            _request.AreaName = areaRegistration.AreaName;
        }

        /// <summary>
        /// Get <see cref="RouteData"/> for relative URL.
        /// </summary>
        /// <param name="relativeUrl">The relative URL (e.g. "Home/Index").</param>
        /// <returns>The <see cref="RouteData"/></returns>
        public RouteData GetRouteData(string relativeUrl)
        {
            _request.RelativeUrl = relativeUrl;
            return RouteCollection.GetRouteData(_context);
        }

        /// <summary>
        /// Define controller/action expression.
        /// </summary>
        /// <typeparam name="TController">The <see cref="Controller"/> type.</typeparam>
        /// <param name="expression">The controller/action expression.</param>
        /// <returns>The controller/action expression.</returns>
        /// <example>
        /// <code><![CDATA[
        ///     var expectedAction = _routeTester.ControllerAction<HomeController>(x => x.Index);
        ///     _routeTester.ForUrl("Home/Index").AssertMapTo(expectedAction);
        /// ]]></code>
        /// </example>
        public Expression<Func<TController, Func<ActionResult>>> ControllerAction<TController>(
            Expression<Func<TController, Func<ActionResult>>> expression)
            where TController : Controller
        {
            return expression;
        }

        /// <summary>
        /// Creates a new <see cref="RoutingAsserter"/> instance, in preparation for using its "Assert" methods.
        /// </summary>
        /// <param name="relativeUrl">The relative URL to be tested.</param>
        /// <returns>New <see cref="RoutingAsserter"/> instance.</returns>
        /// <example>
        /// <code><![CDATA[
        ///     _routeTester.ForUrl("Home/Index").AssertMapTo<HomeController>(x => x.Index);
        ///     
        ///     _routeTester.ForUrl("Home/Index").AssertMapTo("Home", "Index");
        ///     
        ///     _routeTester.ForUrl("Order/Details/3").AssertMapTo(new Dictionary<string, object>
        ///     {
        ///         { "controller", "Order" },
        ///         { "action", "Details" },
        ///         { "id", 3 }
        ///     });
        /// ]]></code>
        /// </example>
        /// <seealso cref="RoutingAsserter.AssertMapTo(string, string, object)" />
        /// <seealso cref="RoutingAsserter.AssertMapTo(System.Collections.Generic.IDictionary{string, object})"/>.
        /// <seealso cref="RoutingAsserter.AssertMapTo{TController}(Expression{Func{TController, Func{ActionResult}}}) />
        /// <seealso cref="RoutingAsserter.AssertRedirectTo(string)" />
        public RoutingAsserter ForUrl(string relativeUrl)
        {
            return new RoutingAsserter(relativeUrl, this);
        }

        /// <summary>
        /// Assert that routing rules direct the specified <paramref name="relativeUrl"/> to the expected
        /// <paramref name="redirectUrl"/> and return the expected HTTP status code.
        /// </summary>
        /// <param name="relativeUrl">The relative URL.</param>
        /// <param name="redirectUrl">The expected redirect URL.</param>
        /// <param name="httpStatusCode">The expected HTTP status code (default is 302 - Redirect).</param>
        /// <returns>"This" <see cref="RouteTester"/>.</returns>
        /// <seealso cref="RoutingAsserter.AssertRedirectTo(string)"/>
        /// <exception cref="RouteTesterException">if the redirect does not return expected results.</exception>
        public RouteTester AssertRedirect(
            string relativeUrl, string redirectUrl, 
            HttpStatusCode httpStatusCode = HttpStatusCode.Redirect)
        {
            _response.ResetRedirectResults();
            GetRouteData(relativeUrl);
            _response.VerifyRedirectResults(redirectUrl, httpStatusCode);

            return this;
        }

        private static void RegisterArea(AreaRegistration areaRegistration, RouteCollection routeCollection)
        {
            var context = new AreaRegistrationContext(areaRegistration.AreaName, routeCollection);
            areaRegistration.RegisterArea(context);
        }
    }
}
