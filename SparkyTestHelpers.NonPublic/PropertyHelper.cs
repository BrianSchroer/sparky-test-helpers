using System;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.NonPublic
{
    /// <summary>
    /// <see cref="NonPublicMembers" /> property helper.
    /// </summary>
    public class PropertyHelper<T>
    {
        private const BindingFlags _instanceBindingFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
        private const BindingFlags _staticBindingFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;

        private T _instance;
        private string _propertyName;

        /// <summary>
        /// Is the property static?
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Creates a new <see cref="PropertyHelper{T}" instance.
        /// </summary>
        /// <typeparam type="T">The instance type.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/>.</param>
        /// <param name="propertyName">Property name.</param>
        public PropertyHelper(T instance, string propertyName)
        {
            _instance = instance;
            _propertyName = propertyName;
        }

        /// <summary>
        /// Gets property value.
        /// </summary>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        /// <returns>Property value.</returns>
        /// <exception cref="MissingMethodException"> if property does not exist or <paramref name="args"/> types are incorrect.</exception>
        public object Get(params object[] args)
        {
            try
            {
                return new NonPublicMembers<T>(_instance).GetProperty(_propertyName, IsStatic ? _staticBindingFlags : _instanceBindingFlags, args);
            }
            catch (MissingMethodException ex)
            {
                string argsTypes = "(" + string.Join(", ", args.Select(arg => arg.GetType().FullName)) + ")";
                throw new MissingMethodException(ex.Message.Replace("' not found", argsTypes + "' not found"), ex);
            }
}

        /// <summary>
        /// Gets property value and casts to the specified type.
        /// </summary>
        /// <typeparam name="TPropertyType">The property type.</typeparam>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        /// <returns>The <typeparamref name="TPropertyType"/>.</returns>
        /// <exception cref="MissingMethodException"> if property does not exist or <paramref name="args"/> types are incorrect.</exception>
        /// <exception cref="InvalidCastException"> if <typeparamref name="TPropertyType"/> is not the correct type.</exception>
        public TPropertyType Get<TPropertyType>(params object[] args) => (TPropertyType)Convert.ChangeType(Get(args), typeof(TPropertyType));

        /// <summary>
        /// Sets property value.
        /// </summary>
        /// <param name="value">value to set.</param>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        public void Set(object value, params object[] args)
        {
            try
            {
                new NonPublicMembers<T>(_instance).SetProperty(_propertyName, IsStatic ? _staticBindingFlags : _instanceBindingFlags, value, args);
            }
            catch (MissingMethodException ex)
            {
                string argsTypes = "(" + string.Join(", ", args.Select(arg => arg.GetType().FullName)) + ")";
                throw new MissingMethodException(ex.Message.Replace("' not found", argsTypes + "' not found"), ex);
            }
        }
    }
} 