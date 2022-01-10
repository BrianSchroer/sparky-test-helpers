using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace SparkyTestHelpers.Moq.Fluent.UnitTests
{
    [TestClass]
    public class FluentMockAssertionsTests
    {
        private Mock<IMockable> _mock;
        private IMockable _instance;
        private string _test;

        [TestInitialize]
        public void TestInitialize()
        {
            _mock = new Mock<IMockable>();
            _instance = _mock.Object;
        }

        [TestMethod]
        public void Mock_Should_should_return_FluentMockAssertions()
        {
            _mock.Should().Should().BeOfType<FluentMockAssertions<IMockable>>();
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_should_return_FluentMockCountAssertions()
        {
            _mock.Should().HaveCallCount(3).Should().BeOfType<FluentMockCountAssertions<IMockable>>();
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_should_return_FluentMockCountAssertions_for_range()
        {
            _mock.Should().HaveCallCount(3, 5).Should().BeOfType<FluentMockCountAssertions<IMockable>>();
        }

        [TestMethod]
        public void Mock_Should_HaveCalledOnly_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveCalledOnly(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock once, but was 0 times: x => x.TestMethod");

            _instance.TestMethod();
            AssertSuccess(verifyAction);

            _instance.TestMethod();
            _instance.TestFunction("test");

            _test = _instance.TestProperty;

            AssertException(verifyAction, "The following invocations on mock");
        }

        [TestMethod]
        public void Mock_Should_HaveHadNoOtherCalls_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveHadNoOtherCalls();

            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;

            AssertException(verifyAction, "The following invocations on mock");
        }

        [TestMethod]
        public void Mock_Should_HaveCallsTo_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveCallsTo(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestMethod");

            _instance.TestMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveNoCallsTo_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveNoCallsTo(x => x.TestMethod());

            AssertSuccess(verifyAction);

            _instance.TestMethod();

            AssertException(verifyAction, "Expected invocation on the mock should never have been performed, but was 1 times: x => x.TestMethod");
        }

        [TestMethod]
        public void Mock_Should_HaveOneCallTo_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveOneCallTo(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock once, but was 0 times: x => x.TestMethod");

            _instance.TestMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtLeastOneCallTo_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveAtLeastOneCallTo(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestMethod");

            _instance.TestMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtMostOneCallTo_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveAtMostOneCallTo(x => x.TestMethod());

            AssertSuccess(verifyAction);

            _instance.TestMethod();
            AssertSuccess(verifyAction);

            _instance.TestMethod();
            _instance.TestMethod();
            AssertException(verifyAction, "Expected invocation on the mock at most once, but was 2 times: x => x.TestMethod");
        }

        [TestMethod]
        public void Mock_Should_HaveCallsTo_should_work_as_expected_for_function()
        {
            Action verifyAction = () => _mock.Should().HaveCallsTo(x => x.TestFunction(Any.String));

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestFunction(Any.String)");

            _instance.TestFunction("test");
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveNoCallsTo_should_work_as_expected_for_function()
        {
            Action verifyAction = () => _mock.Should().HaveNoCallsTo(x => x.TestFunction(Any.String));

            AssertSuccess(verifyAction);

            _instance.TestFunction("test");

            AssertException(verifyAction, "Expected invocation on the mock should never have been performed, but was 1 times: x => x.TestFunction(Any.String)");
        }

        [TestMethod]
        public void Mock_Should_HaveOneCallTo_should_work_as_expected_for_function()
        {
            Action verifyAction = () => _mock.Should().HaveOneCallTo(x => x.TestFunction(Any.String));

            AssertException(verifyAction, "Expected invocation on the mock once, but was 0 times: x => x.TestFunction(Any.String)");

            _instance.TestFunction("test");
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtLeastOneCallTo_should_work_as_expected_for_function()
        {
            Action verifyAction = () => _mock.Should().HaveAtLeastOneCallTo(x => x.TestFunction(Any.String));

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestFunction(Any.String)");

            _instance.TestFunction("test");
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtMostOneCallTo_should_work_as_expected_for_function()
        {
            Action verifyAction = () => _mock.Should().HaveAtMostOneCallTo(x => x.TestFunction(Any.String));

            AssertSuccess(verifyAction);

            _instance.TestFunction("test");
            AssertSuccess(verifyAction);

            _instance.TestFunction("test");
            _instance.TestFunction("test");
            AssertException(verifyAction, "Expected invocation on the mock at most once, but was 2 times: x => x.TestFunction(Any.String)");
        }

        [TestMethod]
        public void Mock_Should_HaveCallsToGet_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveCallsToGet(x => x.TestProperty);

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestProperty");

            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveNoCallsToGet_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveNoCallsToGet(x => x.TestProperty);

            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;

            AssertException(verifyAction, "Expected invocation on the mock should never have been performed, but was 1 times: x => x.TestProperty");
        }

        [TestMethod]
        public void Mock_Should_HaveOneCallToGet_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveOneCallToGet(x => x.TestProperty);

            AssertException(verifyAction, "Expected invocation on the mock once, but was 0 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtLeastOneCallToGet_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveAtLeastOneCallToGet(x => x.TestProperty);

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestProperty");

            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtMostOneCallToGet_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveAtMostOneCallToGet(x => x.TestProperty);

            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertException(verifyAction, "Expected invocation on the mock at most once, but was 2 times: x => x.TestProperty");
        }

        [TestMethod]
        public void Mock_Should_HaveCallsToset_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveCallsToSet(x => x.TestProperty = Any.String);

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestProperty");

            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveNoCallsToset_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveNoCallsToSet(x => x.TestProperty = Any.String);

            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";

            AssertException(verifyAction, "Expected invocation on the mock should never have been performed, but was 1 times: x => x.TestProperty");
        }

        [TestMethod]
        public void Mock_Should_HaveOneCallToset_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveOneCallToSet(x => x.TestProperty = Any.String);

            AssertException(verifyAction, "Expected invocation on the mock once, but was 0 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtLeastOneCallToset_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveAtLeastOneCallToSet(x => x.TestProperty = Any.String);

            AssertException(verifyAction, "Expected invocation on the mock at least once, but was never performed: x => x.TestProperty");

            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveAtMostOneCallToset_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveAtMostOneCallToSet(x => x.TestProperty = Any.String);

            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertException(verifyAction, "Expected invocation on the mock at most once, but was 2 times: x => x.TestProperty");
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