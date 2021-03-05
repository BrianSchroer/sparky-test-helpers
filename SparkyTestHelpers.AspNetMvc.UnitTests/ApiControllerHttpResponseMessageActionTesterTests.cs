using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.Scenarios;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    [TestClass]
    public class ApiControllerHttpResponseMessageActionTesterTests
    {
        private TestApiController _controller;
        private ApiControllerTester<TestApiController> _controllerTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new TestApiController
            {
                ActionResponseMessage = new HttpResponseMessage { StatusCode = HttpStatusCode.OK }
            };

            _controllerTester = _controller.CreateTester();
        }

        [DataTestMethod]
        public void WhenModelStateIsValidEquals_method_should_work_as_expected()
        {
            ForTest.Scenarios(
                (Valid: true, ExpectedStatus: HttpStatusCode.OK),
                (Valid: false, ExpectedStatus: HttpStatusCode.BadRequest)
            )
            .TestEach(scenario =>
            {
                _controllerTester.Action(x => x.HttpResponseMessageActionThatChecksModelState)
                    .WhenModelStateIsValidEquals(scenario.Valid)
                    .ExpectingHttpStatusCode(scenario.ExpectedStatus)
                    .Test();
            });
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_with_siteUrlPrefix_and_with_QueryStringParameters()
        {
            _controllerTester
                .Action(x => x.HttpResponseMessageActionWithoutArguments)
                .WhenRequestHasQueryStringParameters("http://fake.com", new QueryStringParameter("parm1", "value1"), new QueryStringParameter("parm2", 2));

            _controller.Request.RequestUri.ToString().Should().Be("http://fake.com/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_without_siteUrlPrefix_and_with_QueryStringParameters()
        {
            _controllerTester
                .Action(x => x.HttpResponseMessageActionWithoutArguments)
                .WhenRequestHasQueryStringParameters(new QueryStringParameter("parm1", "value1"), new QueryStringParameter("parm2", 2));

            _controller.Request.RequestUri.ToString().Should().Be("http://localhost/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_with_siteUrlPrefix_and_with_NameValueCollection()
        {
            _controllerTester
                .Action(x => x.HttpResponseMessageActionWithoutArguments)
                .WhenRequestHasQueryStringParameters(
                    "http://fake.com", new NameValueCollection { { "parm1", "value1" }, { "parm2", "2" } });

            _controller.Request.RequestUri.ToString().Should().Be("http://fake.com/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_without_siteUrlPrefix_and_with_NameValueCollection()
        {
            _controllerTester
                .Action(x => x.HttpResponseMessageActionWithoutArguments)
                .WhenRequestHasQueryStringParameters(new NameValueCollection { { "parm1", "value1" }, { "parm2", "2" } });

            _controller.Request.RequestUri.ToString().Should().Be("http://localhost/?parm1=value1&parm2=2");
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.BadRequest, HttpStatusCode.OK)]
        public void Test_method_should_check_ExpectingHttpStatusCode(HttpStatusCode expectedStatusCode, HttpStatusCode actualStatusCode)
        {
            _controller.ActionResponseMessage = new HttpResponseMessage { StatusCode = actualStatusCode };

            Action action = () =>
                _controllerTester.Action(x => x.HttpResponseMessageActionWithoutArguments)
                .ExpectingHttpStatusCode(expectedStatusCode)
                .Test();

            if (expectedStatusCode == actualStatusCode)
            {
                action.Should().NotThrow();
            }
            else
            {
                action.Should().Throw<ControllerTestException>().WithMessage(
                    $"Expected HTTP Status {expectedStatusCode} ({(int)expectedStatusCode})."
                    + $" Actual: {actualStatusCode} ({(int)actualStatusCode}).");
            }
        }

        [DataTestMethod]
        [DataRow(HttpStatusCode.OK, HttpStatusCode.OK)]
        [DataRow(HttpStatusCode.OK, HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.BadRequest, HttpStatusCode.BadRequest)]
        [DataRow(HttpStatusCode.BadRequest, HttpStatusCode.OK)]
        public void Test_method_should_work_with_action_with_parameters(HttpStatusCode expectedStatusCode, HttpStatusCode actualStatusCode)
        {
            _controller.ActionResponseMessage = new HttpResponseMessage { StatusCode = actualStatusCode };

            Action action = () =>
                _controllerTester.Action(x => () => x.HttpResponseMessageActionWithArgument("test"))
                .ExpectingHttpStatusCode(expectedStatusCode)
                .Test();

            if (expectedStatusCode == actualStatusCode)
            {
                action.Should().NotThrow();
            }
            else
            {
                action.Should().Throw<ControllerTestException>().WithMessage(
                    $"Expected HTTP Status {expectedStatusCode} ({(int)expectedStatusCode})."
                    + $" Actual: {actualStatusCode} ({(int)actualStatusCode}).");
            }
        }
    }
}