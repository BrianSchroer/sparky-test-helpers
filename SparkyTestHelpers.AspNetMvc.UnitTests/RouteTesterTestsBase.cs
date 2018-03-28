using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Routing;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System.Collections.Generic;
using System.Net;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    /// <summary>
    /// <see cref="RouteTester"/> unit tests base class.
    /// </summary>
    public class RouteTesterTestsBase
    {
        protected RouteTester _routeTester;

        [TestMethod]
        public void RouteTester_AssertRedirect_should_not_throw_exception_for_expected_redirect()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _routeTester.AssertRedirect("Default.aspx", "Home/LegacyRedirect"));
        }

        [TestMethod]
        public void RouteTester_AssertRedirect_should_throw_exception_when_RedirectLocation_does_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected RedirectLocation: <Home/WrongAction>. Actual: <Home/LegacyRedirect>.")
                .WhenExecuting(() => _routeTester.AssertRedirect("Default.aspx", "Home/WrongAction"));
        }

        [TestMethod]
        public void RouteTester_AssertRedirect_should_throw_exception_when_Status_does_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected Status: <301 MovedPermanently>. Actual: <302 Redirect>.")
                .WhenExecuting(() => _routeTester.AssertRedirect("Default.aspx", "Home/LegacyRedirect", HttpStatusCode.Moved));
        }

        [TestMethod]
        public void RouteTester_ControllerAction_should_return_expression()
        {
            var expression = _routeTester.ControllerAction<HomeController>(x => x.Index);
            _routeTester.ForUrl("Home/Index").AssertMapTo(expression);
        }

        [TestMethod]
        public void RouteTester_ForUrl_should_return_RoutingAsserter()
        {
            RoutingAsserter asserter = _routeTester.ForUrl("Home/Index");
            Assert.IsNotNull(asserter);
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
        public void RoutingAsserter_AssertMapTo_controller_action_id_should_throw_exception_when_values_do_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected: \naction: \"Details\", controller: \"Order\", id: \"3\". \nActual: \naction: \"Index\", controller: \"Home\", id: (null).")
                .WhenExecuting(() =>
                    _routeTester.ForUrl("Home/Index")
                    .AssertMapTo("Order", "Details", 3));
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_dynamic_object_should_not_throw_exception_when_values_match()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _routeTester.ForUrl("Home/INdex").AssertMapTo(new { controller = "Home", action = "Index" }));

            AssertExceptionNotThrown.WhenExecuting(() =>
                _routeTester.ForUrl("Order/Details/3").AssertMapTo(new { controller = "Order", action = "Details", id = 3 }));
        }

        [TestMethod]
        public void RoutingAsserter_AssertMapTo_dynamic_object_should_throw_exception_when_values_do_not_match()
        {
            AssertExceptionThrown
                .OfType<RouteTesterException>()
                .WithMessage("Expected: \naction: \"Details\", controller: \"Order\", id: \"3\". \nActual: \naction: \"Index\", controller: \"Home\", id: (null).")
                .WhenExecuting(() =>
                    _routeTester.ForUrl("Home/Index")
                    .AssertMapTo(new { controller = "Order", action = "Details", id = 3 }));
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
