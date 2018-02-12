using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    public static class MoqExtensions
    {
        /// <summary>
        /// Defines expression to be used for mock .Setup/.Verify methods. This helper allows you to code
        /// the lambda just once and use it for both the .Setup and .Verify methods.
        /// </summary>
        /// <typeparam name="T">The type being mocked.</typeparam>
        /// <typeparam name="TResult">The expression result type.</typeparam>
        /// <param name="mock">The mock instance.</param>
        /// <param name="expression">The expression (callback).</param>
        /// <returns>The expression.</returns>
        /// <example>
        ///     <para>This code:</para>
        ///     <code><![CDATA[
        ///     _mockAdapter.Setup(x => 
        ///         x.GetUsefulData(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>()))
        ///         .Returns(new UsefulData());
        ///     
        ///     _logic.GetUsefulData(_testAccountNumber, _usefulInputArray);
        ///     
        ///     _mockAdapter.Verify(x => 
        ///         x.GetUsefulData(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>())), 
        ///         Times.Once);
        ///     ]]></code>
        ///     <para>...cound be written as:</para>
        ///     <code><![CDATA[
        ///     var getUsefulDataExpression = _mockAdapter.DefineExpression(x => 
        ///         x.GetUsefulData(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>()));
        ///     
        ///     _mockAdapter.Setup(getUsefulDataExpression).Returns(new UsefulData());
        ///     
        ///     _logic.GetUsefulData(_testAccountNumber, _usefulInputArray);
        ///     
        ///     _mockAdapter.Verify(getUsefulDataExpression, Times.Once);
        ///     ]]></code>
        /// </example>
        public static Expression<Func<T, TResult>> DefineExpression<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
        {
            return expression;
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// exactly <paramref name="callCount"/> times.
        /// </summary>
        public static void VerifyCallCount<T>(
            this Mock<T> mock, int callCount, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.Exactly(callCount));
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// exactly <paramref name="callCount"/> times.
        /// </summary>
        public static void VerifyCallCount<T, TResult>(
            this Mock<T> mock, int callCount, Expression<Func<T, TResult>> expression) where T : class
        {
            mock.Verify(expression, Times.Exactly(callCount));
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// exactly 1 time.
        /// </summary>
        public static void VerifyOneCallTo<T>(
            this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.Once);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// exactly 1 time.
        /// </summary>
        public static void VerifyOneCallTo<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
        {
            mock.Verify(expression, Times.Once);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// at least 1 time.
        /// </summary>
        public static void VerifyAtLeastOneCallTo<T>(
            this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// at least 1 time.
        /// </summary>
        public static void VerifyAtLeastOneCallTo<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
        {
            mock.Verify(expression, Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// zero or one time.
        /// </summary>
        public static void VerifyAtMostOneCallTo<T>(
            this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.AtMostOnce);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was performed on the mock
        /// zero or one time.
        /// </summary>
        public static void VerifyAtMostOneCallTo<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
        {
            mock.Verify(expression, Times.AtMostOnce);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was never performed on the mock.
        /// </summary>
        public static void VerifyNoCallsTo<T>(
            this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            mock.Verify(expression, Times.Never);
        }

        /// <summary>
        /// Verifies that a specific invocation matching the given expression was never performed on the mock.
        /// </summary>
        public static void VerifyNoCallsTo<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
        {
            mock.Verify(expression, Times.Never);
        }

        /// <summary>
        /// Verifies that a property was read on the mock exactly <paramref name="getCount" /> times.
        /// </summary>
        public static void VerifyGetCount<T, TProperty>(
            this Mock<T> mock, int getCount, Expression<Func<T, TProperty>> expression) where T : class
        {
            mock.VerifyGet(expression, Times.Exactly(getCount));
        }

        /// <summary>
        /// Verifies that a property was read on the mock exactly 1 time.
        /// </summary>
        public static void VerifyOneGet<T, TProperty>(
            this Mock<T> mock, Expression<Func<T, TProperty>> expression) where T : class
        {
            mock.VerifyGet(expression, Times.Once);
        }

        /// <summary>
        /// Verifies that a property was read on the mock at least 1 time.
        /// </summary>
        public static void VerifyAtLeastOneGet<T, TProperty>(
            this Mock<T> mock, Expression<Func<T, TProperty>> expression) where T : class
        {
            mock.VerifyGet(expression, Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies that a property was read on the mock zero or one time.
        /// </summary>
        public static void VerifyAtMostOneGet<T, TProperty>(
            this Mock<T> mock, Expression<Func<T, TProperty>> expression) where T : class
        {
            mock.VerifyGet(expression, Times.AtMostOnce);
        }

        /// <summary>
        /// Verifies that a property was never read on the mock.
        /// </summary>
        public static void VerifyNoGets<T, TProperty>(
            this Mock<T> mock, Expression<Func<T, TProperty>> expression) where T : class
        {
            mock.VerifyGet(expression, Times.Never);
        }

        /// <summary>
        /// Verifies that a property was set on the mock exactly <paramref name="setCount"/> times.
        /// </summary> 
        public static void VerifySetCount<T>(
            this Mock<T> mock, int setCount, Action<T> expression) where T : class
        {
            mock.VerifySet(expression, Times.Exactly(setCount));
        }

        /// <summary>
        /// Verifies that a property was set on the mock exactly 1 time.
        /// </summary> 
        public static void VerifyOneSet<T>(
            this Mock<T> mock, Action<T> expression) where T : class
        {
            mock.VerifySet(expression, Times.Once);
        }
        
        /// <summary>
        /// Verifies that a property was set on the mock at least 1 time.
        /// </summary> 
        public static void VerifyAtLeastOneSet<T>(
            this Mock<T> mock, Action<T> expression) where T : class
        {
            mock.VerifySet(expression, Times.AtLeastOnce);
        }

        /// <summary>
        /// Verifies that a property was set on the mock zero or one times.
        /// </summary> 
        public static void VerifyAtMostOneSet<T>(
            this Mock<T> mock, Action<T> expression) where T : class
        {
            mock.VerifySet(expression, Times.AtMostOnce);
        }

        /// <summary>
        /// Verifies that a property was never set on the mock.
        /// </summary> 
        public static void VerifyNoSets<T>(
            this Mock<T> mock, Action<T> expression) where T : class
        {
            mock.VerifySet(expression, Times.Never);
        }

        /// <summary>
        /// <c>Moq</c> "It.Is" alternative - Matches any value that satisfies the given predicate.
        /// </summary>
        /// <example>
        ///     <para>This code:</para>
        ///     <code><![CDATA[
        ///     _mock.Setup(x => x.Do(It.Is<int>(i => i % 2 == 0))).Returns(1);
        ///     ]]></code>
        ///     <para>...cound be written as:</para>
        ///     <code><![CDATA[
        ///     _mock.Setup(x => x.Do(Any.Int.Where(i => i % 2 == 0))).Returns(1);
        ///     ]]></code>
        /// </example>
        /// <typeparam name="T">The type being mocked.</typeparam>
        /// <param name="mock">The mock instance.</param>
        /// <param name="match">The predicate used to match the method argument.</param>
        /// <returns>A <see cref="Match{T}"/> that matches any value of <typeparamref name="T"/>.</returns>
        public static T Where<T>(this T mock, Expression<Func<T, bool>> match)
        {
            return It.Is(match);
        }
    }
}
