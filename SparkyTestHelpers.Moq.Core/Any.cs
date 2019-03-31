using System;
using System.Collections.Generic;
using System.Linq;
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
    public static partial class Any
    {
        /// <summary>
        /// <see cref="It.IsAny{Action}"/> wrapper.
        /// </summary>
        public static Action Action() => It.IsAny<Action>();

        /// <summary>
        /// <see cref="It.IsAny{Action}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static Action<T> Action<T>() => It.IsAny<Action<T>>();

        /// <summary>
        /// <see cref="It.IsAny{Action}"/> of <typeparamref name="T1"/>, <typeparamref name="T2"/> wrapper.
        /// </summary>
        public static Action<T1, T2> Action<T1, T2>() => It.IsAny<Action<T1, T2>>();

        /// <summary>
        /// <see cref="It.IsAny{Action}"/> of <typeparamref name="T1"/>, <typeparamref name="T2"/>, <typeparamref name="T3"/> wrapper.
        /// </summary>
        public static Action<T1, T2, T3> Action<T1, T2, T3>() => It.IsAny<Action<T1, T2, T3>>();

        /// <summary>
        /// <see cref="It.IsAny{Array}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static T[] Array<T>() => It.IsAny<T[]>();

        /// <summary>
        /// <see cref="It.IsAny{Boolean}()"/> wrapper.
        /// </summary>
        public static bool Boolean => It.IsAny<bool>();

        /// <summary>
        /// <see cref="It.IsAny{DateTime}()"/> wrapper.
        /// </summary>
        public static DateTime DateTime => It.IsAny<DateTime>();

        /// <summary>
        /// <see cref="It.IsAny{Decimal}()"/> wrapper.
        /// </summary>
        public static decimal Decimal => It.IsAny<decimal>();

        /// <summary>
        /// <see cref="It.IsAny{Dictionary}" /> wrapper.
        /// </summary>
        public static Dictionary<TKey, TValue> Dictionary<TKey, TValue>()
            => It.IsAny<Dictionary<TKey, TValue>>();

        /// <summary>
        /// <see cref="It.IsAny{Double}()"/> wrapper.
        /// </summary>
        public static double Double => It.IsAny<double>();

        /// <summary>
        /// <see cref="It.IsAny{Exception}()"/> wrapper.
        /// </summary>
        public static Exception Exception => It.IsAny<Exception>();

        /// <summary>
        /// <see cref="It.IsAny{Func}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static Func<T> Func<T>() => It.IsAny<Func<T>>();

        /// <summary>
        /// <see cref="It.IsAny{Func}"/> of <typeparamref name="T1"/>, <typeparamref name="T2"/>> wrapper.
        /// </summary>
        public static Func<T1, T2> Func<T1, T2>() => It.IsAny<Func<T1, T2>>();

        /// <summary>
        /// <see cref="It.IsAny{Func}"/> of <typeparamref name="T1"/>, <typeparamref name="T2"/>>, <typeparamref name="T3"/> wrapper.
        /// </summary>
        public static Func<T1, T2, T3> Func<T1, T2, T3>() => It.IsAny<Func<T1, T2, T3>>();

        /// <summary>
        /// <see cref="It.IsAny{Guid}()"/> wrapper.
        /// </summary>
        public static Guid Guid => It.IsAny<Guid>();

        /// <summary>
        /// <see cref="It.IsAny{IEnumerable}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static IEnumerable<T> IEnumerable<T>() => It.IsAny<IEnumerable<T>>();

        /// <summary>
        /// <see cref="It.IsAny{IList}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static IList<T> IList<T>() => It.IsAny<IList<T>>();

        /// <summary>
        /// <see cref="It.IsAny{T}" /> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static T InstanceOf<T>() => It.IsAny<T>();

        /// <summary>
        /// <see cref="It.IsAny{Int}()"/> wrapper.
        /// </summary>
        public static int Int => It.IsAny<int>();

        /// <summary>
        /// <see cref="It.IsAny{IQueryable}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static IQueryable<T> IQueryable<T>() => It.IsAny<IQueryable<T>>();

        /// <summary>
        /// <see cref="It.IsAny{KeyValuePair}"/> wrapper.
        /// </summary>
        public static KeyValuePair<TKey, TValue> KeyValuePair<TKey, TValue>()
            => It.IsAny<KeyValuePair<TKey, TValue>>();

        /// <summary>
        /// <see cref="It.IsAny{Lazy}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static Lazy<T> Lazy<T>() => It.IsAny<Lazy<T>>();

        /// <summary>
        /// <see cref="It.IsAny{List}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static List<T> List<T>() => It.IsAny<List<T>>();

        /// <summary>
        /// <see cref="It.IsAny{Int64}()"/> wrapper.
        /// </summary>
        public static long Long => It.IsAny<long>();

        /// <summary>
        /// <see cref="It.IsAny{Nullable}"/> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static T? Nullable<T>() where T : struct
        {
            return It.IsAny<T?>();
        }

        /// <summary>
        /// <see cref="It.IsAny{Object}"/> wrapper.
        /// </summary>
        public static object Object => It.IsAny<object>();

        /// <summary>
        /// <see cref="It.IsAny{T}" /> of <typeparamref name="T"/> wrapper.
        /// </summary>
        public static T One<T>() => It.IsAny<T>();

        /// <summary>
        /// <see cref="It.IsAny{Int16}()"/> wrapper.
        /// </summary>
        public static short Short => It.IsAny<short>();

        /// <summary>
        /// <see cref="It.IsAny{Single}()"/> wrapper.
        /// </summary>
        public static Single Single => It.IsAny<Single>();

        /// <summary>
        /// <see cref="It.IsAny{String}()"/> wrapper.
        /// </summary>
        public static string String => It.IsAny<string>();

        /// <summary>
        /// <see cref="It.IsAny{TimeSpan}()"/> wrapper.
        /// </summary>
        public static TimeSpan TimeSpan => It.IsAny<TimeSpan>();

        /// <summary>
        /// <see cref="It.IsAny{Tuple}"/> wrapper.
        /// </summary>
        public static Tuple<T1, T2> Tuple<T1, T2>() => It.IsAny<Tuple<T1, T2>>();

        /// <summary>
        /// <see cref="It.IsAny{Type}()"/> wrapper.
        /// </summary>
        public static Type Type => It.IsAny<Type>();

        /// <summary>
        /// <see cref="It.IsAny{UInt32}()"/> wrapper.
        /// </summary>
        public static uint UInt => It.IsAny<uint>();

        /// <summary>
        /// <see cref="It.IsAny{UInt64}()"/> wrapper.
        /// </summary>
        public static ulong ULong => It.IsAny<ulong>();

        /// <summary>
        /// <see cref="It.IsAny{UINt16}()"/> wrapper.
        /// </summary>
        public static ushort UShort => It.IsAny<ushort>();

        /// <summary>
        /// Alernative syntax for <c>Moq</c> It.Ref methods.
        /// </summary>
        public static class Out
        {
            public static Action Action;
            public static bool Boolean;
            public static DateTime DateTime;
            public static decimal Decimal;
            public static double Double;
            public static Exception Exception;
            public static Guid Guid;
            public static int Int;
            public static long Long;
            public static object Object;
            public static short Short;
            public static Single Single;
            public static string String;
            public static TimeSpan TimeSpan;
            public static Type Type;
            public static uint UInt;
            public static ulong ULong;
            public static ushort UShort;
        }

        /// <summary>
        /// Alernative syntax for <c>Moq</c> It.Ref methods.
        /// </summary>
        public static class Ref
        {
            public static Action Action;
            public static bool Boolean;
            public static DateTime DateTime;
            public static decimal Decimal;
            public static double Double;
            public static Exception Exception;
            public static Guid Guid;
            public static int Int;
            public static long Long;
            public static object Object;
            public static short Short;
            public static Single Single;
            public static string String;
            public static TimeSpan TimeSpan;
            public static Type Type;
            public static uint UInt;
            public static ulong ULong;
            public static ushort UShort;
        }
    }
}