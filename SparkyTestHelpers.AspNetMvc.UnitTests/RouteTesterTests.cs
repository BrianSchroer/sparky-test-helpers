using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Routing;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.AspNetMvc.UnitTests.Routing;
using SparkyTestHelpers.Exceptions;
using System.Net;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    /// <summary>
    /// <see cref="RouteTester"/> unit tests.
    /// </summary>
    [TestClass]
    public class RouteTesterTests
    {
        private RouteTester _routeTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _routeTester = new RouteTester(RouteConfig.RegisterRoutes);
        }

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
    }
}
