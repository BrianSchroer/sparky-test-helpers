using System;
using System.Linq;
using System.Linq.Expressions;
using FluentAssertions;
using FluentAssertions.Execution;
using FluentAssertions.Primitives;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public class FluentMockCountAssertions<T> : ReferenceTypeAssertions<Mock<T>, FluentMockCountAssertions<T>>
        where T : class
    {
        protected override string Identifier => $"Mock call counts: {typeof(T).FullName}";

        private readonly int _callCountFrom;
        private readonly int? _callCountTo;
        private readonly Range _rangeKind;
        private bool _orMore;
        private bool _orLess;

        public FluentMockCountAssertions(Mock<T> mock, int callCountFrom, int? callCountTo = null, Range rangeKind = Range.Inclusive)
        {
            Subject = mock;
            _callCountFrom = callCountFrom;
            _callCountTo = callCountTo;
            _rangeKind = rangeKind;
        }

        public FluentMockCountAssertions<T> OrMore()
        {
            _orMore = true;
            return this;
        }

        public FluentMockCountAssertions<T> OrLess()
        {
            _orLess = true;
            return this;
        }

        public AndConstraint<FluentMockCountAssertions<T>> To(
            Expression<Action<T>> methodExpression, string because = "", params object[] becauseArgs)
        {
            return Verify(() => Subject.Verify(methodExpression, DetermineTimes()), because, becauseArgs);
        }

        public AndConstraint<FluentMockCountAssertions<T>> ToGet<TProperty>(
            Expression<Func<T, TProperty>> getExpression, string because = "", params object[] becauseArgs)
        {
            return Verify(() => Subject.VerifyGet(getExpression, DetermineTimes()), because, becauseArgs);
        }

        public AndConstraint<FluentMockCountAssertions<T>> ToSet(
            Action<T> setAction, string because = "", params object[] becauseArgs)
        {
            return Verify(() => Subject.VerifySet(setAction, DetermineTimes()), because, becauseArgs);
        }

        private Times DetermineTimes()
        {
            if (new[] {_orMore, _orLess, _callCountTo.HasValue}.Count(b => b) > 1)
            {
                throw new InvalidOperationException($"Invalid {nameof(OrMore)} / {nameof(OrLess)} / CallCountTo combination.");
            }

            if (_orMore) return Times.AtLeast(_callCountFrom);
            if (_orLess) return Times.AtMost(_callCountFrom);

            return (_callCountTo.HasValue)
                ? Times.Between(_callCountFrom, _callCountTo.Value, _rangeKind)
                : Times.Exactly(_callCountFrom);
        }

        private AndConstraint<FluentMockCountAssertions<T>> Verify(
            Action action, string because, params object[] becauseArgs)
        {
            Execute.Assertion.BecauseOf(because, becauseArgs).Invoking(_ => action()).Invoke();

            return new AndConstraint<FluentMockCountAssertions<T>>(this);
        }
    }
}