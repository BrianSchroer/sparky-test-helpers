using System.Web.Http;

namespace SparkyTestHelpers.AspNetMvc
{
    public static class ApiControllerExtensionMethods
    {
        /// <summary>
        /// Creates a new <see cref="ApiControllerTester{TController}" /> instance.
        /// </summary>
        /// <typeparam name="TController">The <see cref="ApiController" /> type.</typeparam>
        /// <param name="controller">THe <see cref="ApiController"/> to be tested.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}" /> instance.</returns>
        public static ApiControllerTester<TController> CreateTester<TController>(this TController controller) where TController : ApiController
        {
            return new ApiControllerTester<TController>(controller);
        }
    }
}
