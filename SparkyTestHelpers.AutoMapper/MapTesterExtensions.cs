using AutoMapper;
using SparkyTestHelpers.Mapping;
using System;

namespace SparkyTestHelpers.AutoMapper
{
    /// <summary>
    /// <see cref="MapTester{TSource, TDestination}"/> extensions for AutoMapper.
    /// </summary>
    public static class MapTesterExtensions
    {
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
        public static void AssertMappedValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester, TSource source, TDestination dest)
        {
            WithExceptionHandling(mapTester, () => mapTester.AssertMappedValues(source, dest));
        }

        /// <summary>
        /// Using static Mapper, 
        /// maps <paramref name="source"/> to new <typeparamref name="TDestination"/> instance and
        /// calls <see cref="MapTester{TSource, TDestination}.AssertMappedValues(TSource, TDestination)"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="mapTester">The <see cref="MapTester{TSource, TDestination}"/>.</param>
        /// <param name="source">The source object.</param>
        /// <returns>The mapped <typeparamref name="TDestination"/> instance.</returns>
        public static TDestination AssertAutoMappedValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester, TSource source)
        {
            TDestination mapped = default(TDestination);

            WithExceptionHandling(mapTester, () =>
            {
                mapped = Mapper.Map<TSource, TDestination>(source);

                mapTester.AssertMappedValues(source, mapped);
            });

            return mapped;
        }

        /// <summary>
        /// Using <see cref="IMapper"/> instance, 
        /// maps <paramref name="source"/> to new <typeparamref name="TDestination"/> instance and
        /// calls <see cref="MapTester{TSource, TDestination}.AssertMappedValues(TSource, TDestination)"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="mapTester">The <see cref="MapTester{TSource, TDestination}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="source">The source object.</param>
        /// <returns>The mapped <typeparamref name="TDestination"/> instance.</returns>
        public static TDestination AssertAutoMappedValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester, IMapper mapper, TSource source)
        {
            TDestination mapped = default(TDestination);

            WithExceptionHandling(mapTester, () =>
            {
                mapped = mapper.Map<TSource, TDestination>(source);
                mapTester.AssertMappedValues(source, mapped);
            });

            return mapped;
        }

        /// <summary>
        /// Using static Mapper, uses <see cref="RandomValuesHelper"/> to create a new <typeparamref name="TSource"/> instance,
        /// maps it to a new <typeparamref name="TDestination"/> instance, and
        /// calls <see cref="MapTester{TSource, TDestination}.AssertMappedValues(TSource, TDestination)"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="mapTester">The <see cref="MapTester{TSource, TDestination}"/>.</param>
        /// <returns>The mapped <typeparamref name="TDestination"/> instance.</returns>
        public static TDestination AssertAutoMappedRandomValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester)
        {
            var source = new RandomValuesHelper().CreateInstanceWithRandomValues<TSource>();
            return mapTester.AssertAutoMappedValues(source);
        }

        /// <summary>
        /// Using <see cref="IMapper"/>, uses <see cref="RandomValuesHelper"/> to create a new <typeparamref name="TSource"/> instance,
        /// maps it to a new <typeparamref name="TDestination"/> instance, and
        /// calls <see cref="MapTester{TSource, TDestination}.AssertMappedValues(TSource, TDestination)"/>.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TDestination">The type of the destination.</typeparam>
        /// <param name="mapTester">The <see cref="MapTester{TSource, TDestination}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <returns>The mapped <typeparamref name="TDestination"/> instance.</returns>
        public static TDestination AssertAutoMappedRandomValues<TSource, TDestination>(
            this MapTester<TSource, TDestination> mapTester, IMapper mapper)
        {
            var source = new RandomValuesHelper().CreateInstanceWithRandomValues<TSource>();
            return mapTester.AssertAutoMappedValues(mapper, source);
        }

        private static void WithExceptionHandling<TSource, TDestination>(MapTester<TSource, TDestination> mapTester, Action action)
        {
            action();

            //Remove exception message prettifier that turned out to be not so pretty:

            //try
            //{
            //    action();
            //}
            //catch (MapTesterException ex)
            //{
            //    string message = ExceptionHelper.AddSuggestedFixInfoToMessage(mapTester, ex);
            //    if (message == ex.Message)
            //    {
            //        throw;
            //    }
            //    throw new MapTesterException(message);
            //}
            //catch (AutoMapperConfigurationException ex)
            //{
            //    string message = ExceptionHelper.AddSuggestedFixInfoToMessage(mapTester, ex);
            //    if (message == ex.Message)
            //    {
            //        throw;
            //    }
            //    throw new AutoMapperConfigurationException(message);
            //}
        }
    }
}
