using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Common;
using SparkyTestHelpers.AspNetMvc.Core.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;

namespace SparkyTestHelpers.AspNetMvc.Core.UnitTests
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
            _controllerTester = new ControllerTester<ControllerActionTesterTests>(this);
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToActionResult_should_throw_exception_when_result_is_not_RedirectToActionResult()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage(
                    $"Expected IActionResult type {typeof(RedirectToActionResult).FullName}. Actual: {typeof(ViewResult).FullName}.")
                .WhenExecuting(() => _controllerTester.Action(x => x.Index).TestRedirectToActionResult("ActionName"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToActionResult_should_throw_exception_for_unexpected_ActionName()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected ActionName <WrongName>. Actual: <ActionName>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectToActionAction).TestRedirectToActionResult("WrongName"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToActionResult_should_not_throw_exception_for_expected_ActionName()
        {
            AssertExceptionNotThrown.WhenExecuting(() => 
            {
                RedirectToActionResult result = 
                    _controllerTester.Action(x => x.RedirectToActionAction).TestRedirectToActionResult("ActionName");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRouteResult_should_throw_exception_when_result_is_not_RedirectToRouteResult()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage(
                    $"Expected IActionResult type {typeof(RedirectToRouteResult).FullName}. Actual: {typeof(ViewResult).FullName}.")
                .WhenExecuting(() => _controllerTester.Action(x => x.Index).TestRedirectToRouteResult("Foo/Details/3"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRouteResult_should_throw_exception_for_unexpected_route()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected route <wrong>. Actual: <Foo/Details/3>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectToRouteAction).TestRedirectToRouteResult("wrong"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToActionResult_should_not_throw_exception_for_expected_route()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToRouteResult result =
                    _controllerTester.Action(x => x.RedirectToRouteAction).TestRedirectToRouteResult("Foo/Details/3");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_return_ViewResult()
        {
            ViewResult viewResult = _controllerTester.Action(x => x.Index).TestViewResult();
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_default_View()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _controllerTester.Action(x => x.Index).TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_when_result_is_not_ViewResult()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage(
                    $"Expected IActionResult type {typeof(ViewResult).FullName}. Actual: {typeof(RedirectToActionResult).FullName}.")
                .WhenExecuting(() => _controllerTester.Action(x => () => x.ModelValidationAction(666)).TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_expected_ViewName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _controllerTester.Action(x => x.DisplayDifferentView)
                    .ExpectingViewName("DifferentViewName").TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_ViewName()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <DifferentViewName>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayDifferentView)
                        .ExpectingViewName("expected").TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_view_name_when_actual_is_none()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <>.")
                .WhenExecuting(() => 
                    _controllerTester.Action(x => x.Index).ExpectingViewName("expected").TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() => 
                    _controllerTester.Action(x => x.Index).ExpectingModel<TestModel>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ControllerTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() => 
                    _controllerTester.Action(x => x.DisplayTestModel).ExpectingModel<TestModel2>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_call_model_validate_method()
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
                        .TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.DisplayTestModel)
                        .TestViewResult(r => 
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
                    .TestRedirectToActionResult(scenario.Expected);
            });
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

        public IActionResult DisplayDifferentView()
        {
            return View("DifferentViewName", new TestModel());
        }

        public IActionResult ModelValidationAction(int id)
        {
            return (ModelState.IsValid)
                ? RedirectToAction("ValidAction")
                : RedirectToAction("InvalidAction");
        }

        public IActionResult RedirectToActionAction()
        {
            return RedirectToAction("ActionName");
        }

        public IActionResult RedirectToRouteAction()
        {
            return RedirectToRoute(new { controller = "Foo", action = "Details", id = 3 });
        }

        #endregion
    }
}
