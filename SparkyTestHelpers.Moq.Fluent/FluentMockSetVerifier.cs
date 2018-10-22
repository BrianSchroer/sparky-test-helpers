using System;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockSetVerifier<T> : FluentMockVerifier where T : class
    {
        private readonly Action<T> _action;

        private readonly Mock<T> _mock;

        internal FluentMockSetVerifier(Mock<T> mock, Action<T> setAction)
        {
            _mock = mock;
            _action = setAction;
        }

        internal override void ShouldHaveBeenCalled()
            => _mock.VerifySet(_action);

        internal override void ShouldHaveBeenCalledOnce()
            => _mock.VerifySet(_action, Times.Once);

        internal override void ShouldHaveBeenCalledAtLeastOnce()
            => _mock.VerifySet(_action, Times.AtLeastOnce);

        internal override void ShouldHaveBeenCalledAtMostOnce()
            => _mock.VerifySet(_action, Times.AtMostOnce);

        internal override void ShouldHaveCallCount(int callCount) =>
            _mock.VerifySet(_action, Times.Exactly(callCount));

        internal override void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive)
            => _mock.VerifySet(_action, Times.Between(callCountFrom, callCountTo, rangeKind));

        internal override void ShouldHaveCallCountOfAtLeast(int callCount)
            => _mock.VerifySet(_action, Times.AtLeast(callCount));

        internal override void ShouldHaveCallCountOfAtMost(int callCount)
            => _mock.VerifySet(_action, Times.AtMost(callCount));

        internal override void ShouldNotHaveBeenCalled() =>
            _mock.VerifySet(_action, Times.Never);
    }
}
