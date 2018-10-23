using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockMethodVerifier<T> : FluentMockVerifier where T : class
    {
        public Expression<Action<T>> Expression { get; }

        private readonly Mock<T> _mock;

        internal FluentMockMethodVerifier(Mock<T> mock, Expression<Action<T>> methodExpression)
        {
            _mock = mock;
            Expression = methodExpression;
        }

        internal override void ShouldHaveBeenCalled()
            => _mock.Verify(Expression);

        internal override void ShouldHaveBeenCalledOnce()
            => _mock.Verify(Expression, Times.Once);

        internal override void ShouldHaveBeenCalledAtLeastOnce()
            => _mock.Verify(Expression, Times.AtLeastOnce);

        internal override void ShouldHaveBeenCalledAtMostOnce()
            => _mock.Verify(Expression, Times.AtMostOnce);

        internal override void ShouldHaveCallCount(int callCount) =>
            _mock.Verify(Expression, Times.Exactly(callCount));

        internal override void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive)
            => _mock.Verify(Expression, Times.Between(callCountFrom, callCountTo, rangeKind));

        internal override void ShouldHaveCallCountOfAtLeast(int callCount)
            => _mock.Verify(Expression, Times.AtLeast(callCount));

        internal override void ShouldHaveCallCountOfAtMost(int callCount)
            => _mock.Verify(Expression, Times.AtMost(callCount));

        internal override void ShouldNotHaveBeenCalled() =>
            _mock.Verify(Expression, Times.Never);
    }
}
