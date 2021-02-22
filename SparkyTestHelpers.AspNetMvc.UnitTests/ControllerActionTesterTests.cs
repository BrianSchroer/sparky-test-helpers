using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SparkyTestHelpers.AspNetMvc.UnitTests
{
    /// <summary>
    /// <see cref="ControllerActionTester"/> unit tests.
    /// </summary>
    [TestClass]
    public class ControllerActionTesterTests : Controller
    {
        private ControllerTester<ControllerActionTesterTests> _controllerTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _controllerTester = this.CreateTester();
        }

        [TestMethod]
        public void ControllerActionTester_Test_methods_should_throw_exception_when_result_is_not_expected_ActionResult_type()
        {
            ForTest.Scenarios
            (
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestContent(), typeof(ContentResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestEmpty(), typeof(EmptyResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestFile(), typeof(FileResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestJson(), typeof(JsonResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestPartialView(), typeof(PartialViewResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestRedirectToAction("x"), typeof(RedirectToRouteResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestRedirect("testUrl"), typeof(RedirectResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestRedirectToRoute("x"), typeof(RedirectToRouteResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestResult<JsonResult>(), typeof(JsonResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Json).TestView(), typeof(ViewResult), typeof(JsonResult))
            )
            .TestEach(scenario =>
            {
                AssertExceptionThrown
                    .OfType<ControllerTestException>()
                    .WithMessage($"Expected ActionResult type {scenario.ExpectedTypeName}. Actual: {scenario.ActualTypeName}.")
                    .WhenExecuting(() => scenario.TestAction());
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestContent_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Content).TestContent(r =>
                    {
                        Assert.AreEqual("test content", r.Content);
                        throw testException;
                    }));
        }

        [TestMethod]
        public void ControllerActionTester_TestEmpty_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Empty).TestEmpty(r => throw testException));
        }

        [TestMethod]
        public void ControllerActionTester_TestFile_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.FileAction).TestFile(r =>
                    {
                        Assert.AreEqual("testFileName", r.FileDownloadName);
                        throw testException;
                    }));
        }

        [TestMethod]
        public void ControllerActionTester_TestJson_should_return_JsonResult()
        {
            JsonResult JsonResult = _controllerTester.Action(x => x.Json).TestJson();
            Assert.IsNotNull(JsonResult);
        }

        [TestMethod]
        public void ControllerActionTester_TestJson_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.JsonNull).ExpectingModel<TestModel>().TestJson());
        }

        [TestMethod]
        public void ControllerActionTester_TestJson_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Json).ExpectingModel<TestModel2>().TestJson());
        }

        [TestMethod]
        public void ControllerActionTester_TestJson_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.Json)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestJson());
        }

        [TestMethod]
        public void ControllerActionTester_TestJson_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.Json)
                        .TestJson(r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(JsonResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_return_PartialViewResult()
        {
            PartialViewResult PartialViewResult = _controllerTester.Action(x => x.PartialViewAction).TestPartialView();
            Assert.IsNotNull(PartialViewResult);
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_not_throw_exception_for_expected_PartialViewName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _controllerTester.Action(x => x.DisplayDifferentPartialView)
                    .ExpectingViewName("DifferentPartialViewName").TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_throw_exception_for_unexpected_PartialViewName()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <DifferentPartialViewName>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayDifferentPartialView)
                        .ExpectingViewName("expected").TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.PartialViewWithNullModel).ExpectingModel<TestModel>().TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.PartialViewAction).ExpectingModel<TestModel2>().TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.PartialViewAction)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.PartialViewAction)
                        .TestPartialView(r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(PartialViewResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_throw_exception_for_unexpected_ActionName()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected action name <WrongName>. Actual: <ActionName>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectToActionAction).TestRedirectToAction("WrongName"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_not_throw_exception_for_expected_ActionName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToRouteResult result =
                    _controllerTester.Action(x => x.RedirectToActionAction).TestRedirectToAction("ActionName");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.RedirectToActionAction)
                        .TestRedirectToAction("ActionName", r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToRouteResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_throw_exception_for_unexpected_route()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected route <wrong>. Actual: <Foo/Details/3>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectToRouteAction).TestRedirectToRoute("wrong"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_not_throw_exception_for_expected_route()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToRouteResult result =
                    _controllerTester.Action(x => x.RedirectToRouteAction).TestRedirectToRoute("Foo/Details/3");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.RedirectToRouteAction)
                        .TestRedirectToRoute("Foo/Details/3", r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToRouteResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerPageTester_TestRedirect_should_throw_exception_for_unexpected_URL()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected Url <WrongName>. Actual: <testUrl>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectAction).TestRedirect("WrongName"));
        }

        [TestMethod]
        public void ControllerPageTester_TestRedirect_should_not_throw_exception_for_expected_URL()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectResult result =
                    _controllerTester.Action(x => x.RedirectAction).TestRedirect("testUrl");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirect_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.RedirectAction)
                        .TestRedirect("testUrl", r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_return_ViewResult()
        {
            ViewResult viewResult = _controllerTester.Action(x => x.Index).TestView();
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_not_throw_exception_for_default_View()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _controllerTester.Action(x => x.Index).TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_not_throw_exception_for_expected_ViewName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _controllerTester.Action(x => x.DisplayDifferentView)
                    .ExpectingViewName("DifferentViewName").TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_for_unexpected_ViewName()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <DifferentViewName>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayDifferentView)
                        .ExpectingViewName("expected").TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_for_unexpected_view_name_when_actual_is_none()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Index).ExpectingViewName("expected").TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Index).ExpectingModel<TestModel>().TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayTestModel).ExpectingModel<TestModel2>().TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.DisplayTestModel)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.DisplayTestModel)
                        .TestView(r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(ViewResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_WhenModelStateIsValidEquals_should_work_properly()
        {
            ForTest.Scenarios
            (
                new { IsValid = true, Expected = "ValidAction" },
                new { IsValid = false, Expected = "InvalidAction" }
            )
            .TestEach(scenario =>
            {
                _controllerTester
                    .Action(x => () => x.ModelValidationAction(3))
                    .WhenModelStateIsValidEquals(scenario.IsValid)
                    .TestRedirectToAction(scenario.Expected);
            });
        }

        [TestMethod]
        public void ControllerActionTester_using_async_constructor_should_work_properly()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _controllerTester.Action(x => x.AsyncAction)
                .TestView());
        }

        [TestMethod]
        public void GenericControllerActionTester_WhenModelStateIsValidEquals_method_should_work_as_expected()
        {
            ForTest.Scenarios(true, false)
            .TestEach(isValid =>
            {
                bool result = _controllerTester.Action<bool>(x => x.BoolResultActionThatChecksModelState)
                    .WhenModelStateIsValidEquals(isValid)
                    .Test(response => response.Should().Be(isValid));

                result.Should().Be(isValid);
            });
        }

        [TestMethod]
        public void GenericControllerActionTester_Test_action_without_parameters_should_work_as_expected()
        {
            var result = _controllerTester.Action<string>(x => x.StringResultActionWithoutArguments)
                .Test(response => response.Should().Be("test"));

            result.Should().Be("test");
        }

        [TestMethod]
        public void GenericControllerActionTester_Test_action_with_parameters_should_work_as_expected()
        {
            var result = _controllerTester.Action<string>(x => () => x.StringResultActionWithArgument("yo"))
                .Test(response => response.Should().Be("yo"));

            result.Should().Be("yo");
        }

        #region Controller actions
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> AsyncAction()
        {
            return View();
        }

        public ActionResult Content()
        {
            return Content("test content");
        }

        public ActionResult DisplayTestModel()
        {
            return View(new TestModel());
        }

        public ActionResult DisplayDifferentView()
        {
            return View("DifferentViewName", new TestModel());
        }

        public ActionResult DisplayDifferentPartialView()
        {
            return PartialView("DifferentPartialViewName", new TestModel());
        }

        public ActionResult Empty()
        {
            return new EmptyResult();
        }

        public ActionResult FileAction()
        {
            return File(new byte[0], "application/x-msdownload", "testFileName");
        }

        public ActionResult Json()
        {
            return Json(new TestModel());
        }

        public ActionResult JsonNull()
        {
            return Json(null);
        }

        public ActionResult ModelValidationAction(int id)
        {
            return (ModelState.IsValid)
                ? RedirectToAction("ValidAction")
                : RedirectToAction("InvalidAction");
        }

        public ActionResult PartialViewAction()
        {
            return PartialView("PartialViewName", new TestModel());
        }

        public ActionResult PartialViewWithNullModel()
        {
            return PartialView("PartialViewName");
        }

        public ActionResult RedirectAction()
        {
            return Redirect("testUrl");
        }

        public ActionResult RedirectToActionAction()
        {
            return RedirectToAction("ActionName");
        }

        public ActionResult RedirectToRouteAction()
        {
            return RedirectToRoute(new { controller = "Foo", action = "Details", id = 3 });
        }
        public string StringResultActionWithoutArguments()
        {
            return "test";
        }

        public string StringResultActionWithArgument(string input)
        {
            return input;
        }

        public bool BoolResultActionThatChecksModelState() => ModelState.IsValid;

        #endregion

        private class ActionTypeScenario
        {
            public string ActualTypeName { get; set; }
            public string ExpectedTypeName { get; set; }
            public Action TestAction { get; set; }

            public ActionTypeScenario(
                Action testAction,
                Type expectedType = null,
                Type actualType = null)
            {
                ExpectedTypeName = expectedType?.FullName;
                ActualTypeName = actualType?.FullName;
                TestAction = testAction;
            }
        }
    }
}

