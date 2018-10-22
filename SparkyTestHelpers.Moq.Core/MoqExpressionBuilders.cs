using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    /// <summary>
    /// Extension methods for Moq.
    /// </summary>
    public static class MoqExpressionBuilders
    {
        /// <summary>
        /// Defines expression to be used for mock action .Setup/.Verify methods. This helper allows you to code
        /// the lambda just once and use it for both the .Setup and .Verify methods.
        /// </summary>
        /// <typeparam name="T">The type being mocked.</typeparam>
        /// <param name="mock">The mock instance.</param>
        /// <param name="expression">The expression (callback).</param>
        /// <returns>The expression.</returns>
        /// <example>
        ///     <para>This code:</para>
        ///     <code><![CDATA[
        ///     _mockAdapter.Setup(x =>
        ///         x.Initialize(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>()));
        ///
        ///     _logic.Initialize(_testAccountNumber, _usefulInputArray);
        ///
        ///     _mockAdapter.Verify(x =>
        ///         x.Initialize(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>())),
        ///         Times.Once);
        ///     ]]></code>
        ///     <para>...could be written as:</para>
        ///     <code><![CDATA[
        ///     var initializeExpression = _mockAdapter.Expression(x =>
        ///         x.Initialize(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>()));
        ///
        ///     _mockAdapter.Setup(initializeExpression).Returns(new UsefulData());
        ///
        ///     _logic.Initialize(_testAccountNumber, _usefulInputArray);
        ///
        ///     _mockAdapter.Verify(initializeExpression, Times.Once);
        ///     ]]></code>
        /// </example>

        public static Expression<Action<T>> Expression<T>(
            this Mock<T> mock, Expression<Action<T>> expression) where T : class
        {
            return expression;
        }

        /// <summary>
        /// Defines expression to be used for mock function .Setup/.Verify methods. This helper allows you to code
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
        ///     <para>...could be written as:</para>
        ///     <code><![CDATA[
        ///     var getUsefulDataExpression = _mockAdapter.Expression(x =>
        ///         x.GetUsefulData(It.IsAny<string>(), It.IsAny<IEnumerable<UsefulInput>>()));
        ///
        ///     _mockAdapter.Setup(getUsefulDataExpression).Returns(new UsefulData());
        ///
        ///     _logic.GetUsefulData(_testAccountNumber, _usefulInputArray);
        ///
        ///     _mockAdapter.Verify(getUsefulDataExpression, Times.Once);
        ///     ]]></code>
        /// </example>
        public static Expression<Func<T, TResult>> Expression<T, TResult>(
            this Mock<T> mock, Expression<Func<T, TResult>> expression) where T : class
        {
            return expression;
        }
    }
}
