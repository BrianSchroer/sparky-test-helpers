namespace SparkyTestHelpers.NonPublic
{
    /// <summary>
    /// <see cref="NonPublicMembers" /> fluent syntax helper.
    /// </summary>
    public class NonPublic<T>
    {
        private T _instance;

        /// <summary>
        /// Creates a new <see cref="NonPublic{T}" instance.
        /// </summary>
        /// <typeparam type="T">The instance type.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/>.</param>
        public NonPublic(T instance)
        {
            _instance = instance;
        }

        /// <summary>
        /// Creates a new <see cref="FieldHelper{T}" instance.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns><see cref="FieldHelper{T}" /></returns>
        public FieldHelper<T> Field(string fieldName) => new FieldHelper<T>(_instance, fieldName);

        /// <summary>
        /// Creates a new <see cref="MethodHelper{T}" instance.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <returns><see cref="MethodHelper{T}" /></returns>
        public MethodHelper<T> Method(string methodName) => new MethodHelper<T>(_instance, methodName);

        /// <summary>
        /// Creates a new <see cref="PropertyHelper{T}" instance.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns><see cref="PropertyHelper{T}" /></returns>
        public PropertyHelper<T> Property(string propertyName) => new PropertyHelper<T>(_instance, propertyName);

        /// <summary>
        /// Creates a new <see cref="FieldHelper{T}" instance for a static field.
        /// </summary>
        /// <param name="fieldName">Field name.</param>
        /// <returns><see cref="FieldHelper{T}" /></returns>
        public FieldHelper<T> StaticField(string fieldName) => new FieldHelper<T>(_instance, fieldName) { IsStatic = true };

        /// <summary>
        /// Creates a new <see cref="MethodHelper{T}" instance for a static method.
        /// </summary>
        /// <param name="methodName">Method name.</param>
        /// <returns><see cref="MethodHelper{T}" /></returns>
        public MethodHelper<T> StaticMethod(string methodName) => new MethodHelper<T>(_instance, methodName) { IsStatic = true };

        /// <summary>
        /// Creates a new <see cref="PropertyHelper{T}" instance for a static property.
        /// </summary>
        /// <param name="propertyName">Property name.</param>
        /// <returns><see cref="PropertyHelper{T}" /></returns>
        public PropertyHelper<T> StaticProperty(string propertyName) => new PropertyHelper<T>(_instance, propertyName) { IsStatic = true };
    }
} 