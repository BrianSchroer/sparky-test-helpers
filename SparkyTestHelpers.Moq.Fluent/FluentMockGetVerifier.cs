using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockGetVerifier<T, TProperty> : FluentMockVerifier where T : class
    {
        private readonly Expression<Func<T, TProperty>> _expression;
        private readonly Mock<T> _mock;

        internal FluentMockGetVerifier(Mock<T> mock, Expression<Func<T, TProperty>> getExpression)
        {
            _mock = mock;
            _expression = getExpression;
        }

        internal override void ShouldHaveBeenCalled()
            => _mock.VerifyGet(_expression);

        internal override void ShouldHaveBeenCalledOnce()
            => _mock.VerifyGet(_expression, Times.Once);

        internal override void ShouldHaveBeenCalledAtLeastOnce()
            => _mock.VerifyGet(_expression, Times.AtLeastOnce);

        internal override void ShouldHaveBeenCalledAtMostOnce()
            => _mock.VerifyGet(_expression, Times.AtMostOnce);

        internal override void ShouldHaveCallCount(int callCount) =>
            _mock.VerifyGet(_expression, Times.Exactly(callCount));

        internal override void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive)
            => _mock.VerifyGet(_expression, Times.Between(callCountFrom, callCountTo, rangeKind));

        internal override void ShouldHaveCallCountOfAtLeast(int callCount)
            => _mock.VerifyGet(_expression, Times.AtLeast(callCount));

        internal override void ShouldHaveCallCountOfAtMost(int callCount)
            => _mock.VerifyGet(_expression, Times.AtMost(callCount));

        internal override void ShouldNotHaveBeenCalled() =>
            _mock.VerifyGet(_expression, Times.Never);
    }
}
