using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Moq;

namespace SparkyTestHelpers.UnitTests.Moq
{
    /// <summary>
    /// <see cref="MoqExtensions"/> tests.
    /// </summary>
    [TestClass]
    public partial class MoqExtensionsTests
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
        public void VerifyCallCount_should_work_with_no_parms()
        {
            _test.WithNoParms();
            _test.WithNoParms();
            _test.WithNoParms();

            _mock.VerifyCallCount(3, x => x.WithNoParms());
        }
    }
}
