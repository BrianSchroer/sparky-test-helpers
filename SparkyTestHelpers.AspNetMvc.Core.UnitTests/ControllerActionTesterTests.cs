using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Core.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;
using System.Threading.Tasks;

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
        public void ControllerActionTester_Test_methods_should_throw_exception_when_result_is_not_expected_IActionResult_type()
        {
            ForTest.Scenarios
            (
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestContent(), typeof(ContentResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestEmpty(), typeof(EmptyResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestFile(), typeof(FileResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestJson(), typeof(JsonResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestOkObject(), typeof(OkObjectResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestPartialView(), typeof(PartialViewResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestRedirectToAction("x"), typeof(RedirectToActionResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestRedirectToPage("testPageName"), typeof(RedirectToPageResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestRedirectToRoute("x"), typeof(RedirectToRouteResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Index).TestResult<JsonResult>(), typeof(JsonResult), typeof(ViewResult)),
                new ActionTypeScenario(() => _controllerTester.Action(x => x.Json).TestView(), typeof(ViewResult), typeof(JsonResult))
            )
            .TestEach(scenario =>
            {
                AssertExceptionThrown
                    .OfType<ActionTestException>()
                    .WithMessage($"Expected IActionResult type {scenario.ExpectedTypeName}. Actual: {scenario.ActualTypeName}.")
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
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.JsonNull).ExpectingModel<TestModel>().TestJson());
        }

        [TestMethod]
        public void ControllerActionTester_TestJson_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
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
                .OfType<ActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <DifferentPartialViewName>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayDifferentPartialView)
                        .ExpectingViewName("expected").TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.PartialViewWithNullModel).ExpectingModel<TestModel>().TestPartialView());
        }

        [TestMethod]
        public void ControllerActionTester_TestPartialView_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
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
                .OfType<ActionTestException>()
                .WithMessage("Expected <{\"ActionName\":\"WrongName\",\"ControllerName\":null,\"RouteValues\":null}>."
                    + " Actual: <{\"ActionName\":\"ActionName\",\"ControllerName\":null,\"RouteValues\":null}>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectToActionAction).TestRedirectToAction("WrongName"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToAction_should_not_throw_exception_for_expected_ActionName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToActionResult result =
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
                        .TestRedirectToAction("ActionName", null, null, r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToActionResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToRoute_should_throw_exception_for_unexpected_route()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
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
        public void ControllerActionTester_TestRedirectToPage_should_throw_exception_for_unexpected_PageName()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage("Expected PageName <WrongName>. Actual: <testPageName>.")
                .WhenExecuting(() => _controllerTester.Action(x => x.RedirectToPageAction).TestRedirectToPage("WrongName"));
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToPage_should_not_throw_exception_for_expected_PageName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToPageResult result =
                    _controllerTester.Action(x => x.RedirectToPageAction).TestRedirectToPage("testPageName");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void ControllerActionTester_TestRedirectToPage_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.RedirectToPageAction)
                        .TestRedirectToPage("testPageName", r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToPageResult));
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
                .OfType<ActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <DifferentViewName>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.DisplayDifferentView)
                        .ExpectingViewName("expected").TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_for_unexpected_view_name_when_actual_is_none()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <>.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Index).ExpectingViewName("expected").TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.Index).ExpectingModel<TestModel>().TestView());
        }

        [TestMethod]
        public void ControllerActionTester_TestView_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
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
        public void ControllerActionTester_TestOkObject_should_return_OkObjectResult()
        {
            OkObjectResult OkObjectResult = _controllerTester.Action(x => x.OkObject).TestOkObject();
            Assert.IsNotNull(OkObjectResult);
        }

        [TestMethod]
        public void ControllerActionTester_TestOkObject_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.OkObjectNull).ExpectingModel<TestModel>().TestOkObject());
        }

        [TestMethod]
        public void ControllerActionTester_TestOkObject_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _controllerTester.Action(x => x.OkObject).ExpectingModel<TestModel2>().TestOkObject());
        }

        [TestMethod]
        public void ControllerActionTester_TestOkObject_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.OkObject)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestOkObject());
        }

        [TestMethod]
        public void ControllerActionTester_TestOkObject_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _controllerTester
                        .Action(x => x.OkObject)
                        .TestOkObject(r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(OkObjectResult));
                            throw testException;
                        }));
        }

        #region Controller actions
        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> AsyncAction()
        {
            return View();
        }

        public IActionResult Content()
        {
            return Content("test content");
        }

        public IActionResult DisplayTestModel()
        {
            return View(new TestModel());
        }

        public IActionResult DisplayDifferentView()
        {
            return View("DifferentViewName", new TestModel());
        }

        public IActionResult DisplayDifferentPartialView()
        {
            return PartialView("DifferentPartialViewName", new TestModel());
        }

        public IActionResult Empty()
        {
            return new EmptyResult();
        }

        public IActionResult FileAction()
        {
            return File(new byte[0], "application/x-msdownload", "testFileName");
        }

        public IActionResult Json()
        {
            return Json(new TestModel());
        }

        public IActionResult JsonNull()
        {
            return Json(null);
        }

        public IActionResult ModelValidationAction(int id)
        {
            return (ModelState.IsValid)
                ? RedirectToAction("ValidAction")
                : RedirectToAction("InvalidAction");
        }

        public IActionResult OkObjectNull()
        {
            return Ok(null);
        }

        public IActionResult OkObject()
        {
            return Ok(new TestModel());
        }

        public IActionResult PartialViewAction()
        {
            return PartialView("PartialViewName", new TestModel());
        }

        public IActionResult PartialViewWithNullModel()
        {
            return PartialView("PartialViewName");
        }

        public IActionResult RedirectToActionAction()
        {
            return RedirectToAction("ActionName");
        }

        public IActionResult RedirectToRouteAction()
        {
            return RedirectToRoute(new { controller = "Foo", action = "Details", id = 3 });
        }

        public IActionResult RedirectToPageAction()
        {
            return RedirectToPage("testPageName");
        }

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
