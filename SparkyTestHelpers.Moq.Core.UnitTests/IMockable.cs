using System;
using System.Collections.Generic;
using SparkyTestHelpers.Scenarios;

namespace SparkyTestHelpers.Moq.Core.UnitTests
{
    public interface IMockable
    {
        string Prop { get; set; }

        void WithNoParms();

        void WithMultipleParms(string inputString, int inputInt);

        string WithResponse(int input);

        void WithArray<T>(T[] input);

        void WithBoolean(bool input);

        void WithDecimal(decimal input);

        void WithDouble(double input);

        void WithDateTime(DateTime input);

        void WithDictionary<TKey, TValue>(Dictionary<TKey, TValue> input);

        void WithIEnumerable<T>(IEnumerable<T> input);

        void WithInt(int input);

        void WithKeyValuePair<TKey, TValue>(KeyValuePair<TKey, TValue> input);

        void WithList<T>(List<T> input);

        void WithObject(object input);

        bool WithOut(int value, out int count);

        bool WithOutObject(string value, out TestClass testInstance);

        void WithRef(string value, ref int count);

        void WithString(string input);

        void WithScenarioTester<TScenario>(ScenarioTester<TScenario> input);

        void WithTuple<T1, T2>(Tuple<T1, T2> input);
    }
}
