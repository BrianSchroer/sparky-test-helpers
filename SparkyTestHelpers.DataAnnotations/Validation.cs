namespace SparkyTestHelpers.DataAnnotations
{
    /// <summary>
    /// <see cref="ValidationForModel{TModel}"/> "factory" class.
    /// </summary>
    public static class Validation
    {
        /// <summary>
        /// Creates a new <see cref="ValidationForModel{TModel}"/> instance.
        /// </summary>
        /// <typeparam name="TModel">The model type.</typeparam>
        /// <param name="model">The model.</param>
        /// <returns>New <see cref="ValidationForModel{TModel}"/> instance.</returns>
        /// <example>
        /// <code><![CDATA[
        /// var foo = new Foo { /* valid property assignments */ };
        /// 
        /// Validation
        ///     .For(foo)
        ///     .ShouldReturn.NoErrors();
        /// ]]></code>
        /// </example>
        public static ValidationForModel<TModel> For<TModel>(TModel model)
        {
            return new ValidationForModel<TModel>(model);
        }
    }
}
