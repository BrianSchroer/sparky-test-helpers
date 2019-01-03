using System;
using SparkyTestHelpers.Population;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Test helper for updating class instance properties with random values
    /// (usually for testing "mapping" from one type to another).
    /// </summary>
    [Obsolete("SparkyTestHelpers.Mapping.RandomValuesHelper has been deprecated in favor of SparkyTestHelpers.Population Populater/RandomValueProvider")]
    public class RandomValuesHelper
    {
        private readonly Populater _populater = new Populater();
        private readonly RandomValueProvider _randomValueProvider = new RandomValueProvider();

        /// <summary>
        /// Maximum number of items to generate for arrays / lists / IEnumerables (default value is 3).
        /// </summary>
        public int MaximumIEnumerableSize
        {
            get => _randomValueProvider.MaximumIEnumerableSize;
            set => _randomValueProvider.MaximumIEnumerableSize = value;
        }

        /// <summary>
        /// Maximum "depth" of "child" class instances to create.
        /// </summary>
        public int? MaximumDepth
        {
            get => _populater.MaximumDepth;

            set
            {
                if (value != null)
                {
                    _populater.MaximumDepth = value.Value;
                }
            }
        }

        /// <summary>
        /// Sets <see cref="MaximumIEnumerableSize"/> value.
        /// </summary>
        /// <param name="maximumIEnumerableSize">The maximum IENumerable size.</param>
        /// <returns>"This" <see cref="RandomValuesHelper"/> instance.</returns>
        // ReSharper disable once InconsistentNaming
        public RandomValuesHelper WithMaximumIENumerableSize(int maximumIEnumerableSize)
        {
            MaximumIEnumerableSize = maximumIEnumerableSize;
            return this;
        }

        /// <summary>
        /// Sets <see cref="MaximumDepth"/> value.
        /// </summary>
        /// <param name="maximumDepth">The maximum depth.</param>
        /// <returns>"This" <see cref="RandomValuesHelper"/> instance.</returns>
        public RandomValuesHelper WithMaximumDepth(int maximumDepth)
        {
            MaximumDepth = maximumDepth;
            return this;
        }

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <remarks>"Pseudonym" of <see cref="CreateRandom{T}"/></remarks>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public T CreateInstanceWithRandomValues<T>(Action<T> callback = null)
        {
            T instance = _populater.CreateAndPopulate<T>(_randomValueProvider);

            callback?.Invoke(instance);

            return instance;
        }

        /// <summary>
        /// Create an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <remarks>"Pseudonym" of <see cref="CreateInstanceWithRandomValues{T}"/></remarks>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>New instance.</returns>
        public T CreateRandom<T>(Action<T> callback = null)
        {
            return CreateInstanceWithRandomValues<T>(callback);
        }

        /// <summary>
        /// Update <typeparamref name="T"/> instance properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="instance">An instance of <typeparamref name="T"/>.</param>
        /// <returns>The <paramref name="instance"/> with updated property values.</returns>
        public T UpdatePropertiesWithRandomValues<T>(T instance)
        {
            return _populater.Populate(instance, _randomValueProvider);
        }

        /// <summary>
        /// Get random Enum value.
        /// </summary>
        /// <typeparam name="TEnum">The Enum type.</typeparam>
        /// <returns>Random <typeparamref name="TEnum"/> value.</returns>
        public TEnum RandomEnumValue<TEnum>() where TEnum : Enum => _randomValueProvider.GetEnum<TEnum>();

        /// <summary>
        /// Get random bool value.
        /// </summary>
        /// <returns>Random <see cref="bool" /> value.</returns>
        public bool RandomBool() => _randomValueProvider.GetBool();

        /// <summary>
        /// Get random byte value.
        /// </summary>
        /// <returns>Random <see cref="byte" /> value.</returns>
        public byte RandomByte() => _randomValueProvider.GetByte();

        /// <summary>
        /// Get random char value.
        /// </summary>
        /// <returns>Random <see cref="char" /> value.</returns>
        public char RandomChar() => _randomValueProvider.GetChar();

        /// <summary>
        /// Get random DateTime value.
        /// </summary>
        /// <returns>Random <see cref="DateTime" /> value.</returns>
        public DateTime RandomDateTime() => _randomValueProvider.GetDateTime();

        /// <summary>
        /// Get random decimal value.
        /// </summary>
        /// <returns>Random <see cref="decimal" /> value.</returns>
        public decimal RandomDecimal() => _randomValueProvider.GetDecimal();

        /// <summary>
        /// Get random double value.
        /// </summary>
        /// <returns>Random <see cref="double" /> value.</returns>
        public double RandomDouble() => _randomValueProvider.GetDouble();

        /// <summary>
        /// Get random Guid value.
        /// </summary>
        /// <returns>Random <see cref="Guid" /> value.</returns>
        public Guid RandomGuid() => _randomValueProvider.GetGuid();

        /// <summary>
        /// Get random int value.
        /// </summary>
        /// <returns>Random <see cref="int" /> value.</returns>
        public int RandomInt() => _randomValueProvider.GetInt();

        /// <summary>
        /// Get random uint value.
        /// </summary>
        /// <returns>Random <see cref="uint" /> value.</returns>
        public uint RandomUInt() => _randomValueProvider.GetUInt();

        /// <summary>
        /// Get random short value.
        /// </summary>
        /// <returns>Random <see cref="short" /> value.</returns>
        public short RandomShort() => _randomValueProvider.GetShort();

        /// <summary>
        /// Get random ushort value.
        /// </summary>
        /// <returns>Random <see cref="ushort" /> value.</returns>
        public ushort RandomUShort() => _randomValueProvider.GetUShort();

        /// <summary>
        /// Get random long value.
        /// </summary>
        /// <returns>Random <see cref="long" /> value.</returns>
        public long RandomLong() => _randomValueProvider.GetLong();

        /// <summary>
        /// Get random ulong value.
        /// </summary>
        /// <returns>Random <see cref="ulong" /> value.</returns>
        public ulong RandomULong() => _randomValueProvider.GetULong();

        /// <summary>
        /// Get random float value.
        /// </summary>
        /// <returns>Random <see cref="float" /> value.</returns>
        public float RandomFloat() => _randomValueProvider.GetFloat();

        /// <summary>
        /// Get random sbyte value.
        /// </summary>
        /// <returns>Random <see cref="sbyte" /> value.</returns>
        public sbyte RandomSByte() => _randomValueProvider.GetSByte();

        /// <summary>
        /// Get random string value.
        /// </summary>
        /// <param name="prefix">optional string prefix.</param>
        /// <returns>Random <see cref="string"/> value.</returns>
        public string RandomString(string prefix = null) => _randomValueProvider.GetString(prefix);
    }
}
