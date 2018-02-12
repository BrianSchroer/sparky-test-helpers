using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Scenarios;
using SparkyTestHelpers.Moq;

namespace SparkyTestHelpers.UnitTests.Moq
{
    /// <summary>
    /// <see cref="MoqExtensions"/> tests.
    /// </summary>
    [TestClass]
    public partial class AnyTests
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
        public void Any_Boolean_should_work()
        {
            ForTest.Scenarios(true, false)
                .TestEach(scenario => 
                {
                    _mock.ResetCalls();
                    _test.WithBoolean(scenario);
                    _mock.VerifyOneCallTo(x => x.WithBoolean(Any.Boolean));
                });
        }
    }
}
