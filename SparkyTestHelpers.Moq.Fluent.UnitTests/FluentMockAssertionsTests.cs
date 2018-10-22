using System;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace SparkyTestHelpers.Moq.Fluent.UnitTests
{
    [TestClass]
    public class FluentMockAssertionsTests
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
        public void Mock_Should_should_return_FluentMockAssertions()
        {
            _mock.Should().Should().BeOfType<FluentMockAssertions<IMockable>>();
        }

        [TestMethod]
        public void Mock_Should_HaveHadNoOtherCalls_should_work_as_expected()
        {
            Action verifyAction = () => _mock.Should().HaveHadNoOtherCalls();

            AssertSuccess(verifyAction);

            _instance.TestProperty = "test";

            AssertException(verifyAction, "The following invocations on mock");
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
