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
        /// <summary>
        /// The <typeparamref name="TController"/> instance being tested.
        /// </summary>
        public TController Controller { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ControllerTester{T}" /> instance.
        /// </summary>
        /// <typeparam name="T">The <see cref="System.Web.Mvc.Controller" /> type.</typeparam>
        /// <param name="controller">THe <see cref="System.Web.Mvc.Controller"/> to be tested.</param>
        /// <returns>New <see cref="Controller{T}" /> instance.</returns>
        public static ControllerTester<T> CreateTester<T>(T controller) where T : Controller
        {
            return new ControllerTester<T>(controller);
        }

        /// <summary>
        /// Creates new <see cref="ControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public ControllerTester(TController controller)
        {
            Controller = controller;
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

            return new ControllerActionTester(Controller, func(Controller));
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
                Task<ActionResult> task = funcWithTask(Controller)();
                task.Wait();
                return task.Result;
            };

            return new ControllerActionTester(Controller, func);
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

            return new ControllerActionTester<TResponse>(Controller, func(Controller));
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
                Task<TResponse> task = funcWithTask(Controller)();
                task.Wait();
                return task.Result;
            };

            return new ControllerActionTester<TResponse>(Controller, func);
        }
    }
}
