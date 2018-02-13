using AutoMapper;
using SparkyTestHelpers.Mapping;

namespace SparkyTestHelpers.AutoMapper
{
    /// <summary>
    /// <see cref="MapTester{TSource, TDestination}"/> extensions for AutoMapper.
    /// </summary>
    public static class MapTesterExtensions
    {
        public static TDestination AssertAutoMappedValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester, TSource source)
        {
            TDestination mapped = Mapper.Map<TSource, TDestination>(source);

            mapTester.AssertMappedValues(source, mapped);

            return mapped;
        }

        public static TDestination AssertAutoMappedRandomValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester)
        {
            var source = new RandomValuesHelper().CreateInstanceWithRandomValues<TSource>();
            return mapTester.AssertAutoMappedValues(source);
        }
    }
}
