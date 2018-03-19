using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SparkyTestHelpers.DataAnnotations
{
    /// <summary>
    /// Model validation tester.
    /// </summary>
    /// <remarks>
    /// Instances of this class will usually be created via the static
    /// <see cref="Validation.For{TModel}(TModel)"/> method.
    /// </remarks>
    /// <typeparam name="TModel">Model type.</typeparam>
    /// <seealso cref="Validation" />
    /// <seealso cref="ExpectedValidationError{TModel}" />
    public class ValidationForModel<TModel>
    {
        private static JsonSerializerSettings _deserializeSettings = new JsonSerializerSettings
        {
            ObjectCreationHandling = ObjectCreationHandling.Replace
        };

        private TModel _model;
        private readonly TModel _originalModel;
        private List<ValidationResult> _validationResults;
        private string _formattedValidationResults = null;

        /// <summary>
        /// Gets <see cref="ValidationResults"/> for the <see cref="TModel"/>.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// IEnumerable<ValidationResult> results =  Validation.For(foo).ValidationResults();
        /// ]]></code>
        /// </example>
        public IEnumerable<ValidationResult> ValidationResults
        {
            get
            {
                if (_validationResults == null)
                {
                    _validationResults = new List<ValidationResult>();

                    Validator.TryValidateObject(
                        _model,
                        new ValidationContext(_model, null, null),
                        _validationResults,
                        validateAllProperties: true);
                }

                return _validationResults;
            }
        }

        private string FormattedValidationResults
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_formattedValidationResults))
                {
                    _formattedValidationResults = string.Join("\n", ValidationResults.Select(FormatValidationResult));

                    if (string.IsNullOrWhiteSpace(_formattedValidationResults))
                    {
                        _formattedValidationResults = "(no validation errors)";
                    }
                }

                return _formattedValidationResults;
            }
        }

        /// <summary>
        /// Creates a new <see cref="ValidationShouldReturn{TModel}"/> instance.
        /// </summary>
        /// <example>
        /// <code><![CDATA[
        /// var foo = new Foo { /* valid property assignments */ };
        /// 
        /// Validation
        ///     .For(foo)
        ///     .ShouldReturn.NoErrors();
        /// ]]></code>
        /// </example>
        public ValidationShouldReturn<TModel> ShouldReturn
        {
            get { return new ValidationShouldReturn<TModel>(this); }
        }

        /// <summary>
        /// Creates a new <see cref="ValidationForModel{TModel}"/> instance.
        /// </summary>
        /// <remarks>
        /// Usually called via the static <see cref="Validation.For{TModel}(TModel)"/> method.
        /// </remarks>
        /// <param name="model">The <see cref="TModel"/> instance to be validated.</param>
        public ValidationForModel(TModel model)
        {
            _model = _originalModel = model;
        }

        /// <summary>
        /// Creates a "clone" of the <see cref="TModel"/> instance that was passed to
        /// <see cref="Validation.For{TModel}(TModel)"/> and calls back to
        /// <paramref name="modelModificationAction"/> to make the model modifications that
        /// are to be tested.
        /// </summary>
        /// <param name="modelModificationAction">"callback" action.</param>
        /// <returns>"This" <see cref="ValidationForModel{TModel}"/> instance.</returns>
        public ValidationForModel<TModel> When(Action<TModel> modelModificationAction)
        {
            _model = Clone(_originalModel);
            modelModificationAction(_model);

            _validationResults = null;
            _formattedValidationResults = null;

            return this;
        }

        /// <summary>
        /// Called by the <see cref="ValidationShouldReturn{TModel}.NoErrors"/> method.
        /// </summary>
        /// <returns>"This" <see cref="ValidationForModel{TModel}"/> instance.</returns>
        internal ValidationForModel<TModel> ShouldReturnNoErrors()
        {
            var validationResults = ValidationResults;

            if (validationResults.Any())
            {
                throw new ValidationTestException(
                    $"Expected 0 validation errors. Found {validationResults.Count()}."
                    + $"\n{FormattedValidationResults}\n");
            }

            return this;
        }

        /// <summary>
        /// Called by
        /// <see cref="ValidationResultTester{TModel}.WithMessage(string)"/>,
        /// <see cref="ValidationResultTester{TModel}.WithErrorMessageContaining(string)"/>,
        /// <see cref="ValidationResultTester{TModel}.WithErrorMessageMatching(string)"/>,
        /// <see cref="ValidationResultTester{TModel}.WithErrorMessageStartingWith(string)"/>.
        /// </summary>
        /// <param name="memberNames">The member names expected in the <see cref="ValidationResult"/>.</param>
        /// <param name="predicate">Predication for successful error message match.</param>
        /// <param name="errorMessage">Message to be included in the exception if no match is found.</param>
        /// <returns></returns>
        internal ValidationForModel<TModel> AssertResultMatch(
            IEnumerable<string> memberNames, Func<string, bool> predicate, string errorMessage)
        {
            string formattedMemberNames = FormatMemberNames(memberNames);

            ValidationResult result = ValidationResults.FirstOrDefault(x =>
                FormatMemberNames(x.MemberNames) == formattedMemberNames && predicate(x.ErrorMessage));

            if (result == null)
            {
                throw new ValidationTestException(
                    $"Expected {errorMessage}. {formattedMemberNames}. Found:\n{FormattedValidationResults}\n");
            }

            return this;
        }

        private TModel Clone(TModel model)
        {
            return (ReferenceEquals(model, null))
                ? default(TModel)
                : JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(model), _deserializeSettings);
        }

        private static string FormatValidationResult(ValidationResult validationResult)
        {
            IEnumerable<string> memberNames = validationResult.MemberNames ?? new string[0];

            return $"Error Message: \"{validationResult.ErrorMessage}\". {FormatMemberNames(validationResult.MemberNames)}.";
        }

        private static string FormatMemberNames(IEnumerable<string> memberNames)
        {
            return $"Member(s): \"{string.Join("\", \"", memberNames.OrderBy(x => x))}\"";
        }
    }
}
