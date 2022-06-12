using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace SparkyTestHelpers.Moq.Fluent.UnitTests
{
    [TestClass]
    public class FluentMockCountAssertionsTests
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
        public void Mock_Should_HaveCallCount_should_throw_exception_for_invalid_combination()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrMore().OrLess().To(x => x.TestMethod());

            verifyAction.Should().Throw<InvalidOperationException>().WithMessage("Invalid OrMore / OrLess / CallCountTo combination.");
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).To(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 0 times: x => x.TestMethod");

            _instance.TestMethod();
            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 1 times: x => x.TestMethod");

            _instance.TestMethod();
            _instance.TestMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_OrMore_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrMore().To(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 0 times: x => x.TestMethod");

            _instance.TestMethod();
            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 1 times: x => x.TestMethod");

            _instance.TestMethod();
            _instance.TestMethod();
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_OrLess_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrLess().To(x => x.TestMethod());

            AssertSuccess(verifyAction);

            _instance.TestMethod();
            AssertSuccess(verifyAction);

            _instance.TestMethod();
            _instance.TestMethod();
            AssertSuccess(verifyAction);

            _instance.TestMethod();
            _instance.TestMethod();
            _instance.TestMethod();
            AssertException(verifyAction, "Expected invocation on the mock at most 2 times, but was 3 times: x => x.TestMethod");
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_with_range_should_work_as_expected_for_method()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2, 3).To(x => x.TestMethod());

            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 0 times: x => x.TestMethod");

            _instance.TestMethod();
            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 1 times: x => x.TestMethod");

            _instance.TestMethod();
            _instance.TestMethod();
            AssertSuccess(verifyAction);

            _instance.TestMethod();
            _instance.TestMethod();
            _instance.TestMethod();
            AssertSuccess(verifyAction);

            _instance.TestMethod();
            _instance.TestMethod();
            _instance.TestMethod();
            _instance.TestMethod();
            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 4 times: x => x.TestMethod");
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_should_work_as_expected_for_property_get()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).ToGet(x => x.TestProperty);

            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 0 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 1 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_OrMore_should_work_as_expected_for_property_get()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrMore().ToGet(x => x.TestProperty);

            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 0 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 1 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_OrLess_should_work_as_expected_for_property_get()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrLess().ToGet(x => x.TestProperty);

            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertException(verifyAction, "Expected invocation on the mock at most 2 times, but was 3 times: x => x.TestProperty");
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_with_range_should_work_as_expected_for_property_get()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2, 3).ToGet(x => x.TestProperty);

            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 0 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 1 times: x => x.TestProperty");

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertSuccess(verifyAction);

            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            _test = _instance.TestProperty;
            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 4 times: x => x.TestProperty");
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_should_work_as_expected_for_property_set()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).ToSet(x => x.TestProperty = Any.String);

            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 0 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            AssertException(verifyAction, "Expected invocation on the mock exactly 2 times, but was 1 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_OrMore_should_work_as_expected_for_property_set()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrMore().ToSet(x => x.TestProperty = Any.String);

            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 0 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            AssertException(verifyAction, "Expected invocation on the mock at least 2 times, but was 1 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_OrLess_should_work_as_expected_for_property_set()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2).OrLess().ToSet(x => x.TestProperty = Any.String);

            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertException(verifyAction, "Expected invocation on the mock at most 2 times, but was 3 times: x => x.TestProperty");
        }

        [TestMethod]
        public void Mock_Should_HaveCallCount_with_range_should_work_as_expected_for_property_set()
        {
            Action verifyAction = () => _mock.Should().HaveCallCount(2, 3).ToSet(x => x.TestProperty = Any.String);

            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 0 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 1 times: x => x.TestProperty");

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            _instance.TestProperty = "test";
            AssertException(verifyAction, "Expected invocation on the mock between 2 and 3 times (Inclusive), but was 4 times: x => x.TestProperty");
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