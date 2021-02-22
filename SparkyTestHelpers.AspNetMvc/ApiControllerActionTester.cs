using System;
using System.Web.Http;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing <see cref="ApiController"/> actions that return a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The action response type.</typeparam>
    public class ApiControllerActionTester<TResponse>
    {
        private readonly ApiController _controller;
        private readonly Func<TResponse> _controllerAction;

        /// <summary>
        /// Creates a new <see cref="ApiControllerActionTester{TResponse}" instance.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="ApiController"/>.</param>
        /// <param name="controllerAction">"Callback" function that provides the controller action to be tested.</param>
        public ApiControllerActionTester(ApiController controller, Func<TResponse> controllerAction)
        {
            _controller = controller;
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerActionTester<TResponse> WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
        }
        
        /// <summary>
        /// Calls controller action, validates <see cref="HttpResponseMessage.StatusCode"/>
        /// (if <see cref="ExpectingHttpStatusCode(HttpStatusCode)" has been called)
        /// and returns the <see cref="HttpResponseMessage"/> returned from the action.
        /// </summary>
        /// <param name="validate">Optional "callback" method to validate the response message.</param>
        /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
        public TResponse Test(Action<TResponse> validate = null)
        {
            TResponse response = _controllerAction();

            validate?.Invoke(response);

            return response;
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
