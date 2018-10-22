using Moq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Moq;
using System;
using System.Linq.Expressions;

namespace SparkyTestHelpers.Moq.UnitTests
{
    /// <summary>
    /// <see cref="MoqVerifyExtensions"/> tests.
    /// </summary>
    [TestClass]
    public partial class MoqExtensionsTests
    {
        private Mock<IMockable> _mock;
        private IMockable _test;
        Expression<Func<IMockable, string>> _funcExpression;
        Expression<Func<IMockable, string>> _propGetExpression;
        Action<IMockable> _propSetExpression;

        [TestInitialize]
        public void TestInitialize()
        {
            _mock = new Mock<IMockable>();
            _test = _mock.Object;

            _funcExpression = _mock.Expression(x => x.WithResponse(Any.Int));
            _propGetExpression = _mock.Expression(x => x.Prop);
            _propSetExpression = (IMockable m) => m.Prop = Any.String;
        }

        [TestMethod]
        public void Expression_for_function_should_work_with_Setup_and_Verify()
        {
            var exp = _mock.Expression(x => x.WithResponse(Any.Int));

            _mock.Setup(exp).Returns("response");

            string actual = _test.WithResponse(1);
            Assert.AreEqual("response", actual);

            _mock.VerifyOneCallTo(exp);

            AssertExceptionMessageContaining("should never have been performed, but was 1 times",
                () => _mock.VerifyNoCallsTo(exp));
        }

        [TestMethod]
        public void VerifyCallCount_action_should_work()
        {
            var exp = _mock.Expression(m => m.WithNoParms());

            _test.WithNoParms();
            _test.WithNoParms();
            _test.WithNoParms();

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyCallCount(3, exp));

            AssertExceptionMessageContaining("exactly 2 times, but was 3 times",
                () => _mock.VerifyCallCount(2, exp));
        }

        [TestMethod]
        public void VerifyCallCount_function_should_work()
        {
            _test.WithResponse(1);
            _test.WithResponse(2);
            _test.WithResponse(3);

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyCallCount(3, _funcExpression));

            AssertExceptionMessageContaining("exactly 2 times, but was 3 times",
                () => _mock.VerifyCallCount(2, _funcExpression));
        }

        [TestMethod]
        public void VerifyOneCallTo_action_should_work()
        {
            var exp = _mock.Expression(m => m.WithInt(2));

            _test.WithInt(2);

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyOneCallTo(exp));

            _test.WithInt(2);
            AssertExceptionMessageContaining("once, but was 2 times", () => _mock.VerifyOneCallTo(exp));
        }

        [TestMethod]
        public void VerifyOneCallTo_function_should_work()
        {
            _test.WithResponse(2);

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyOneCallTo(_funcExpression));

            _test.WithResponse(2);

            AssertExceptionMessageContaining("once, but was 2 times", () => _mock.VerifyOneCallTo(_funcExpression));
        }

        [TestMethod]
        public void VerifyAtLeastOneCallTo_action_should_work()
        {
            var exp = _mock.Expression(x => x.WithString("test"));

            AssertExceptionMessageContaining("at least once, but was never performed",
                () => _mock.VerifyAtLeastOneCallTo(exp));

            _test.WithString("test");
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneCallTo(exp));

            _test.WithString("test");
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneCallTo(exp));
        }

        [TestMethod]
        public void VerifyAtLeastOneCallTo_function_should_work()
        {
            AssertExceptionMessageContaining("at least once, but was never performed",
                () => _mock.VerifyAtLeastOneCallTo(_funcExpression));

            _test.WithResponse(15);
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneCallTo(_funcExpression));

            _test.WithResponse(20);
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneCallTo(_funcExpression));
        }

        [TestMethod]
        public void VerifyAtMostOneCallTo_action_should_work()
        {
            var exp = _mock.Expression(x => x.WithDouble(1.5));

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneCallTo(exp));

            _test.WithDouble(1.5);
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneCallTo(exp));

            _test.WithDouble(1.5);
            AssertExceptionMessageContaining("at most once, but was 2 times",
                () => _mock.VerifyAtMostOneCallTo(exp));
        }

        [TestMethod]
        public void VerifyAtMostOneCallTo_function_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneCallTo(_funcExpression));

            _test.WithResponse(1);
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneCallTo(_funcExpression));

            _test.WithResponse(5);
            AssertExceptionMessageContaining("at most once, but was 2 times",
                () => _mock.VerifyAtMostOneCallTo(_funcExpression));
        }

        [TestMethod]
        public void VerifyNoCallsTo_action_should_work()
        {
            var exp = _mock.Expression(x => x.WithDateTime(Any.DateTime));

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyNoCallsTo(exp));

            _test.WithDateTime(DateTime.Now);

            AssertExceptionMessageContaining("should never have been performed, but was 1 times",
                () => _mock.VerifyNoCallsTo(exp));
        }

        [TestMethod]
        public void VerifyNoCallsTo_function_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyNoCallsTo(_funcExpression));

            _test.WithResponse(666);

            AssertExceptionMessageContaining("should never have been performed, but was 1 times",
                () => _mock.VerifyNoCallsTo(_funcExpression));
        }

        [TestMethod]
        public void VerifyGetCount_should_work()
        {
            AssertExceptionMessageContaining("exactly 2 times, but was 0 times",
                () => _mock.VerifyGetCount(2, _propGetExpression));

            GetProp();
            GetProp();

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyGetCount(2, _propGetExpression));
        }

        [TestMethod]
        public void VerifyOneGet_should_work()
        {
            AssertExceptionMessageContaining("once, but was 0 times",
                () => _mock.VerifyOneGet(_propGetExpression));

            GetProp();

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyOneGet(_propGetExpression));
        }

        [TestMethod]
        public void VerifyAtLeastOneGet_should_work()
        {
            AssertExceptionMessageContaining("at least once, but was never performed",
                () => _mock.VerifyAtLeastOneGet(_propGetExpression));

            GetProp(); 
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneGet(_propGetExpression));

            GetProp();
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneGet(_propGetExpression));
        }

        [TestMethod]
        public void VerifyAtMostOneGet_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneGet(_propGetExpression));

            GetProp();
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneGet(_propGetExpression));

            GetProp();
            AssertExceptionMessageContaining("at most once, but was 2 times",
                () => _mock.VerifyAtMostOneGet(_propGetExpression));
        }

        [TestMethod]
        public void VerifyNoGets_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyNoGets(_propGetExpression));

            GetProp();
            AssertExceptionMessageContaining("should never have been performed, but was 1 times",
                () => _mock.VerifyNoGets(_propGetExpression));
        }

        [TestMethod]
        public void VerifySetCount_should_work()
        {
            AssertExceptionMessageContaining("exactly 2 times, but was 0 times",
                () => _mock.VerifySetCount(2, _propSetExpression));

            SetProp();
            SetProp();

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifySetCount(2, _propSetExpression));
        }

        [TestMethod]
        public void VerifyOneSet_should_work()
        {
            AssertExceptionMessageContaining("once, but was 0 times",
                () => _mock.VerifyOneSet(_propSetExpression));

            SetProp();

            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyOneSet(_propSetExpression));
        }

        [TestMethod]
        public void VerifyAtLeastOneSet_should_work()
        {
            AssertExceptionMessageContaining("at least once, but was never performed",
                () => _mock.VerifyAtLeastOneSet(_propSetExpression));

            SetProp();
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneSet(_propSetExpression));

            SetProp();
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtLeastOneSet(_propSetExpression));
        }

        [TestMethod]
        public void VerifyAtMostOneSet_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneSet(_propSetExpression));

            SetProp();
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyAtMostOneSet(_propSetExpression));

            SetProp();
            AssertExceptionMessageContaining("at most once, but was 2 times",
                () => _mock.VerifyAtMostOneSet(_propSetExpression));
        }

        [TestMethod]
        public void VerifyNoSets_should_work()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _mock.VerifyNoSets(_propSetExpression));

            SetProp();
            AssertExceptionMessageContaining("should never have been performed, but was 1 times",
                () => _mock.VerifyNoSets(_propSetExpression));
        }

        [TestMethod]
        public void Where_extension_should_work()
        {
            _test.WithInt(5);

            AssertExceptionNotThrown.WhenExecuting(() =>
            _mock.VerifyOneCallTo(x => x.WithInt(Any.Int.Where(i => i < 10))));

            AssertExceptionMessageContaining("once, but was 0 times",
                () => _mock.VerifyOneCallTo(x => x.WithInt(Any.Int.Where(i => i > 10))));
        }

        private string GetProp()
        {
            return _test.Prop;
        }

        private void SetProp(string value = "")
        {
            _test.Prop = value;
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
