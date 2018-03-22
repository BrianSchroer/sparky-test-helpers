using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// Helper for testing ASP.NET Core MVC controller actions.
    /// </summary>
    public class ControllerActionTester
    {
        private readonly Controller _controller;
        private readonly Func<IActionResult> _controllerAction;
        private IModelTester _modelTester;
        private string _expectedViewName = null;
        private bool _expectedViewNameSpecified;

        /// <summary>
        /// Called by the 
        /// <see cref="ControllerTester{TController}.Action(System.Linq.Expressions.Expression{Func{TController, Func{IActionResult}}})"/>
        /// method.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="Controller"/>.</param>
        /// <param name="controllerAction">"Callback" function that privides the controller action to be tested.</param>
        internal ControllerActionTester(Controller controller, Func<IActionResult> controllerAction)
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
        /// Specifies that the
        /// <see cref="TestView(Action{ViewResult})"/> and
        /// <see cref="TestPartialView(Action{PartialViewResult})"/> methods should throw an exception
        /// if the action result's .ViewName doesn't match the <paramref name="expectedViewName"/>.
        /// </summary>
        /// <param name="expectedViewName">The expected view name.</param>
        /// <returns>"This" <see cref="ControllerActionTester"/>.</returns>
        public ControllerActionTester ExpectingViewName(string expectedViewName)
        {
            _expectedViewName = expectedViewName;
            _expectedViewNameSpecified = true;
            return this;
        }

        /// <summary>
        /// Specifies that the
        /// <see cref="TestView(Action{ViewResult})"/> or
        /// <see cref="TestJson(Action{JsonResult})"/> method should throw an exception
        /// if the action result's .Model or .Value type doesn't match <typeparamref name="TModelType"/>.
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
        /// Tests that controller action returns a <see cref="ContentResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ContentResult"/> returned from the controller action.</returns>
        public ContentResult TestContent(Action<ContentResult> validate = null)
        {
            return TestResult<ContentResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="EmptyResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="EmptyResult"/> returned from the controller action.</returns>
        public EmptyResult TestEmpty(Action<EmptyResult> validate = null)
        {
            return TestResult<EmptyResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="FileResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="FileResult"/> returned from the controller action.</returns>
        public FileResult TestFile(Action<FileResult> validate = null)
        {
            return TestResult<FileResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="JsonResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="JsonResult"/> returned from the controller action.</returns>
        public JsonResult TestJson(Action<JsonResult> validate = null)
        {
            return TestResult<JsonResult>(result =>
            {
                _modelTester?.Test(result.Value);
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Test that controller action returns a <see cref="PartialViewResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="PartialViewResult"/> returned from the controller action.</returns>
        public PartialViewResult TestPartialView(Action<PartialViewResult> validate = null)
        {
            return TestResult<PartialViewResult>(result =>
            {
                AssertViewName(_expectedViewName, result.ViewName);
                _modelTester?.Test(result.Model);
                validate?.Invoke(result);
            });
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
                    throw new ControllerTestException($"Expected ActionName <{expectedActionName}>. Actual: <{actual}>.");
                }

                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="RedirectToPageResult"/>.
        /// </summary>
        /// <param name="expectedPageName">The expected page name.</param>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="FileResult"/> returned from the controller action.</returns>
        public RedirectToPageResult TestRedirectToPage(string expectedPageName, Action<RedirectToPageResult> validate = null)
        {
            return TestResult<RedirectToPageResult>(result =>
            {
                string actual = result.PageName;

                if (!string.Equals(expectedPageName, actual, StringComparison.InvariantCulture))
                {
                    throw new ControllerTestException($"Expected PageName <{expectedPageName}>. Actual: <{actual}>.");
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
        /// Test the result of the controller action.
        /// </summary>
        /// <typeparam name="TActionResultType">The expected <see cref="IActionResult"/> 
        /// type that should be returned from the action.</typeparam>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <typeparamref name="TActionResultType"/> returned from the controller action.</returns>
        /// <exception cref="ControllerTestException">if any errors are asserted.</exception>
        public TActionResultType TestResult<TActionResultType>(Action<TActionResultType> validate = null)
            where TActionResultType : IActionResult
        {
            TActionResultType actionResult = AssertActionResultType<TActionResultType>(_controllerAction());

            validate?.Invoke(actionResult);

            return actionResult;
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="ViewResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ViewResult"/> returned from the controller action.</returns>
        public ViewResult TestView(Action<ViewResult> validate = null)
        {
            return TestResult<ViewResult>(result =>
            {
                AssertViewName(_expectedViewName, result.ViewName);
                _modelTester?.Test(result.Model);
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
                throw new ControllerTestException(
                    $"Expected IActionResult type {expectedType.FullName}. Actual: {actualTypeName}.");
            }

            return (TActionResultType)actionResult;
        }

        private void AssertViewName(string expectedViewName, string actualViewName)
        {
            if (_expectedViewNameSpecified)
            {
                string expected = expectedViewName ?? string.Empty;
                string actual = actualViewName ?? string.Empty;

                if (!string.Equals(expected, actual, StringComparison.InvariantCulture))
                {
                    throw new ControllerTestException($"Expected ViewName <{expected}>. Actual: <{actual}>.");
                }
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
