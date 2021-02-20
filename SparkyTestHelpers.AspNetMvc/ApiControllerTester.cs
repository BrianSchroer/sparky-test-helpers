using System;
using System.Linq.Expressions;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing <see cref="ApiController"/>s.
    /// </summary>
    /// <typeparam name="TController">The <see cref="ApiController"/> type.</typeparam>
    public class ApiControllerTester<TController> where TController : ApiController
    {
        private readonly TController _controller;

        /// <summary>
        /// Creates new <see cref="ApiControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="controller">The <typeparamref name="TController"/> to be tested.</param>
        public ApiControllerTester(TController controller)
        {
            _controller = controller;

            if (_controller.Request is null)
            {
                _controller.Request = new HttpRequestMessage();
            }
        }

        /// <summary>
        /// Creates a new <see cref="ApiControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="actionResultProvider">Function that returns an <see cref="IHttpActionResult"/>.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}"/> instance.</returns>
        public ApiControllerHttpActionResultActionTester Action(Func<IHttpActionResult> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ApiControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}"/> instance.</returns>
        public ApiControllerHttpActionResultActionTester Action(Expression<Func<TController, Func<IHttpActionResult>>> expression)
        {
            var func = expression.Compile();

            return new ApiControllerHttpActionResultActionTester(_controller, func(_controller));
        }

        /// <summary>
        /// Creates new <see cref="ApiControllerTester{TController}"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}"/> instance.</returns>
        public ApiControllerHttpActionResultActionTester Action(Expression<Func<TController, Func<Task<IHttpActionResult>>>> expression)
        {
            Func<TController, Func<Task<IHttpActionResult>>> funcWithTask = expression.Compile();

            Func<IHttpActionResult> func = () =>
            {
                Task<IHttpActionResult> task = funcWithTask(_controller)();
                task.Wait();
                return task.Result;
            };

            return new ApiControllerHttpActionResultActionTester(_controller, func);
        }

        /// <summary>
        /// Creates a new <see cref="ApiControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="actionResultProvider">Function that returns an <see cref="HttpResponseMessage"/>.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}"/> instance.</returns>
        public ApiControllerHttpResponseMessageActionTester Action(Func<HttpResponseMessage> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ApiControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}"/> instance.</returns>
        public ApiControllerHttpResponseMessageActionTester Action(Expression<Func<TController, Func<HttpResponseMessage>>> expression)
        {
            var func = expression.Compile();

            return new ApiControllerHttpResponseMessageActionTester(_controller, func(_controller));
        }

        /// <summary>
        /// Creates new <see cref="ApiControllerTester{TController}"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerTester{TController}"/> instance.</returns>
        public ApiControllerHttpResponseMessageActionTester Action(Expression<Func<TController, Func<Task<HttpResponseMessage>>>> expression)
        {
            Func<TController, Func<Task<HttpResponseMessage>>> funcWithTask = expression.Compile();

            Func<HttpResponseMessage> func = () =>
            {
                Task<HttpResponseMessage> task = funcWithTask(_controller)();
                task.Wait();
                return task.Result;
            };

            return new ApiControllerHttpResponseMessageActionTester(_controller, func);
        }
    }
}