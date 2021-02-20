using System;
using System.Web.Http;
using System.Web.Http.Results;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing <see cref="ApiController"/> actions that return an <see cref="IHttpActionResult"/>.
    /// </summary>
    public class ApiControllerHttpActionResultActionTester
    {
        private readonly ApiController _controller;
        private readonly Func<IHttpActionResult> _controllerAction;

        /// <summary>
        /// Called by the
        /// <see cref="ApiControllerTester{TController}.Action(System.Linq.Expressions.Expression{Func{TController, Func{IHttpActionResult}}})"/>
        /// method.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="ApiController"/>.</param>
        /// <param name="controllerAction">"Callback" function that provides the controller action to be tested.</param>
        internal ApiControllerHttpActionResultActionTester(ApiController controller, Func<IHttpActionResult> controllerAction)
        {
            _controller = controller;
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ApiControllerHttpActionResultActionTester"/>.</returns>
        public ApiControllerHttpActionResultActionTester WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="BadRequestErrorMessageResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="BadRequestErrorMessageResult"/> returned from the controller action.</returns>
        public BadRequestErrorMessageResult TestBadRequestErrorMessageResult(Action<BadRequestErrorMessageResult> validate = null)
        {
            return TestResult<BadRequestErrorMessageResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="BadRequestResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="BadRequestResult"/> returned from the controller action.</returns>
        public BadRequestResult TestBadRequestResult(Action<BadRequestResult> validate = null)
        {
            return TestResult<BadRequestResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="ConflictResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ConflictResult"/> returned from the controller action.</returns>
        public ConflictResult TestConflictResult(Action<ConflictResult> validate = null)
        {
            return TestResult<ConflictResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="ExceptionResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ExceptionResult"/> returned from the controller action.</returns>
        public ExceptionResult TestExceptionResult(Action<ExceptionResult> validate = null)
        {
            return TestResult<ExceptionResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="InternalServerErrorResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="InternalServerErrorResult"/> returned from the controller action.</returns>
        public InternalServerErrorResult TestInternalServerErrorResult(Action<InternalServerErrorResult> validate = null)
        {
            return TestResult<InternalServerErrorResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="InvalidModelStateResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="InvalidModelStateResult"/> returned from the controller action.</returns>
        public InvalidModelStateResult TestInvalidModelStateResult(Action<InvalidModelStateResult> validate = null)
        {
            return TestResult<InvalidModelStateResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="NotFoundResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="NotFoundResult"/> returned from the controller action.</returns>
        public NotFoundResult TestNotFoundResult(Action<NotFoundResult> validate = null)
        {
            return TestResult<NotFoundResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="OkResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="OkResult"/> returned from the controller action.</returns>
        public OkResult TestOkResult(Action<OkResult> validate = null)
        {
            return TestResult<OkResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="RedirectResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="RedirectResult"/> returned from the controller action.</returns>
        public RedirectResult TestRedirectResult(Action<RedirectResult> validate = null)
        {
            return TestResult<RedirectResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="RedirectToRouteResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="RedirectToRouteResult"/> returned from the controller action.</returns>
        public RedirectToRouteResult TestRedirectToRouteResult(Action<RedirectToRouteResult> validate = null)
        {
            return TestResult<RedirectToRouteResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="ResponseMessageResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="ResponseMessageResult"/> returned from the controller action.</returns>
        public ResponseMessageResult TestResponseMessageResult(Action<ResponseMessageResult> validate = null)
        {
            return TestResult<ResponseMessageResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="StatusCodeResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="StatusCodeResult"/> returned from the controller action.</returns>
        public StatusCodeResult TestStatusCodeResult(Action<StatusCodeResult> validate = null)
        {
            return TestResult<StatusCodeResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="UnauthorizedResult"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="UnauthorizedResult"/> returned from the controller action.</returns>
        public UnauthorizedResult TestUnauthorizedResult(Action<UnauthorizedResult> validate = null)
        {
            return TestResult<UnauthorizedResult>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="CreatedAtRouteNegotiatedContentResult{T}"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="CreatedAtRouteNegotiatedContentResult{T}"/> returned from the controller action.</returns>
        public CreatedAtRouteNegotiatedContentResult<T> TestCreatedAtRouteNegotiatedContentResult<T>(Action<CreatedAtRouteNegotiatedContentResult<T>> validate = null)
        {
            return TestResult<CreatedAtRouteNegotiatedContentResult<T>>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="CreatedNegotiatedContentResult{T}"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="CreatedNegotiatedContentResult{T}"/> returned from the controller action.</returns>
        public CreatedNegotiatedContentResult<T> TestCreatedNegotiatedContentResult<T>(Action<CreatedNegotiatedContentResult<T>> validate = null)
        {
            return TestResult<CreatedNegotiatedContentResult<T>>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="FormattedContentResult{T}"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="FormattedContentResult{T}"/> returned from the controller action.</returns>
        public FormattedContentResult<T> TestFormattedContentResult<T>(Action<FormattedContentResult<T>> validate = null)
        {
            return TestResult<FormattedContentResult<T>>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="JsonResult{T}"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="JsonResult{T}"/> returned from the controller action.</returns>
        public JsonResult<T> TestJsonResult<T>(Action<JsonResult<T>> validate = null)
        {
            return TestResult<JsonResult<T>>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="NegotiatedContentResult{T}"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="NegotiatedContentResult{T}"/> returned from the controller action.</returns>
        public NegotiatedContentResult<T> TestNegotiatedContentResult<T>(Action<NegotiatedContentResult<T>> validate = null)
        {
            return TestResult<NegotiatedContentResult<T>>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Tests that controller action returns a <see cref="OkNegotiatedContentResult{T}"/>.
        /// </summary>
        /// <param name="validate">(Optional) callback validation function.</param>
        /// <returns>The <see cref="OkNegotiatedContentResult{T}"/> returned from the controller action.</returns>
        public OkNegotiatedContentResult<T> TestOkNegotiatedContentResult<T>(Action<OkNegotiatedContentResult<T>> validate = null)
        {
            return TestResult<OkNegotiatedContentResult<T>>(result =>
            {
                validate?.Invoke(result);
            });
        }

        /// <summary>
        /// Test the result of the controller action.
        /// </summary>
        /// <typeparam name="THttpActionResult">The expected <see cref="ActionResult"/>
        /// type that should be returned from the action.</typeparam>
        /// <param name="validate">(Optional) callback validation action.</param>
        /// <returns>The <typeparamref name="THttpActionResult"/> returned from the controller action.</returns>
        /// <exception cref="ControllerTestException">if any errors are asserted.</exception>
        public THttpActionResult TestResult<THttpActionResult>(Action<THttpActionResult> validate = null) where THttpActionResult : IHttpActionResult
        {
            THttpActionResult actionResult = AssertHttpActionResult<THttpActionResult>(_controllerAction());

            validate?.Invoke(actionResult);

            return actionResult;
        }

        private THttpActionResult AssertHttpActionResult<THttpActionResult>(IHttpActionResult actionResult) where THttpActionResult : IHttpActionResult
        {
            Type expectedType = typeof(THttpActionResult);

            if (actionResult == null || !TypeTester.IsOfType(actionResult, expectedType))
            {
                string actualTypeName = (actionResult == null) ? "null" : actionResult.GetType().FullName;
                throw new ControllerTestException($"Expected IHttpActionResult type {expectedType.FullName}. Actual: {actualTypeName}.");
            }

            return (THttpActionResult)actionResult;
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