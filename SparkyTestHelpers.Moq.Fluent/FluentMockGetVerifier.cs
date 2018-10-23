using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockGetVerifier<T, TProperty> : FluentMockVerifier where T : class
    {
        public Expression<Func<T, TProperty>> Expression { get; }

        private readonly Mock<T> _mock;

        internal FluentMockGetVerifier(Mock<T> mock, Expression<Func<T, TProperty>> getExpression)
        {
            _mock = mock;
            Expression = getExpression;
        }

        internal override void ShouldHaveBeenCalled()
            => _mock.VerifyGet(Expression);

        internal override void ShouldHaveBeenCalledOnce()
            => _mock.VerifyGet(Expression, Times.Once);

        internal override void ShouldHaveBeenCalledAtLeastOnce()
            => _mock.VerifyGet(Expression, Times.AtLeastOnce);

        internal override void ShouldHaveBeenCalledAtMostOnce()
            => _mock.VerifyGet(Expression, Times.AtMostOnce);

        internal override void ShouldHaveCallCount(int callCount) =>
            _mock.VerifyGet(Expression, Times.Exactly(callCount));

        internal override void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive)
            => _mock.VerifyGet(Expression, Times.Between(callCountFrom, callCountTo, rangeKind));

        internal override void ShouldHaveCallCountOfAtLeast(int callCount)
            => _mock.VerifyGet(Expression, Times.AtLeast(callCount));

        internal override void ShouldHaveCallCountOfAtMost(int callCount)
            => _mock.VerifyGet(Expression, Times.AtMost(callCount));

        internal override void ShouldNotHaveBeenCalled() =>
            _mock.VerifyGet(Expression, Times.Never);
    }
}
