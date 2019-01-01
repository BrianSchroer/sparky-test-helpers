using System;
using System.Collections.Generic;
using System.Linq;

namespace SparkyTestHelpers.Populater
{
    /// <summary>
    /// Test helper methods using <see cref="RandomValueProvider"/> to generate random values.
    /// </summary>
    public static class GetRandom 
    {
        private static readonly RandomValueProvider _randomValueProvider = new RandomValueProvider();

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="callback">"Callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public static T InstanceOf<T>(Action<T> callback) where T : class
        {
            return InstanceOf(null, null, callback);
        }

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create (default value is 5).</param>
        /// <param name="maximumIEnumerableSize">Optional maximum number of items to generate for arrays / lists / IEnumerables (default value is 3).</param>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public static T InstanceOf<T>(
            int? maximumDepth = null,
            int? maximumIEnumerableSize = null,
            Action<T> callback = null) where T : class
        {
            var populater = new Populater();
            int savedMaximumIEnumerableSize = _randomValueProvider.MaximumIEnumerableSize;

            try
            {
                if (maximumDepth.HasValue)
                {
                    populater.MaximumDepth = maximumDepth.Value;
                }

                if (maximumIEnumerableSize.HasValue)
                {
                    _randomValueProvider.MaximumIEnumerableSize = maximumIEnumerableSize.Value;
                }

                var instance = populater.CreateAndPopulate<T>(_randomValueProvider);

                callback?.Invoke(instance);

                return instance;
            }
            finally
            {
                _randomValueProvider.MaximumIEnumerableSize = savedMaximumIEnumerableSize;
            }
        }

        /// <summary>
        /// Create an <see cref="IEnumerable{T}"/> with properties populated with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="count">The desired <see cref="IEnumerable{T}"/> count.</param>
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create.</param>
        /// <param name="maximumIEnumerableSize">Optional maximum number of items to generate for "child" arrays / lists / IEnumerables.</param>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>New <see cref="IEnumerable{T}"/>.</returns>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static IEnumerable<T> IEnumerableOf<T>(
            int count,
            int? maximumDepth = null,
            int? maximumIEnumerableSize = null,
            Action<T> callback = null) where T : class
        {
            var populater = new Populater();
            int savedMaximumIEnumerableSize = _randomValueProvider.MaximumIEnumerableSize;

            try
            {
                if (maximumDepth.HasValue)
                {
                    populater.MaximumDepth = maximumDepth.Value;
                }

                if (maximumIEnumerableSize.HasValue)
                {
                    _randomValueProvider.MaximumIEnumerableSize = maximumIEnumerableSize.Value;
                }

                T[] items = populater.CreateIEnumerableOf<T>(count).ToArray();

                if (callback != null)
                {
                    foreach (T item in items)
                    {
                        callback(item);
                    }
                }

                return items;
            }
            finally
            {
                _randomValueProvider.MaximumIEnumerableSize = savedMaximumIEnumerableSize;
            }
        }

        /// <summary>
        /// Get random <see cref="bool"/> value.
        /// </summary>
        /// <returns>Random <see cref="bool"/> value.</returns>
        public static bool Bool() => _randomValueProvider.GetBool();

        /// <summary>
        /// Get random <see cref="byte"/> value.
        /// </summary>
        /// <returns>Random <see cref="byte"/> value.</returns>
        public static byte Byte() => _randomValueProvider.GetByte();

        /// <summary>
        /// Get random <see cref="char"/> value.
        /// </summary>
        /// <returns>Random <see cref="char"/> value.</returns>
        public static char Char() => _randomValueProvider.GetChar();

        /// <summary>
        /// Get random <see cref="DateTime"/> value.
        /// </summary>
        /// <returns>Random <see cref="DateTime"/> value.</returns>
        public static DateTime DateTime() => _randomValueProvider.GetDateTime();

        /// <summary>
        /// Get random <see cref="decimal"/> value.
        /// </summary>
        /// <returns>Random <see cref="decimal"/> value.</returns>
        public static decimal Decimal() => _randomValueProvider.GetDecimal();

        /// <summary>
        /// Get random <see cref="double"/> value.
        /// </summary>
        /// <returns>Random <see cref="double"/> value.</returns>
        public static double Double() => _randomValueProvider.GetDouble();

        /// <summary>
        /// Get random <see cref="Enum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
        /// <returns>Random <typeparamref name="TEnum"/> value.</returns>
        public static TEnum EnumValue<TEnum>() where TEnum : Enum => _randomValueProvider.GetEnum<TEnum>();

        /// <summary>
        /// Get random <see cref="float"/> value.
        /// </summary>
        /// <returns>Random <see cref="float"/> value.</returns>
        public static float Float() => _randomValueProvider.GetFloat();

        /// <summary>
        /// Get random <see cref="Guid"/> value.
        /// </summary>
        /// <returns>Random <see cref="Guid"/> value.</returns>
        public static Guid Guid() => _randomValueProvider.GetGuid();

        /// <summary>
        /// Get random <see cref="int"/> value.
        /// </summary>
        /// <returns>Random <see cref="int"/> value.</returns>
        public static int Int() => _randomValueProvider.GetInt();

        /// <summary>
        /// Get random <see cref="long"/> value.
        /// </summary>
        /// <returns>Random <see cref="long"/> value.</returns>
        public static long Long() => _randomValueProvider.GetLong();

        /// <summary>
        /// Get random <see cref="sbyte"/> value.
        /// </summary>
        /// <returns>Random <see cref="sbyte"/> value.</returns>
        public static sbyte SByte() => _randomValueProvider.GetSByte();

        /// <summary>
        /// Get random <see cref="short"/> value.
        /// </summary>
        /// <returns>Randm <see cref="short"/> value.</returns>
        public static short Short() => _randomValueProvider.GetShort();

        /// <summary>
        /// Get random <see cref="string"/> value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        /// <returns>Random <see cref="string"/> value.</returns>
        public static string String(string prefix = null) => _randomValueProvider.GetString(prefix);

        /// <summary>
        /// Get random <see cref="uint"/> value.
        /// </summary>
        /// <returns>Random <see cref="uint"/> value.</returns>
        public static uint UInt() => _randomValueProvider.GetUInt();

        /// <summary>
        /// Get random <see cref="ulong"/> value.
        /// </summary>
        /// <returns>Random <see cref="ulong"/> value.</returns>
        public static ulong ULong() => _randomValueProvider.GetULong();

        /// <summary>
        /// Get random <see cref="ushort"/> value.
        /// </summary>
        /// <returns>Random <see cref="ushort"/> value.</returns>
        public static ushort UShort() => _randomValueProvider.GetUShort();
    }
}
