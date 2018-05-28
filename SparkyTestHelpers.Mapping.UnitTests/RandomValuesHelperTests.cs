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
    public class RandomValuesHelperTests
    {
        [TestMethod]
        public void CreateInstanceWithRandomValues_should_work()
        {
            var dummy = new RandomValuesHelper().CreateInstanceWithRandomValues<Dummy>();

            Console.WriteLine(
                JsonConvert.SerializeObject(
                    dummy,
                    Formatting.Indented,
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
                    Formatting.Indented,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()));
        }

        [TestMethod]
        public void WithMaximumDepth_should_work()
        {
            var helper = new RandomValuesHelper();

            var result = helper.CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNotNull(result.Depth2.Depth3.Depth4, "No maximum depth");

            result = helper.WithMaximumDepth(3).CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNull(result.Depth2.Depth3.Depth4, $"Maximum depth {helper.MaximumDepth}");

            result = helper.WithMaximumDepth(2).CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNull(result.Depth2.Depth3, $"Maximum depth {helper.MaximumDepth}");

            result = helper.WithMaximumDepth(1).CreateInstanceWithRandomValues<Depth1>();
            Assert.IsNull(result.Depth2, $"Maximum depth {helper.MaximumDepth}");
        }
    }
}
