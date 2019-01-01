using System;
using System.Text;

namespace SparkyTestHelpers.Populater
{
    /// <summary>
    /// Sequential value provider for <see cref="Populater"/>.
    /// </summary>
    public class SequentialValueProvider : IPopulaterValueProvider
    {
        private int _value;

        /// <summary>
        /// Base date to be used for populating <see cref="DateTime"/> properties.
        /// </summary>
        public DateTime BaseDate { get; set; } = new DateTime(2019, 1, 1);

        /// <summary>
        /// Number of items to generate for arrays / lists / IEnumerables (default value is 3).
        /// </summary>
        public int EnumerableSize { get; set; } = 3;

        /// <summary>
        /// Creates a new <see cref="SequentialValueProvider"/> instance.
        /// </summary>
        public SequentialValueProvider()
        {
            SetValue(0);
        }

        /// <inheritdoc />
        public virtual bool GetBool() => (GetInt() % 2 == 0);

        /// <inheritdoc />
        public virtual byte GetByte()
        {
            byte[] bytes = GetByteArray();
            return bytes[bytes.Length - 1];
        }

        /// <inheritdoc />
        public byte[] GetByteArray() => Encoding.ASCII.GetBytes(GetString());

        /// <inheritdoc />
        public virtual char GetChar() => GetInt().ToString()[0];

        /// <inheritdoc />
        public virtual DateTime GetDateTime() => BaseDate.AddDays(GetInt());

        /// <inheritdoc />
        public virtual decimal GetDecimal() => Convert.ToDecimal(GetAmountString());

        /// <inheritdoc />
        public virtual double GetDouble() => Convert.ToDouble(GetAmountString());

        /// <inheritdoc />
        public virtual TEnum GetEnum<TEnum>() where TEnum : Enum => (TEnum) GetEnum(typeof(TEnum));

        /// <inheritdoc />
        public object GetEnum(Type enumType)
        {
            Array values = Enum.GetValues(enumType);

            int index = NextIntValue(values.Length) - 1;

            return values.GetValue(index);
        }

        /// <inheritdoc />
        public virtual int GetEnumerableSize() => EnumerableSize;

        /// <inheritdoc />
        public virtual float GetFloat() => (float) GetDouble();

        /// <inheritdoc />
        public virtual Guid GetGuid() => Guid.Parse(GetInt().ToString("00000000-0000-0000-0000-000000000000"));

        /// <inheritdoc />
        public virtual int GetInt() => NextIntValue(int.MaxValue);

        /// <inheritdoc />
        public virtual long GetLong() => GetInt();

        /// <inheritdoc />
        public virtual sbyte GetSByte() => Convert.ToSByte(NextIntValue(SByte.MaxValue));

        /// <inheritdoc />
        public virtual short GetShort() => Convert.ToInt16(NextIntValue(Int16.MaxValue));

        /// <inheritdoc />
        public virtual string GetString(string prefix = null) =>
            (string.IsNullOrWhiteSpace(prefix)) ? GetGuid().ToString() : $"{prefix}-{GetInt()}";

        /// <inheritdoc />
        public virtual uint GetUInt() => Convert.ToUInt32(NextIntValue(Int32.MaxValue));

        /// <inheritdoc />
        public virtual ulong GetULong() => Convert.ToUInt64(NextIntValue(Int32.MaxValue));

        /// <inheritdoc />
        public virtual ushort GetUShort() => Convert.ToUInt16(NextIntValue(UInt16.MaxValue));

        internal void SetValue(int value) => _value = value;

        private string GetAmountString()
        {
            int intValue = GetInt();

            return $"{intValue}.{GetRemainder(intValue, 100, true):00}";
        }

        private int NextIntValue(int maxValue, bool allowZero = false)
        {
            if (_value == int.MaxValue)
            {
                _value = 0;
            }

            return GetRemainder(++_value, maxValue, allowZero);
        }

        private int GetRemainder(int value, int maxValue, bool allowZero = false)
        {
            int remainder = value % maxValue;

            return (remainder == 0 && !allowZero) ? maxValue : remainder;
        }
    }
}