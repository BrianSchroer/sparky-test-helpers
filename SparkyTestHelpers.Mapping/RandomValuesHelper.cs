using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// Test helper for updating class instance properties with random values
    /// (usually for testing "mapping" from one type to another).
    /// </summary>
    public class RandomValuesHelper
    {
        private static readonly Dictionary<Type, Func<Random, string, object>> _generatorDictionary
            = new Dictionary<Type, Func<Random, string, object>>
            {
                { typeof(string), RandomString },
                { typeof(bool), RandomBool },
                { typeof(bool?), RandomBool },
                { typeof(DateTime), RandomDateTime },
                { typeof(DateTime?), RandomDateTime },
                { typeof(decimal), RandomDecimal },
                { typeof(decimal?), RandomDecimal },
                { typeof(double), RandomDouble },
                { typeof(double?), RandomDouble },
                { typeof(int), RandomInt },
                { typeof(int?), RandomInt },
                { typeof(long), RandomLong },
                { typeof(long?), RandomLong }
            };

        private static readonly Type _iEnumerableType = typeof(IEnumerable);
        private static readonly Type _iListType = typeof(IList);

        private Dictionary<Type, int> _createdTypes;

        /// <summary>
        /// Creae an instance of the specified type and populate its properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <returns>New instance.</returns>
        public T CreateInstanceWithRandomValues<T>()
        {
            return UpdatePropertiesWithRandomValues((T)Activator.CreateInstance(typeof(T)));
        }

        /// <summary>
        /// Update <typeparamref name="T"/> instance properties with random values.
        /// </summary>
        /// <typeparam name="T">The type of the instance for which properties are to be updated.</typeparam>
        /// <param name="instance">An instance of <typeparamref name="T"/>.</param>
        /// <returns>The <paramref name="instance"/> with updated property values.</returns>
        public T UpdatePropertiesWithRandomValues<T>(T instance)
        {
            var random = new Random();
            int depth = 0;
            _createdTypes = new Dictionary<Type, int>();

            UpdatePropertiesWithRandomValues(instance, random, typeof(T), depth);

            return instance;
        }

        private void UpdatePropertiesWithRandomValues(object instance, Random random, Type typ, int depth)
        {
            PropertyInfo[] properties = PropertyEnumerator.GetPublicInstanceReadWriteProperties(typ);

            foreach (PropertyInfo property in properties)
            {
                var value = GetRandomValue(random, property.PropertyType, property.Name, depth);

                if (value != null)
                {
                    property.SetValue(instance, value, null);
                }
            }
        }

        private object GetRandomValue(Random random, Type typ, string prefix, int depth)
        {
            object value = null;

            if (_generatorDictionary.ContainsKey(typ))
            {
                value = _generatorDictionary[typ](random, prefix);
            }
            else if (typ.IsArray)
            {
                value = RandomArray(random, typ.GetElementType(), prefix, depth + 1);
            }
            else if (_iListType.IsAssignableFrom(typ))
            {
                value = RandomList(random, typ, prefix, depth + 1);
            }
            else if (_iEnumerableType.IsAssignableFrom(typ))
            {
                value = RandomIEnumerable(random, typ, prefix, depth + 1);
            }
            else if (typ.IsClass)
            {
                value = RandomClassInstance(random, typ, depth + 1);
            }
            else if (typ.IsEnum)
            {
                value = RandomEnum(random, typ);
            }

            return value;
        }

        private Array RandomArray(Random random, Type typ, string prefix, int depth)
        {
            Array array = null;

            if (typ != null)
            {
                int itemCount = random.Next(1, 10);

                array = TryToCreateInstance(() => Array.CreateInstance(typ, itemCount)) as Array;

                if (array != null)
                {
                    for (int i = 0; i < itemCount; i++)
                    {
                        string childPrefix = string.Format("{0}[{1}]", prefix, i);
                        var item = GetRandomValue(random, typ, childPrefix, depth);
                        array.SetValue(item, i);
                    }
                }
            }

            return array;
        }

        private object RandomClassInstance(Random random, Type typ, int depth)
        {
            object value = null;
            bool shouldCreateInstance;

            if (_createdTypes.ContainsKey(typ))
            {
                shouldCreateInstance = (_createdTypes[typ] == depth);
            }
            else
            {
                shouldCreateInstance = true;
                _createdTypes.Add(typ, depth);
            }

            if (shouldCreateInstance)
            {
                value = TryToCreateInstance(() => Activator.CreateInstance(typ));

                if (value != null)
                {
                    UpdatePropertiesWithRandomValues(value, random, typ, depth);
                }
            }

            return value;
        }

        private object RandomEnum(Random random, Type enumType)
        {
            Array values = Enum.GetValues(enumType);

            int index = random.Next(0, values.Length - 1);

            return values.GetValue(index);
        }

        private object RandomIEnumerable(Random random, Type typ, string prefix, int depth)
        {
            Array array = null;
            Type[] types = typ.GetGenericArguments();

            if (types.Length == 1)
            {
                array = RandomArray(random, types[0], prefix, depth);
            }

            return array;
        }

        private object RandomList(Random random, Type typ, string prefix, int depth)
        {
            object list = null;

            Type[] genericArguments = typ.GetGenericArguments();

            if (genericArguments.Length == 1)
            {
                Array array = RandomArray(random, genericArguments[0], prefix, depth);

                if (array != null)
                {
                    Type genericListType = typeof(List<>).MakeGenericType(genericArguments);

                    TryToCreateInstance(() => list = Activator.CreateInstance(genericListType, array));
                }
            }

            return list;
        }

        private static object RandomBool(Random random, string prefix)
        {
            return (random.Next() % 2 == 0);
        }

        private static object RandomDateTime(Random random, string prefix)
        {
            return DateTime.Today.AddDays(random.Next(-1000, 1000));
        }

        private static object RandomDecimal(Random random, string prefix)
        {
            return (decimal)random.NextDouble();
        }

        private static object RandomDouble(Random random, string prefix)
        {
            return random.NextDouble();
        }

        private static object RandomInt(Random random, string prefix)
        {
            return random.Next();
        }

        private static object RandomLong(Random random, string prefix)
        {
            return (long)random.Next();
        }

        private static object RandomString(Random random, string prefix)
        {
            return string.Format("{0}{1}", prefix, random.Next());
        }

        private object TryToCreateInstance(Func<object> callback)
        {
            object value = null;

            try { value = callback(); } catch { }

            return value;
        }
    }
}
