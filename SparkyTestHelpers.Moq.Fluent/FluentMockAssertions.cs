using System;
using System.Linq.Expressions;
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
            string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyNoOtherCalls(), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveNoCallsTo(
            Expression<Action<T>> methodExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(methodExpression, Times.Never), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveCallsTo(
            Expression<Action<T>> expression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(expression), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveOneCallTo(
            Expression<Action<T>> methodExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(methodExpression, Times.Once), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtLeastOneCallTo(
            Expression<Action<T>> methodExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(methodExpression, Times.AtLeastOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtMostOneCallTo(
            Expression<Action<T>> methodExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(methodExpression, Times.AtMostOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveNoCallsTo<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.Never), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveCallsTo<TProperty>(
            Expression<Func<T, TProperty>> expression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(expression), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveOneCallTo<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.Once), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtLeastOneCallTo<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.AtLeastOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtMostOneCallTo<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.AtMostOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveNoCallsToSet(
            Action<T> setAction, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifySet(setAction, Times.Never), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveCallsToSet(
            Action<T> expression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifySet(expression), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveOneCallToSet(
            Action<T> setAction, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifySet(setAction, Times.Once), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtLeastOneCallToSet(
            Action<T> setAction, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifySet(setAction, Times.AtLeastOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtMostOneCallToSet(
            Action<T> setAction, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifySet(setAction, Times.AtMostOnce), because, becauseArgs);

        public FluentMockCountAssertions<T> HaveCallCount(int count) =>
            new FluentMockCountAssertions<T>(Subject, count);

        public FluentMockCountAssertions<T> HaveCallCount(
            int callCountFrom, int callCountTo, Range rangeKind = Range.Inclusive) =>
            new FluentMockCountAssertions<T>(Subject, callCountFrom, callCountTo, rangeKind);

        private AndConstraint<FluentMockAssertions<T>> Verify(
            Action action, string because, params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs).Invoking(_ => action()).Invoke();

            return new AndConstraint<FluentMockAssertions<T>>(this);
        }
    }
}