using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using SparkyTestHelpers.AspNetCore.Validation;
using System.Linq;
using SparkyTestHelpers.Exceptions;

namespace SparkyTestHelpers.AspNetCore.UnitTests
{
    [TestClass]
    public class ValidationTests
    {
        private ValidationForModel<ValidationTests> _validationForModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _validationForModel = Validation.Validation.For(this);
        }

        [TestMethod]
        public void Validation_For_should_create_new_ValidationForModel_instance()
        {
            Assert.IsInstanceOfType(_validationForModel, typeof(ValidationForModel<ValidationTests>));
        }

        [TestMethod]
        public void ValidationForModel_ValidationResults_should_validate_and_return_results()
        {
            ValidationResult[] results = _validationForModel.ValidationResults.ToArray();
            Assert.AreEqual(0, results.Length);

            _validationForModel.When(x => x.StringProp1 = null);
            results = _validationForModel.ValidationResults.ToArray();
            Assert.AreEqual(1, results.Length);
        }

        [TestMethod]
        public void ValidationForModel_With_should_return_self()
        {
            ValidationForModel<ValidationTests> response = _validationForModel.When(x => x.StringProp1 = null);
            Assert.AreSame(_validationForModel, response);
        }

        [TestMethod]
        public void ValidationForModel_ShouldReturn_should_create_new_ValidationShouldReturn_instance()
        {
            ValidationShouldReturn<ValidationTests> shouldReturn = _validationForModel.ShouldReturn;
            Assert.IsNotNull(shouldReturn);
        }

        [TestMethod]
        public void ValidationShouldReturn_NoErrors_should_not_throw_exception_when_there_are_no_errors()
        {
            AssertExceptionNotThrown.WhenExecuting(() => _validationForModel.ShouldReturn.NoErrors());
        }

        [TestMethod]
        public void ValidationShouldReturn_NoErrors_should_throw_exception_when_there_are_errors()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                 .WithMessage("Expected 0 validation errors. Found 1."
                    + "\nError Message: \"The StringProp1 field is required.\". Member(s): \"StringProp1\".\n")
                .WhenExecuting(() => _validationForModel.When(x => x.StringProp1 = null).ShouldReturn.NoErrors());
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredFieldErrorFor_should_not_throw_exception_when_error_is_found()
        {
            AssertExceptionNotThrown.WhenExecuting(() => 
                _validationForModel.When(x => x.StringProp1 = null).ShouldReturn.RequiredFieldErrorFor(x => x.StringProp1));
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredFieldErrorFor_should_throw_exception_when_wrong_field_name_is_specified()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected " 
                    + "\"The StringProp2 field is required.\". Member(s): \"StringProp2\"." 
                    + " Found:\nError Message: "
                    + "\"The StringProp1 field is required.\". Member(s): \"StringProp1\".\n")
                .WhenExecuting(() =>
                    _validationForModel.When(x => x.StringProp1 = null).ShouldReturn.RequiredFieldErrorFor(x => x.StringProp2)
                );
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredFieldErrorFor_should_throw_exception_when_no_errors_are_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage("Expected \"The StringProp2 field is required.\". Member(s): \"StringProp2\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => _validationForModel.ShouldReturn.RequiredFieldErrorFor(x => x.StringProp2));
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredFieldErrorFor_should_use_Display_Names()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _validationForModel.When(x => x.StringProp3 = null).ShouldReturn.RequiredFieldErrorFor(x => x.StringProp3));

            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage("Expected \"The StringProp3 Display Name field is required.\". Member(s): \"StringProp3\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => 
                    _validationForModel.When(x => x.StringProp3 = "x").ShouldReturn.RequiredFieldErrorFor(x => x.StringProp3));
        }

        [TestMethod]
        public void ValidationShouldReturn_StringLengthErrorFor_should_not_throw_exception_when_error_is_found()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _validationForModel
                    .When(x => x.StringProp1 = new string('x', 81))
                    .ShouldReturn.StringLengthErrorFor(x => x.StringProp1, 80));
        }

        [TestMethod]
        public void ValidationShouldReturn_StringLengthErrorFor_should_throw_exception_when_no_errors_are_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected \"The field StringProp1 must be a string or array type with a maximum length of '80'.\". Member(s): \"StringProp1\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => _validationForModel.ShouldReturn.StringLengthErrorFor(x => x.StringProp1, 80));
        }

        [TestMethod]
        public void ValidationShouldReturn_StringLengthErrorFor_should_throw_exception_when_wrong_length_is_specified()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected "
                    + "\"The field StringProp1 must be a string or array type with a maximum length of '70'.\". Member(s): \"StringProp1\"." 
                    + " Found:\nError Message: "
                    + "\"The field StringProp1 must be a string or array type with a maximum length of '80'.\". Member(s): \"StringProp1\".\n")
                .WhenExecuting(() => 
                    _validationForModel.When(x => x.StringProp1 = new string('x', 81)).ShouldReturn.StringLengthErrorFor(x => x.StringProp1, 70));
        }

        [TestMethod]
        public void ValidationShouldReturn_ErrorFor_should_return_new_ValidationResultTester_instance()
        {
            ValidationResultTester<ValidationTests> tester = _validationForModel.ShouldReturn.ErrorFor(x => x.StringProp1);
            Assert.IsNotNull(tester);
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessage_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1 = string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessage("The StringProp1 field is required.");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessage_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                 .WithMessage("Expected \"The StringProp1 field is required.\". Member(s): \"StringProp1\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() =>
                    _validationForModel.ShouldReturn.ErrorFor(x => x.StringProp1).WithErrorMessage("The StringProp1 field is required."));
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageStartingWith_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1 = string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessageStartingWith("The StringProp1 field is req");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageStartingWith_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected error message starting with \"wrong\". Member(s): \"StringProp1\"."
                    + " Found:\nError Message: "
                    + "\"The StringProp1 field is required.\". Member(s): \"StringProp1\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                        .When(x => x.StringProp1 = null)
                        .ShouldReturn.ErrorFor(x => x.StringProp1)
                        .WithErrorMessageStartingWith("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageContaining_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1 = string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessageContaining("StringProp1 field is req");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageContaining_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected error message containing \"wrong\". Member(s): \"StringProp1\"."
                    + " Found:\nError Message: "
                    + "\"The StringProp1 field is required.\". Member(s): \"StringProp1\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                        .When(x => x.StringProp1 = null)
                        .ShouldReturn.ErrorFor(x => x.StringProp1)
                        .WithErrorMessageContaining("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageMatching_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1 = string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessageMatching(".*StringProp1 field.*");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageMatching_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected error message matching \"wrong\". Member(s): \"StringProp1\"."
                    + " Found:\nError Message: "
                    + "\"The StringProp1 field is required.\". Member(s): \"StringProp1\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                        .When(x => x.StringProp1 = null)
                        .ShouldReturn.ErrorFor(x => x.StringProp1)
                        .WithErrorMessageMatching("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_with_multiple_member_names_should_not_throw_exception_when_expected_error_is_found()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _validationForModel
                    .When(x => x.StringProp2 = "wrong")
                    .ShouldReturn
                        .ErrorFor(x => x.StringProp2)
                        .AndFor(x => x.StringProp3)
                        .WithErrorMessage("Invalid StringProp2/StringProp3 combination."));
        }

        [TestMethod]
        public void ValidationResultTester_with_multiple_member_names_should_throw_exception_when_message_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected \"wrong\". Member(s): \"StringProp2\", \"StringProp3\"."
                    + " Found:\nError Message: "
                    + "\"Invalid StringProp2/StringProp3 combination.\". Member(s): \"StringProp2\", \"StringProp3\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                    .When(x => x.StringProp2 = "wrong")
                    .ShouldReturn.ErrorFor(x => x.StringProp2).AndFor(x => x.StringProp3)
                    .WithErrorMessage("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_with_multiple_member_names_should_throw_exception_when_wrong_member_names_are_specified()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected \"wrong\". Member(s): \"StringProp1\", \"StringProp2\"."
                    + " Found:\nError Message: "
                    + "\"Invalid StringProp2/StringProp3 combination.\". Member(s): \"StringProp2\", \"StringProp3\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                    .When(x => x.StringProp2 = "wrong")
                    .ShouldReturn.ErrorFor(x => x.StringProp1).AndFor(x => x.StringProp2)
                    .WithErrorMessage("wrong"));
        }

        #region Model properties and logic
        [Required]
        [MaxLength(80)]
        public string StringProp1 { get; set; } = "StringProp1 value";

        [CustomValidation(typeof(ValidationTests), "ValidateStringProp2")]
        public string StringProp2 { get; set; } = "Valid value";

        [Required]
        [Display(Name = "StringProp3 Display Name")]
        public string StringProp3 { get; set; } = "StringProp3 value";

        public static ValidationResult ValidateStringProp2(string value, ValidationContext context)
        {
            if (value != "Valid value")
            {
                return new ValidationResult("Invalid StringProp2/StringProp3 combination.", new[] { nameof(StringProp2), nameof(StringProp3) });
            }

            return ValidationResult.Success;
        }

        #endregion
    }
}
