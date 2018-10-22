using System;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockAssertions<T> : ReferenceTypeAssertions<Mock<T>, FluentMockAssertions<T>>
        where T : class
    {
        protected override string Identifier => $"Mock<{typeof(T).FullName}>";

        public FluentMockAssertions(Mock<T> mock)
        {
            Subject = mock;
        }

        public AndConstraint<FluentMockAssertions<T>> HaveHadNoOtherCalls(
            string because = "", params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs).Invoking(_ => Subject.VerifyNoOtherCalls()).Invoke();

            return new AndConstraint<FluentMockAssertions<T>>(this);
        }
    }
}