using SparkyTestHelpers.AspNetMvc.Common;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing ASP.NET Core MVC controller actions.
    /// </summary>
    public class ControllerActionTester
    {
        private readonly Controller _controller;
        private readonly Func<ActionResult> _controllerAction;
        private IModelTester _modelTester;
        private string _expectedViewName = null;

        /// <summary>
        /// Called by the 
        /// <see cref="ControllerTester{TController}.Action(System.Linq.Expressions.Expression{Func{TController, Func{ActionResult}}})"/>
        /// method.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="Controller"/>.</param>
        /// <param name="controllerAction">"Callback" function that privides the controller action to be tested.</param>
        internal ControllerActionTester(Controller controller, Func<ActionResult> controllerAction)
        {
            _controller = controller;
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ControllerActionTester"/>.</returns>
        public ControllerActionTester WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
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
        /// <typeparam name="TActionResultType">The expected <see cref="ActionResult"/> 
        /// type that should be returned from the action.</typeparam>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <typeparamref name="TActionResultType"/> returned from the controller action.</returns>
        /// <exception cref="ControllerTestException">if any errors are asserted.</exception>
        public TActionResultType TestResult<TActionResultType>(Action<TActionResultType> validate = null)
            where TActionResultType : ActionResult
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
        /// <returns>The <see cref="RedirectResult"/> returned from the controller action.</returns>
        public RedirectToRouteResult TestRedirectToAction(
            string expectedActionName, Action<RedirectToRouteResult> validate = null)
        {
            return TestResult<RedirectToRouteResult>(result =>
            {
                if (!result.RouteValues.ContainsKey("action"))
                {
                    throw new ControllerTestException("RouteValues dictionary does not contain \"action\" value.");
                }

                var actual = result.RouteValues["action"].ToString();

                if (!string.Equals(expectedActionName, actual, StringComparison.InvariantCulture))
                {
                    throw new ControllerTestException($"Expected action name <{expectedActionName}>. Actual: <{actual}>.");
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
                    throw new ControllerTestException($"Expected route <{expectedRoute}>. Actual: <{actual}>.");
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
                _modelTester?.Test(result.Model);
                validate?.Invoke(result);
            });
        }

        private TActionResultType AssertActionResultType<TActionResultType>(ActionResult actionResult)
            where TActionResultType : ActionResult
        {
            Type expectedType = typeof(TActionResultType);

            if (actionResult == null || !TypeTester.IsOfType(actionResult, expectedType))
            {
                string actualTypeName = (actionResult == null) ? "null" : actionResult.GetType().FullName;
                throw new ControllerTestException(
                    $"Expected ActionResult type {expectedType.FullName}. Actual: {actualTypeName}.");
            }

            return (TActionResultType)actionResult;
        }

        private void AssertViewName(ActionResult actionResult, string expectedViewName)
        {
            string expected = expectedViewName ?? string.Empty;
            string actual = ((ViewResult)actionResult).ViewName ?? string.Empty;

            if (!string.Equals(expected, actual, StringComparison.InvariantCulture))
            {
                throw new ControllerTestException($"Expected ViewName <{expectedViewName}>. Actual: <{actual}>.");
            }
        }

        private void SetModelStateIsValid(bool isValid = true)
        {
            _controller.ModelState.Clear();

            if (!isValid)
            {
                _controller.ModelState.AddModelError("key", "message");
            }
        }
    }
}
