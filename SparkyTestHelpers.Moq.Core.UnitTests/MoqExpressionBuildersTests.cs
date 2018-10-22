using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SparkyTestHelpers.Exceptions;
using System;

namespace SparkyTestHelpers.Moq.Core.UnitTests
{
    /// <summary>
    /// <see cref="MoqExpressionBuilders"/> tests.
    /// </summary>
    [TestClass]
    public class MoqExpressionBuildersTests
    {
        private Mock<IMockable> _mock;
        private IMockable _test;

        [TestInitialize]
        public void TestInitialize()
        {
            _mock = new Mock<IMockable>();
            _test = _mock.Object;
        }

        [TestMethod]
        public void Expression_for_function_should_work_with_Setup_and_Verify()
        {
            var exp = _mock.Expression(x => x.WithResponse(Any.Int));

            _mock.Setup(exp).Returns("response");

            string actual = _test.WithResponse(1);
            Assert.AreEqual("response", actual);

            _mock.Verify(exp, Times.Once);

            AssertExceptionMessageContaining("should never have been performed, but was 1 times",
                () => _mock.Verify(exp, Times.Never));
        }

        private void AssertExceptionMessageContaining(string subString, Action action)
        {
            AssertExceptionThrown
                .OfType<MockException>()
                .WithMessageContaining(subString)
                .WhenExecuting(action);
        }
    }
}
