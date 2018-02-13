using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SparkyTestHelpers.Mapping.UnitTests.TestClasses;
using System;

namespace SparkyTestHelpers.Mapping.UnitTests
{
    /// <summary>
    /// <see cref="RandomValuesHelper"/> tests.
    /// </summary>
    [TestClass]
    public class RandomValuesHelperTest
    {
        [TestMethod]
        public void CreateInstanceWithRandomValues_should_work()
        {
            var dummy = new RandomValuesHelper().CreateInstanceWithRandomValues<Dummy>();

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy, 
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void UpdatePropertiesWithRandomValues_should_work()
        {
            var dummy = new Dummy();
            new RandomValuesHelper().UpdatePropertiesWithRandomValues(dummy);

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }
    }
}
