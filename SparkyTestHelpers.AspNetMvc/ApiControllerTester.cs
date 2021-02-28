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
        /// <summary>
        /// The <typeparamref name="TController"/> instance being tested.
        /// </summary>
        public TController Controller { get; private set; }

        /// <summary>
        /// Creates new <see cref="ApiControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="controller">The <typeparamref name="TController"/> to be tested.</param>
        public ApiControllerTester(TController controller)
        {
            Controller = controller;

            if (Controller.Request is null)
            {
                Controller.Request = new HttpRequestMessage();
                Controller.Request.SetConfiguration(new HttpConfiguration());
            }
        }

        /// <summary>
        /// Creates a new <see cref="ApiControllerHttpActionResultActionTester"/> instance.
        /// </summary>
        /// <param name="actionResultProvider">Function that returns an <see cref="IHttpActionResult"/>.</param>
        /// <returns>New <see cref="ApiControllerHttpActionResultActionTester"/> instance.</returns>
        public ApiControllerHttpActionResultActionTester Action(Func<IHttpActionResult> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ApiControllerHttpActionResultActionTester"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerHttpActionResultActionTester"/> instance.</returns>
        public ApiControllerHttpActionResultActionTester Action(Expression<Func<TController, Func<IHttpActionResult>>> expression)
        {
            var func = expression.Compile();

            return new ApiControllerHttpActionResultActionTester(Controller, func(Controller));
        }

        /// <summary>
        /// Creates new <see cref="ApiControllerHttpActionResultActionTester"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerHttpActionResultActionTester"/> instance.</returns>
        public ApiControllerHttpActionResultActionTester Action(Expression<Func<TController, Func<Task<IHttpActionResult>>>> expression)
        {
            Func<TController, Func<Task<IHttpActionResult>>> funcWithTask = expression.Compile();

            Func<IHttpActionResult> func = () =>
            {
                Task<IHttpActionResult> task = funcWithTask(Controller)();
                task.Wait();
                return task.Result;
            };

            return new ApiControllerHttpActionResultActionTester(Controller, func);
        }

        /// <summary>
        /// Creates a new <see cref="ApiControllerHttpResponseMessageActionTester"/> instance.
        /// </summary>
        /// <param name="actionResultProvider">Function that returns an <see cref="HttpResponseMessage"/>.</param>
        /// <returns>New <see cref="ApiControllerHttpResponseMessageActionTester"/> instance.</returns>
        public ApiControllerHttpResponseMessageActionTester Action(Func<HttpResponseMessage> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ApiControllerHttpResponseMessageActionTester"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerHttpResponseMessageActionTester"/> instance.</returns>
        public ApiControllerHttpResponseMessageActionTester Action(Expression<Func<TController, Func<HttpResponseMessage>>> expression)
        {
            var func = expression.Compile();

            return new ApiControllerHttpResponseMessageActionTester(Controller, func(Controller));
        }

        /// <summary>
        /// Creates new <see cref="ApiControllerHttpResponseMessageActionTester"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerHttpResponseMessageActionTester"/> instance.</returns>
        public ApiControllerHttpResponseMessageActionTester Action(Expression<Func<TController, Func<Task<HttpResponseMessage>>>> expression)
        {
            Func<TController, Func<Task<HttpResponseMessage>>> funcWithTask = expression.Compile();

            Func<HttpResponseMessage> func = () =>
            {
                Task<HttpResponseMessage> task = funcWithTask(Controller)();
                task.Wait();
                return task.Result;
            };

            return new ApiControllerHttpResponseMessageActionTester(Controller, func);
        }
        
        /// <summary>
        /// Creates a new <see cref="ApiControllerActionTester{TResponse}"/> instance.
        /// </summary>
        /// <typeparam name="TResponse">The action response type.</typeparam>
        /// <param name="actionResultProvider">Function that returns an istance of type <see cref="TResponse"/>.</param>
        /// <returns>New <see cref="ApiControllerActionTester{TController}"/> instance.</returns>
        public ApiControllerActionTester<TResponse> Action<TResponse>(Func<TResponse> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ApiControllerActionTester{TResponse}"/> instance.
        /// </summary>
        /// <typeparam name="TResponse">The action response type.</typeparam>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerActionTester{TResponse}"/> instance.</returns>
        public ApiControllerActionTester<TResponse> Action<TResponse>(Expression<Func<TController, Func<TResponse>>> expression)
        {
            var func = expression.Compile();

            return new ApiControllerActionTester<TResponse>(Controller, func(Controller));
        }

        /// <summary>
        /// Creates new <see cref="ApiControllerActionTester{TResponse}"/> instance for an async action.
        /// </summary>
        /// <typeparam name="TResponse">The action response type.</typeparam>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ApiControllerActionTester{TResponse}"/> instance.</returns>
        public ApiControllerActionTester<TResponse> Action<TResponse>(Expression<Func<TController, Func<Task<TResponse>>>> expression)
        {
            Func<TController, Func<Task<TResponse>>> funcWithTask = expression.Compile();

            Func<TResponse> func = () =>
            {
                Task<TResponse> task = funcWithTask(Controller)();
                task.Wait();
                return task.Result;
            };

            return new ApiControllerActionTester<TResponse>(Controller, func);
        }
    }
}