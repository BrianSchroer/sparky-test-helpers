using System;
using System.Collections.Generic;
using System.Linq;

namespace SparkyTestHelpers.Scenarios.MsTest
{
    /// <summary>
    /// "Syntactic sugar" methods for working with <see cref="MsTestScenarioTester{TScenario}" />,
    /// </summary>
    public static class ForTest
    {
        /// <summary>
        /// Creates an array of <typeparamref name="TScenario" /> from a "paramref name="scenarios"" array,
        /// for use with the ".TestEach" extension method.
        /// </summary>
        /// <returns>Array of <typeparamref name="TScenario" />.</returns>
        /// <example>
        ///     <code><![CDATA[
        ///  ForTest.Scenarios
        /// (
        ///     new { DateString = "1/31/2023", ShouldBeValid = true },  
        ///     new { DateString = "2/31/2023", ShouldBeValid = false },  
        ///     new { DateString = "3/31/2023", ShouldBeValid = true },  
        ///     new { DateString = "4/31/2023", ShouldBeValid = false },  
        ///     new { DateString = "5/31/2023", ShouldBeValid = true },  
        ///     new { DateString = "6/31/2023", ShouldBeValid = false } 
        /// )
        /// .TestEach(scenario =>
        /// {
        ///     DateTime dt;
        ///     Assert.AreEqual(scenario.ShouldBeValid, DateTime.TryParse(scenario.DateString, out dt));  
        /// });  
        ///     ]]></code>
        /// </example>
        public static TScenario[] Scenarios<TScenario>(params TScenario[] scenarios)
        {
            return scenarios;
        }

        /// <summary>
        /// Creates an IEnumerable of values for the specified enum typeparamref name="TEnum",
        /// for use with the ".TestEach" extension method.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>IEnumerable of <typeparamref name="TEnum" />.</returns>
        /// <example>
        ///     <code><![CDATA[
        ///  ForTest.EnumValues<OrderStatus>().TestEach(orderStatus => {});
        ///     ]]></code>
        /// </example>
        public static IEnumerable<TEnum> EnumValues<TEnum>() where TEnum : struct
        {
            Type enumType = typeof(TEnum);

            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException($"{enumType} is not an Enum type.");
            }

            return Enum.GetValues(enumType).Cast<TEnum>();
        }


        /// <summary>
        /// "params" alternative to IEnumerable.Except,
        /// for use with the ".TestEach" extension method.
        /// </summary>
        /// <typeparam name="TEnum">The enum type.</typeparam>
        /// <returns>IEnumerable of <typeparamref name="TEnum" />.</returns>
        /// <example>
        ///     <code><![CDATA[
        ///  ForTest.EnumValues<OrderStatus>().ExceptFor(OrderStatus.Cancelled).TestEach(orderStatus => {});
        ///     ]]></code>
        /// </example>
        public static IEnumerable<TEnum> ExceptFor<TEnum>(this IEnumerable<TEnum> values, params TEnum[] valuesToExclude)
        {
            return values.Except(valuesToExclude);
        }
    }
}