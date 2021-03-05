using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SparkyTestHelpers.AspNetMvc.UnitTests.Controllers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http.ModelBinding;
using System.Web.Http.Results;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    [TestClass]
    public class ApiControllerHttpActionResultActionTesterTests
    {
        private TestApiController _controller;
        private ApiControllerTester<TestApiController> _controllerTester;
        private OkResult _okResult;

        [TestInitialize]
        public void TestInitialize()
        {
            _controller = new TestApiController();
            _controller.HttpActionResult = _okResult = new OkResult(_controller);
            _controllerTester = _controller.CreateTester();
        }

        [DataTestMethod]
        public void WhenModelStateIsValidEquals_method_should_work_as_expected()
        {
            _controllerTester.Action(x => x.HttpActionResultActionThatChecksModelState)
                .WhenModelStateIsValidEquals(true)
                .TestOkResult();

            _controllerTester.Action(x => x.HttpActionResultActionThatChecksModelState)
                .WhenModelStateIsValidEquals(false)
                .TestBadRequestResult();
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_with_siteUrlPrefix_and_with_QueryStringParameters()
        {
            _controllerTester
                .Action(x => x.HttpActionResultActionWithoutArguments)
                .WhenRequestHasQueryStringParameters("http://fake.com", new QueryStringParameter("parm1", "value1"), new QueryStringParameter("parm2", 2));

            _controller.Request.RequestUri.ToString().Should().Be("http://fake.com/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_without_siteUrlPrefix_and_with_QueryStringParameters()
        {
            _controllerTester
                .Action(x => x.HttpActionResultActionWithoutArguments)
                .WhenRequestHasQueryStringParameters(new QueryStringParameter("parm1", "value1"), new QueryStringParameter("parm2", 2));

            _controller.Request.RequestUri.ToString().Should().Be("http://localhost/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_with_siteUrlPrefix_and_with_NameValueCollection()
        {
            _controllerTester
                .Action(x => x.HttpActionResultActionWithoutArguments)
                .WhenRequestHasQueryStringParameters(
                    "http://fake.com", new NameValueCollection { { "parm1", "value1" }, { "parm2", "2" } });

            _controller.Request.RequestUri.ToString().Should().Be("http://fake.com/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void WithRequestQueryStringValues_should_build_expected_uri_without_siteUrlPrefix_and_with_NameValueCollection()
        {
            _controllerTester
                .Action(x => x.HttpActionResultActionWithoutArguments)
                .WhenRequestHasQueryStringParameters(new NameValueCollection { { "parm1", "value1" }, { "parm2", "2" } });

            _controller.Request.RequestUri.ToString().Should().Be("http://localhost/?parm1=value1&parm2=2");
        }

        [TestMethod]
        public void TestBadRequestErrorMessageResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new BadRequestErrorMessageResult("message", _controller);

            BadRequestErrorMessageResult result = null;
            BadRequestErrorMessageResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestBadRequestErrorMessageResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.BadRequestErrorMessageResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestBadRequestErrorMessageResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new BadRequestErrorMessageResult("message", _controller);

            BadRequestErrorMessageResult result = null;
            BadRequestErrorMessageResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestBadRequestErrorMessageResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.BadRequestErrorMessageResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestBadRequestResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new BadRequestResult(_controller);

            BadRequestResult result = null;
            BadRequestResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestBadRequestResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.BadRequestResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestBadRequestResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new BadRequestResult(_controller);

            BadRequestResult result = null;
            BadRequestResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestBadRequestResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.BadRequestResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestConflictResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new ConflictResult(_controller);

            ConflictResult result = null;
            ConflictResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestConflictResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.ConflictResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestConflictResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new ConflictResult(_controller);

            ConflictResult result = null;
            ConflictResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestConflictResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.ConflictResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestExceptionResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new ExceptionResult(new Exception(), _controller);

            ExceptionResult result = null;
            ExceptionResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestExceptionResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.ExceptionResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestExceptionResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new ExceptionResult(new Exception(), _controller);

            ExceptionResult result = null;
            ExceptionResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestExceptionResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.ExceptionResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestInternalServerErrorResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new InternalServerErrorResult(_controller);

            InternalServerErrorResult result = null;
            InternalServerErrorResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestInternalServerErrorResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.InternalServerErrorResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestInternalServerErrorResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new InternalServerErrorResult(_controller);

            InternalServerErrorResult result = null;
            InternalServerErrorResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestInternalServerErrorResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.InternalServerErrorResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestInvalidModelStateResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new InvalidModelStateResult(new ModelStateDictionary(), _controller);

            InvalidModelStateResult result = null;
            InvalidModelStateResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestInvalidModelStateResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.InvalidModelStateResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestInvalidModelStateResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new InvalidModelStateResult(new ModelStateDictionary(), _controller);

            InvalidModelStateResult result = null;
            InvalidModelStateResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestInvalidModelStateResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.InvalidModelStateResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestNotFoundResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new NotFoundResult(_controller);

            NotFoundResult result = null;
            NotFoundResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestNotFoundResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.NotFoundResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestNotFoundResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new NotFoundResult(_controller);

            NotFoundResult result = null;
            NotFoundResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestNotFoundResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.NotFoundResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestOkNegotiatedContentResult_should_work_as_expected()
        {
            _controllerTester
                .Action(x => x.HttpActionResultThatReturnsOkResultWithTestClass)
                .TestOkNegotiatedContentResult<TestClass>(result =>
                {
                    result.Content.StringProp.Should().Be("testStringProp");
                });
        }

        [TestMethod]
        public void TestOkResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new OkResult(_controller);

            OkResult result = null;
            OkResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestOkResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = new NotFoundResult(_controller);
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.OkResult. Actual: System.Web.Http.Results.NotFoundResult.");
        }

        [TestMethod]
        public void TestOkResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new OkResult(_controller);

            OkResult result = null;
            OkResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestOkResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = new NotFoundResult(_controller);
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.OkResult. Actual: System.Web.Http.Results.NotFoundResult.");
        }

        [TestMethod]
        public void TestRedirectResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new RedirectResult(new Uri("http://test.com"), _controller);

            RedirectResult result = null;
            RedirectResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestRedirectResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.RedirectResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestRedirectResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new RedirectResult(new Uri("http://test.com"), _controller);

            RedirectResult result = null;
            RedirectResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestRedirectResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.RedirectResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestRedirectToRouteResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new RedirectToRouteResult("test", new Dictionary<string, object>(), _controller);

            RedirectToRouteResult result = null;
            RedirectToRouteResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestRedirectToRouteResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.RedirectToRouteResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestRedirectToRouteResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new RedirectToRouteResult("test", new Dictionary<string, object>(), _controller);

            RedirectToRouteResult result = null;
            RedirectToRouteResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestRedirectToRouteResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.RedirectToRouteResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestResponseMessageResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new ResponseMessageResult(new HttpResponseMessage());

            ResponseMessageResult result = null;
            ResponseMessageResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestResponseMessageResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.ResponseMessageResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestResponseMessageResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new ResponseMessageResult(new HttpResponseMessage());

            ResponseMessageResult result = null;
            ResponseMessageResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestResponseMessageResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.ResponseMessageResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestStatusCodeResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new StatusCodeResult(HttpStatusCode.OK, _controller);

            StatusCodeResult result = null;
            StatusCodeResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestStatusCodeResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.StatusCodeResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestStatusCodeResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new StatusCodeResult(HttpStatusCode.OK, _controller);

            StatusCodeResult result = null;
            StatusCodeResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestStatusCodeResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.StatusCodeResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestUnauthorizedResult_should_work_as_expected_with_method_with_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new UnauthorizedResult(Enumerable.Empty<AuthenticationHeaderValue>(), _controller);

            UnauthorizedResult result = null;
            UnauthorizedResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestUnauthorizedResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.UnauthorizedResult. Actual: System.Web.Http.Results.OkResult.");
        }

        [TestMethod]
        public void TestUnauthorizedResult_should_work_as_expected_with_method_without_arguments()
        {
            ApiControllerHttpActionResultActionTester actionTester = _controllerTester.Action(x => x.HttpActionResultActionWithoutArguments);
            _controller.HttpActionResult = new UnauthorizedResult(Enumerable.Empty<AuthenticationHeaderValue>(), _controller);

            UnauthorizedResult result = null;
            UnauthorizedResult validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestUnauthorizedResult(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                "Expected IHttpActionResult type System.Web.Http.Results.UnauthorizedResult. Actual: System.Web.Http.Results.OkResult.");
        }


        [TestMethod]
        public void TestJsonResult_T_should_work_as_expected()
        {
            ApiControllerHttpActionResultActionTester actionTester =
                _controllerTester.Action(x => () => x.HttpActionResultActionWithArgument("test"));

            _controller.HttpActionResult = new JsonResult<TestClass>(
                new TestClass(),
                new JsonSerializerSettings(), System.Text.Encoding.UTF8, new HttpRequestMessage());

            JsonResult<TestClass> result = null;
            JsonResult<TestClass> validatedResult = null;
            bool validateWasCalled = false;

            Action action = () =>
            {
                result = actionTester.TestJsonResult<TestClass>(r =>
                {
                    validatedResult = r;
                    validateWasCalled = true;
                });
            };

            action.Should().NotThrow();
            result.Should().Be(_controller.HttpActionResult);
            validateWasCalled.Should().BeTrue();
            validatedResult.Should().Be(_controller.HttpActionResult);

            _controller.HttpActionResult = _okResult;
            action.Should().Throw<ControllerTestException>().WithMessage(
                $"Expected IHttpActionResult type {result.GetType().FullName}. Actual: System.Web.Http.Results.OkResult.");
        }
    }
}