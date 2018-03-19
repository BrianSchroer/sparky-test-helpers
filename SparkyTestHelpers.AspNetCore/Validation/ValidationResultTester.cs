using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.RegularExpressions;

namespace SparkyTestHelpers.AspNetCore.Validation
{
    /// <summary>
    /// This class is used to specify <see cref="ValidationResult"/> expectations.
    /// </summary>
    /// <typeparam name="TModel">The model type.</typeparam>
    public class ValidationResultTester<TModel>
    {
        private readonly ValidationForModel<TModel> _validationForModel;
        private readonly MemberInfo _memberInfo;
        private readonly List<string> _memberNames;

        /// <summary>
        /// Called by <see cref="ValidationShouldReturn{TModel}.NoErrors"/>
        /// </summary>
        /// <param name="validationForModel"></param>
        /// <param name="memberName"></param>
        internal ValidationResultTester(ValidationForModel<TModel> validationForModel, MemberInfo memberInfo)
        {
            _validationForModel = validationForModel;
            _memberInfo = memberInfo;
            _memberNames = new List<string> { memberInfo.Name };
        }

        /// <summary>
        /// Specifies additional field name for the expected <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="expression">"Callback" expression to specify the field name.</param>
        /// <returns>"This" <see cref="ValidationResultTester{TModel}"/> instance.</returns>
        public ValidationResultTester<TModel> AndFor(Expression<Func<TModel, object>> expression)
        {
            _memberNames.Add(ReflectionHelper.GetFieldName(expression));
            return this;
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a <see cref="ValidationResult"/>
        /// for the specified member name(s) and <typeparamref name="TAttribute"/> type.
        /// </summary>
        /// <typeparam name="TAttribute">The <see cref="ValidationAttribute"/> type.</typeparam>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> WithErrorForAttribute<TAttribute>() where TAttribute : ValidationAttribute
        {
            string memberName = _memberInfo.Name;
            string displayName = ReflectionHelper.GetDisplayName(_memberInfo);
            var validationAttribute = ReflectionHelper.GetValidationAttribute<TAttribute>(_memberInfo);

            return WithErrorMessage(validationAttribute.FormatErrorMessage(displayName));
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a <see cref="ValidationResult"/>
        /// for the specified member name(s) and the <paramref name="expectedErrorMessage"/>.
        /// </summary>
        /// <param name="expectedErrorMessage">The expected error message.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> WithErrorMessage(string expectedErrorMessage)
        {
            return _validationForModel.AssertResultMatch(
                _memberNames, 
                actual => actual.Equals(expectedErrorMessage, StringComparison.CurrentCulture),
                $"\"{expectedErrorMessage}\"");
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a <see cref="ValidationResult"/>
        /// for the specified member name(s) and an error messsage starting with the <paramref name="expected"/> string.
        /// </summary>
        /// <param name="expected">The string that the error message should start with.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> WithErrorMessageStartingWith(string expected)
        {
            return _validationForModel.AssertResultMatch(
                _memberNames,
                actual => actual.StartsWith(expected, StringComparison.CurrentCulture),
                $"error message starting with \"{expected}\"");
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a <see cref="ValidationResult"/>
        /// for the specified member name(s) and an error message containing the <paramref name="expected"/> string.
        /// </summary>
        /// <param name="expected">The string that the error message should contain.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> WithErrorMessageContaining(string expected)
        {
            return _validationForModel.AssertResultMatch(
                _memberNames,
                actual => actual.Contains(expected),
                $"error message containing \"{expected}\"");
        }

        /// <summary>
        /// Asserts that <typeparamref name="TModel"/> validation results in a <see cref="ValidationResult"/>
        /// for the specified member name(s) and an error message matching the specified 
        /// <paramref name="regularExpressionPattern"/>.
        /// </summary>
        /// <param name="expected">The regular expression pattern to match.</param>
        /// <returns>The <see cref="ValidationForModel{TModel}"/>.</returns>
        /// <exception cref="ValidationTestException">if validation did not result in the expected error.</exception>
        public ValidationForModel<TModel> WithErrorMessageMatching(string regularExpressionPattern)
        {
            return _validationForModel.AssertResultMatch(
                _memberNames,
                actual => Regex.IsMatch(actual, regularExpressionPattern),
                $"error message matching \"{regularExpressionPattern}\"");
        }
    }
}
