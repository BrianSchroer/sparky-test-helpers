using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SparkyTestHelpers.AspNetCore.Controllers
{
    /// <summary>
    /// Helper for testing ASP.NET Core MVC controller actions.
    /// </summary>
    public class ControllerActionTester
    {
        private readonly Func<IActionResult> _controllerAction;
        private IModelTester _modelTester;
        private string _expectedViewName = null;

        /// <summary>
        /// Private constructor - Only called by <see cref="ControllerActionTester.ForAction(Func{IActionResult})"/>.
        /// </summary>
        /// <param name="controllerAction"></param>
        private ControllerActionTester(Func<IActionResult> controllerAction)
        {
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// Creates a new <see cref="ControllerActionTester"/> for the specified controller action.
        /// </summary>
        /// <param name="controllerAction">The controller action.</param>
        /// <returns></returns>
        public static ControllerActionTester ForAction(Func<IActionResult> controllerAction)
        {
            return new ControllerActionTester(controllerAction);
        }

        /// <summary>
        /// Specifies that the <see cref="TestViewResult(Action{ViewResult})"/> method should throw an exception
        /// if the action result's .ViewName doesn't match the <paramref name="expectedViewName"/>.
        /// </summary>
        /// <param name="expectedViewName">The expected view name.</param>
        /// <returns>"This" <see cref="ControllerActionTester"/>.</returns>
        public ControllerActionTester ExpectingViewName(string expectedViewName)
        {
            _expectedViewName = expectedViewName;
            return this;
        }

        /// <summary>
        /// Specifies that the <see cref="TestViewResult(Action{ViewResult})"/> method should throw an exception
        /// if the action result's .Model type doesn't match <typeparamref name="TModelType"/>.
        /// </summary>
        /// <typeparam name="TModelType">The expected model type.</typeparam>
        /// <param name="validate">(Optional) callback action to preform additional model validation.</param>
        /// <returns>"This" <see cref="ControllerActionTester"/>.</returns>
        public ControllerActionTester ExpectingModel<TModelType>(Action<TModelType> validate = null)
        {
            _modelTester = new ModelTester<TModelType>(validate);
            return this;
        }

        /// <summary>
        /// Test the result of the controller action.
        /// </summary>
        /// <typeparam name="TActionResultType">The expected <see cref="IActionResult"/> 
        /// type that should be returned from the action.</typeparam>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <typeparamref name="TActionResultType"/> returned from the controller action.</returns>
        /// <exception cref="ControllerActionTestException">if any errors are asserted.</exception>
        public TActionResultType TestResult<TActionResultType>(Action<TActionResultType> validate = null)
            where TActionResultType : IActionResult
        {
            TActionResultType actionResult = AssertActionResultType<TActionResultType>(_controllerAction());

            validate?.Invoke(actionResult);

            return actionResult;
        }

        /// <summary>
        /// Tests that controller action redirects to the specified action name.
        /// </summary>
        /// <param name="expectedActionName">The expected action name.</param>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <see cref="RedirectToActionResult"/> returned from the controller action.</returns>
        public RedirectToActionResult TestRedirectToAction(
            string expectedActionName, Action<RedirectToActionResult> validate = null)
        {
            return TestResult<RedirectToActionResult>(result =>
            {
                string actual = result.ActionName;

                if (!string.Equals(expectedActionName, actual, StringComparison.InvariantCulture))
                {
                    throw new ControllerActionTestException($"Expected ActionName <{expectedActionName}>. Actual: <{actual}>.");
                }

                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action redirects to the specified route.
        /// </summary>
        /// <param name="expectedRoute">The expected route.</param>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="RedirectToRouteResult"/> returned from the controller action.</returns>
        public RedirectToRouteResult TestRedirectToRoute(
            string expectedRoute, Action<RedirectToRouteResult> validate = null)
        {
            return TestResult<RedirectToRouteResult>(result =>
            {
                string actual = string.Join("/", result.RouteValues.Values.Select(v => v.ToString()));

                if (!string.Equals(expectedRoute, actual, StringComparison.InvariantCulture))
                {
                    throw new ControllerActionTestException($"Expected route <{expectedRoute}>. Actual: <{actual}>.");
                }

                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="ViewResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>THe <see cref="ViewResult"/> returned from the controller action.</returns>
        public ViewResult TestViewResult(Action<ViewResult> validate = null)
        {
            return TestResult<ViewResult>(result =>
            {
                AssertViewName(result, _expectedViewName);
                AssertModelType(result);
                validate?.Invoke(result);
            });
        }

        private TActionResultType AssertActionResultType<TActionResultType>(IActionResult actionResult)
            where TActionResultType : IActionResult
        {
            Type expectedType = typeof(TActionResultType);

            if (actionResult == null || !TypeTester.IsOfType(actionResult, expectedType))
            {
                string actualTypeName = (actionResult == null) ? "null" : actionResult.GetType().FullName;
                throw new ControllerActionTestException(
                    $"Expected IActionResult type: {expectedType.FullName}. Actual: {actualTypeName}.");
            }

            return (TActionResultType)actionResult;
        }

        private void AssertModelType(IActionResult actionResult)
        {
            if (_modelTester != null)
            {
                _modelTester.Test(((ViewResult)actionResult)?.Model);
            }
        }

        private void AssertViewName(IActionResult actionResult, string expectedViewName)
        {
            string actual = ((ViewResult)actionResult).ViewName;

            if (!string.Equals(expectedViewName, actual, StringComparison.InvariantCulture))
            {
                throw new ControllerActionTestException($"Expected ViewName <{expectedViewName}>. Actual: <{actual}>.");
            }
        }
    }
}
