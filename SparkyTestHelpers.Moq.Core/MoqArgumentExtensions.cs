using System;
using System.Linq.Expressions;
using Moq;

namespace SparkyTestHelpers.Moq
{
    /// <summary>
    /// Extension methods for Moq method arguments.
    /// </summary>
    public static class MoqArgumentExtensions
    {
        /// <summary>
        /// <c>Moq</c> Verification "It.Is" alternative, using "callback" action that performs assertion.
        /// </summary>
        /// <example>
        ///     <para>This code:</para>
        ///     <code><![CDATA[
        ///     _mock.Verify(x => x.Do(It.Is<int>(i => i > 3)), Times.Once);
        ///     ]]></code>
        ///     <para>...could be written as:</para>
        ///     <code><![CDATA[
        ///     _mock.Verify(x => x.Do(Any.Int.Where(i => Assert.IsTrue(i > 3)), Times.Once);
        ///     ]]></code>
        ///     <para>...or, with SparkyTestHelpers.Moq.Fluent:</para>
        ///     <code><![CDATA[
        ///     _mock.ShouldHaveOneCallTo(x => x.Do(Any.Int.Where(i => i.Should().BeGreaterThan(3)));
        ///     ]]></code>
        /// </example>
        /// <typeparam name="TValue">The method argument type.</typeparam>
        /// <param name="value">The method argument.</param>
        /// <param name="assert">"Callback" action that performs assertions.</param>
        /// <returns>The argument value.</returns>
        public static TValue Where<TValue>(this TValue value, Action<TValue> assert)
        {
            return Match.Create(new Predicate<TValue>(val =>
            {
                assert(val);
                return true;
            }));
        }

        /// <summary>
        /// <c>Moq</c> "It.Is" alternative - Matches any value that satisfies the given predicate.
        /// </summary>
        /// <example>
        ///     <para>This code:</para>
        ///     <code><![CDATA[
        ///     _mock.Verify(x => x.Do(It.Is<int>(i => i % 2 == 0))).Returns(1);
        ///     ]]></code>
        ///     <para>...could be written as:</para>
        ///     <code><![CDATA[
        ///     _mock.Setup(x => x.Do(Any.Int.Where(i => i % 2 == 0))).Returns(1);
        ///     ]]></code>
        /// </example>
        /// <typeparam name="TValue">The method argument type.</typeparam>
        /// <param name="value">The method argument.</param>
        /// <param name="match">The predicate used to match the method argument.</param>
        /// <returns>A <see cref="Match{T}"/> that matches any value of <typeparamref name="TValue"/>.</returns>
        public static TValue Where<TValue>(this TValue value, Expression<Func<TValue, bool>> match)
        {
            return It.Is(match);
        }
    }
}
