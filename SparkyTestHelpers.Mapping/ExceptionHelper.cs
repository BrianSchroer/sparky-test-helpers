using SparkyTestHelpers.Scenarios;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SparkyTestHelpers.Mapping
{
    internal static class ExceptionHelper
    {
        private static readonly Regex _scenarioCountRegex = new Regex(@"Scenario\[\d*\] \(\d* of \d*\) - ",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        public static string ConvertToMapTesterExceptionMessage(ScenarioTestFailureException ex)
        {
            string message = ex.Message;

            try
            {
                message = ex.Message
                    .Replace("scenarios tested", "properties tested")
                    .Replace("Scenario data - ", string.Empty);

                message = _scenarioCountRegex.Replace(message, string.Empty);
            }
            catch
            {
            }

            return message;
        }

        public static string FormatUnmappedPropertiesHelperMessage(IEnumerable<string> untestedPropertiesCollection)
        {
            string[] untestedProperties = untestedPropertiesCollection.ToArray();

            if (untestedProperties.Length == 0)
            {
                return null;
            }

            string ignoringMessage = $".IgnoringMember{((untestedProperties.Length == 1) ? "" : "s")}(dest => "
                + string.Join(", dest => dest.", untestedProperties) + ")";

            return
                $"\n{new string('_', ConsoleHelper.GetWidth())}\n"
                + $"If you want to ignore the untested member(s), you can code:\n\n{ignoringMessage}";
        }
    }
}