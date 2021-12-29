using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockVerifierAssertions<T> : ReferenceTypeAssertions<FluentMockVerifier, FluentMockVerifierAssertions<T>>
        where T : class
    {
        protected override string Identifier => nameof(FluentMockVerifier);

        public FluentMockVerifierAssertions(FluentMockVerifier verifier) : base(verifier)
        {
        }

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveBeenCalled(
            string because = "", params object[] becauseArgs)
            => Verify(Subject.ShouldHaveBeenCalled, because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveBeenCalledOnce(
            string because = "", params object[] becauseArgs)
            => Verify(Subject.ShouldHaveBeenCalledOnce, because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveBeenCalledAtLeastOnce(
            string because = "", params object[] becauseArgs)
            => Verify(Subject.ShouldHaveBeenCalledAtLeastOnce, because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveBeenCalledAtMostOnce(
            string because = "", params object[] becauseArgs)
            => Verify(Subject.ShouldHaveBeenCalledAtMostOnce, because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveCallCount(
            int callCount, string because = "", params object[] becauseArgs)
            => Verify(() => Subject.ShouldHaveCallCount(callCount), because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveCallCountBetween(
            int callCountFrom, int callCountTo, string because = "", params object[] becauseArgs)
            => Verify(() => Subject.ShouldHaveCallCountBetween(callCountFrom, callCountTo), because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveCallCountOfAtLeast(
            int callCount, string because = "", params object[] becauseArgs)
            => Verify(() => Subject.ShouldHaveCallCountOfAtLeast(callCount), because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> HaveCallCountOfAtMost(
            int callCount, string because = "", params object[] becauseArgs)
            => Verify(() => Subject.ShouldHaveCallCountOfAtMost(callCount), because, becauseArgs);

        public AndConstraint<FluentMockVerifierAssertions<T>> NotHaveBeenCalled(
            string because = "", params object[] becauseArgs)
            => Verify(Subject.ShouldNotHaveBeenCalled, because, becauseArgs);

        private AndConstraint<FluentMockVerifierAssertions<T>> Verify(
            Action action, string because, params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs).Invoking(_ => action()).Invoke();

            return new AndConstraint<FluentMockVerifierAssertions<T>>(this);
        }
    }
}