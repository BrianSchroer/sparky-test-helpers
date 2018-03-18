using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetCore.Controllers;
using SparkyTestHelpers.AspNetCore.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using System;

namespace SparkyTestHelpers.AspNetCore.UnitTests
{
    /// <summary>
    /// <see cref="ControllerActionTester"/> unit tests.
    /// </summary>
    [TestClass]
    public class ControllerActionTesterTests : Controller
    {
        [TestMethod]
        public void ControllerActionTester_TestResult_should_throw_exception_when_action_returns_null()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected IActionResult type: {typeof(ViewResult).FullName}. Actual: null.")
                .WhenExecuting(() => ControllerActionTester.ForAction(ReturnNull).TestResult<ViewResult>());
        }

        [TestMethod]
        public void ControllerActionTester_TestResult_should_throw_exception_when_action_returns_unexpected_type()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected IActionResult type: {typeof(ViewResult).FullName}. Actual: {typeof(ContentResult).FullName}.")
                .WhenExecuting(() => ControllerActionTester.ForAction(ReturnContentResult).TestResult<ViewResult>());
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_not_throw_exception_for_expected_ActionName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToActionResult result =
                    ControllerActionTester.ForAction(RedirectToAction).TestRedirectToAction("DifferentAction");
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectToActionResult));
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_throw_exception_for_unexpected_ActionName()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage("Expected ActionName <ExpectedAction>. Actual: <DifferentAction>.")
                .WhenExecuting(() =>
                    ControllerActionTester.ForAction(RedirectToAction).TestRedirectToAction("ExpectedAction"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_not_throw_exception_for_expected_route()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToRouteResult result =
                    ControllerActionTester.ForAction(RedirectToRoute).TestRedirectToRoute("Home/Index/1");
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(RedirectToRouteResult));
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_not_throw_exception_for_unexpected_route()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage("Expected route <X/Y/3>. Actual: <Home/Index/1>.")
                .WhenExecuting(() =>
                    ControllerActionTester.ForAction(RedirectToRoute).TestRedirectToRoute("X/Y/3"));
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_default_View()
        {
            AssertExceptionNotThrown.WhenExecuting(() => ControllerActionTester.ForAction(Index).TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_return_ViewResult()
        {
            ViewResult result = ControllerActionTester.ForAction(Index).TestViewResult();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_expected_ViewName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                ControllerActionTester
                    .ForAction(DisplayTestModelWithDifferentViewName)
                    .ExpectingViewName("DifferentViewName")
                        .TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_ViewName()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage("Expected ViewName <ViewName>. Actual: <DifferentViewName>.")
                .WhenExecuting(() =>
                    ControllerActionTester
                        .ForAction(DisplayTestModelWithDifferentViewName)
                        .ExpectingViewName("ViewName")
                        .TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_view_name_when_actual_is_none()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <>.")
                .WhenExecuting(() => ControllerActionTester.ForAction(Index).ExpectingViewName("expected").TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() => ControllerActionTester.ForAction(Index).ExpectingModel<TestModel>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_expected_model_type()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                ControllerActionTester.ForAction(DisplayTestModel).ExpectingModel<TestModel>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_model_type()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    ControllerActionTester.ForAction(DisplayTestModel).ExpectingModel<TestModel2>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_call_model_validate_callback_method()
        {
            var testException = new InvalidOperationException("my test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    ControllerActionTester
                        .ForAction(DisplayTestModel)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsNotNull(m);
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_call_validate_callback_method()
        {
            var testException = new InvalidOperationException("my test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    ControllerActionTester
                        .ForAction(Index)
                        .TestViewResult(result =>
                        {
                            Assert.IsNotNull(result);
                            Assert.IsInstanceOfType(result, typeof(ViewResult));
                            throw testException;
                        }));
        }

        #region Controller actions
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult DisplayTestModel()
        {
            return View(new TestModel());
        }

        public IActionResult DisplayTestModelWithDifferentViewName()
        {
            return View("DifferentViewName", new TestModel());
        }

        public IActionResult RedirectToAction()
        {
            return RedirectToAction("DifferentAction");
        }

        public IActionResult RedirectToRoute()
        {
            return RedirectToRoute(new
            {
                controller = "Home",
                action = "Index",
                id = 1
            });
        }

        public IActionResult ReturnNull()
        {
            return null;
        }

        public IActionResult ReturnContentResult()
        {
            return new ContentResult();
        }

        #endregion
    }
}
