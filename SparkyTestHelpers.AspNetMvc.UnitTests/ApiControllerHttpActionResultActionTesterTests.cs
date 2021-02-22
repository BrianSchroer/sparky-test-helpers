using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using SparkyTestHelpers.Scenarios;
using System;
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
