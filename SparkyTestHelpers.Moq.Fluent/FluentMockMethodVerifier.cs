using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockMethodVerifier<T> : FluentMockVerifier where T : class
    {
        private readonly Expression<Action<T>> _expression;
        private readonly Mock<T> _mock;

        internal FluentMockMethodVerifier(Mock<T> mock, Expression<Action<T>> methodExpression)
        {
            _mock = mock;
            _expression = methodExpression;
        }

        internal override void ShouldHaveBeenCalled()
            => _mock.Verify(_expression);

        internal override void ShouldHaveBeenCalledOnce()
            => _mock.Verify(_expression, Times.Once);

        internal override void ShouldHaveBeenCalledAtLeastOnce()
            => _mock.Verify(_expression, Times.AtLeastOnce);

        internal override void ShouldHaveBeenCalledAtMostOnce()
            => _mock.Verify(_expression, Times.AtMostOnce);

        internal override void ShouldHaveCallCount(int callCount) =>
            _mock.Verify(_expression, Times.Exactly(callCount));

        internal override void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive)
            => _mock.Verify(_expression, Times.Between(callCountFrom, callCountTo, rangeKind));

        internal override void ShouldHaveCallCountOfAtLeast(int callCount)
            => _mock.Verify(_expression, Times.AtLeast(callCount));

        internal override void ShouldHaveCallCountOfAtMost(int callCount)
            => _mock.Verify(_expression, Times.AtMost(callCount));

        internal override void ShouldNotHaveBeenCalled() =>
            _mock.Verify(_expression, Times.Never);
    }
}
