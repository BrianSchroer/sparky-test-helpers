using System;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockSetVerifier<T> : FluentMockVerifier where T : class
    {
        public Action<T> Action { get; }

        private readonly Mock<T> _mock;

        internal FluentMockSetVerifier(Mock<T> mock, Action<T> setAction)
        {
            _mock = mock;
            Action = setAction;
        }

        internal override void ShouldHaveBeenCalled()
            => _mock.VerifySet(Action);

        internal override void ShouldHaveBeenCalledOnce()
            => _mock.VerifySet(Action, Times.Once);

        internal override void ShouldHaveBeenCalledAtLeastOnce()
            => _mock.VerifySet(Action, Times.AtLeastOnce);

        internal override void ShouldHaveBeenCalledAtMostOnce()
            => _mock.VerifySet(Action, Times.AtMostOnce);

        internal override void ShouldHaveCallCount(int callCount) =>
            _mock.VerifySet(Action, Times.Exactly(callCount));

        internal override void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive)
            => _mock.VerifySet(Action, Times.Between(callCountFrom, callCountTo, rangeKind));

        internal override void ShouldHaveCallCountOfAtLeast(int callCount)
            => _mock.VerifySet(Action, Times.AtLeast(callCount));

        internal override void ShouldHaveCallCountOfAtMost(int callCount)
            => _mock.VerifySet(Action, Times.AtMost(callCount));

        internal override void ShouldNotHaveBeenCalled() =>
            _mock.VerifySet(Action, Times.Never);
    }
}
