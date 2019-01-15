using System;
using System.Collections.Generic;
using System.Linq;

namespace SparkyTestHelpers.Population
{
    /// <summary>
    /// Test helper methods using <see cref="RandomValueProvider"/> to generate random values.
    /// </summary>
    public static class GetRandom
    {
        private static readonly RandomValueProvider _randomValueProvider = new RandomValueProvider();
        private static Random _random = new Random();

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="randomValueProvider">The <see cref="RandomValueProvider"/>.</param>
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create (default value is 5).</param>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public static T InstanceOf<T>(
            RandomValueProvider randomValueProvider,
            int? maximumDepth = null,
            Action<T> callback = null) where T : class
        {
            var populater = new Populater();

            if (maximumDepth.HasValue) populater.MaximumDepth = maximumDepth.Value;

            var instance = populater.CreateAndPopulate<T>(randomValueProvider);

            callback?.Invoke(instance);

            return instance;
        }

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="randomValueProvider">The <see cref="RandomValueProvider"/>.</param>
        /// <param name="callback">"Callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public static T InstanceOf<T>(RandomValueProvider randomValueProvider, Action<T> callback) where T : class
        {
            return InstanceOf(randomValueProvider, (int?)null, callback);
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
            int savedMaximumIEnumerableSize = _randomValueProvider.MaximumIEnumerableSize;

            try
            {
                if (maximumIEnumerableSize.HasValue) _randomValueProvider.MaximumIEnumerableSize = maximumIEnumerableSize.Value;

                return InstanceOf(_randomValueProvider, maximumDepth, callback);
            }
            finally
            {
                _randomValueProvider.MaximumIEnumerableSize = savedMaximumIEnumerableSize;
            }
        }

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="callback">"Callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public static T InstanceOf<T>(Action<T> callback) where T : class
        {
            return InstanceOf((int?)null, (int?)null, callback);
        }

        /// <summary>
        /// Create an <see cref="IEnumerable{T}"/> with properties populated with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="randomValueProvider">The <see cref="RandomValueProvider"/>.</param>
        /// <param name="count">The desired <see cref="IEnumerable{T}"/> count.</param>
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create.</param>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>New <see cref="IEnumerable{T}"/>.</returns>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public static IEnumerable<T> IEnumerableOf<T>(
            RandomValueProvider randomValueProvider,
            int count,
            int? maximumDepth = null,
            Action<T> callback = null) where T : class
        {
            var populater = new Populater();

            if (maximumDepth.HasValue) populater.MaximumDepth = maximumDepth.Value;

            T[] items = populater.CreateIEnumerableOf<T>(count, randomValueProvider).ToArray();

            if (callback != null)
                foreach (T item in items) callback(item);

            return items;
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
            int savedMaximumIEnumerableSize = _randomValueProvider.MaximumIEnumerableSize;

            try
            {
                if (maximumIEnumerableSize.HasValue) _randomValueProvider.MaximumIEnumerableSize = maximumIEnumerableSize.Value;

                return IEnumerableOf(_randomValueProvider, count, maximumDepth, callback);
            }
            finally
            {
                _randomValueProvider.MaximumIEnumerableSize = savedMaximumIEnumerableSize;
            }
        }

        /// <summary>
        /// Populate existing instance of <typeparamref name="T"/> with random values.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="randomValueProvider">The <see cref="RandomValueProvider"/> instance.</param>
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create (default value is 5).</param>
        /// <returns>Populated instance of <typeparamref name="T"/>.</returns>
        public static T ValuesFor<T>(T instance, RandomValueProvider randomValueProvider, int? maximumDepth = null) where T : class
        {
            var populater = new Populater();

            if (maximumDepth.HasValue) populater.MaximumDepth = maximumDepth.Value;

            return populater.Populate(instance, randomValueProvider);
        }

        /// <summary>
        /// Populate existing instance of <typeparamref name="T"/> with random values.
        /// </summary>
        /// <typeparam name="T">The instance type.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <param name="maximumDepth">Optional maximum "depth" of "child" class instances to create (default value is 5).</param>
        /// <param name="maximumIEnumerableSize">Optional maximum number of items to generate for arrays / lists / IEnumerables (default value is 3).</param>
        /// <returns>Populated instance of <typeparamref name="T"/>.</returns>
        public static T ValuesFor<T>(T instance,
            int? maximumDepth = null,
            int? maximumIEnumerableSize = null) where T : class
        {
            int savedMaximumIEnumerableSize = _randomValueProvider.MaximumIEnumerableSize;

            try
            {
                if (maximumIEnumerableSize.HasValue) _randomValueProvider.MaximumIEnumerableSize = maximumIEnumerableSize.Value;

                return ValuesFor(instance, _randomValueProvider, maximumDepth);
            }
            finally
            {
                _randomValueProvider.MaximumIEnumerableSize = savedMaximumIEnumerableSize;
            }
        }

        /// <summary>
        /// Get random <see cref="bool"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="bool"/> value.</returns>
        public static bool Bool(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetBool();
        }

        /// <summary>
        /// Get random <see cref="byte"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="byte"/> value.</returns>
        public static byte Byte(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetByte();
        }

        /// <summary>
        /// Get random <see cref="char"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="char"/> value.</returns>
        public static char Char(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetChar();
        }

        /// <summary>
        /// Get random <see cref="DateTime"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="DateTime"/> value.</returns>
        public static DateTime DateTime(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetDateTime();
        }

        /// <summary>
        /// Get random <see cref="decimal"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="decimal"/> value.</returns>
        public static decimal Decimal(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetDecimal();
        }

        /// <summary>
        /// Get random <see cref="double"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="double"/> value.</returns>
        public static double Double(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetDouble();
        }

        /// <summary>
        /// Get random <see cref="float"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="float"/> value.</returns>
        public static float Float(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetFloat();
        }

        /// <summary>
        /// Get random <see cref="Guid"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="Guid"/> value.</returns>
        public static Guid Guid(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetGuid();
        }

        /// <summary>
        /// Get random <see cref="int"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="int"/> value.</returns>
        public static int Int(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetInt();
        }

        /// <summary>
        /// Get random integer with the specified range.
        /// </summary>
        /// <param name="minValue">Minimum value.</param>
        /// <param name="maxValue">Maximum value.</param>
        /// <returns></returns>
        public static int IntegerInRange(int minValue, int maxValue)
        {
            return _random.Next(minValue, maxValue + 1);
        }

        /// <summary>
        /// Get random <see cref="long"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="long"/> value.</returns>
        public static long Long(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetLong();
        }

        /// <summary>
        /// Get random <see cref="sbyte"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="sbyte"/> value.</returns>
        public static sbyte SByte(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetSByte();
        }

        /// <summary>
        /// Get random <see cref="short"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="short"/> value.</returns>
        public static short Short(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetShort();
        }

        /// <summary>
        /// Get random <see cref="uint"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="uint"/> value.</returns>
        public static uint UInt(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetUInt();
        }

        /// <summary>
        /// Get random <see cref="ulong"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="ulong"/> value.</returns>
        public static ulong ULong(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetULong();
        }

        /// <summary>
        /// Get random <see cref="ushort"/> value.
        /// </summary>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="ushort"/> value.</returns>
        public static ushort UShort(RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetUShort();
        }

        /// <summary>
        /// Get random <see cref="Enum"/> value.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <typeparamref name="TEnum"/> value.</returns>
        public static TEnum EnumValue<TEnum>(RandomValueProvider randomValueProvider = null) where TEnum : Enum
        {
            return (randomValueProvider ?? _randomValueProvider).GetEnum<TEnum>();
        }

        /// <summary>
        /// Get random <see cref="string"/> value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        /// <param name="randomValueProvider">(Optional) <see cref="RandomValueProvider"/> override.</param>
        /// <returns>Random <see cref="string"/> value.</returns>
        public static string String(string prefix = null, RandomValueProvider randomValueProvider = null)
        {
            return (randomValueProvider ?? _randomValueProvider).GetString(prefix);
        }
    }
}