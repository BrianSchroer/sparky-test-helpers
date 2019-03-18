using System;
using System.Collections.Generic;

namespace SparkyTestHelpers.Scenarios
{
    public class ScenarioFactory<TArrangeObject, TAssertObject>
    {
        private readonly IList<Scenario<TArrangeObject, TAssertObject>> _list;
        private readonly Scenario<TArrangeObject, TAssertObject> _scenario;

        public ScenarioFactory(IList<Scenario<TArrangeObject, TAssertObject>> list, Func<TArrangeObject, TArrangeObject> arrange)
        {
            _list = list;

            _scenario = new Scenario<TArrangeObject, TAssertObject>
            {
                Arrange = arrange,
                Assert = instance => instance
            };

            _list.Add(_scenario);
        }

        public ScenarioFactory(IList<Scenario<TArrangeObject, TAssertObject>> list, Action<TArrangeObject> arrange)
        {
            _list = list;

            _scenario = new Scenario<TArrangeObject, TAssertObject>
            {
                Arrange = instance =>
                {
                    arrange(instance);
                    return instance;
                },
                Assert = instance => instance
            };

            _list.Add(_scenario);
        }

        public ScenarioFactory<TArrangeObject, TAssertObject> Arrange(Func<TArrangeObject, TArrangeObject> arrange)
        {
            return _list.Arrange(arrange);
        }

        public ScenarioFactory<TArrangeObject, TAssertObject> Arrange(Action<TArrangeObject> arrange)
        {
            return _list.Arrange(arrange);
        }

        public IList<Scenario<TArrangeObject, TAssertObject>> Assert(Func<TAssertObject, TAssertObject> assert)
        {
            _scenario.Assert = assert;
            return _list;
        }

        public IList<Scenario<TArrangeObject, TAssertObject>> Assert(Action<TAssertObject> assert)
        {
            _scenario.Assert = instance =>
            {
                assert(instance);
                return instance;
            };

            return _list;
        }

        public ScenarioTester<Scenario<TArrangeObject, TAssertObject>> TestEach(Action<Scenario<TArrangeObject, TAssertObject>> testAction)
        {
            return _list.TestEach(testAction);
        }
    }
}
