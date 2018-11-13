using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public static class FluentMockExtensions
    {
        public static FluentMockGetVerifier<T, TProperty> Get<T, TProperty>(
            this Mock<T> mock, Expression<Func<T, TProperty>> getExpression) where T : class
            => new FluentMockGetVerifier<T, TProperty>(mock, getExpression);

        public static FluentMockMethodVerifier<T> Method<T>(
            this Mock<T> mock, Expression<Action<T>> methodExpression) where T : class
            => new FluentMockMethodVerifier<T>(mock, methodExpression);

        public static FluentMockFunctionVerifier<T, TResult> Method<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> functionExpression) where T : class
            => new FluentMockFunctionVerifier<T, TResult>(mock, functionExpression);

        public static FluentMockSetVerifier<T> Set<T>(
            this Mock<T> mock, Action<T> setAction) where T : class
            => new FluentMockSetVerifier<T>(mock, setAction);

        public static FluentMockAssertions<T> Should<T>(
            this Mock<T> mock) where T : class
                => new FluentMockAssertions<T>(mock);

        public static FluentMockVerifierAssertions<T> Should<T>(
            this FluentMockMethodVerifier<T> instance) where T : class
            => new FluentMockVerifierAssertions<T>(instance);

        public static FluentMockVerifierAssertions<T> Should<T, TResult>(
            this FluentMockFunctionVerifier<T, TResult> instance) where T : class
            => new FluentMockVerifierAssertions<T>(instance);

        public static FluentMockVerifierAssertions<T> Should<T, TProperty>(
            this FluentMockGetVerifier<T, TProperty> instance) where T : class
            => new FluentMockVerifierAssertions<T>(instance);

        public static FluentMockVerifierAssertions<T> Should<T>(
            this FluentMockSetVerifier<T> instance) where T : class
            => new FluentMockVerifierAssertions<T>(instance);
    }
}
