using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// ASP.NET Core MVC Controller tester.
    /// </summary>
    /// <typeparam name="TController">The controller type.</typeparam>
    public class ControllerTester<TController> where TController : Controller
    {
        private readonly TController _controller;

        /// <summary>
        /// Creates new <see cref="ControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public ControllerTester(TController controller)
        {
            _controller = controller;
        }

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> instance.
        /// </summary>
        /// <param name="actionResultProvider">Function that returns an <see cref="ActionResult"/>.</param>
        /// <returns>New <see cref="ControllerActionTester"/> instance.</returns>
        public ControllerActionTester Action(Func<ActionResult> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ControllerActionTester"/> instance.</returns>
        public ControllerActionTester Action(Expression<Func<TController, Func<ActionResult>>> expression)
        {
            var func = expression.Compile();

            return new ControllerActionTester(_controller, func(_controller));
        }

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ControllerActionTester"/> instance.</returns>
        public ControllerActionTester Action(Expression<Func<TController, Func<Task<ActionResult>>>> expression)
        {
            Func<TController, Func<Task<ActionResult>>> funcWithTask = expression.Compile();

            Func<ActionResult> func = () =>
            {
                Task<ActionResult> task = funcWithTask(_controller)();
                task.Wait();
                return task.Result;
            };

            return new ControllerActionTester(_controller, func);
        }

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester{TResponse}"/> instance.
        /// </summary>
        /// <typeparam name="TResponse">The action response type.</typeparam>
        /// <param name="actionResultProvider">Function that returns an istance of type <see cref="TResponse"/>.</param>
        /// <returns>New <see cref="ControllerActionTester{TController}"/> instance.</returns>
        public ControllerActionTester<TResponse> Action<TResponse>(Func<TResponse> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester{TResponse}"/> instance.
        /// </summary>
        /// <typeparam name="TResponse">The action response type.</typeparam>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ControllerActionTester{TResponse}"/> instance.</returns>
        public ControllerActionTester<TResponse> Action<TResponse>(Expression<Func<TController, Func<TResponse>>> expression)
        {
            var func = expression.Compile();

            return new ControllerActionTester<TResponse>(_controller, func(_controller));
        }

        /// <summary>
        /// Creates new <see cref="ControllerActionTester{TResponse}"/> instance for an async action.
        /// </summary>
        /// <typeparam name="TResponse">The action response type.</typeparam>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ControllerActionTester{TResponse}"/> instance.</returns>
        public ControllerActionTester<TResponse> Action<TResponse>(Expression<Func<TController, Func<Task<TResponse>>>> expression)
        {
            Func<TController, Func<Task<TResponse>>> funcWithTask = expression.Compile();

            Func<TResponse> func = () =>
            {
                Task<TResponse> task = funcWithTask(_controller)();
                task.Wait();
                return task.Result;
            };

            return new ControllerActionTester<TResponse>(_controller, func);
        }
    }
}
