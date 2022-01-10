using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Moq;
using System;
using System.Linq.Expressions;

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

        public AndConstraint<FluentMockAssertions<T>> HaveCalledOnly(
            Expression<Action<T>> methodExpression, string because = "", params object[] becauseArgs) =>
            HaveOneCallTo(methodExpression, because, becauseArgs).And
            .HaveHadNoOtherCalls(because, becauseArgs);

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

        public AndConstraint<FluentMockAssertions<T>> HaveNoCallsTo<TResult>(
            Expression<Func<T, TResult>> functionExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(functionExpression, Times.Never), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveCallsTo<TResult>(
            Expression<Func<T, TResult>> expression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(expression), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveOneCallTo<TResult>(
            Expression<Func<T, TResult>> functionExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(functionExpression, Times.Once), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtLeastOneCallTo<TResult>(
            Expression<Func<T, TResult>> functionExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(functionExpression, Times.AtLeastOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtMostOneCallTo<TResult>(
            Expression<Func<T, TResult>> functionExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.Verify(functionExpression, Times.AtMostOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveNoCallsToGet<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.Never), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveCallsToGet<TProperty>(
            Expression<Func<T, TProperty>> expression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(expression), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveOneCallToGet<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.Once), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtLeastOneCallToGet<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs) =>
            Verify(() => Subject.VerifyGet(getExpression, Times.AtLeastOnce), because, becauseArgs);

        public AndConstraint<FluentMockAssertions<T>> HaveAtMostOneCallToGet<TProperty>(
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