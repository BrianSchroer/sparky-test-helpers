using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SparkyTestHelpers.Exceptions;

namespace SparkyTestHelpers.Moq.Core.UnitTests
{
    /// <summary>
    /// <see cref="MoqArgumentExtensions"/> tests.
    /// </summary>
    [TestClass]
    public class MoqArgumentExtensionsTests
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
        public void Where_extension_with_action_should_work()
        {
            _test.WithInt(7);

            AssertExceptionNotThrown.WhenExecuting(() =>
                _mock.Verify(x => x.WithInt(It.Is<int>(i => i > 6)), Times.Once));

            AssertExceptionNotThrown.WhenExecuting(() =>
                _mock.Verify(x => x.WithInt(Any.Int.Where(i => Assert.IsTrue(i > 6))), Times.Once));

            AssertExceptionThrown
                .OfType<AssertFailedException>()
                .WithMessageContaining("Assert.AreEqual failed. Expected:<3>. Actual:<7>")
                .WhenExecuting(() =>_mock.Verify(x => x.WithInt(Any.Int.Where(i => Assert.AreEqual(3, i))), Times.Once));

            AssertExceptionThrown
                .OfType<MockException>()
                .WithMessageContaining("once, but was 0 times")
                .WhenExecuting(() => _mock.Verify(x => x.WithString(Any.String.Where(s => s.Contains("x"))), Times.Once));
        }

        [TestMethod]
        public void Where_extension_with_expression_should_work()
        {
            _test.WithInt(5);

            AssertExceptionNotThrown.WhenExecuting(() =>
                _mock.Verify(x => x.WithInt(Any.Int.Where(i => i < 10)), Times.Once));

            AssertExceptionThrown
                .OfType<MockException>()
                .WithMessageContaining("once, but was 0 times")
                .WhenExecuting(() => _mock.Verify(x => x.WithInt(Any.Int.Where(i => i > 10)), Times.Once));
        }
    }
}
