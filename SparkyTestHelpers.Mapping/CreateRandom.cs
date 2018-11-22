using System;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Test helper methods using <see cref="RandomValuesHelper"/> to generate random values
    /// (usually for testing "mapping" from one type to another).
    /// </summary>
    public static class CreateRandom
    {
        private static readonly RandomValuesHelper _randomValuesHelper = new RandomValuesHelper();

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
            int? savedMaximumDepth = _randomValuesHelper.MaximumDepth;
            int savedMaximumIEnumerableSize = _randomValuesHelper.MaximumIEnumerableSize;

            try
            {
                if (maximumDepth.HasValue)
                {
                    _randomValuesHelper.MaximumDepth = maximumDepth;
                }

                if (maximumIEnumerableSize.HasValue)
                {
                    _randomValuesHelper.MaximumIEnumerableSize = maximumIEnumerableSize.Value;
                }

                return _randomValuesHelper.CreateRandom(callback);
            }
            finally
            {
                _randomValuesHelper.MaximumDepth = savedMaximumDepth;
                _randomValuesHelper.MaximumIEnumerableSize = savedMaximumIEnumerableSize;
            }
        }

        /// <summary>
        /// Get random bool value.
        /// </summary>
        public static bool Bool() => _randomValuesHelper.RandomBool();

        /// <summary>
        /// Get random byte value.
        /// </summary>
        public static byte Byte() => _randomValuesHelper.RandomByte();

        /// <summary>
        /// Get random char value.
        /// </summary>
        public static char Char() => _randomValuesHelper.RandomChar();

        /// <summary>
        /// Get random DateTime value.
        /// </summary>
        public static DateTime DateTime() => _randomValuesHelper.RandomDateTime();

        /// <summary>
        /// Get random decimal value.
        /// </summary>
        public static decimal Decimal() => _randomValuesHelper.RandomDecimal();

        /// <summary>
        /// Get random double value.
        /// </summary>
        public static double Double() => _randomValuesHelper.RandomDouble();

        /// <summary>
        /// Get random Enum value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        public static TEnum EnumValue<TEnum>() where TEnum : struct, IConvertible => _randomValuesHelper.RandomEnumValue<TEnum>();

        /// <summary>
        /// Get random float value.
        /// </summary>
        public static float Float() => _randomValuesHelper.RandomFloat();

        /// <summary>
        /// Get random Guid value.
        /// </summary>
        public static Guid Guid() => _randomValuesHelper.RandomGuid();

        /// <summary>
        /// Get random int value.
        /// </summary>
        public static int Int() => _randomValuesHelper.RandomInt();

        /// <summary>
        /// Get random long value.
        /// </summary>
        public static long Long() => _randomValuesHelper.RandomLong();

        /// <summary>
        /// Get random sbyte value.
        /// </summary>
        public static sbyte SByte() => _randomValuesHelper.RandomSByte();

        /// <summary>
        /// Get random short value.
        /// </summary>
        public static short Short() => _randomValuesHelper.RandomShort();

        /// <summary>
        /// Get random string value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        public static string String(string prefix = null) => _randomValuesHelper.RandomString(prefix);

        /// <summary>
        /// Get random uint value.
        /// </summary>
        public static uint UInt() => _randomValuesHelper.RandomUInt();

        /// <summary>
        /// Get random ulong value.
        /// </summary>
        public static ulong ULong() => _randomValuesHelper.RandomULong();

        /// <summary>
        /// Get random ushort value.
        /// </summary>
        public static ushort UShort() => _randomValuesHelper.RandomUShort();
    }
}
