using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    /// <summary>
    /// Extension methods for Moq.
    /// </summary>
    public static class MoqWhereExtensions
    {
        /// <summary>
        /// <c>Moq</c> "It.Is" alternative - Matches any value that satisfies the given predicate.
        /// </summary>
        /// <example>
        ///     <para>This code:</para>
        ///     <code><![CDATA[
        ///     _mock.Setup(x => x.Do(It.Is<int>(i => i % 2 == 0))).Returns(1);
        ///     ]]></code>
        ///     <para>...could be written as:</para>
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
