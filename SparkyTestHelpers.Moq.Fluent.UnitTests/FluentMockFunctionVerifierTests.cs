using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SparkyTestHelpers.Moq.Fluent.UnitTests
{
    [TestClass]
    public class FluentMockFunctionVerifierTests
    {
        private Mock<IMockable> _mock;
        private IMockable _instance;

        [TestInitialize]
        public void TestInitialize()
        {
            _mock = new Mock<IMockable>();
            _instance = _mock.Object;
        }

        [TestMethod]
        public void FluentMockFunctionVerifier_Expression_should_be_usable_for_Setup_and_Verify()
        {
            var expression = _mock.Method(x => x.TestFunction(Any.String)).Expression;

            _mock.Setup(expression).Returns(true);

            _mock.Object.TestFunction("test").Should().BeTrue();

            _mock.Method(expression).Should().HaveBeenCalledOnce();
            _mock.Should().HaveOneCallTo(expression);
        }

        [TestMethod]
        public void Mock_Method_for_function_with_func_expression_should_return_FluentMockFunctionVerifier()
        {
            _mock.Method(x => x.TestFunction(Any.String)).Should().BeOfType<FluentMockFunctionVerifier<IMockable, bool>>();
        }

        [TestMethod]
        public void FluentMockFunctionVerifier_Should_should_return_FluentMockVerifierAssertions()
        {
            _mock.Method(x => x.TestFunction(Any.String)).Should().Should().BeOfType<FluentMockVerifierAssertions<IMockable>>();
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveBeenCalled_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveBeenCalled();

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveBeenCalledOnce_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveBeenCalledOnce();

            AssertException(verifyAction, "Expected invocation on the mock once, but was 0 times: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveBeenCalledAtLeastOnce_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveBeenCalledAtLeastOnce();

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveBeenCalledAtMostOnce_should_work_as_expected()
        {
            InvokeMethod(2);
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveBeenCalledAtMostOnce();

            AssertException(verifyAction, "Expected invocation on the mock at most once, but was 2 times: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveCallCount_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveCallCount(2);

            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 0 times: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 1 times: x => x.TestFunction(Any.String)");

            InvokeMethod(2);
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveCallCountBetween_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveCallCountBetween(1, 3);

            AssertException(verifyAction, "Expected invocation on the mock between 1 and 3 times (Inclusive), but was 0 times: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertSuccess(verifyAction);

            InvokeMethod(2);
            AssertSuccess(verifyAction);

            InvokeMethod(3);
            AssertSuccess(verifyAction);

            InvokeMethod(4);
            AssertException(verifyAction, "Expected invocation on the mock between 1 and 3 times (Inclusive), but was 4 times: x => x.TestFunction(Any.String)");
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveCallCountOfAtLeast_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveCallCountOfAtLeast(2);

            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 0 times: x => x.TestFunction(Any.String)");

            InvokeMethod();
            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 1 times: x => x.TestFunction(Any.String)");

            InvokeMethod(2);
            AssertSuccess(verifyAction);

            InvokeMethod(4);
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_HaveCallCountOfAtMost_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().HaveCallCountOfAtMost(2);

            AssertSuccess(verifyAction);

            InvokeMethod();
            AssertSuccess(verifyAction);

            InvokeMethod(2);
            AssertSuccess(verifyAction);

            InvokeMethod(3);
            AssertException(verifyAction, "Expected invocation on the mock at most 2 times, but was 3 times: x => x.TestFunction(Any.String)");
        }

        [TestMethod]
        public void Mock_Method_for_function_Should_NotHaveBeenCalled_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Method(x => x.TestFunction(Any.String)).Should().NotHaveBeenCalled();

            AssertSuccess(verifyAction);

            InvokeMethod();
            AssertException(verifyAction, "Expected invocation on the mock should never have been performed, but was 1 times: x => x.TestFunction(Any.String)");
        }

        private void InvokeMethod(int count = 1)
        {
            for (int i = 0; i < count; i++)
            {
                _instance.TestFunction("test");
            }
        }

        private void AssertSuccess(Action verifyAction)
        {
            verifyAction.Should().NotThrow();
            _mock.Invocations.Clear();
        }

        private void AssertException(Action verifyAction, string expectedMessage)
        {
            verifyAction.Should().Throw<MockException>().Where(ex => ex.Message.Contains(expectedMessage));
            _mock.Invocations.Clear();
        }
    }
}
