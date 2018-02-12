using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SparkyTestHelpers.Scenarios
{
    /// <summary>
    /// <para>
    /// VisualStudio.TestTools doesn't have "RowTest" or "TestCase" attributes like
    /// NUnit or other .NET testing frameworks. (It does have a way to do data-driven
    /// tests, but it's pretty cumbersome.)
    /// </para>
    /// <para>
    /// This class provides the ability to execute the same test code for multiple test
    /// cases and, after all test cases have been executed, failing the unit test if 
    /// any of the test cases failed.
    /// </para>
    /// </summary>
    /// <remarks>
    /// This class is rarely used directly. It is more often used via the
    /// <see cref="ScenarioTesterExtension.TestEach{TScenario}(IEnumerable{TScenario}, Action{TScenario})"/>
    /// extension method.
    /// </remarks>
    /// <typeparam name="TScenario">The "test scenario" class type.</typeparam>
    /// <seealso cref="ScenarioTesterExtension.TestEach{TScenario}(IEnumerable{TScenario}, Action{TScenario})"/>
    public class ScenarioTester<TScenario>
    {
        private readonly TScenario[] _scenarios;

        private bool _hasAfterTestFunction = false;
        private bool _hasBeforeTestFunction = false;

        private Func<TScenario, Exception, bool> _afterTest = (scenario, exception) => exception == null;
        private Action<TScenario> _beforeTest = (scenario) => { };

        private Type[] _inconclusiveExceptionTypes = new Type[0];

        private readonly Regex _typeNameSuffixRegex
            = new Regex(@", [^\]]*, Version=[^\]]*, Culture=[^\]]*, PublicKeyToken=[^\]]*",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        private readonly Regex _genericVersionRegex = new Regex(@"`\d+",
            RegexOptions.Compiled | RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);

        /// <summary>
        /// Defines <see cref="Func{TScenario, Exception, Boolean}"/> to be called after testing each scenario.
        /// The function receives the scenario and the exception (if any) caught by the test. 
        /// If the function returns true, the scenario test is "passed". If false, exception is thrown to fail the test.
        /// </summary>
        /// <param name="func">The "callback" function.</param>
        /// <returns>"This" <see cref="ScenarioTester{TScenario}"/> instance.</returns>
        public ScenarioTester<TScenario> AfterEachTest(Func<TScenario, Exception, bool> func)
        {
            if (_hasAfterTestFunction)
            {
                throw new InvalidOperationException("AfterEachTest cannot be called more than once.");
            }

            _afterTest = func;
            _hasAfterTestFunction = true;
            return this;
        }

        /// <summary>
        /// Defines <see cref="Action"/> to be called before testing each scenario.
        /// </summary>
        /// <param name="action">The action.</param>
        /// <returns>"This" <see cref="ScenarioTester{TScenario}"/> instance.</returns>
        public ScenarioTester<TScenario> BeforeEachTest(Action<TScenario> action)
        {
            if (_hasBeforeTestFunction)
            {
                throw new InvalidOperationException("BeforeEachTest cannot be called more than once.");
            }

            _beforeTest = action;
            _hasBeforeTestFunction = true;
            return this;
        }

        /// <summary>
        /// Used by subclass to specify "inconclusive test" exception types.
        /// </summary>
        /// <param name="types">The exception type(s).</param>
        protected void SetInconclusiveExceptionTypes(params Type[] types)
        {
            _inconclusiveExceptionTypes = types;
        }

        /// <summary>
        /// Throw "tests inconclusive" exception.
        /// </summary>
        /// <param name="message">The exception message.</param>
        protected virtual void ThrowInconclusiveException(string message)
        {
            throw new ScenarioTestsInconclusiveException(message);
        }

        /// <summary>
        /// Test scenarios.
        /// </summary>
        public IEnumerable<TScenario> Scenarios
        {
            get { return _scenarios; }
        }

        /// <summary>
        /// Instantiates a new <see cref="ScenarioTester{TScenario}" /> object.
        /// </summary>
        /// <param name="scenarios">The test scenarios.</param>
        public ScenarioTester(IEnumerable<TScenario> scenarios)
        {
            _scenarios = scenarios.ToArray();
        }

        /// <summary>
        /// Call the specified <paramref name="test" /> action for each <see cref="Scenarios" /> item.
        /// </summary>
        /// <param name="test">"Callback" test action.</param>
        public ScenarioTester<TScenario> TestEach(Action<TScenario> test)
        {
            var caughtExceptions = new List<ScenarioException>();
            int scenarioIndex = -1;

            foreach (TScenario scenario in _scenarios)
            {
                Exception scenarioException = null;

                scenarioIndex++;

                try
                {
                    _beforeTest(scenario);
                    test(scenario);
                }
                catch (Exception ex)
                {
                    scenarioException = ex;
                }

                bool passed = false;
                try
                {
                    passed = _afterTest(scenario, scenarioException);

                    if (!passed && scenarioException == null)
                    {
                        throw new InvalidOperationException("Scenario failed by \"AfterEachTest\" function.");
                    }
                }
                catch (Exception afterTestEx)
                {
                    scenarioException = afterTestEx;
                }

                if (!passed)
                {
                    SaveCaughtException(scenarioIndex, scenarioException, caughtExceptions);
                }
            }

            HandleCaughtExceptions(caughtExceptions);

            return this;
        }

        private void SaveCaughtException(
            int scenarioIndex,
            Exception ex,
            List<ScenarioException> caughtException)
        {
            caughtException.Add(
              new ScenarioException(
                ex.GetType(),
                $"{BuildScenarioDescription(scenarioIndex)} - {ex.Message}{BuildScenarioJson(scenarioIndex)}"));
        }

        private string BuildScenarioDescription(int scenarioIndex)
        {
            return $"Scenario[{scenarioIndex}] ({scenarioIndex + 1} of {_scenarios.Length})";
        }

        private string BuildScenarioJson(int scenarioIndex)
        {
            TScenario scenario = _scenarios[scenarioIndex];
            string jsonString = null;

            try
            {
                jsonString = JsonConvert.SerializeObject(
                    scenario,
                    new IsoDateTimeConverter { DateTimeFormat = "MM/dd/yyyy hh:mm:ss" },
                    new StringEnumConverter()
                );
            }
            catch (Exception)
            {
                jsonString = string.Empty;
            }

            if (string.IsNullOrWhiteSpace(jsonString) || jsonString == "{}")
            {
                jsonString = string.Empty;
            }
            else
            {
                string typeName = (scenario == null) ? "null" : scenario.GetType().FullName;


                if (typeName.Contains("_Anonymous"))
                {
                    typeName = "anonymousType";
                }

                typeName = _typeNameSuffixRegex.Replace(typeName, string.Empty);
                typeName = _genericVersionRegex.Replace(typeName, string.Empty);

                jsonString = $"\n\nScenario data - {typeName}: {jsonString}\n";
            }

            return jsonString;
        }

        private void HandleCaughtExceptions(List<ScenarioException> caughtExceptions)
        {
            if (caughtExceptions.Any())
            {
                string message = string.Join("\n", caughtExceptions.Select(x => x.Message).ToArray());

                if (caughtExceptions.All(IsInconclusiveException))
                {
                    ThrowInconclusiveException(message);
                }

                throw new ScenarioTestFailureException(message);
            }
        }

        private bool IsInconclusiveException(ScenarioException scenarioException)
        {
            return _inconclusiveExceptionTypes != null
                && _inconclusiveExceptionTypes.Contains(scenarioException.ExceptionType);
        }

        private class ScenarioException
        {
            public Type ExceptionType { get; private set; }
            public string Message { get; private set; }

            public ScenarioException(Type exceptionType, string message)
            {
                ExceptionType = exceptionType;
                Message = message;
            }
        }
    }
}