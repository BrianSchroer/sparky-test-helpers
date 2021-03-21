using System;
using System.Linq;
using System.Reflection;

namespace SparkyTestHelpers.NonPublic
{
    /// <summary>
    /// <see cref="NonPublicMembers" /> method helper.
    /// </summary>
    public class MethodHelper<T>
    {
        private const BindingFlags _instanceBindingFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public;
        private const BindingFlags _staticBindingFlags = BindingFlags.Default | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public;

        private T _instance;
        private string _methodName;

        /// <summary>
        /// Is the method static?
        /// </summary>
        public bool IsStatic { get; set; }

        /// <summary>
        /// Creates a new <see cref="MethodHelper{T}" instance.
        /// </summary>
        /// <typeparam type="T">The instance type.</typeparam>
        /// <param name="instance">Instance of <typeparamref name="T"/>.</param>
        /// <param name="methodName">Method name.</param>
        public MethodHelper(T instance, string methodName)
        {
            _instance = instance;
            _methodName = methodName;
        }

        /// <summary>
        /// Invokes the specified method.
        /// </summary>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        /// <returns>Result of method call (null for void method).</returns>
        /// <exception cref="MissingMethodException"> if method does not exist or <paramref name="args"/> types are incorrect.</exception>
        public object Invoke(params object[] args)
        {
            try
            {
                return new NonPublicMembers<T>(_instance).Invoke(_methodName, IsStatic ? _staticBindingFlags : _instanceBindingFlags, args);
            }
            catch (MissingMethodException ex)
            {
                string argsTypes = "(" + string.Join(", ", args.Select(arg => arg.GetType().FullName)) + ")";
                throw new MissingMethodException(ex.Message.Replace("' not found", argsTypes + "' not found"), ex);
            }
        }

        /// <summary>
        /// Invokes the specified method and casts the result to the specified result type.
        /// </summary>
        /// <typeparam name="TResult">The result type.</typeparam>
        /// <param name="args">Arguments to pass to the member to invoke.</param>
        /// <returns>The <typeparamref name="TResult"/>.</returns>
        /// <exception cref="MissingMethodException"> if method does not exist or <paramref name="args"/> types are incorrect.</exception>
        /// <exception cref="InvalidCastException"> if <typeparamref name="TResult"/> is not the correct type.</exception>
        public TResult Invoke<TResult>(params object[] args) => (TResult) Convert.ChangeType(Invoke(args), typeof(TResult));
    }
} 