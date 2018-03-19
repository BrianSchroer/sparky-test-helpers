using System;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace SparkyTestHelpers.DataAnnotations
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
        /// Asserts that <typeparamref name="TModel"/> validation results in a CreditCard error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> CreditCardErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<CreditCardAttribute>(expression); 
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a EmailAddress error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> EmailAddressErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<EmailAddressAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a EnumDataType error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> EnumDataTypeErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<EnumDataTypeAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a MaxLength error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> MaxLengthErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<MaxLengthAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a MinLength error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> MinLengthErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<MinLengthAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a Phone error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> PhoneErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<PhoneAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a Range error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> RangeErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<RangeAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a RegularExpression error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> RegularExpressionErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<RegularExpressionAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a Required error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> RequiredErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<RequiredAttribute>(expression);
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a StringLength error for the specified field.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> StringLengthErrorFor(Expression<Func<TModel, object>> expression)
        {
            return ValidationAttributeError<StringLengthAttribute>(expression);
        }

        /// <summary>
        /// Creates new <see cref="ValidationResultTester{TModel}"/> for the specified <typeparamref name="TModel"/> field name.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>New <see cref="ValidationResultTester{TModel}"/> instance.</returns>
        public ValidationResultTester<TModel> ErrorFor(Expression<Func<TModel, object>> expression)
        {
            return new ValidationResultTester<TModel>(_validationForModel, ReflectionHelper.GetMemberInfo(expression));
        }

        private ValidationForModel<TModel> ValidationAttributeError<TAttribute>(
            Expression<Func<TModel, object>> expression) where TAttribute : ValidationAttribute
        {
            MemberInfo memberInfo = ReflectionHelper.GetMemberInfo(expression);
            return new ValidationResultTester<TModel>(_validationForModel, memberInfo).ForValidationAttribute<TAttribute>();
        }
    }
}
