using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Reflection;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc.Routing
{
    /// <summary>
    /// Helper for routing assertions for a specified URL.
    /// </summary>
    public class RoutingAsserter
    {
        private readonly string _relativeUrl;
        private readonly RouteTester _routeTester;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoutingAsserter"/> class.
        /// </summary>
        /// <remarks>Called by <see cref="RouteTester.ForUrl(string)"/>.</remarks>
        /// <param name="relativeUrl"></param>
        /// <param name="routeTester"></param>
        internal RoutingAsserter(string relativeUrl, RouteTester routeTester)
        {
            _relativeUrl = relativeUrl;
            _routeTester = routeTester;
        }

        /// <summary>
        /// Assert that the test URL is mapped to the expected <see cref="RouteData"/> values.
        /// </summary>
        /// <param name="expectedValues">The expected <see cref="RouteData"/> values.</param>
        /// <returns>The "parent" <see cref="RouteTester"/>.</returns>
        /// <exception cref="RouteTesterException">if mapping does not result in the expected values.</exception>
        /// <example>
        /// <code><![CDATA[
        ///     _routeTester.ForUrl("Order/Details/3").AssertMapTo(new Dictionary<string, object>
        ///     {
        ///         { "controller", "Order" },
        ///         { "action, "Details" }
        ///         { "id", 3 }
        ///     });
        /// ]]></code>
        /// </example>
        public RouteTester AssertMapTo(IDictionary<string, object> expectedValues)
        {
            string expected = FormatValues(expectedValues);
            string actual = FormatValues(_routeTester.GetRouteData(_relativeUrl).Values);

            if (!actual.Equals(expected, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new RouteTesterException($"Expected: \n{expected}. \nActual: \n{actual}.");
            }

            return _routeTester;
        }

        /// <summary>
        /// Assert that the test URL is mapped to the expected <see cref="RouteData"/> controller, action and (optional) id values.
        /// </summary>
        /// <param name="controller">The expected controller name.</param>
        /// <param name="action">The expected action name.</param>
        /// <param name="id">(Optional) expected ID value.</param>
        /// <returns>The "parent" <see cref="RouteTester"/>.</returns>
        /// <exception cref="RouteTesterException">if mapping does not result in the expected values.</exception>
        /// <example>
        /// <code><![CDATA[
        ///     _routeTester.ForUrl("Home/Index").AssertMapTo("Home", "Index");
        ///     _routeTester.ForUrl("Order/Details/3").AssertMapTo("Order", "Details", 3);
        /// ]]></code>
        /// </example>
        public RouteTester AssertMapTo(string controller, string action, object id = null)
        {
            return AssertMapTo(new Dictionary<string, object>
            {
                { "controller", controller },
                { "action", action },
                { "id", id }
            });
        }

        /// <summary>
        /// Assert that the test URL is mapped to the expected controller action.
        /// </summary>
        /// <typeparam name="TController">The controller type.</typeparam>
        /// <param name="actionExpression">The controller/action expression.</param>
        /// <returns>The "parent" <see cref="RouteTester"/>.</returns>
        /// <exception cref="RouteTesterException">if mapping does not result in the expected values.</exception>
        /// <example>
        /// <code><![CDATA[
        ///     _routeTester.ForUrl("Home/Index").AssertMapTo<HomeController>(x => x.Index);
        /// ]]></code>
        /// </example>
        public RouteTester AssertMapTo<TController>(Expression<Func<TController, Func<ActionResult>>> actionExpression)
            where TController : Controller
        {
            string controllerName = GetControllerName<TController>();
            string actionName = null;
            object id = null;

            if (actionExpression.Body is UnaryExpression)
            {
                var unaryExpression = actionExpression.Body as UnaryExpression;
                var methodCallExpression = unaryExpression.Operand as MethodCallExpression;
                var constantExpression = methodCallExpression.Object as ConstantExpression;
                var methodInfo = constantExpression.Value as MethodInfo;
                actionName = GetActionName(methodInfo);
            }
            else if (actionExpression.Body is LambdaExpression)
            {
                var lambdaExpression = actionExpression.Body as LambdaExpression;
                var methodCallExpression = lambdaExpression.Body as MethodCallExpression;
                actionName = GetActionName(methodCallExpression.Method);
                id = GetId(methodCallExpression);
            }
            else
            {
                throw new RouteTesterException($"Unexpected expression type: {actionExpression.Body.GetType().FullName}.");
            }

            return AssertMapTo(controllerName, actionName, id);
        }

        /// <summary>
        /// Assert that routing rules redirect the test URL to the expected <paramref name="redirectUrl"/>
        /// and return the expected HTTP status code. 
        /// </summary>
        /// <param name="redirectUrl">The expected redirect URL.</param>
        /// <param name="httpStatusCode">The expected HTTP status code (default is 302 - Redirect).</param>
        /// <returns>The "parent" <see cref="RouteTester"/>.</returns>
        /// <exception cref="RouteTesterException">if the redirect does not return expected results.</exception>
        public RouteTester AssertRedirectTo(string redirectUrl, HttpStatusCode httpStatusCode = HttpStatusCode.Redirect)
        {
            return _routeTester.AssertRedirect(_relativeUrl, redirectUrl, httpStatusCode);
        }

        private static string FormatValues(IDictionary<string, object> dict)
        {
            return string.Join(
                ", ",
                dict.Keys.OrderBy(key => key).Select(key => $"{key}: {FormatDictionaryValue(dict[key])}"));
        }

        private static string FormatDictionaryValue(object obj)
        {
            if (obj == null || obj is UrlParameter && obj.ToString().Length == 0)
            { 
                return "(null)";
            }

            return $"\"{obj}\"";
        }

        private static string GetControllerName<TController>() where TController : Controller
        {
            string name = typeof(TController).Name;
            return (name.EndsWith("Controller")) ? name.Substring(0, name.Length - 10) : name;
        }

        private static string GetActionName(MethodInfo methodInfo)
        {
            ActionNameAttribute[] attributes = methodInfo.GetCustomAttributes<ActionNameAttribute>().ToArray();
            return (attributes.Length > 0) ? attributes[0].Name : methodInfo.Name;
        }

        private static object GetId(MethodCallExpression methodCallExpression)
        {
            object id = null;

            if (methodCallExpression.Arguments.Any())
            {
                id = GetParameterValue(methodCallExpression.Arguments[0]);
            }

            return id;
        }

        private static object GetParameterValue(Expression expression)
        {
            switch (expression.NodeType)
            {
                case ExpressionType.Constant:
                    return (expression as ConstantExpression).Value;
                case ExpressionType.MemberAccess:
                case ExpressionType.New:
                    return Expression.Lambda(expression, new ParameterExpression[0]).Compile().DynamicInvoke(new object[0]);
                default:
                    return null;
            }
        }
    }
}
