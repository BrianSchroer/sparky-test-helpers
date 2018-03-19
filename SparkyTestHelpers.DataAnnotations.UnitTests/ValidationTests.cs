using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.ComponentModel.DataAnnotations;
using SparkyTestHelpers.DataAnnotations;
using System.Linq;
using SparkyTestHelpers.Exceptions;
using System;

namespace SparkyTestHelpers.DataAnnotations.UnitTests
{
    [TestClass]
    public class ValidationTests
    {
        private ValidationForModel<ValidationTests> _validationForModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _validationForModel = Validation.For(this);
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

            _validationForModel.When(x => x.StringProp1= null);
            results = _validationForModel.ValidationResults.ToArray();
            Assert.AreEqual(1, results.Length);
        }

        [TestMethod]
        public void ValidationForModel_With_should_return_self()
        {
            ValidationForModel<ValidationTests> response = _validationForModel.When(x => x.StringProp1= null);
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
                    + $"\nError Message: \"The {nameof(StringProp1)} field is required.\". Member(s): \"{nameof(StringProp1)}\".\n")
                .WhenExecuting(() => _validationForModel.When(x => x.StringProp1= null).ShouldReturn.NoErrors());
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredErrorFor_should_not_throw_exception_when_error_is_found()
        {
            AssertExceptionNotThrown.WhenExecuting(() => 
                _validationForModel.When(x => x.StringProp1= null).ShouldReturn.RequiredErrorFor(x => x.StringProp1));
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredErrorFor_should_throw_exception_when_wrong_field_name_is_specified()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    "Expected " 
                    + $"\"The {nameof(StringProp3)} Display Name field is required.\". Member(s): \"{nameof(StringProp3)}\"." 
                    + " Found:\nError Message: "
                    + $"\"The {nameof(StringProp1)} field is required.\". Member(s): \"{nameof(StringProp1)}\".\n")
                .WhenExecuting(() =>
                    _validationForModel.When(x => x.StringProp1= null).ShouldReturn.RequiredErrorFor(x => x.StringProp3)
                );
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredErrorFor_should_throw_exception_when_no_errors_are_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage($"Expected \"The {nameof(StringProp3)} Display Name field is required.\". Member(s): \"{nameof(StringProp3)}\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => _validationForModel.ShouldReturn.RequiredErrorFor(x => x.StringProp3));
        }

        [TestMethod]
        public void ValidationShouldReturn_RequiredErrorFor_should_use_Display_Names()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _validationForModel.When(x => x.StringProp3 = null).ShouldReturn.RequiredErrorFor(x => x.StringProp3));

            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage($"Expected \"The {nameof(StringProp3)} Display Name field is required.\". Member(s): \"{nameof(StringProp3)}\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => 
                    _validationForModel.When(x => x.StringProp3 = "x").ShouldReturn.RequiredErrorFor(x => x.StringProp3));
        }
         
        [TestMethod]
        public void ValidationShouldReturn_MaxLengthErrorFor_should_not_throw_exception_when_error_is_found()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _validationForModel
                    .When(x => x.StringProp1= new string('x', 81))
                    .ShouldReturn.MaxLengthErrorFor(x => x.StringProp1));
        }

        [TestMethod]
        public void ValidationShouldReturn_MaxLengthErrorFor_should_throw_exception_when_no_errors_are_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    $"Expected \"The field {nameof(StringProp1)} must be a string or array type with a maximum length of '80'.\"." 
                    + $" Member(s): \"{nameof(StringProp1)}\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => _validationForModel.ShouldReturn.MaxLengthErrorFor(x => x.StringProp1));
        }

        [TestMethod]
        public void ValidationShouldReturn_StringLengthErrorFor_should_not_throw_exception_when_error_is_found()
        {
            AssertExceptionNotThrown.WhenExecuting(() =>
                _validationForModel
                    .When(x => x.StringProp4 = new string('x', 51))
                    .ShouldReturn.StringLengthErrorFor(x => x.StringProp4));
        }

        [TestMethod]
        public void ValidationShouldReturn_StringLengthErrorFor_should_throw_exception_when_no_errors_are_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    $"Expected \"The field StringProp4 must be a string with a maximum length of 50.\". Member(s): \"StringProp4\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() => _validationForModel.ShouldReturn.StringLengthErrorFor(x => x.StringProp4));
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
                .When(x => x.StringProp1= string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessage($"The {nameof(StringProp1)} field is required.");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessage_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                 .WithMessage($"Expected \"The {nameof(StringProp1)} field is required.\". Member(s): \"{nameof(StringProp1)}\". Found:"
                    + "\n(no validation errors)\n")
                .WhenExecuting(() =>
                    _validationForModel.ShouldReturn.ErrorFor(x => x.StringProp1).WithErrorMessage($"The {nameof(StringProp1)} field is required."));
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageStartingWith_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1= string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessageStartingWith($"The {nameof(StringProp1)} field is req");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageStartingWith_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    $"Expected error message starting with \"wrong\". Member(s): \"{nameof(StringProp1)}\"."
                    + " Found:\nError Message: "
                    + $"\"The {nameof(StringProp1)} field is required.\". Member(s): \"{nameof(StringProp1)}\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                        .When(x => x.StringProp1= null)
                        .ShouldReturn.ErrorFor(x => x.StringProp1)
                        .WithErrorMessageStartingWith("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageContaining_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1= string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessageContaining($"{nameof(StringProp1)} field is req");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageContaining_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    $"Expected error message containing \"wrong\". Member(s): \"{nameof(StringProp1)}\"."
                    + " Found:\nError Message: "
                    + $"\"The {nameof(StringProp1)} field is required.\". Member(s): \"{nameof(StringProp1)}\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                        .When(x => x.StringProp1= null)
                        .ShouldReturn.ErrorFor(x => x.StringProp1)
                        .WithErrorMessageContaining("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageMatching_should_not_throw_exception_when_expected_error_is_found()
        {
            _validationForModel
                .When(x => x.StringProp1= string.Empty)
                .ShouldReturn.ErrorFor(x => x.StringProp1)
                .WithErrorMessageMatching($".*{nameof(StringProp1)} field.*");
        }

        [TestMethod]
        public void ValidationResultTester_WithErrorMessageMatching_should_throw_exception_when_expected_error_is_not_found()
        {
            AssertExceptionThrown
                .OfType<ValidationTestException>()
                .WithMessage(
                    $"Expected error message matching \"wrong\". Member(s): \"{nameof(StringProp1)}\"."
                    + " Found:\nError Message: "
                    + $"\"The {nameof(StringProp1)} field is required.\". Member(s): \"{nameof(StringProp1)}\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                        .When(x => x.StringProp1= null)
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
                    $"Expected \"wrong\". Member(s): \"{nameof(StringProp2)}\", \"{nameof(StringProp3)}\"."
                    + " Found:\nError Message: "
                    + $"\"Invalid StringProp2/StringProp3 combination.\". Member(s): \"{nameof(StringProp2)}\", \"{nameof(StringProp3)}\".\n")
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
                    $"Expected \"wrong\". Member(s): \"{nameof(StringProp1)}\", \"{nameof(StringProp2)}\"."
                    + " Found:\nError Message: "
                    + $"\"Invalid StringProp2/StringProp3 combination.\". Member(s): \"{nameof(StringProp2)}\", \"StringProp3\".\n")
                .WhenExecuting(() =>
                    _validationForModel
                    .When(x => x.StringProp2 = "wrong")
                    .ShouldReturn.ErrorFor(x => x.StringProp1).AndFor(x => x.StringProp2)
                    .WithErrorMessage("wrong"));
        }

        [TestMethod]
        public void ValidationResultTester_should_handle_CreditCard_error()
        {
            _validationForModel.When(x => x.CreditCard = "x").ShouldReturn.CreditCardErrorFor(x => x.CreditCard);
        }

        [TestMethod]
        public void ValidationResultTester_should_handle_EmailAddress_error()
        {
            _validationForModel.When(x => x.EmailAddress = "some@emai l.com").ShouldReturn.EmailAddressErrorFor(x => x.EmailAddress);
        }

        [TestMethod]
        public void ValidationResultTester_should_handle_Enum_error()
        {
            _validationForModel.When(x => x.EnumString = "Invalid").ShouldReturn.EnumDataTypeErrorFor(x => x.EnumString);
        }

        [TestMethod]
        public void ValidationResultTester_should_handle_MinLength_error()
        {
            _validationForModel.When(x => x.MinLength = "A").ShouldReturn.MinLengthErrorFor(x => x.MinLength);
        }

        [TestMethod]
        public void ValidationResultTester_should_handle_Phone_error()
        {
            _validationForModel.When(x => x.Phone = "A").ShouldReturn.PhoneErrorFor(x => x.Phone);
        }

        [TestMethod]
        public void ValidationResultTester_should_handle_Range_error()
        {
            _validationForModel.When(x => x.Range = 100).ShouldReturn.RangeErrorFor(x => x.Range);
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

        [StringLength(50)]
        public string StringProp4 { get; set; } = "StringProp4 value";

        [RegularExpression(@"^[a-zA-Z''-'\s]{1,40}$")]
        public string StringProp5 { get; set; } = "Value";

        [CreditCard]
        public string CreditCard { get; set; } = "4012888888881881";

        [EmailAddress]
        public string EmailAddress { get; set; } = "brianschroer@gmail.com";

        [EnumDataType(typeof(StringComparison))]
        public string EnumString { get; set; } = StringComparison.CurrentCulture.ToString();

        [MinLength(5)]
        public string MinLength { get; set; } = "ABCDEFG";

        [Phone]
        public string Phone { get; set; } = "(314) 567-8900";

        [Range(10, 20)]
        public int Range { get; set; } = 15;

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
