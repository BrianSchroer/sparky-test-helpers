using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json;

namespace SparkyTestHelpers.AspNetMvc
{
    /// <summary>
    /// Helper for testing <see cref="ApiController"/> actions that return a <see cref="HttpResponseMessage"/>.
    /// </summary>
    public class ApiControllerHttpResponseMessageActionTester
    {
        private readonly Func<HttpResponseMessage> _controllerAction;
        private HttpStatusCode? _expectedStatusCode = null;

        /// <summary>
        /// The <see cref="ApiController"/> instance being tested.
        /// </summary>
        public ApiController Controller { get; private set; }

        /// <summary>
        /// Called by the
        /// <see cref="ApiControllerTester{TController}.Action(System.Linq.Expressions.Expression{Func{TController, Func{HttpResponseMessage}}})"/>
        /// method.
        /// </summary>
        /// <param name="controller">The "parent" <see cref="ApiController"/>.</param>
        /// <param name="controllerAction">"Callback" function that provides the controller action to be tested.</param>
        internal ApiControllerHttpResponseMessageActionTester(ApiController controller, Func<HttpResponseMessage> controllerAction)
        {
            Controller = controller;
            _controllerAction = controllerAction;
        }

        /// <summary>
        /// "Callback" action to "arrange" the test.
        /// </summary>
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester When(Action action)
        {
            action?.Invoke();
            return this;
        }

        /// <summary>
        /// Sets up ModelState.IsValid value.
        /// </summary>
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester WhenModelStateIsValidEquals(bool isValid)
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
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester WhenRequestHasQueryStringParameters(string siteUrlPrefix, params QueryStringParameter[] queryStringParameters)
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
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester WhenRequestHasQueryStringParameters(params QueryStringParameter[] queryStringParameters)
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
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester WhenRequestHasQueryStringParameters(string siteUrlPrefix, NameValueCollection queryStringParameters)
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
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester WhenRequestHasQueryStringParameters(NameValueCollection queryStringParameters)
        {
            if (Controller.Request is null)
            {
                Controller.Request = new HttpRequestMessage();
            }

            Controller.Request.RequestUri = UriBuilder.Build(queryStringParameters);

            return this;
        }

        /// <summary>
        /// Specifies that the 
        /// <see cref="TestContentString(Action{string})"/> and 
        /// <see cref="TestContentJsonDeserialization{TContent}(Action{TContent})"/> methods should throw an exception
        /// if the action's returned <see cref="HttpResponseMessage.StatusCode" /> doesn't match the specified
        /// <paramref name="statusCode"/> value.
        /// </summary>
        /// <param name="statusCode">The expected <see cref="HttpStatusCode"/>.</param>
        /// <returns>"This" <see cref="ApiControllerHttpResponseMessageActionTester"/>.</returns>
        public ApiControllerHttpResponseMessageActionTester ExpectingHttpStatusCode(HttpStatusCode statusCode)
        {
            _expectedStatusCode = statusCode;
            return this;
        }

        /// <summary>
        /// Calls controller action, validates <see cref="HttpResponseMessage.StatusCode"/>
        /// (if <see cref="ExpectingHttpStatusCode(HttpStatusCode)" has been called)
        /// and returns the <see cref="HttpResponseMessage"/> returned from the action.
        /// </summary>
        /// <param name="validate">Optional "callback" method to validate the response message.</param>
        /// <returns>The <see cref="HttpResponseMessage"/>.</returns>
        public HttpResponseMessage Test(Action<HttpResponseMessage> validate = null)
        {
            HttpResponseMessage responseMessage = _controllerAction();

            if (_expectedStatusCode.HasValue)
            {
                if (responseMessage is null)
                {
                    throw new ControllerTestException(
                        $"Expected HTTP Status {_expectedStatusCode} ({(int)_expectedStatusCode})"
                        + $" but the action returned null.");
                }

                HttpStatusCode statusCode = responseMessage.StatusCode;
                if (statusCode != _expectedStatusCode.Value)
                {
                    throw new ControllerTestException(
                        $"Expected HTTP Status {_expectedStatusCode} ({(int)_expectedStatusCode})."
                        + $" Actual: {statusCode} ({(int)statusCode}).");
                }
            }

            validate?.Invoke(responseMessage);

            return responseMessage;
        }

        /// <summary>
        /// Calls controller action, validates <see cref="HttpResponseMessage.StatusCode"/>
        /// (if <see cref="ExpectingHttpStatusCode(HttpStatusCode)" has been called), and
        /// deserializes the <see cref="HttpResponseMessage.Content"/>'s JSON string to a
        /// <typeparamref name="TContent"/> instance.
        /// </summary>
        /// <typeparam name="TContent">The JSON-serialized content type.</typeparam>
        /// <param name="validate">Optional "callback" method to validate the content.</param>
        /// <returns>Deserialized <typeparamref name="TContent"/> instance.</returns>
        public TContent TestContentJsonDeserialization<TContent>(Action<TContent> validate = null)
        {
            TContent content = JsonConvert.DeserializeObject<TContent>(TestContentString());

            validate?.Invoke(content);

            return content;
        }

        /// <summary>
        /// Calls controller action, validates <see cref="HttpResponseMessage.StatusCode"/>
        /// (if <see cref="ExpectingHttpStatusCode(HttpStatusCode)" has been called) and
        /// returns the <see cref="HttpResponseMessage.Content"/>'s string value.
        /// </summary>
        /// <param name="validate">Optional "callback" method to validate the content.</param>
        /// <returns>The <see cref="HttpResponseMessage.Content"/> string.</returns>
        public string TestContentString(Action<string> validate = null)
        {
            string contentString = null;

            Test(httpResponseMessage =>
            {
                if (httpResponseMessage is null)
                {
                    throw new ControllerTestException("Action returned null.");
                }

                contentString = httpResponseMessage.Content.ReadAsStringAsync().Result;

                validate?.Invoke(contentString);
            });

            return contentString;
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