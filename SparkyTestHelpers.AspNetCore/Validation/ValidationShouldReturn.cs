using System;
using System.Linq.Expressions;
using System.Reflection;

namespace SparkyTestHelpers.AspNetCore.Validation
{
    /// <summary>
    /// Expectation to be verified after <typeparamref name="TModel"/> is validated.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    public class ValidationShouldReturn<TModel>
    {
        private readonly ValidationForModel<TModel> _validationForModel;
        private readonly string _typeName;

        /// <summary>
        /// Called by <see cref="ValidationForModel{TModel}.ShouldReturn" />
        /// </summary>
        /// <param name="validationForModel"></param>
        internal ValidationShouldReturn(ValidationForModel<TModel> validationForModel)
        {
            _validationForModel = validationForModel;
            _typeName = typeof(TModel).Name;
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation returns no errors.
        /// </summary>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation resulted in error(s).</exception>
        public ValidationForModel<TModel> NoErrors()
        {
            return _validationForModel.ShouldReturnNoErrors();
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a "required field" error for the
        /// specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> RequiredFieldErrorFor(Expression<Func<TModel, object>> expression)
        {
            MemberInfo memberInfo = ReflectionHelper.GetMemberInfo(expression);
            string memberName = memberInfo.Name;
            string displayName = ReflectionHelper.GetDisplayName(memberInfo);

            return new ValidationResultTester<TModel>(_validationForModel, memberName)
                .WithErrorMessage($"The {displayName} field is required.");
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a string length error for the
        /// specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <param name="maxLength">The maximum string length.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> StringLengthErrorFor(Expression<Func<TModel, object>> expression, int maxLength)
        {
            MemberInfo memberInfo = ReflectionHelper.GetMemberInfo(expression);
            string memberName = memberInfo.Name;
            string displayName = ReflectionHelper.GetDisplayName(memberInfo);

            return new ValidationResultTester<TModel>(_validationForModel, memberName)
                .WithErrorMessage(
                    $"The field {displayName} must be a string or array type with a maximum length of '{maxLength}'.");
        }

        /// <summary>
        /// Creates new <see cref="ValidationResultTester{TModel}"/> for the specified <typeparamref name="TModel"/> field name.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>New <see cref="ValidationResultTester{TModel}"/> instance.</returns>
        public ValidationResultTester<TModel> ErrorFor(Expression<Func<TModel, object>> expression)
        {
            return new ValidationResultTester<TModel>(_validationForModel, ReflectionHelper.GetFieldName(expression));
        }
    }
}
