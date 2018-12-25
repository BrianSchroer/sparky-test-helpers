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
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create.</param>
        /// <param name="maximumIEnumerableSize">Optional maximum number of items to generate for arrays / lists / IEnumerables.</param>
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
        /// <returns>New instance.</returns>
        // ReSharper disable once UnusedMember.Global
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
        /// Get random bool value.
        /// </summary>
        public static bool Bool() => _randomValueProvider.GetBool();

        /// <summary>
        /// Get random byte value.
        /// </summary>
        public static byte Byte() => _randomValueProvider.GetByte();

        /// <summary>
        /// Get random char value.
        /// </summary>
        public static char Char() => _randomValueProvider.GetChar();

        /// <summary>
        /// Get random DateTime value.
        /// </summary>
        public static DateTime DateTime() => _randomValueProvider.GetDateTime();

        /// <summary>
        /// Get random decimal value.
        /// </summary>
        public static decimal Decimal() => _randomValueProvider.GetDecimal();

        /// <summary>
        /// Get random double value.
        /// </summary>
        public static double Double() => _randomValueProvider.GetDouble();

        /// <summary>
        /// Get random Enum value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        public static TEnum EnumValue<TEnum>() where TEnum : Enum => _randomValueProvider.GetEnum<TEnum>();

        /// <summary>
        /// Get random float value.
        /// </summary>
        public static float Float() => _randomValueProvider.GetFloat();

        /// <summary>
        /// Get random Guid value.
        /// </summary>
        public static Guid Guid() => _randomValueProvider.GetGuid();

        /// <summary>
        /// Get random int value.
        /// </summary>
        public static int Int() => _randomValueProvider.GetInt();

        /// <summary>
        /// Get random long value.
        /// </summary>
        public static long Long() => _randomValueProvider.GetLong();

        /// <summary>
        /// Get random sbyte value.
        /// </summary>
        public static sbyte SByte() => _randomValueProvider.GetSByte();

        /// <summary>
        /// Get random short value.
        /// </summary>
        public static short Short() => _randomValueProvider.GetShort();

        /// <summary>
        /// Get random string value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        public static string String(string prefix = null) => _randomValueProvider.GetString(prefix);

        /// <summary>
        /// Get random uint value.
        /// </summary>
        public static uint UInt() => _randomValueProvider.GetUInt();

        /// <summary>
        /// Get random ulong value.
        /// </summary>
        public static ulong ULong() => _randomValueProvider.GetULong();

        /// <summary>
        /// Get random ushort value.
        /// </summary>
        public static ushort UShort() => _randomValueProvider.GetUShort();
    }
}
