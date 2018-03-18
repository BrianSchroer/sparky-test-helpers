using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetCore.Controllers;
using SparkyTestHelpers.AspNetCore.UnitTests.Models;
using SparkyTestHelpers.Exceptions;

namespace SparkyTestHelpers.AspNetCore.UnitTests
{
    /// <summary>
    /// <see cref="ControllerActionTester"/> unit tests.
    /// </summary>
    [TestClass]
    public class ControllerActionTesterTests : Controller
    {
        [TestMethod]
        public void ControllerActionTester_TestViewResult_should_not_throw_exception_for_default_View()
        {
            AssertExceptionNotThrown.WhenExecuting(() => ControllerActionTester.ForAction(Index).TestViewResult());
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
        public void ControllerActionTester_TestViewResult_should_throw_exception_for_unexpected_view_name_when_actual_is_none()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <>.")
                .WhenExecuting(() => ControllerActionTester.ForAction(Index).ExpectingViewName("expected").TestViewResult());
        }

        [TestMethod]
        public void ControllerActionTester_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ControllerActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() => ControllerActionTester.ForAction(Index).ExpectingModel<TestModel>().TestViewResult());
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
        #endregion
    }
}
