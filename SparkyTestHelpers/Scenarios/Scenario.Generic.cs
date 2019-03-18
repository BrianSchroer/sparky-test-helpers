using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.Scenarios
{
    /// <summary>
    /// Test scenario Arrange/Assert definitions.
    /// </summary>
    /// <typeparam name="TArrangeObject">"Arrange" object type.</typeparam>
    /// <typeparam name="TAssertObject">"Assert" object type.</typeparam>
    public class Scenario<TArrangeObject, TAssertObject>
    {
        /// <summary>
        /// "Arrange" function.
        /// </summary>
        public Func<TArrangeObject, TArrangeObject> Arrange { get; set; }

        /// <summary>
        /// "Assert" function.
        /// </summary>
        public Func<TAssertObject, TAssertObject> Assert { get; set; }
    }

    public static class ScenarioListExtensionMethods
    {
        public static ScenarioFactory<TArrangeObject, TAssertObject> Arrange<TArrangeObject, TAssertObject>(
            this IList<Scenario<TArrangeObject, TAssertObject>> list,
            Func<TArrangeObject, TArrangeObject> arrange)
        {
            return new ScenarioFactory<TArrangeObject, TAssertObject>(list, arrange);
        }

        public static ScenarioFactory<TArrangeObject, TAssertObject> Arrange<TArrangeObject, TAssertObject>(
            this IList<Scenario<TArrangeObject, TAssertObject>> list,
            Action<TArrangeObject> arrange)
        {
            return new ScenarioFactory<TArrangeObject, TAssertObject>(list, arrange);
        }
    }
}
