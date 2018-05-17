using SparkyTestHelpers.Mapping;
using System;

namespace SparkyTestHelpers.AutoMapper
{
    internal static class ExceptionHelper
    {
        public static string AddSuggestedFixInfoToMessage<TSource, TDestination>(
            MapTester<TSource, TDestination> mapTester, Exception ex)
        {
            string message = ex.Message;

            try
            {
                string mapTesterString = mapTester.ToString();

                if (mapTesterString.Contains("MapTester.ForMap"))
                {
                    string suggestedCode = mapTesterString
                        .Replace("MapTester.ForMap", "cfg.CreateMap")
                        .Replace(".WhereMember(", ".ForMember(")
                        .Replace(".ShouldEqualValue(", ".UseValue(")
                        .Replace(".ShouldEqual(", ".MapFrom(")
                        .Replace(".IsTestedBy((src, dest) => { /* custom test */ })", ".UseValue(() => ???)")
                        .Replace(".IgnoringMember", ".IgnoreMember");

                    message = $"{message}\n{new string('_', Console.BufferWidth)}" 
                        + $"\nThe following code snippet might be useful for making your map match the test:\n"
                        + suggestedCode;
                }
            }
            catch
            {
            }

            return message;
        }
    }
}