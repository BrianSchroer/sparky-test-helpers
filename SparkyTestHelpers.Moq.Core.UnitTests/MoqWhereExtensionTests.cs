using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SparkyTestHelpers.Exceptions;

namespace SparkyTestHelpers.Moq.Core.UnitTests
{
    /// <summary>
    /// <see cref="MoqWhereExtensionTests"/> tests.
    /// </summary>
    [TestClass]
    public partial class MoqWhereExtensionTests
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
        public void Where_extension_should_work()
        {
            _test.WithInt(5);

            AssertExceptionNotThrown.WhenExecuting(() =>
                _mock.Verify(x => x.WithInt(Any.Int.Where(i => i < 10)), Times.Once));

            AssertExceptionMessageContaining("once, but was 0 times",
                () => _mock.Verify(x => x.WithInt(Any.Int.Where(i => i > 10)), Times.Once));
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
