using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.AspNetMvc.Core.UnitTests.Models;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Scenarios;
using System;
using System.Threading.Tasks;

namespace SparkyTestHelpers.AspNetMvc.Core.UnitTests
{
    /// <summary>
    /// <see cref="ViewComponentTester{TViewComponent}"/> and
    /// <see cref="ViewComponentInvocationTester"/> unit tests.
    /// </summary>
    [TestClass]
    public class ViewComponentInvocationTesterTests : ViewComponent
    {
        private ViewComponentTester<ViewComponentInvocationTesterTests> _viewComponentTester;

        [TestInitialize]
        public void TestInitialize()
        {
            _viewComponentTester = new ViewComponentTester<ViewComponentInvocationTesterTests>(this);
        }

        [TestMethod]
        public void ViewComponentTester_constructor_should_check_for_valid_ViewComponent_class()
        {
            ForTest.Scenarios
            (
                new ConstructorScenario(true, 
                    () => new ViewComponentTester<ViewComponentInvocationTesterTests>(this)),
                new ConstructorScenario(true, 
                    () => new ViewComponentTester<ClassWithViewComponentAttribute>(new ClassWithViewComponentAttribute())),
                new ConstructorScenario(true, 
                    () => new ViewComponentTester<ClassWithNameEndingWithViewComponent>(new ClassWithNameEndingWithViewComponent())),
                new ConstructorScenario(false, 
                    () => new ViewComponentTester<InvalidViewComponentClass>(new InvalidViewComponentClass()))
            )
            .TestEach(scenario =>
            {
                if (scenario.IsValidViewComponent)
                {
                    AssertExceptionNotThrown.WhenExecuting(scenario.ConstructorCall);
                }
                else
                {
                    AssertExceptionThrown
                        .OfType<ActionTestException>()
                        .WithMessageStartingWith("A ViewComponent must inherit from ViewComponent, have a name ending with")
                        .WhenExecuting(scenario.ConstructorCall);
                }
            });
        }

        [TestMethod]
        public void ViewComponentTester_Invocation_should_return_ViewComponentInvocationTester()
        {
            ViewComponentInvocationTester tester = _viewComponentTester.Invocation(x => x.Invoke);
            Assert.IsNotNull(tester);
        }

        [TestMethod]
        public void ViewComponentInvocationTester_Test_methods_should_throw_exception_when_result_is_not_expected_IViewComponentResult_type()
        {
            ForTest.Scenarios
            (
                new InvocationScenario(
                    () => _viewComponentTester.Invocation(x => x.Invoke).TestResult<ContentViewComponentResult>(),
                    typeof(ContentViewComponentResult), typeof(ViewViewComponentResult)),
                new InvocationScenario(
                    () => _viewComponentTester.Invocation(x => x.Invoke).TestContent(),
                    typeof(ContentViewComponentResult), typeof(ViewViewComponentResult)),
                 new InvocationScenario(
                    () => _viewComponentTester.Invocation(x => x.Invoke).TestHtmlContent(),
                    typeof(HtmlContentViewComponentResult), typeof(ViewViewComponentResult)),
                 new InvocationScenario(
                    () => _viewComponentTester.Invocation(x => x.InvokeContent).TestView(),
                    typeof(ViewViewComponentResult), typeof(ContentViewComponentResult))
            )
            .TestEach(scenario =>
            {
                AssertExceptionThrown
                    .OfType<ActionTestException>()
                    .WithMessage($"Expected IViewComponentResult type {scenario.ExpectedTypeName}. Actual: {scenario.ActualTypeName}.")
                    .WhenExecuting(() => scenario.TestAction());
            });
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestContent_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _viewComponentTester.Invocation(x => x.InvokeContent).TestContent(r =>
                    {
                        Assert.AreEqual("test content", r.Content);
                        throw testException;
                    }));
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestHtmlContent_should_call_validate()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _viewComponentTester.Invocation(x => x.InvokeHtmlContent).TestHtmlContent(r =>
                    {
                        Assert.AreEqual("test HTML content", r.EncodedContent.ToString());
                        throw testException;
                    }));
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_return_ViewResult()
        {
            ViewViewComponentResult viewComponentResult = _viewComponentTester.Invocation(x => x.Invoke).TestView();
            Assert.IsNotNull(viewComponentResult);
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_not_throw_exception_for_expected_ViewName()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _viewComponentTester.Invocation(x => x.Invoke)
                    .ExpectingViewName("Default").TestView());
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_throw_exception_for_unexpected_ViewName()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage("Expected ViewName <expected>. Actual: <Default>.")
                .WhenExecuting(() =>
                    _viewComponentTester.Invocation(x => x.Invoke)
                        .ExpectingViewName("expected").TestView());
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_throw_exception_when_ExpectingModel_but_model_is_null()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel).FullName}. Actual: null.")
                .WhenExecuting(() =>
                    _viewComponentTester.Invocation(x => x.InvokeEmptyModel).ExpectingModel<TestModel>().TestView());
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_throw_exception_when_ExpectingModel_and_model_is_different_type()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"Expected model type: {typeof(TestModel2).FullName}. Actual: {typeof(TestModel).FullName}.")
                .WhenExecuting(() =>
                    _viewComponentTester.Invocation(x => x.Invoke).ExpectingModel<TestModel2>().TestView());
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_call_model_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _viewComponentTester
                        .Invocation(x => x.Invoke)
                        .ExpectingModel<TestModel>(m =>
                        {
                            Assert.IsInstanceOfType(m, typeof(TestModel));
                            throw testException;
                        })
                        .TestView());
        }

        [TestMethod]
        public void ViewComponentInvocationTester_TestView_should_call_result_validate_method()
        {
            var testException = new InvalidOperationException("test exception");

            AssertExceptionThrown
                .OfType<InvalidOperationException>()
                .WithMessage(testException.Message)
                .WhenExecuting(() =>
                    _viewComponentTester
                        .Invocation(x => x.Invoke)
                        .TestView(r =>
                        {
                            Assert.IsInstanceOfType(r, typeof(ViewViewComponentResult));
                            throw testException;
                        }));
        }

        [TestMethod]
        public void ViewComponentInvocationTester_should_work_with_async_constructor()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _viewComponentTester
                    .Invocation(x => x.InvokeAsync)
                    .ExpectingViewName("Default")
                    .TestView());
        }

        [TestMethod]
        public void ViewComponentInvocationTester_WhenModelStateIsValidEquals_should_work_properly()
        {
            ForTest.Scenarios
            (
                new { IsValid = true, Expected = "ValidView" },
                new { IsValid = false, Expected = "ErrorView" }
            )
            .TestEach(scenario =>
            {
                _viewComponentTester
                    .Invocation(x => () => x.InvokeWithModelValidation(3))
                    .WhenModelStateIsValidEquals(scenario.IsValid)
                    .ExpectingViewName(scenario.Expected)
                    .TestView();
            });
        }
    
        [TestMethod]
        public void ViewComponentInvocationTester_WhenModelStateIsValidEquals_should_work_via_reflection()
        {
            ForTest.Scenarios
            (
                new { IsValid = true, Expected = "ValidView" },
                new { IsValid = false, Expected = "ErrorView" }
            )
            .TestEach(scenario =>
            {
                new ViewComponentTester<ClassWithViewComponentAttribute>(new ClassWithViewComponentAttribute())
                    .Invocation(x => () => x.InvokeWithModelValidation(3))
                    .WhenModelStateIsValidEquals(scenario.IsValid)
                    .ExpectingViewName(scenario.Expected)
                    .TestView();
            });
        }

        [TestMethod]
        public void ViewComponentInvocationTester_WhenModelStateIsValidEquals_should_throw_exception_when_component_has_no_ModelState()
        {
            AssertExceptionThrown
                .OfType<ActionTestException>()
                .WithMessage($"{typeof(ClassWithNameEndingWithViewComponent).FullName} does not have a public \"ModelState\" ModelStateDictionary property.")
                .WhenExecuting(() =>
                    new ViewComponentTester<ClassWithNameEndingWithViewComponent>(new ClassWithNameEndingWithViewComponent())
                        .Invocation(x => x.Invoke)
                        .WhenModelStateIsValidEquals(true)
                        .TestView()
                );
        }

        public IViewComponentResult Invoke()
        {
            return View("Default", new TestModel());
        }

        public IViewComponentResult InvokeEmptyModel()
        {
            return View("Default");
        }

        public IViewComponentResult InvokeContent()
        {
            return Content("test content");
        }

        public IViewComponentResult InvokeHtmlContent()
        {
            return new HtmlContentViewComponentResult(new HtmlString("test HTML content"));
        }

        public IViewComponentResult InvokeWithModelValidation(int id)
        {
            string viewName = (ModelState.IsValid) ? "ValidView" : "ErrorView";

            return View(viewName, new TestModel());
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return new ViewViewComponentResult
            {
                ViewName = "Default"
            };
        }

        private class InvalidViewComponentClass { }

        [ViewComponent]
        private class ClassWithViewComponentAttribute
        {
            private readonly ModelStateDictionary _modelState = new ModelStateDictionary();

            public ModelStateDictionary ModelState => _modelState;

            public IViewComponentResult InvokeWithModelValidation(int id)
            {
                string viewName = (ModelState.IsValid) ? "ValidView" : "ErrorView";

                return new ViewViewComponentResult
                {
                    ViewName = viewName
                };
            }
        }

        private class ClassWithNameEndingWithViewComponent
        {
            public IViewComponentResult Invoke()
            {
                return new ViewViewComponentResult
                {
                    ViewName = "Default"
                };
            }
        }

        private class ConstructorScenario
        {
            public bool IsValidViewComponent { get; set; }
            public Action ConstructorCall { get; set; }

            public ConstructorScenario(bool isValidViewComponent, Action constructorCall)
            {
                IsValidViewComponent = isValidViewComponent;
                ConstructorCall = constructorCall;
            }
        }

        private class InvocationScenario
        {
            public string ActualTypeName { get; set; }
            public string ExpectedTypeName { get; set; }
            public Action TestAction { get; set; }

            public InvocationScenario(
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
