using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SparkyTestHelpers.AutoMapper.UnitTests.TestClasses;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Mapping;
using System;

namespace SparkyTestHelpers.AutoMapper.UnitTests
{
    /// <summary>
    /// <see cref="MapTesterExtensions"/> tests.
    /// </summary>
    [TestClass]
    public class MapTesterExtensionsTests
    {
        /// <summary>
        /// <see cref="MapTesterExtensions"/> tests.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg => cfg.CreateMap<Source, Dest>());

            Mapper.AssertConfigurationIsValid();
        }

        [TestMethod]
        public void AssertAutoMappedValues_should_work()
        {
            Source source = new RandomValuesHelper().CreateInstanceWithRandomValues<Source>();
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() => 
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedValues(source));

            Assert.IsInstanceOfType(dest, typeof(Dest));
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }

        [TestMethod]
        public void AssertAutoMappedRandomValues_should_work()
        {
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedRandomValues());

            Assert.IsInstanceOfType(dest, typeof(Dest));
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }
    }
}
