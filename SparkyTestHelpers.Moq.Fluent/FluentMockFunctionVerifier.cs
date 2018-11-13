using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockFunctionVerifier<T, TResult> : FluentMockVerifier where T : class
    {
        public Expression<Func<T, TResult>> Expression { get; }

        private readonly Mock<T> _mock;

        internal FluentMockFunctionVerifier(Mock<T> mock, Expression<Func<T, TResult>> functionExpression)
        {
            _mock = mock;
            Expression = functionExpression;
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
