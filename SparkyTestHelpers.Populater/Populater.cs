using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.Population
{
    /// <summary>
    /// This class populates class instances with test data.
    /// </summary>
    public class Populater
    {
        /// <summary>
        /// Maximum "depth" of "child" class instances to create (default value is 5).
        /// </summary>
        public int MaximumDepth { get; set; } = 5;

        private static readonly TypeInfo _iEnumerableTypeInfo = typeof(IEnumerable).GetTypeInfo();
        private static readonly TypeInfo _iListTypeInfo = typeof(IList).GetTypeInfo();

        private static readonly Dictionary<Type, Func<IPopulaterValueProvider, string, object>> _generatorDictionary
            = new Dictionary<Type, Func<IPopulaterValueProvider, string, object>>
            {
                {typeof(bool), GetBool},
                {typeof(bool?), GetBool},
                {typeof(byte), GetByte},
                {typeof(byte?), GetByte},
                {typeof(byte[]), GetByteArray},
                {typeof(char), GetChar},
                {typeof(char?), GetChar},
                {typeof(DateTime), GetDateTime},
                {typeof(DateTime?), GetDateTime},
                {typeof(decimal), GetDecimal},
                {typeof(decimal?), GetDecimal},
                {typeof(double), GetDouble},
                {typeof(double?), GetDouble},
                {typeof(float), GetFloat},
                {typeof(float?), GetFloat},
                {typeof(Guid), GetGuid},
                {typeof(Guid?), GetGuid},
                {typeof(int), GetInt},
                {typeof(int?), GetInt},
                {typeof(long), GetLong},
                {typeof(long?), GetLong},
                {typeof(object), GetString},
                {typeof(sbyte), GetSByte},
                {typeof(sbyte?), GetSByte},
                {typeof(short), GetShort},
                {typeof(short?), GetShort},
                {typeof(string), GetString},
                {typeof(uint), GetUInt},
                {typeof(uint?), GetUInt},
                {typeof(ulong), GetULong},
                {typeof(ulong?), GetULong},
                {typeof(ushort), GetUShort},
                {typeof(ushort?), GetUShort}
            };

        /// <summary>
        /// Sets <see cref="MaximumDepth"/> value.
        /// </summary>
        /// <param name="maximumDepth">The maximum depth.</param>
        /// <returns>"This" <see cref="Populater"/> instance.</returns>
        public Populater WithMaximumDepth(int maximumDepth)
        {
            MaximumDepth = maximumDepth;
            return this;
        }

        /// <summary>
        /// Create new instance of <typeparamref name="T"/> and populate with test data.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="valueProvider">The <see cref="IPopulaterValueProvider"/>
        /// (defaults to <see cref="SequentialValueProvider"/>).</param>
        /// <returns>The created and populated instance of <typeparamref name="T"/>.</returns>
        public T CreateAndPopulate<T>(IPopulaterValueProvider valueProvider = null)
        {
            return Populate((T)Activator.CreateInstance(typeof(T)), valueProvider);
        }

        /// <summary>
        /// Create <see cref="IEnumerable{T}"/> and populate with test data.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="count">The desired <see cref="IEnumerable{T}"/> count.</param>
        /// <param name="valueProvider">The <see cref="IPopulaterValueProvider"/>
        /// (defaults to <see cref="SequentialValueProvider"/>).</param>
        /// <returns>The created and populated instance of <typeparamref name="T"/>.</returns>
        public IEnumerable<T> CreateIEnumerableOf<T>(int count, IPopulaterValueProvider valueProvider = null)
        {
            valueProvider = valueProvider ?? new SequentialValueProvider();

            return Enumerable.Range(1, count).Select(_ => CreateAndPopulate<T>(valueProvider));
        }

        /// <summary>
        /// Create <see cref="IEnumerable{T}"/> and populate with test data.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="count">The desired <see cref="IEnumerable{T}"/> count.</param>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>The created and populated instance of <typeparamref name="T"/>.</returns>
        public IEnumerable<T> CreateRandomIEnumerableOf<T>(int count, Action<T> callback = null)
        {
            var valueProvider = new RandomValueProvider();

            T[] items = Enumerable.Range(1, count).Select(_ => CreateAndPopulate<T>(valueProvider)).ToArray();

            if (callback != null)
            {
                foreach (T item in items)
                {
                    callback(item);
                }
            }

            return items;
        }

        /// <summary>
        /// Populate existing instance of <typeparamref name="T"/> with test data.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="instance">The instance of <typeparamref name="T"/>.</param>
        /// <param name="valueProvider">The <see cref="IPopulaterValueProvider"/>
        /// (defaults to <see cref="SequentialValueProvider"/>.</param>
        /// <returns>The populated <typeparamref name="T"/> instance.</returns>
        public T Populate<T>(T instance, IPopulaterValueProvider valueProvider = null)
        {
            valueProvider = valueProvider ?? new SequentialValueProvider();

            int depth = 1;

            Populate(instance, valueProvider, typeof(T).GetTypeInfo(), depth);

            return instance;
        }

        /// <summary>
        /// Create new instance of <typeparamref name="T"/> and populate with random values.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="callback">Optional "callback" function to perform additional property assignments.</param>
        /// <returns>The created and populated instance of <typeparamref name="T"/>.</returns>
        public T CreateRandom<T>(Action<T> callback = null)
        {
            T instance = CreateAndPopulate<T>(new RandomValueProvider());

            callback?.Invoke(instance);

            return instance;
        }

        /// <summary>
        /// Populate existing instance of <typeparamref name="T"/> with random test data.
        /// </summary>
        /// <typeparam name="T">The class type.</typeparam>
        /// <param name="instance">The instance of <typeparamref name="T"/>.</param>
        /// <returns>The populated <typeparamref name="T"/> instance.</returns>
        public T PopulateWithRandomValues<T>(T instance) => Populate(instance, new RandomValueProvider());

        private void Populate(object instance, IPopulaterValueProvider valueProvider, TypeInfo typeInfo, int depth)
        {
            PropertyInfo[] publicReadWriterProperties = typeInfo
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(propertyInfo => propertyInfo.CanRead && propertyInfo.CanWrite)
                .ToArray();

            foreach (PropertyInfo property in publicReadWriterProperties)
            {
                object value = GetValue(valueProvider, property.PropertyType, property.Name, depth);

                if (value != null)
                {
                    property.SetValue(instance, value, null);
                }
            }
        }

        /// <summary>
        /// Get value.
        /// </summary>
        /// <param name="valueProvider">The <see cref="IPopulaterValueProvider"/>.</param>
        /// <param name="type">The value type.</param>
        /// <param name="prefix">Prefix for string values.</param>
        /// <param name="depth">Depth of value within object hierarchy.</param>
        /// <returns>The value.</returns>
        protected object GetValue(IPopulaterValueProvider valueProvider, Type type, string prefix, int depth)
        {
            object value = null;
            TypeInfo typeInfo = type.GetTypeInfo();

            if (_generatorDictionary.ContainsKey(type))
            {
                value = _generatorDictionary[type](valueProvider, prefix);
            }
            else if (typeInfo.IsArray)
            {
                value = GetArray(valueProvider, type.GetElementType(), prefix, depth + 1);
            }
            else if (_iListTypeInfo.IsAssignableFrom(type))
            {
                value = GetList(valueProvider, typeInfo, prefix, depth + 1);
            }
            else if (_iEnumerableTypeInfo.IsAssignableFrom(type))
            {
                value = GetIEnumerable(valueProvider, typeInfo, prefix, depth + 1);
            }
            else if (typeInfo.IsClass)
            {
                value = GetClassInstance(valueProvider, type, depth + 1);
            }
            else if (typeInfo.IsEnum)
            {
                value = valueProvider.GetEnum(type);
            }

            return value;
        }

        private Array GetArray(IPopulaterValueProvider valueProvider, Type type, string prefix, int depth)
        {
            Array array = null;

            if (type != null)
            {
                int itemCount = valueProvider.GetEnumerableSize();

                array = TryToCreateInstance(type, () => Array.CreateInstance(type, itemCount)) as Array;

                if (array != null)
                {
                    for (int i = 0; i < itemCount; i++)
                    {
                        string childPrefix = $"{prefix}[{i}]";
                        var item = GetValue(valueProvider, type, childPrefix, depth);
                        array.SetValue(item, i);
                    }
                }
            }

            return array;
        }

        private static object GetBool(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetBool();

        private static object GetByte(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetByte();

        private static object GetByteArray(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetByteArray();

        private static object GetChar(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetChar();

        private object GetClassInstance(IPopulaterValueProvider valueProvider, Type type, int depth)
        {
            if (MaximumDepth < depth)
            {
                return null;
            }

            object value = null;

            value = TryToCreateInstance(type, () => Activator.CreateInstance(type));

            if (value != null)
            {
                Populate(value, valueProvider, type.GetTypeInfo(), depth);
            }

            return value;
        }

        private static object GetDateTime(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetDateTime();

        private static object GetDecimal(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetDecimal();

        private static object GetDouble(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetDouble();

        private static object GetFloat(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetFloat();

        private static object GetGuid(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetGuid();

        private object GetIEnumerable(IPopulaterValueProvider valueProvider, TypeInfo typeInfo, string prefix, int depth)
        {
            Array array = null;
            Type[] types = typeInfo.GetGenericArguments();

            if (types.Length == 1)
            {
                array = GetArray(valueProvider, types[0], prefix, depth);
            }

            return array;
        }

        private static object GetInt(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetInt();

        private object GetList(IPopulaterValueProvider valueProvider, TypeInfo typeInfo, string prefix, int depth)
        {
            object list = null;

            Type[] genericArguments = typeInfo.GetGenericArguments();

            if (genericArguments.Length == 1)
            {
                Array array = GetArray(valueProvider, genericArguments[0], prefix, depth);

                if (array != null)
                {
                    Type genericListType = typeof(List<>).MakeGenericType(genericArguments);

                    TryToCreateInstance(genericListType, () => list = Activator.CreateInstance(genericListType, array));
                }
            }

            return list;
        }

        private static object GetLong(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetLong();

        private static object GetSByte(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetSByte();

        private static object GetShort(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetShort();

        private static object GetString(IPopulaterValueProvider valueProvider, string prefix) => valueProvider.GetString(prefix);

        private static object GetUInt(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetUInt();

        private static object GetULong(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetULong();

        private static object GetUShort(IPopulaterValueProvider valueProvider, string _) => valueProvider.GetUShort();

        private static object TryToCreateInstance(Type type, Func<object> callback)
        {
            object value = null;

            try
            {
                value = callback();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{nameof(Populater)} was unable to create a {type.FullName} instance: {ex.Message}.");
            }

            return value;
        }
    }
}
