using System;
using SparkyTestHelpers.Population;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Test helper methods using <see cref="RandomValuesHelper"/> to generate random values
    /// (usually for testing "mapping" from one type to another).
    /// </summary>
    [Obsolete("SparkyTestHelpers.Mapping.CreateRandom has been deprecated in favor of SparkyTestHelpers.Population.Populater.GetRandom")]
    public static class CreateRandom
    {
        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="callback">"Callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public static T InstanceOf<T>(Action<T> callback) where T : class => GetRandom.InstanceOf(callback);

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
            Action<T> callback = null) where T : class => GetRandom.InstanceOf<T>(maximumDepth, maximumIEnumerableSize, callback);

        /// <summary>
        /// Get random bool value.
        /// </summary>
        public static bool Bool() => GetRandom.Bool();

        /// <summary>
        /// Get random byte value.
        /// </summary>
        public static byte Byte() => GetRandom.Byte();

        /// <summary>
        /// Get random char value.
        /// </summary>
        public static char Char() => GetRandom.Char();

        /// <summary>
        /// Get random DateTime value.
        /// </summary>
        public static DateTime DateTime() => GetRandom.DateTime();

        /// <summary>
        /// Get random decimal value.
        /// </summary>
        public static decimal Decimal() => GetRandom.Decimal();

        /// <summary>
        /// Get random double value.
        /// </summary>
        public static double Double() => GetRandom.Double();

        /// <summary>
        /// Get random Enum value.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        public static TEnum EnumValue<TEnum>() where TEnum : Enum => GetRandom.EnumValue<TEnum>();

        /// <summary>
        /// Get random float value.
        /// </summary>
        public static float Float() => GetRandom.Float();

        /// <summary>
        /// Get random Guid value.
        /// </summary>
        public static Guid Guid() => GetRandom.Guid();

        /// <summary>
        /// Get random int value.
        /// </summary>
        public static int Int() => GetRandom.Int();

        /// <summary>
        /// Get random long value.
        /// </summary>
        public static long Long() => GetRandom.Long();

        /// <summary>
        /// Get random sbyte value.
        /// </summary>
        public static sbyte SByte() => GetRandom.SByte();

        /// <summary>
        /// Get random short value.
        /// </summary>
        public static short Short() => GetRandom.Short();

        /// <summary>
        /// Get random string value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        public static string String(string prefix = null) => GetRandom.String(prefix);

        /// <summary>
        /// Get random uint value.
        /// </summary>
        public static uint UInt() => GetRandom.UInt();

        /// <summary>
        /// Get random ulong value.
        /// </summary>
        public static ulong ULong() => GetRandom.ULong();

        /// <summary>
        /// Get random ushort value.
        /// </summary>
        public static ushort UShort() => GetRandom.UShort();
    }
}