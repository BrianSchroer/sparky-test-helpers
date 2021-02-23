using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing <see cref="ApiController"/> actions that return a response of type <typeparamref name="TResponse"/>.
    /// </summary>
    /// <typeparam name="TResponse">The action response type.</typeparam>
    public class ApiControllerActionTester<TResponse>
    {
        private readonly Func<TResponse> _controllerAction;

        /// <summary>
        /// The <see cref="ApiController"/> instance being tested.
        /// </summary>
        public ApiController Controller { get; private set; }

        /// <summary>
        /// Creates a new <see cref="ApiControllerActionTester{TResponse}" instance.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="ApiController"/>.</param>
        /// <param name="controllerAction">"Callback" function that provides the controller action to be tested.</param>
        public ApiControllerActionTester(ApiController controller, Func<TResponse> controllerAction)
        {
            Controller = controller;
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ApiControllerActionTester{TResponse}"/>.</returns>
        public ApiControllerActionTester<TResponse> WhenModelStateIsValidEquals(bool isValid)
        {
            SetModelStateIsValid(isValid);
            return this;
        }

        /// <summary>
        /// Sets <see cref="Controller.Request" /> to a new <see cref="HttpRequestMessage" /> with a
        /// <see cref="HttpRequestMessage.RequestUri" /> with specified <paramref name="siteUrlPrefix"/> and
        /// <paramref name="queryStringParameters"/>.
        /// </summary>
        /// <param name="siteUrlPrefix">site URL prefix, e.g. "http://mySite.com", "http://localhost".</param>
        /// <param name="queryStringParameters">The <see cref="QueryStringParameter"/>s.</param>
        /// <returns>"This" <see cref="ApiControllerActionTester{TResponse}"/>.</returns>
        public ApiControllerActionTester<TResponse> WithRequestQueryStringValues(string siteUrlPrefix, params QueryStringParameter[] queryStringParameters)
        {
            if (Controller.Request is null)
            {
                Controller.Request = new HttpRequestMessage();
            }

            Controller.Request.RequestUri = UriBuilder.Build(siteUrlPrefix, queryStringParameters);

            return this;
        }

        /// <summary>
        /// Sets <see cref="Controller.Request" /> to a new <see cref="HttpRequestMessage" /> with a
        /// <see cref="HttpRequestMessage.RequestUri" /> with the site URL prefix "http://localhost" and the
        /// specified <paramref name="queryStringParameters"/>.
        /// </summary>
        /// <param name="queryStringParameters">The <see cref="QueryStringParameter"/>s.</param>
        /// <returns>"This" <see cref="ApiControllerActionTester{TResponse}"/>.</returns>
        public ApiControllerActionTester<TResponse> WithRequestQueryStringValues(params QueryStringParameter[] queryStringParameters)
        {
            if (Controller.Request is null)
            {
                Controller.Request = new HttpRequestMessage();
            }

            Controller.Request.RequestUri = UriBuilder.Build(queryStringParameters);

            return this;
        }

        /// <summary>
        /// Sets <see cref="Controller.Request" /> to a new <see cref="HttpRequestMessage" /> with a
        /// <see cref="HttpRequestMessage.RequestUri" /> with specified <paramref name="siteUrlPrefix"/> and
        /// <paramref name="queryStringParameters"/>.
        /// </summary>
        /// <param name="siteUrlPrefix">site URL prefix, e.g. "http://mySite.com", "http://localhost".</param>
        /// <param name="queryStringParameters">The <see cref="QueryStringParameter"/>s.</param>
        /// <returns>"This" <see cref="ApiControllerActionTester{TResponse}"/>.</returns>
        public ApiControllerActionTester<TResponse> WithRequestQueryStringValues(string siteUrlPrefix, NameValueCollection queryStringParameters)
        {
            if (Controller.Request is null)
            {
                Controller.Request = new HttpRequestMessage();
            }

            Controller.Request.RequestUri = UriBuilder.Build(siteUrlPrefix, queryStringParameters);

            return this;
        }

        /// <summary>
        /// Sets <see cref="Controller.Request" /> to a new <see cref="HttpRequestMessage" /> with a
        /// <see cref="HttpRequestMessage.RequestUri" /> with the site URL prefix "http://localhost" and the
        /// specified <paramref name="queryStringParameters"/>.
        /// </summary>
        /// <param name="queryStringParameters">The <see cref="QueryStringParameter"/>s.</param>
        /// <returns>"This" <see cref="ApiControllerActionTester{TResponse}"/>.</returns>
        public ApiControllerActionTester<TResponse> WithRequestQueryStringValues(NameValueCollection queryStringParameters)
        {
            if (Controller.Request is null)
            {
                Controller.Request = new HttpRequestMessage();
            }

            Controller.Request.RequestUri = UriBuilder.Build(queryStringParameters);

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
            Controller.ModelState.Clear();

            if (!isValid)
            {
                Controller.ModelState.AddModelError("key", "message");
            }
        }
    }
}
