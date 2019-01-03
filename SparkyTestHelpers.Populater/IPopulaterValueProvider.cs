using System;

namespace SparkyTestHelpers.Population
{
    /// <summary>
    /// <see cref="Populater"/> value provider.
    /// </summary>
    public interface IPopulaterValueProvider
    {
        /// <summary>
        /// Gets bool value.
        /// </summary>
        /// <returns>A <see cref="bool" /> value.</returns>
        bool GetBool();

        /// <summary>
        /// Gets byte value.
        /// </summary>
        /// <returns>A <see cref="byte" /> value.</returns>
        byte GetByte();

        /// <summary>
        /// Gets byte array.
        /// </summary>
        /// <returns>A <see cref="byte"/> array.</returns>
        byte[] GetByteArray();

        /// <summary>
        /// Gets char value.
        /// </summary>
        /// <returns>A <see cref="char" /> value.</returns>
        char GetChar();

        /// <summary>
        /// Gets DateTime value.
        /// </summary>
        /// <returns>A <see cref="DateTime" /> value.</returns>
        DateTime GetDateTime();

        /// <summary>
        /// Gets decimal value.
        /// </summary>
        /// <returns>A <see cref="decimal" /> value.</returns>
        decimal GetDecimal();

        /// <summary>
        /// Gets double value.
        /// </summary>
        /// <returns>A <see cref="double" /> value.</returns>
        double GetDouble();

        /// <summary>
        /// Gets Enum value.
        /// </summary>
        /// <typeparam name="TEnum">The Enum type.</typeparam>
        /// <returns>A <typeparamref name="TEnum"/> value.</returns>
        TEnum GetEnum<TEnum>() where TEnum : Enum;

        /// <summary>
        /// Gets Enum value.
        /// </summary>
        /// <param name="enumType">The Enum type.</param>
        /// <returns>Enum value.</returns>
        object GetEnum(Type enumType);

        /// <summary>
        /// Gets size to be used for array / list / IEnumerable.
        /// </summary>
        /// <returns>Enumerable size.</returns>
        int GetEnumerableSize();

        /// <summary>
        /// Gets float value.
        /// </summary>
        /// <returns>A <see cref="float" /> value.</returns>
        float GetFloat();

        /// <summary>
        /// Gets Guid value.
        /// </summary>
        /// <returns>A <see cref="Guid" /> value.</returns>
        Guid GetGuid();

        /// <summary>
        /// Gets int value.
        /// </summary>
        /// <returns>An <see cref="int" /> value.</returns>
        int GetInt();

        /// <summary>
        /// Gets long value.
        /// </summary>
        /// <returns>A <see cref="long" /> value.</returns>
        long GetLong();

        /// <summary>
        /// Gets sbyte value.
        /// </summary>
        /// <returns>A <see cref="sbyte" /> value.</returns>
        sbyte GetSByte();

        /// <summary>
        /// Gets short value.
        /// </summary>
        /// <returns>A <see cref="short" /> value.</returns>
        short GetShort();

        /// <summary>
        /// Gets string value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        /// <returns>A <see cref="string"/> value.</returns>
        string GetString(string prefix = null);

        /// <summary>
        /// Gets uint value.
        /// </summary>
        /// <returns>A <see cref="uint" /> value.</returns>
        uint GetUInt();

        /// <summary>
        /// Gets ulong value.
        /// </summary>
        /// <returns>A <see cref="ulong" /> value.</returns>
        ulong GetULong();

        /// <summary>
        /// Gets ushort value.
        /// </summary>
        /// <returns>A <see cref="ushort" /> value.</returns>
        ushort GetUShort();
    }
}