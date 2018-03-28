using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Routing;
using SparkyTestHelpers.AspNetMvc.UnitTests.Routing;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    /// <summary>
    /// <see cref="RouteTester"/> / <see cref="RoutingAsserter"/> unit tests, using <see cref="AreaRegistration"/> constructor.
    /// </summary>
    [TestClass]
    public class RouteTesterAreaTests : RouteTesterTestsBase
    {
        [TestInitialize]
        public void TestInitialize()
        {
            _routeTester = new RouteTester(new TestAreaRegistration());
        }

        private class TestAreaRegistration : AreaRegistration
        {
            public override string AreaName => "TestArea";

            public override void RegisterArea(AreaRegistrationContext context)
            {
                context.Routes.Add("Legacy", new LegacyUrlRoute());

                context.MapRoute(
                    "TestArea_default",
                    "TestArea/{controller}/{action}/{id}",
                    new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
            }
        }
    }
}
