using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc
{
    public static class ControllerExtensionMethods
    {
        /// <summary>
        /// Creates a new <see cref="ControllerTester{TController}" /> instance.
        /// </summary>
        /// <typeparam name="TController">The <see cref="Controller" /> type.</typeparam>
        /// <param name="controller">THe <see cref="Controller"/> to be tested.</param>
        /// <returns>New <see cref="ControllerTester{TController}" /> instance.</returns>
        public static ControllerTester<TController> CreateTester<TController>(this TController controller) where TController : Controller
        {
            return new ControllerTester<TController>(controller);
        }
    }
}
