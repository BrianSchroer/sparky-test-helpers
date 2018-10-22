using Moq;

namespace SparkyTestHelpers.Moq
{
    public abstract class FluentMockVerifier
    {
        internal abstract void ShouldHaveBeenCalled();
        internal abstract void ShouldHaveBeenCalledOnce();
        internal abstract void ShouldHaveBeenCalledAtLeastOnce();
        internal abstract void ShouldHaveBeenCalledAtMostOnce();
        internal abstract void ShouldHaveCallCount(int callCount);
        internal abstract void ShouldHaveCallCountBetween(int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive);
        internal abstract void ShouldHaveCallCountOfAtLeast(int callCount);
        internal abstract void ShouldHaveCallCountOfAtMost(int callCount);
        internal abstract void ShouldNotHaveBeenCalled();
    }
}