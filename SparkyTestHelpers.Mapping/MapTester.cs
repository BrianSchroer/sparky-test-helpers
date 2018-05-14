using SparkyTestHelpers.Scenarios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace SparkyTestHelpers.Mapping
{
    /// <summary>
    /// This class is for testing that properties were successfully "mapped" from one type to another.
    /// </summary>
    /// <typeparam name="TSource">The "map from" type.</typeparam>
    /// <typeparam name="TDestination">The "map to" type.</typeparam>
    public class MapTester<TSource, TDestination>
    {
        private readonly Dictionary<string, MapMemberTester<TSource, TDestination>> _memberTesters;

        private readonly PropertyInfo[] _destProperties;

        private bool _ignoringAllOtherMembers = false;
        private List<string> _ignoringMemberNamesStartingWith = new List<string>();

        private List<string> _untestedProperties;

        private Action<string> _log = (_) => { };

        /// <summary>
        /// Creates a new <see cref="MapTester{TSource, TDestination}"/> instance and initializes it
        /// to automatically test properties that have the same name on the "from" and "to"
        /// <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> types.
        /// </summary>
        /// <remarks>
        /// This constructor is called by the "factory"
        /// <see cref="MapTester.ForMap{TSource, TDestination}"/> method.
        /// </remarks>
        internal MapTester()
        {
            Type sourceType = typeof(TSource);
            TypeInfo sourceTypeInfo = sourceType.GetTypeInfo();
            Type destType = typeof(TDestination);
            TypeInfo destTypeInfo = destType.GetTypeInfo();
            _memberTesters = new Dictionary<string, MapMemberTester<TSource, TDestination>>();
            _destProperties = PropertyEnumerator.GetPublicInstanceReadWriteProperties(destTypeInfo);
            PropertyInfo[] srcProperties = PropertyEnumerator.GetPublicInstanceReadWriteProperties(sourceTypeInfo);

            string[] commonPropertyNames = srcProperties
                .Join(_destProperties, dest => dest.Name, src => src.Name, (dest, _) => dest.Name)
                .ToArray();

            foreach (string propertyName in commonPropertyNames)
            {
                PropertyInfo srcProperty = PropertyInfoResolver.Resolve(sourceTypeInfo, propertyName);
                PropertyInfo destProperty = PropertyInfoResolver.Resolve(destTypeInfo, propertyName);
              
                SetTesterForProperty(propertyName,
                    new MapMemberTester<TSource, TDestination>(
                        this,
                        dest => destProperty.GetValue(dest, null),
                        src => srcProperty.GetValue(src, null)));
            }
        }

        /// <summary>
        /// Log destination property values when <see cref="AssertMappedValues(TSource, TDestination)"/> is called.
        /// </summary>
        /// <param name="action">"Callback" function to receive/log messages. 
        /// (If null, <see cref="Console.WriteLine()"/> is used.</param>
        /// <returns>"This" <see cref="MapTester{TSource, TDestination}"/></returns>
        public MapTester<TSource, TDestination> WithLogging(Action<string> action = null)
        {
            _log = action ?? ((message) => Console.WriteLine(message));
            return this;
        }

        /// <summary>
        /// Specify <typeparamref name="TDestination"/> property that should be ignored when asserting mapping results.
        /// </summary>
        /// <param name="destExpression">Expression to get property name.</param>
        /// <returns>"This" <see cref="MapMemberTester{TSource, TDestination}"/>.</returns>
        /// <example><![CDATA[
        ///     MapTester.ForMap<Foo, Bar>()
        ///         .IgnoringMember(dest => dest.PropertyThatOnlyBarHas)
        ///         .AssertMappedValues(foo, bar);
        /// ]]></example>
        public MapTester<TSource, TDestination> IgnoringMember(Expression<Func<TDestination, object>> destExpression)
        {
            return IgnoringMembers(destExpression);
        }

        /// <summary>
        /// Specify <typeparamref name="TDestination"/> properties that should be ignored when asserting mapping results.
        /// </summary>
        /// <param name="destExpressions">Array of expression to get property names.</param>
        /// <returns>"This" <see cref="MapMemberTester{TSource, TDestination}"/>.</returns>
        /// <example><![CDATA[
        ///     MapTester.ForMap<Foo, Bar>()
        ///         .IgnoringMembers(dest => dest.PropertyThatOnlyBarHas, dest => dest.Another, dest => dest.YetAnother)
        ///         .AssertMappedValues(foo, bar);
        /// ]]></example>
        public MapTester<TSource, TDestination> IgnoringMembers(params Expression<Func<TDestination, object>>[] destExpressions)
        {
            foreach (var destExpression in destExpressions)
            {
                WhereMember(destExpression).ShouldBeIgnored = true;
            }

            return this;
        }

        /// <summary>
        /// Specify member name prefix for which properties should be ignored when asserting mapping results.
        /// </summary>
        /// <param name="prefix">The prefix.</param>
        /// <returns>"This" <see cref="MapMemberTester{TSource, TDestination}"/>.</returns>
        public MapTester<TSource, TDestination> IgnoringMemberNamesStartingWith(string prefix)
        {
            _ignoringMemberNamesStartingWith.Add(prefix);
            return this;
        }

        /// <summary>
        /// Specify that all properties for which <see cref="WhereMember(Expression{Func{TDestination, object}})"/> has not been
        /// called should be ignored when asserting mapping results.
        /// </summary>
        /// <returns>"This" <see cref="MapMemberTester{TSource, TDestination}"/>.</returns>
        public MapTester<TSource, TDestination> IgnoringAllOtherMembers()
        {
            _ignoringAllOtherMembers = true;
            return this;
        }

        /// <summary>
        /// Specify <typeparamref name="TDestination"/> property to be tested.
        /// </summary>
        /// <param name="destExpression">Expression to get property name.</param>
        /// <returns>New <see cref="MapMemberTester{TSource, TDestination}"/> instance.</returns>
        /// <example><![CDATA[
        ///     MapTester.ForMap<Foo, Bar>()
        ///         .WhereMember(dest => dest.Baz).ShouldEqual(src => src.Qux)
        ///         .AssertMappedValues(foo, bar);
        /// ]]></example>
        public MapMemberTester<TSource, TDestination> WhereMember(Expression<Func<TDestination, object>> destExpression)
        {
            string propertyName = GetPropertyName(destExpression);

            var memberTester = new MapMemberTester<TSource, TDestination>(this, destExpression.Compile());

            SetTesterForProperty(propertyName, memberTester);

            return memberTester;
        }

        /// <summary>
        /// Assert that all defined properties were successfully maped
        /// from <typeparamref name="TSource"/> to <typeparamref name="TDestination"/>.
        /// </summary>
        /// <param name="source">An instance of type <typeparamref name="TDestination"/>.</param>
        /// <param name="dest">An instance of type <typeparamref name="TDestination"/>.</param>
        /// <exception cref="MapTesterException">if dest properties don't have expected values.</exception>
        /// <example>
        /// <code><![CDATA[
        ///     MapTester
        ///         .ForMap<Foo, Bar>()
        ///         .AssertMappedValues(foo, bar);
        /// ]]>
        /// </code>
        /// </example>
        public void AssertMappedValues(TSource source, TDestination dest)
        {
            string sourceName = typeof(TSource).FullName;
            string destName = typeof(TDestination).FullName;

            _log($"Asserting mapping from \n{sourceName} to \n{destName}:");

            _untestedProperties = new List<string>();

            try
            {
                TestMappedProperties(source, dest);
            }
            catch (ScenarioTestFailureException ex)
            {
                string message = ex.Message;

                try
                {
                    message = 
                        ExceptionHelper.ConvertToMapTesterExceptionMessage(ex)
                        + ExceptionHelper.FormatUnmappedPropertiesHelperMessage(_untestedProperties);
                }
                catch
                {
                }

                throw new MapTesterException(message);
            }
        }

        public override string ToString()
        {
            try
            {
                string[] ignoredProperties = _destProperties.Where(x => MemberShouldBeIgnored(x.Name)).Select(x => x.Name).ToArray();

                var sb = new StringBuilder($"\tMapTester.ForMap<{typeof(TSource).Name}, {typeof(TDestination).Name}>()");

                foreach (string propertyName in _destProperties.Select(x => x.Name).Where(x => !ignoredProperties.Contains(x)))
                {
                    if (_memberTesters.ContainsKey(propertyName))
                    {
                        MapMemberTester<TSource, TDestination> mapMemberTester = _memberTesters[propertyName];
                        string testDescription = mapMemberTester.TestDescription;
                        if (!string.IsNullOrWhiteSpace(testDescription))
                        {
                            sb.Append($"\n\t\t.WhereMember(dest => dest.{propertyName}).{testDescription}");
                        }
                    }
                }

                if (ignoredProperties.Length > 0)
                {
                    sb.Append($"\n\t\t.IgnoringMember{((ignoredProperties.Length > 1) ? "s" : "")}(dest => dest.");
                    sb.Append(string.Join(", dest => dest.", ignoredProperties));
                    sb.Append(")");
                }

                return sb.ToString();
            }
            catch
            {
                return GetType().FullName;
            }
        }

        /// <summary>
        /// Define <see cref="MapMemberTester{TSource, TDestination}"/> for
        /// <typeparamref name="TDestination"/> property.
        /// </summary>
        /// <remarks>
        /// This method is usually called by this class's constructor and by the
        /// "WhereMember" and "IgnoreMember" extension methods.
        /// </remarks>
        /// <param name="propertyName">The <typeparamref name="TDestination"/> property name.</param>
        /// <param name="memberTester">The <see cref="MapMemberTester{TSource, TDestination}"/> to be used
        /// to test mapping for the property.</param>
        private void SetTesterForProperty(string propertyName, MapMemberTester<TSource, TDestination> memberTester)
        {
            if (_memberTesters.ContainsKey(propertyName))
            {
                _memberTesters[propertyName] = memberTester;
            }
            else
            {
                _memberTesters.Add(propertyName, memberTester);
            }
        }

        private void TestMappedProperties(TSource source, TDestination dest)
        {
            _destProperties
                .OrderBy(x => x.Name)
                .Select(x => x.Name)
                .TestEach(propertyName =>
                {
                    MapMemberTester<TSource, TDestination> memberTester = (_memberTesters.ContainsKey(propertyName))
                        ? _memberTesters[propertyName]
                        : null;

                    bool memberShouldBeIgnored = MemberShouldBeIgnored(propertyName, memberTester);

                    if (memberTester == null)
                    {
                        string message = $"Property \"{propertyName}\" was not tested.";
                        if (memberShouldBeIgnored)
                        {
                            _log(message);
                        }
                        else
                        {
                            _untestedProperties.Add(propertyName);
                            throw new MapTesterException(message);
                        }
                    }
                    else
                    {
                        if (memberShouldBeIgnored)
                        {
                            ReportIgnoredMember(propertyName, dest, memberTester);
                        }
                        else
                        {
                            AssertMappedValue(propertyName, source, dest, memberTester);
                        }
                    }
                });
        }

        private bool MemberShouldBeIgnored(string propertyName)
        {
            return MemberShouldBeIgnored(
                propertyName,
                _memberTesters.ContainsKey(propertyName) ? _memberTesters[propertyName] : null);
        }

        private bool MemberShouldBeIgnored(string propertyName, MapMemberTester<TSource, TDestination> memberTester)
        {
            return (memberTester?.ShouldBeIgnored ?? false)
                || _ignoringAllOtherMembers 
                || _ignoringMemberNamesStartingWith.Any(x => propertyName.StartsWith(x));
        }

        private void AssertMappedValue(string propertyName, TSource source, TDestination dest,
            MapMemberTester<TSource, TDestination> memberTester)
        {
            if (memberTester.CustomTest == null)
            {
                object expected = memberTester.GetExpectedValue(source);
                object actual = memberTester.GetActualValue(dest);

                if ((actual == null && expected == null) || (actual != null && actual.Equals(expected)))
                {
                    _log($"\t{propertyName} = {FormatValue(actual)}");
                }
                else
                {
                    AssertAreEqual(propertyName, expected, actual);
                }
            }
            else
            {
                object actual = memberTester.GetActualValue(dest);
                _log($"\t{propertyName} = {FormatValue(actual)}");
                memberTester.CustomTest(source, dest);
            }
        }

        private void AssertAreEqual(string propertyName, object expected, object actual)
        {
            if (actual.Equals(expected))
            {
                return;
            }

            throw new MapTesterException(
                $"Mapping test failed for property \"{propertyName}\"."
                + $" Expected<{FormatValue(expected)}>. Actual: <{FormatValue(actual)}>");
        }

        private void ReportIgnoredMember(
            string propertyName, TDestination dest, MapMemberTester<TSource, TDestination> memberTester)
        {
            object actual = memberTester.GetActualValue(dest);
            _log($"\t{propertyName} = {FormatValue(actual)}");
        }

        private string FormatValue(object value)
        {
            if (value == null)
            {
                return "null";
            }

            if (value is string)
            {
                return value.ToString();
            }

            var enumerable = value as IEnumerable;

            if (enumerable != null)
            {
                var list = new List<string>();
                IEnumerator enumerator = enumerable.GetEnumerator();
                while (enumerator.MoveNext())
                {
                    list.Add(FormatValue(enumerator.Current));
                }
                string[] items = list.ToArray();

                string formattedItems = (items.Length == 0)
                    ? string.Empty
                    : string.Format($"\n\t\t\t{string.Join("\n\t\t\t", items)}");

                return $"({items.Length} items){formattedItems}";
            }

            return value.ToString();
        }

        private static string GetPropertyName(Expression<Func<TDestination, object>> expression)
        {
            bool exceptionWasThrown = false;

            try
            {
                string name = null;

                var memberExpression = expression.Body as MemberExpression;

                if (memberExpression == null)
                {
                    var unaryExpression = expression.Body as UnaryExpression;
                    if (unaryExpression != null)
                    {
                        memberExpression = unaryExpression?.Operand as MemberExpression;
                    }
                }

                if (memberExpression?.Member != null)
                {
                    name = (memberExpression.Member as PropertyInfo)?.Name;
                }

                if (string.IsNullOrWhiteSpace(name))
                {
                    exceptionWasThrown = true;
                    throw new MapTesterException($"Invalid property expression: \"{expression}\".");
                }

                return name;
            }
            catch (Exception ex) when (!exceptionWasThrown)
            {
                throw new MapTesterException($"Invalid property expression: \"{expression}\".", ex);
            }
        }
    }
}
