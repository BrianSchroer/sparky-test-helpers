using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Core.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;

namespace SparkyTestHelpers.AspNetMvc.Core.UnitTests
{
    /// <summary>
    /// <see cref="PageModelActionTester{TPageModel}"/> unit tests.
    /// </summary>
    [TestClass]
    public class PageModelActionTesterTests : PageModel
    {
        private PageModelTester<PageModelActionTesterTests> _pageTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _pageTester = new PageModelTester<PageModelActionTesterTests>(this);
        }

        [TestMethod]
        public void PageModelTester_Test_methods_should_throw_exception_when_result_is_not_expected_IActionResult_type()
        {
            ForTest.Scenarios
            (
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestContent(), typeof(ContentResult), typeof(PageResult)),
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestFile(), typeof(FileResult), typeof(PageResult)),
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestJsonResult(), typeof(JsonResult), typeof(PageResult)),
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestRedirectToAction("action", "controller"), typeof(RedirectToActionResult), typeof(PageResult)),
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestRedirectToPage("testPageName"), typeof(RedirectToPageResult), typeof(PageResult)),
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestRedirectToRoute("x"), typeof(RedirectToRouteResult), typeof(PageResult)),
                new ActionTypeScenario(() => _pageTester.Action(x => x.OnGet).TestResult<JsonResult>(), typeof(JsonResult), typeof(PageResult))
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
        public void PageModelTester_TestContent_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester.Action(x => x.Content).TestContent(r =>
                    {
                        Assert.AreEqual("test content", r.Content);
                        throw testException;
                    }));
        }

        [TestMethod]
        public void PageModelTester_TestFile_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester.Action(x => x.FileAction).TestFile(r => 
                    {
                        Assert.AreEqual("testFileName", r.FileDownloadName);
                        throw testException;
                    }));
        }

        [TestMethod]
        public void PageModelTester_TestJsonResult_should_return_JsonResult()
        {
            JsonResult JsonResult = _pageTester.Action(x => x.Json).TestJsonResult();
            Assert.IsNotNull(JsonResult);
        }

        [TestMethod]
        public void PageModelTester_TestJsonResult_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _pageTester.Action(x => x.JsonNull).ExpectingModel<TestModel>().TestJsonResult());
        }

        [TestMethod]
        public void PageModelTester_TestJsonResult_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _pageTester.Action(x => x.Json).ExpectingModel<TestModel2>().TestJsonResult());
        }

        [TestMethod]
        public void PageModelTester_TestJsonResult_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.Json)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestJsonResult());
        }

        [TestMethod]
        public void PageModelTester_TestJsonResult_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.Json)
                        .TestJsonResult(r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(JsonResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToAction_should_throw_exception_for_unexpected_action()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage("Expected <{\"ActionName\":\"WrongAction\",\"ControllerName\":\"WrongController\",\"RouteValues\":null}>."
                    + " Actual: <{\"ActionName\":\"ActionName\",\"ControllerName\":\"ControllerName\",\"RouteValues\":null}>.")
                .WhenExecuting(() => 
                    _pageTester.Action(x => x.RedirectToActionAction)
                    .TestRedirectToAction("WrongAction", "WrongController"));
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToAction_should_not_throw_exception_for_expected_action()
        {
            AssertExceptionNotThrown.WhenExecuting(() => 
            {
                RedirectToActionResult result = 
                    _pageTester.Action(x => x.RedirectToActionAction)
                        .TestRedirectToAction("ActionName", "ControllerName");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToAction_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.RedirectToActionAction)
                        .TestRedirectToAction("ActionName", "ControllerName", null, r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToActionResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToRoute_should_throw_exception_for_unexpected_route()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage("Expected route <wrong>. Actual: <Foo/Details/3>.")
                .WhenExecuting(() => _pageTester.Action(x => x.RedirectToRouteAction).TestRedirectToRoute("wrong"));
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToRoute_should_not_throw_exception_for_expected_route()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToRouteResult result =
                    _pageTester.Action(x => x.RedirectToRouteAction).TestRedirectToRoute("Foo/Details/3");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToRoute_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.RedirectToRouteAction)
                        .TestRedirectToRoute("Foo/Details/3", r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToRouteResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ControllerPageTester_TestRedirectToPage_should_throw_exception_for_unexpected_PageName()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage("Expected PageName <WrongName>. Actual: <testPageName>.")
                .WhenExecuting(() => _pageTester.Action(x => x.RedirectToPageAction).TestRedirectToPage("WrongName"));
        }

        [TestMethod]
        public void ControllerPageTester_TestRedirectToPage_should_not_throw_exception_for_expected_PageName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
            {
                RedirectToPageResult result =
                    _pageTester.Action(x => x.RedirectToPageAction).TestRedirectToPage("testPageName");
                Assert.IsNotNull(result);
            });
        }

        [TestMethod]
        public void PageModelTester_TestRedirectToPage_should_call_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.RedirectToPageAction)
                        .TestRedirectToPage("testPageName", r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(RedirectToPageResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void PageModelTester_TestPage_should_return_PageResult()
        {
            PageResult PageResult = _pageTester.Action(x => x.OnGet).TestPage();
            Assert.IsNotNull(PageResult);
        }

        [TestMethod]
        public void PageModelTester_TestPage_should_not_throw_exception_for_default_Page()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _pageTester.Action(x => x.OnGet).TestPage());
        }


        [TestMethod]
        public void PageModelTester_TestPage_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.OnGet)
                        .ExpectingModel(m => 
                        {
                            throw testException;
                        })
                        .TestPage());
        }

        [TestMethod]
        public void PageModelTester_TestPage_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _pageTester
                        .Action(x => x.OnGet)
                        .TestPage(r => 
                        {
                            Assert.IsInstanceOfType(r, typeof(PageResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void PageModelTester_WhenModelStateIsValidEquals_should_work_properly()
        {
            ForTest.Scenarios
            (
                new { IsValid = true, Expected = "ValidAction" },
                new { IsValid = false, Expected = "InvalidAction" }
            )
            .TestEach(scenario =>
            {
                _pageTester
                    .Action(x => () => x.ModelValidationAction(3))
                    .WhenModelStateIsValidEquals(scenario.IsValid)
                    .TestRedirectToAction(scenario.Expected);
            });
        }

 #region Controller actions
        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult Content()
        {
            return Content("test content");
        }

        public IActionResult FileAction()
        {
            return File(new byte[0], "application/x-msdownload", "testFileName");
        }

        public IActionResult Json()
        {
            return new JsonResult(new TestModel());
        }

        public IActionResult JsonNull()
        {
            return new JsonResult(null);
        }

        public IActionResult ModelValidationAction(int id)
        {
            return (ModelState.IsValid)
                ? RedirectToAction("ValidAction")
                : RedirectToAction("InvalidAction");
        }

        public IActionResult RedirectToActionAction()
        {
            return RedirectToAction("ActionName", "ControllerName");
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
