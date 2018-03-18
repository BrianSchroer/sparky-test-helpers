using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetCore.Controllers;
using SparkyTestHelpers.AspNetCore.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;

namespace SparkyTestHelpers.AspNetCore.UnitTests.Controllers
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
        public void ControllerActionTester_TestResult_should_throw_exception_when_action_returns_null()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected IActionResult type: {typeof(ViewResult).FullName}. Actual: null.")
                .WhenExecuting(() => _controllerTester.Action(x => x.ReturnNull).TestResult<ViewResult>());
        }

        [TestMethod]
        public void ControllerActionTester_TestResult_should_throw_exception_when_action_returns_unexpected_type()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected IActionResult type: {typeof(ViewResult).FullName}. Actual: {typeof(ContentResult).FullName}.")
                .WhenExecuting(() => _controllerTester.Action(x => x.ReturnContentResult).TestResult<ViewResult>());
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_not_throw_exception_for_expected_ActionName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToActionResult result =
                    _controllerTester.Action(x => x.RedirectToAction).TestRedirectToAction("DifferentAction");
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
                    _controllerTester.Action(x => x.RedirectToAction).TestRedirectToAction("ExpectedAction"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_not_throw_exception_for_expected_route()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToRouteResult result =
                    _controllerTester.Action(x => x.RedirectToRoute).TestRedirectToRoute("Home/Index/1");
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
                    _controllerTester.Action(x => x.RedirectToRoute).TestRedirectToRoute("X/Y/3"));
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_default_View()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _controllerTester.Action(x => x.Index).TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_return_ViewResult()
        {
            ViewResult result = _controllerTester.Action(x => x.Index).TestViewResult();
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_expected_ViewName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _controllerTester.Action(x => x.DisplayTestModelWithDifferentViewName)
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
                    _controllerTester.Action(x => x.DisplayTestModelWithDifferentViewName)
                        .ExpectingViewName("ViewName")
                        .TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_view_name_when_actual_is_none()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.Index).ExpectingViewName("expected").TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() => _controllerTester.Action(x => x.Index).ExpectingModel<TestModel>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_expected_model_type()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _controllerTester.Action(x => x.DisplayTestModel).ExpectingModel<TestModel>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_should_work_with_action_with_parameters()
        {
            _controllerTester.Action(x => () => x.DisplayTestModel2(2)).ExpectingModel<TestModel2>().TestViewResult();
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_model_type()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayTestModel).ExpectingModel<TestModel2>().TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_call_model_validate_callback_method()
        {
            var testException = new InvalidOperationException("my test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayTestModel)
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
                    _controllerTester.Action(x => x.Index)
                        .TestViewResult(result =>
                        {
                            Assert.IsNotNull(result);
                            Assert.IsInstanceOfType(result, typeof(ViewResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_should_handle_ModelState_IsValid()
        {
            ControllerActionTester actionTester = _controllerTester.Action(x => x.CheckModelState);

            ForTest.Scenarios
            (
                new { IsValid = true, Expected = "ValidAction" },
                new { IsValid = false, Expected = "InvalidAction" }
            )
            .TestEach(scenario =>
            {
                if (scenario.IsValid)
                {
                    actionTester.WhenModelStateIsValid();
                }
                else
                {
                    actionTester.WhenModelStateIsNotValid();
                }

                actionTester.TestRedirectToAction(scenario.Expected);
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

        public IActionResult DisplayTestModel2(int id)
        {
            return View(new TestModel2());
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

        public IActionResult CheckModelState()
        {
            return RedirectToAction(ModelState.IsValid ? "ValidAction" : "InvalidAction");
        }

        #endregion
    }
}
