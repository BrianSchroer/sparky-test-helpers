using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Routing;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.AspNetMvc.UnitTests.Routing;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System.Collections.Generic;
using System.Net;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    /// <summary>
    /// <see cref="RoutingAsserter"/> unit tests.
    /// </summary>
    [TestClass]
    public class RoutingAsserterTests
    {
        private RouteTester _routeTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _routeTester = new RouteTester(RouteConfig.RegisterRoutes);
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_Dictionary_should_not_throw_exception_when_values_match()
        {
            ForTest.Scenarios
            (
                new
                {
                    Url = "Home/Index",
                    Expected = new Dictionary<string, object> { { "controller", "Home" }, { "action", "Index" }, { "id", null } }
                },
                new
                {
                    Url = "Order/Details/3",
                    Expected = new Dictionary<string, object> { { "controller", "Order" }, { "action", "Details" }, { "id", 3 } }
                }
            )
            .TestEach(scenario =>
                AssertExceptionNotThrown.WhenExecuting(() => _routeTester.ForUrl(scenario.Url).AssertMapTo(scenario.Expected)));
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_Dictionary_should_throw_exception_when_values_do_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected: \naction: \"Details\", controller: \"Order\", id: \"3\". \nActual: \naction: \"Index\", controller: \"Home\", id: (null).")
                .WhenExecuting(() =>
                    _routeTester.ForUrl("Home/Index")
                    .AssertMapTo(new Dictionary<string, object> { { "controller", "Order" }, { "action", "Details" }, { "id", 3 } }));
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_controller_action_id_should_not_throw_exception_when_values_match()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _routeTester.ForUrl("Home/Index").AssertMapTo("Home", "Index"));
            AssertExceptionNotThrown.WhenExecuting(() => _routeTester.ForUrl("Order/Details/3").AssertMapTo("Order", "Details", 3));
        }

        [TestMethod]
        public void RoutingAssert_AssertMapTo_controller_action_id_should_throw_exception_when_values_do_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected: \naction: \"Details\", controller: \"Order\", id: \"3\". \nActual: \naction: \"Index\", controller: \"Home\", id: (null).")
                .WhenExecuting(() =>
                    _routeTester.ForUrl("Home/Index")
                    .AssertMapTo("Order", "Details", 3));
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_expression_should_not_throw_exception_when_values_match()
        {
            AssertExceptionNotThrown.WhenExecuting(() => 
                _routeTester.ForUrl("Home/Index").AssertMapTo<HomeController>(x => x.Index));

            AssertExceptionNotThrown.WhenExecuting(() =>
                _routeTester.ForUrl("Order/Details/3").AssertMapTo<OrderController>(x => () => x.Details(3)));
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_expression_should_throw_exception_when_values_do_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected: \naction: \"Details\", controller: \"Order\", id: \"3\". \nActual: \naction: \"Index\", controller: \"Home\", id: (null).")
                .WhenExecuting(() =>
                    _routeTester.ForUrl("Home/Index")
                    .AssertMapTo<OrderController>(x => () => x.Details(3)));
        }

        [TestMethod]
        public void RoutingAsserter_AssertRedirectTo_should_not_throw_exception_for_expected_redirect()
        {
            AssertExceptionNotThrown.WhenExecuting(() => 
                _routeTester.ForUrl("Default.aspx").AssertRedirectTo("Home/LegacyRedirect"));
        }

        [TestMethod]
        public void RoutingAsserter_AssertRedirectTo_should_throw_exception_when_RedirectLocation_does_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected RedirectLocation: <Home/WrongAction>. Actual: <Home/LegacyRedirect>.")
                .WhenExecuting(() => 
                    _routeTester.ForUrl("Default.aspx").AssertRedirectTo("Home/WrongAction"));
        }

        [TestMethod]
        public void RoutingAsserter_AssertRedirectTo_should_throw_exception_when_Status_does_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected Status: <301 MovedPermanently>. Actual: <302 Redirect>.")
                .WhenExecuting(() => 
                    _routeTester.ForUrl("Default.aspx").AssertRedirectTo("Home/LegacyRedirect", HttpStatusCode.Moved));
        }
    }
}
