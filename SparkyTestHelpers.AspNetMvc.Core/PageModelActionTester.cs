using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// Helper for testing ASP.NET Core MVC PageModel actions.
    /// </summary>
    public class PageModelActionTester<TPageModel> where TPageModel : PageModel
    {
        private readonly PageModel _pageModel;
        private readonly Func<IActionResult> _pageModelAction;
        private IModelTester _modelTester;

        /// <summary>
        /// Called by the 
        /// <see cref="PageModelTester{TPageModel}.Action(System.Linq.Expressions.Expression{Func{TPageModel, Func{IActionResult}}})"/>
        /// method.
        /// </summary>
        /// <param name="pageModel">The "parent" <see cref="PageModel"/>.</param>
        /// <param name="pageModelAction">"Callback" function that privides the pageModel action to be tested.</param>
        internal PageModelActionTester(TPageModel pageModel, Func<IActionResult> pageModelAction)
        {
            _pageModel = pageModel;
            _pageModelAction = pageModelAction;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="PageModelActionTester"/>.</returns>
        public PageModelActionTester<TPageModel> WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
        }

        /// <summary>
        /// "Callback" action to "arrange" the test.
        /// </summary>
        /// <returns>"This" <see cref="PageModelActionTester{TPageModel}"/>.</returns>
        public PageModelActionTester<TPageModel> When(Action action)
        {
            action?.Invoke();
            return this;
        }

        /// <summary>
        /// Specifies that the
        /// <see cref="TestPage(Action{PageResult})"/> or
        /// <see cref="TestJsonResult(Action{JsonResult})"/> method should throw an exception
        /// if the action result's .Model or .Value type doesn't match <typeparamref name="TModelType"/>.
        /// </summary>
        /// <remarks>
        /// The <see cref="ExpectingModel(Action{TPageModel})"/> method is easier to use 
        /// with <see cref="TestPage(Action{PageResult})"/>.
        /// </remarks>
        /// <typeparam name="TModelType">The expected model type.</typeparam>
        /// <param name="validate">(Optional) callback action to preform additional model validation.</param>
        /// <returns>"This" <see cref="PageModelActionTester"/>.</returns>
        public PageModelActionTester<TPageModel> ExpectingModel<TModelType>(Action<TModelType> validate = null)
        {
            _modelTester = new ModelTester<TModelType>(validate);
            return this;
        }

        /// <summary>
        /// Specifies "callback" function that validates the <see cref="TPageModel"/> 
        /// when <see cref="TestPage(Action{PageResult})"/> is called.
        /// </summary>
        /// <param name="validate">Callback action to perform model validation.</param>
        /// <returns>"This" <see cref="PageModelActionTester"/>.</returns>
        public PageModelActionTester<TPageModel> ExpectingModel(Action<TPageModel> validate)
        {
            return ExpectingModel<TPageModel>(validate);
        }

        /// <summary>
        /// Tests that PageModel action returns a <see cref="ContentResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ContentResult"/> returned from the PageModel action.</returns>
        public ContentResult TestContent(Action<ContentResult> validate = null)
        {
            return TestResult<ContentResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that PageModel action returns a <see cref="FileResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="FileResult"/> returned from the PageModel action.</returns>
        public FileResult TestFile(Action<FileResult> validate = null)
        {
            return TestResult<FileResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that PageModel action returns a <see cref="JsonResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="JsonResult"/> returned from the PageModel action.</returns>
        public JsonResult TestJsonResult(Action<JsonResult> validate = null)
        {
            return TestResult<JsonResult>(result =>
            {
                _modelTester?.Test(result.Value);
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that PageModel action returns a <see cref="PageResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="PageResult"/> returned from the PageModel action.</returns>
        public PageResult TestPage(Action<PageResult> validate = null)
        {
            return TestResult<PageResult>(result =>
            {
                _modelTester?.Test(_pageModel);
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that PageModel action redirects to the specified action name.
        /// </summary>
        /// <param name="expectedActionName">The expected action name.</param>
        /// <param name="expectedControllerName">The expected controller name.</param>
        /// <param name="expectedRouteValues">The expected route values.</param>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <see cref="RedirectToActionResult"/> returned from the PageModel action.</returns>
        public RedirectToActionResult TestRedirectToAction(
            string expectedActionName,
            string expectedControllerName = null,
            object expectedRouteValues = null,
            Action<RedirectToActionResult> validate = null)
        {
            if (expectedActionName == null)
                throw new ArgumentNullException(nameof(expectedActionName));

            return TestResult<RedirectToActionResult>(result =>
            {
                string expected = FormatRedirectToActionValues(expectedActionName, expectedControllerName, expectedRouteValues);
                string actual = FormatRedirectToActionValues(result.ActionName, result.ControllerName, result.RouteValues);

                if (!string.Equals(expected, actual, StringComparison.InvariantCulture))
                {
                    throw new ActionTestException($"Expected <{expected}>. Actual: <{actual}>.");
                }

                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that PageModel action returns a <see cref="RedirectToPageResult"/>.
        /// </summary>
        /// <param name="expectedPageName">The expected page name.</param>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="FileResult"/> returned from the PageModel action.</returns>
        public RedirectToPageResult TestRedirectToPage(string expectedPageName, Action<RedirectToPageResult> validate = null)
        {
            return TestResult<RedirectToPageResult>(result =>
            {
                string actual = result.PageName;

                if (!string.Equals(expectedPageName, actual, StringComparison.InvariantCulture))
                {
                    throw new ActionTestException($"Expected PageName <{expectedPageName}>. Actual: <{actual}>.");
                }

                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that PageModel action redirects to the specified route.
        /// </summary>
        /// <param name="expectedRoute">The expected route.</param>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="RedirectToRouteResult"/> returned from the PageModel action.</returns>
        public RedirectToRouteResult TestRedirectToRoute(
            string expectedRoute, Action<RedirectToRouteResult> validate = null)
        {
            return TestResult<RedirectToRouteResult>(result =>
            {
                string actual = string.Join("/", result.RouteValues.Values.Select(v => v.ToString()));

                if (!string.Equals(expectedRoute, actual, StringComparison.InvariantCulture))
                {
                    throw new ActionTestException($"Expected route <{expectedRoute}>. Actual: <{actual}>.");
                }

                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Test the result of the pageModel action.
        /// </summary>
        /// <typeparam name="TActionResultType">The expected <see cref="IActionResult"/> 
        /// type that should be returned from the action.</typeparam>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <typeparamref name="TActionResultType"/> returned from the PageModel action.</returns>
        /// <exception cref="ActionTestException">if any errors are asserted.</exception>
        public TActionResultType TestResult<TActionResultType>(Action<TActionResultType> validate = null)
            where TActionResultType : IActionResult
        {
            TActionResultType actionResult = AssertActionResultType<TActionResultType>(_pageModelAction());

            validate?.Invoke(actionResult);

            return actionResult;
        }

        private TActionResultType AssertActionResultType<TActionResultType>(IActionResult actionResult)
            where TActionResultType : IActionResult
        {
            Type expectedType = typeof(TActionResultType);

            if (actionResult == null || !TypeTester.IsOfType(actionResult, expectedType))
            {
                string actualTypeName = (actionResult == null) ? "null" : actionResult.GetType().FullName;
                throw new ActionTestException(
                    $"Expected IActionResult type {expectedType.FullName}. Actual: {actualTypeName}.");
            }

            return (TActionResultType)actionResult;
        }

        private void SetModelStateIsValid(bool isValid = true)
        {
            if (_pageModel.PageContext == null)
            {
                _pageModel.PageContext = new PageContext();
            }

            _pageModel.ModelState.Clear();

            if (!isValid)
            {
                _pageModel.ModelState.AddModelError("key", "message");
            }
        }

        private static string FormatRedirectToActionValues(string actionName, string controllerName, object routeValues)
        {
            return JsonConvert.SerializeObject(
                new
                {
                    ActionName = actionName,
                    ControllerName = controllerName,
                    RouteValues = routeValues
                });
        }
    }
}
