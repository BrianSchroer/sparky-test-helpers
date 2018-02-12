using System;
using System.Collections.Generic;
using Moq;

namespace SparkyTestHelpers.Moq
{
    /// <summary>
    /// Wrapper for <c>Moq</c> It.IsAny methods, providing simplified syntax with fewer
    /// parentheses and angle brackets when matching built-in types.
    /// </summary>
    /// <example>
    ///     <para>Normal <c>Moq</c> It.IsAny syntax:</para>
    ///     <code><![CDATA[
    ///     _mock.Setup(x => x.DoSomething(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<IEnumerable<int>>()).Returns(true);
    ///     ]]></code> 
    ///     <para>Any. syntax:</para>
    ///     <code><![CDATA[
    ///     _mock.Setup(x => x.DoSomething(Any.String, Any.Int, Any.IEnumerable<int>()).Returns(true);
    ///     ]]></code>  
    /// </example>
    public static class Any
    {
        /// <summary>
        /// <see cref="It.IsAny{Boolean}()"/> wrapper.
        /// </summary>
        public static bool Boolean => It.IsAny<bool>();

        /// <summary>
        /// <see cref="It.IsAny{Decimal}()"/> wrapper.
        /// </summary>
        public static decimal Decimal => It.IsAny<decimal>();

        /// <summary>
        /// <see cref="It.IsAny{Double}()"/> wrapper.
        /// </summary>
        public static double Double => It.IsAny<double>();

        /// <summary>
        /// <see cref="It.IsAny{DateTime}()"/> wrapper.
        /// </summary>
        public static DateTime DateTime => It.IsAny<DateTime>();

        /// <summary>
        /// <see cref="It.IsAny{Int}()"/> wrapper.
        /// </summary>
        public static int Int => It.IsAny<int>();

        /// <summary>
        /// <see cref="It.IsAny{String}()"/> wrapper.
        /// </summary>
        public static string String => It.IsAny<string>();

        /// <summary>
        /// <see cref="cref="It.IsAny{T}" of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static T InstanceOf<T>() => It.IsAny<T>();

        /// <summary>
        /// <see cref="cref="It.IsAny{Array}" of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static T[] Array<T>() => It.IsAny<T[]>();

        /// <summary>
        /// <see cref="cref="It.IsAny{IEnumerable}" of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static IEnumerable<T> IEnumerable<T>() => It.IsAny<IEnumerable<T>>();

        /// <summary>
        /// <see cref="cref="It.IsAny{List}" of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static List<T> List<T>() => It.IsAny<List<T>>();

        /// <summary>
        /// <see cref="cref="It.IsAny{Dictionary}" wrapper.
        /// </summary>
        public static Dictionary<TKey, TValue> Dictionary<TKey, TValue>() 
            => It.IsAny<Dictionary<TKey, TValue>>();

        /// <summary>
        /// <see cref="cref="It.IsAny{KeyValuePair}" wrapper.
        /// </summary>
        public static KeyValuePair<TKey, TValue> KeyValuePair<TKey, TValue>()
            => It.IsAny<KeyValuePair<TKey, TValue>>();

        /// <summary>
        /// <see cref="cref="It.IsAny{Tuple}" wrapper.
        /// </summary>
        public static Tuple<T1, T2> Tuple<T1, T2>() => It.IsAny<Tuple<T1, T2>>();
    }
}