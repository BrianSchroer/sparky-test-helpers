using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// ASP.NET Core MVC Controller tester.
    /// </summary>
    /// <typeparam name="TController">The controller type.</typeparam>
    public class ControllerTester<TController> where TController : ControllerBase
    {
        /// <summary>
        /// Creates new <see cref="ControllerTester{TController}"/> instance.
        /// </summary>
        /// <param name="controller">The controller.</param>
        public ControllerTester(TController controller)
        {
            Controller = controller;
        }

        /// <summary>
        /// The <typeparamref name="TController"/> instance being tested.
        /// </summary>
        public TController Controller { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> instance.
        /// </summary>
        /// <param name="actionResultProvider">Function that returns an <see cref="IActionResult"/>.</param>
        /// <returns>New <see cref="ControllerActionTester"/> instance.</returns>
        public ControllerActionTester Action(Func<IActionResult> actionResultProvider) => Action(_ => actionResultProvider);

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> instance.
        /// </summary>
        /// <param name="expression">Expression to specify the controller action to be tested.</param>
        /// <returns>New <see cref="ControllerActionTester"/> instance.</returns>
        public ControllerActionTester Action(Expression<Func<TController, Func<IActionResult>>> expression)
        {
            var func = expression.Compile();

            return new ControllerActionTester(Controller, func(Controller));
        }

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> instance for an async action.
        /// </summary>
        /// <param name="expression">Expression to specify the async controller action to be tested.</param>
        /// <returns>New <see cref="ControllerActionTester"/> instance.</returns>
        public ControllerActionTester Action(Expression<Func<TController, Func<Task<IActionResult>>>> expression)
        {
            Func<TController, Func<Task<IActionResult>>> funcWithTask = expression.Compile();

            Func<IActionResult> func = () =>
            {
                Task<IActionResult> task = funcWithTask(Controller)();
                task.Wait();
                return task.Result;
            };

            return new ControllerActionTester(Controller, func);
        }
    }
}
