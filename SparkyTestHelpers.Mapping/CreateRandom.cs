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
        /// Get random <see cref="Bool"/> value.
        /// </summary>
        /// <returns>Random <see cref="Bool"/> value.</returns>
        public static bool Bool() => GetRandom.Bool();

        /// <summary>
        /// Get random <see cref="Byte"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Byte"/> value.</returns>
        public static byte Byte() => GetRandom.Byte();

        /// <summary>
        /// Get random <see cref="Char"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Char"/> value.</returns>
        public static char Char() => GetRandom.Char();

        /// <summary>
        /// Get random <see cref="DateTime"/>  value.
        /// </summary>
        /// <returns>Random <see cref="DateTime"/> value.</returns>
        public static DateTime DateTime() => GetRandom.DateTime();

        /// <summary>
        /// Get random <see cref="Decimal"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Decimal"/> value.</returns>
        public static decimal Decimal() => GetRandom.Decimal();

        /// <summary>
        /// Get random <see cref="Double"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Double"/> value.</returns>
        public static double Double() => GetRandom.Double();

        /// <summary>
        /// Get random <see cref="Enum"/>  value.
        /// </summary>
        /// <typeparam name="TEnum">The <see cref="Enum"/> type.</typeparam>
        /// <returns>Random <typeparamref name="TEnum"/> value.</returns>
        public static TEnum EnumValue<TEnum>() where TEnum : Enum => GetRandom.EnumValue<TEnum>();

        /// <summary>
        /// Get random <see cref="Float"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Float"/> value.</returns>
        public static float Float() => GetRandom.Float();

        /// <summary>
        /// Get random <see cref="Guid"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Guid"/> value.</returns>
        public static Guid Guid() => GetRandom.Guid();

        /// <summary>
        /// Get random <see cref="Int"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Int"/> value.</returns>
        public static int Int() => GetRandom.Int();

        /// <summary>
        /// Get random <see cref="Long"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Long"/> value.</returns>
        public static long Long() => GetRandom.Long();

        /// <summary>
        /// Get random <see cref="SByte"/>  value.
        /// </summary>
        /// <returns>Random <see cref="SByte"/> value.</returns>
        public static sbyte SByte() => GetRandom.SByte();

        /// <summary>
        /// Get random <see cref="Short"/>  value.
        /// </summary>
        /// <returns>Random <see cref="Short"/> value.</returns>
        public static short Short() => GetRandom.Short();

        /// <summary>
        /// Get random <see cref="String"/>  value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        /// <returns>Random <see cref="String"/> value.</returns>
        public static string String(string prefix = null) => GetRandom.String(prefix);

        /// <summary>
        /// Get random <see cref="UInt"/>  value.
        /// </summary>
        /// <returns>Random <see cref="UInt"/> value.</returns>
        public static uint UInt() => GetRandom.UInt();

        /// <summary>
        /// Get random <see cref="ULong"/>  value.
        /// </summary>
        /// <returns>Random <see cref="ULong"/> value.</returns>
        public static ulong ULong() => GetRandom.ULong();

        /// <summary>
        /// Get random <see cref="UShort"/>  value.
        /// </summary>
        /// <returns>Random <see cref="UShort"/> value.</returns>
        public static ushort UShort() => GetRandom.UShort();
    }
}