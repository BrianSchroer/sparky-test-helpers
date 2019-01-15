using AutoMapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SparkyTestHelpers.AutoMapper.UnitTests.TestClasses;
using SparkyTestHelpers.Exceptions;
using SparkyTestHelpers.Mapping;
using SparkyTestHelpers.Population;
using System;
using FluentAssertions;

namespace SparkyTestHelpers.AutoMapper.UnitTests
{
    /// <summary>
    /// <see cref="MapTesterExtensions"/> tests.
    /// </summary>
    [TestClass]
    public class MapTesterExtensionsTests : RandomValueProvider
    {
        private IMapper _mapper;

        private const int _testInt = 666;

        /// <inheritdoc />
        public override int GetInt() => _testInt;

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
            Source source = GetRandom.InstanceOf<Source>();
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedValues(source));

            dest.Should().BeOfType<Dest>();
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }

        [TestMethod]
        public void AssertAutoMappedValues_with_IMapper_should_work()
        {
            Source source = GetRandom.InstanceOf<Source>();
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedValues(_mapper, source));

            dest.Should().BeOfType<Dest>();
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }

        [TestMethod]
        public void AssertAutoMappedRandomValues_should_work()
        {
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedRandomValues());

            dest.Should().BeOfType<Dest>();
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }

        [TestMethod]
        public void AssertAutoMappedRandomValues_with_RandomValueProvider_should_work()
        {
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedRandomValues(this));

            dest.Should().BeOfType<Dest>();
            dest.Id.Should().Be(_testInt);

            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }

        [TestMethod]
        public void AssertAutoMappedRandomValues_with_IMapper_should_work()
        {
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedRandomValues(_mapper));

            dest.Should().BeOfType<Dest>();
            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }


        [TestMethod]
        public void AssertAutoMappedRandomValues_with_IMapper_and_RandomValueProvider_should_work()
        {
            Dest dest = null;

            AssertExceptionNotThrown.WhenExecuting(() =>
                dest = MapTester.ForMap<Source, Dest>().AssertAutoMappedRandomValues(_mapper, this));

            dest.Should().BeOfType<Dest>();
            dest.Id.Should().Be(_testInt);

            Console.WriteLine(JsonConvert.SerializeObject(dest));
        }
    }
}
