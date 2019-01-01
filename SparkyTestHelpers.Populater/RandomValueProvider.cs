using System;

namespace SparkyTestHelpers.Populater
{
    /// <summary>
    /// Random value provider for <see cref="Populater"/>.
    /// </summary>
    public class RandomValueProvider : IPopulaterValueProvider
    {
        private readonly Random _random;

        /// <summary>
        /// Maximum number of items to generate for arrays / lists / IEnumerables (default value is 3).
        /// </summary>
        public int MaximumIEnumerableSize { get; set; } = 3;

        /// <summary>
        /// Creates a new <see cref="RandomValueProvider"/> instance.
        /// </summary>
        public RandomValueProvider()
        {
            _random = new Random();
        }

        /// <inheritdoc />
        public virtual bool GetBool() => (GetInt() % 2 == 0);

        /// <inheritdoc />
        public virtual byte GetByte() => GetByteArray()[0];

        /// <inheritdoc />
        public byte[] GetByteArray()
        {
            var buffer = new byte[_random.Next(10, 100)];
            _random.NextBytes(buffer);
            return buffer;
        }

        /// <inheritdoc />
        public virtual char GetChar() => GetInt().ToString()[0];

        /// <inheritdoc />
        public virtual DateTime GetDateTime() => DateTime.Today.AddDays(_random.Next(-1000, 1000));

        /// <inheritdoc />
        public virtual decimal GetDecimal() => Convert.ToDecimal(_random.Next(1_000_000)) / 100;

        /// <inheritdoc />
        public virtual double GetDouble() => Convert.ToDouble(_random.Next(1_000_000)) / 100;

        /// <inheritdoc />
        public virtual TEnum GetEnum<TEnum>() where TEnum : Enum => (TEnum) GetEnum(typeof(TEnum));

        /// <inheritdoc />
        public object GetEnum(Type enumType)
        {
            Array values = Enum.GetValues(enumType);

            int index = _random.Next(0, values.Length - 1);

            return values.GetValue(index);
        }

        /// <inheritdoc />
        public virtual int GetEnumerableSize() => _random.Next(1, MaximumIEnumerableSize);

        /// <inheritdoc />
        public virtual float GetFloat() => (float)Convert.ToDouble(_random.Next(10_000_000)) / 1000;

        /// <inheritdoc />
        public virtual Guid GetGuid() => Guid.NewGuid();

        /// <inheritdoc />
        public virtual int GetInt() => _random.Next();

        /// <inheritdoc />
        public virtual long GetLong() => GetInt();

        /// <inheritdoc />
        public virtual sbyte GetSByte() => Convert.ToSByte(_random.Next(Convert.ToInt32(sbyte.MinValue), Convert.ToInt32(sbyte.MaxValue)));

        /// <inheritdoc />
        public virtual short GetShort() => Convert.ToInt16(_random.Next(Int16.MinValue, Int16.MaxValue));

        /// <inheritdoc />
        public virtual string GetString(string prefix = null) => $"{prefix}{GetInt()}";

        /// <inheritdoc />
        public virtual uint GetUInt() => Convert.ToUInt32(_random.Next(0, Int32.MaxValue));

        /// <inheritdoc />
        public virtual ulong GetULong() => Convert.ToUInt64(_random.Next(0, Int32.MaxValue));

        /// <inheritdoc />
        public virtual ushort GetUShort() => Convert.ToUInt16(_random.Next(UInt16.MinValue, UInt16.MaxValue));
    }
}