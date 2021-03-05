using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using System;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// Helper for testing ASP.NET Core MVC <see cref="ViewComponent"/> Invoke / InvokeAsync methods.
    /// </summary>
    public class ViewComponentInvocationTester 
    {
        private readonly object _viewComponent;
        private readonly Func<IViewComponentResult> _invokeAction;
        private IModelTester _modelTester;
        private string _expectedViewName = null;
        private bool _expectedViewNameSpecified;

        /// <summary>
        /// Called by the 
        /// <see cref="ControllerTester{TController}.Action(System.Linq.Expressions.Expression{Func{TController, Func{IActionResult}}})"/>
        /// method.
        /// </summary>
        /// <param name="viewComponent">The "parent" ViewComponent.</param>
        /// <param name="controllerAction">"Callback" function that privides the invocation action to be tested.</param>
        internal ViewComponentInvocationTester(object viewComponent, Func<IViewComponentResult> invokeAction)
        {
            _viewComponent = viewComponent;
            _invokeAction = invokeAction;
        }

        /// <summary>
        /// "Callback" action to "arrange" the test.
        /// </summary>
        /// <returns>"This" <see cref="ViewComponentInvocationTester"/>.</returns>
        public ViewComponentInvocationTester When(Action action)
        {
            action?.Invoke();
            return this;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ViewComponentInvocationTester"/>.</returns>
        public ViewComponentInvocationTester WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
        }

        /// <summary>
        /// Specifies that the
        /// <see cref="TestView(Action{ViewViewComponentResult})"/> method should throw an exception
        /// if the  result's .ViewName doesn't match the <paramref name="expectedViewName"/>.
        /// </summary>
        /// <param name="expectedViewName">The expected view name.</param>
        /// <returns>"This" <see cref="ViewComponentInvocationTester"/>.</returns>
        public ViewComponentInvocationTester ExpectingViewName(string expectedViewName)
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
        /// <returns>"This" <see cref="ViewComponentInvocationTester"/>.</returns>
        public ViewComponentInvocationTester ExpectingModel<TModelType>(Action<TModelType> validate = null)
        {
            _modelTester = new ModelTester<TModelType>(validate);
            return this;
        }

        /// <summary>
        /// Tests that invocation returns a <see cref="ContentViewComponentResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ViewViewComponentResult"/> returned from the invocation.</returns>
        public ContentViewComponentResult TestContent(Action<ContentViewComponentResult> validate = null)
        {
            return TestResult<ContentViewComponentResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that invocation returns a <see cref="HtmlContentViewComponentResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ViewViewComponentResult"/> returned from the invocation.</returns>
        public HtmlContentViewComponentResult TestHtmlContent(Action<HtmlContentViewComponentResult> validate = null)
        {
            return TestResult<HtmlContentViewComponentResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Test the result of the invocation.
        /// </summary>
        /// <typeparam name="TActionResultType">The expected <see cref="IViewComponentResult"/> 
        /// type that should be returned from the invocation.</typeparam>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <typeparamref name="IViewComponentResult"/> returned from the invocation.</returns>
        /// <exception cref="ActionTestException">if any errors are asserted.</exception>
        public TActionResultType TestResult<TActionResultType>(Action<TActionResultType> validate = null)
            where TActionResultType : IViewComponentResult
        {
            TActionResultType actionResult = AssertViewComponentResultType<TActionResultType>(_invokeAction());

            validate?.Invoke(actionResult);

            return actionResult;
        }

        /// <summary>
        /// Tests that invocation returns a <see cref="ViewViewComponentResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ViewViewComponentResult"/> returned from the invocation.</returns>
        public ViewViewComponentResult TestView(Action<ViewViewComponentResult> validate = null)
        {
            return TestResult<ViewViewComponentResult>(result =>
            {
                AssertViewName(_expectedViewName, result.ViewName);
                _modelTester?.Test(result.ViewData?.Model);
                validate?.Invoke(result);
            });
        }

        private TResultType AssertViewComponentResultType<TResultType>(IViewComponentResult viewComponentResult)
            where TResultType : IViewComponentResult
        {
            Type expectedType = typeof(TResultType);

            if (viewComponentResult == null || !TypeTester.IsOfType(viewComponentResult, expectedType))
            {
                string actualTypeName = (viewComponentResult == null) ? "null" : viewComponentResult.GetType().FullName;
                throw new ActionTestException(
                    $"Expected IViewComponentResult type {expectedType.FullName}. Actual: {actualTypeName}.");
            }

            return (TResultType)viewComponentResult;
        }

        private void AssertViewName(string expectedViewName, string actualViewName)
        {
            if (_expectedViewNameSpecified)
            {
                string expected = expectedViewName ?? string.Empty;
                string actual = actualViewName ?? string.Empty;

                if (!string.Equals(expected, actual, StringComparison.InvariantCulture))
                {
                    throw new ActionTestException($"Expected ViewName <{expected}>. Actual: <{actual}>.");
                }
            }
        }

        private void SetModelStateIsValid(bool isValid = true)
        {
            ModelStateDictionary modelState = (_viewComponent as ViewComponent)?.ModelState;

            if (modelState == null)
            {
                PropertyInfo propertyInfo = 
                    _viewComponent.GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty)
                    .SingleOrDefault(p => p.Name == "ModelState");

                if (propertyInfo != null && propertyInfo.PropertyType == typeof(ModelStateDictionary))
                {
                    modelState = (ModelStateDictionary) propertyInfo.GetValue(_viewComponent);
                }

                if (modelState == null)
                {
                    throw new ActionTestException(
                        $"{_viewComponent.GetType().FullName} does not have a public \"ModelState\" ModelStateDictionary property.");
                }
            }

            modelState.Clear();

            if (!isValid)
            {
                modelState.AddModelError("key", "message");
            }
        }
    }
}
