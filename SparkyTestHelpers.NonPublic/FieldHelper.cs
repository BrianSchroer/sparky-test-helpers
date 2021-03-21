using System;
using System.Reflection;

namespace SparkyTestHelpers.NonPublic
{
    /// <summary>
    /// <see cref="NonPublicMembers" /> field helper.
    /// </summary>
    public class FieldHelper<T>
    {
        private const BindingFlags _instanceBindingFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
        private const BindingFlags _staticBindingFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;

        private T _instance;
        private string _fieldName;

        /// <summary>
        /// Is the field static?
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Creates a new <see cref="FieldHelper{T}" instance.
        /// </summary>
        /// <typeparam type="T">The instance type.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/>.</param>
        /// <param name="fieldName">Field name.</param>
        public FieldHelper(T instance, string fieldName)
        {
            _instance = instance;
            _fieldName = fieldName;
        }

        /// <summary>
        /// Gets field value.
        /// </summary>
        /// <returns>field value.</returns>
        /// <exception cref="MissingFieldException"> if field does not exist.</exception>
        public object Get()
        {
            return new NonPublicMembers<T>(_instance).GetField(_fieldName, IsStatic ? _staticBindingFlags : _instanceBindingFlags);
        }

        /// <summary>
        /// Gets field value and casts to the specified type.
        /// </summary>
        /// <typeparam name="TFieldType">The field type.</typeparam>
        /// <returns>The <typeparamref name="TFieldType"/>.</returns>
        /// <exception cref="MissingFieldException"> if field does not exist.</exception>
        /// <exception cref="InvalidCastException"> if <typeparamref name="TFieldType"/> is not the correct type.</exception>
        public TFieldType Get<TFieldType>() => (TFieldType)Convert.ChangeType(Get(), typeof(TFieldType));

        /// <summary>
        /// Sets field value.
        /// </summary>
        /// <param name="value">value to set.</param>
        public void Set(object value)
        {
            new NonPublicMembers<T>(_instance).SetField(_fieldName, IsStatic ? _staticBindingFlags : _instanceBindingFlags, value);
        }
    }
}