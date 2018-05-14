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
        private IMapper _mapper;

        /// <summary>
        /// <see cref="MapTesterExtensions"/> tests.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            // Configure static Mapper:
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<Source, Dest>();
            });

            Mapper.AssertConfigurationIsValid();

            // Configure IMapper instance:
            var mapperConfiguration = new MapperConfiguration(cfg => cfg.CreateMap<Source, Dest>());
            _mapper = mapperConfiguration.CreateMapper();
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
        public void AssertAutoMappedValues_with_IMapper_should_work()
        {
            Source source = new RandomValuesHelper().CreateInstanceWithRandomValues<Source>();
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedValues(_mapper, source));

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

        [TestMethod]
        public void AssertAutoMappedRandomValues_with_IMapper_should_work()
        {
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedRandomValues(_mapper));

            Assert.IsInstanceOfType(dest, typeof(Dest));
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }

        [TestMethod]
        public void ExceptionHelper_should_add_helpful_info_to_exception_message()
        {
            var mapperConfiguration = new MapperConfiguration(cfg => { });
            var mapper = mapperConfiguration.CreateMapper();

            AssertExceptionThrown
                .OfType<AutoMapperConfigurationException>()
                .WithMessageContaining("The following code snippet might be useful for making your map match the test:")
                .WhenExecuting(() =>
                    MapTester.ForMap<Dest, Source>()
                        .WhereMember(dest => dest.FirstName).ShouldEqual(src => src.Name)
                        .WhereMember(dest => dest.LastName).ShouldEqualValue(null)
                        .IgnoringMember(dest => dest.Id)
                        .AssertAutoMappedRandomValues(mapper));
        }
    }
}
