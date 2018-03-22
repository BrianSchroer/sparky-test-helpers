using System;

namespace SparkyTestHelpers.AspNetMvc.Core
{
    /// <summary>
    /// <see cref="ViewResult.Model"/> tester for <see cref="ControllerActionTester"/>.
    /// </summary>
    /// <typeparam name="TModel"></typeparam>
    public class ModelTester<TModel> : IModelTester
    {
        private readonly Action<TModel> _validate;

        /// <summary>
        /// Creates new <see cref="ModelTester{TModel}"/> instance.
        /// </summary>
        /// <param name="validate">(Optional) model validation callback function.</param>
        public ModelTester(Action<TModel> validate = null)
        {
            _validate = validate;
        }

        /// <summary>
        /// Tests <paramref name="model"/> type and (optionally) calls validation callback function. 
        /// </summary>
        /// <param name="model">The model to be tested.</param>
        public void Test(object model)
        {
            if (model == null || !TypeTester.IsOfType(model, typeof(TModel)))
            {
                string actualTypeName = (model == null) ? "null" : model.GetType().FullName;
                throw new ControllerTestException(
                   $"Expected model type: {typeof(TModel).FullName}. Actual: {actualTypeName}.");
            }

            _validate?.Invoke((TModel)model);
        }
    }
}
