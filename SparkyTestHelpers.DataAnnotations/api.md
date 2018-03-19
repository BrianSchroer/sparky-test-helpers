_see also_:
* the rest of the [**"Sparky suite"** of .NET utilities and test helpers](https://www.nuget.org/profiles/BrianSchroer)
---
This NuGet package contains helper classes to enable unit testing of .NET models and entities that use [System.ComponentModel.DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netframework-4.7.1) **ValidationAttribute**s via a fluent syntax.

Validation is initialized via the static `Validation.For(obj)` method:

```csharp
using SparkyTestHelpers.DataAnnotation;
. . .
    var foo = new Foo { /* populated with valid values */ };
    ValidationForModel<Foo> validation = Validation.For(foo);
```

The `.ValidationResults()` method validates the object and returns the results:
```csharp
IEnumerable<ValidationResult> validationResults = Validation.For(foo).ValidationResults();
```

...but you'll usually check validation results via other fluently-written methods:
```csharp
Validation.For(foo).ShouldReturn.NoErrors();
```

The **When**(*expression*) method arranges entity adjustments to be tested. (It makes a "clone" of the instance passed to **Validation.For()** before making adjustments, so each "When" call starts fresh with the original values):

```csharp
Validation.For(foo)
    .When(x => x.Bar = null)
    .ShouldReturn.RequiredErrorFor(x => x.Bar);
```

A code example above shows a call to **.ShouldReturn.NoErrors()** to assert that the model has no validation errors. **.ShouldReturn.ErrorFor()** is used to assert that a specific validation error *was* returned:

```csharp
Validation.For(foo)
    .When(x => x.Bar = "13")
    .ShouldReturn.ErrorFor(x => x.Bar).WithMessage("Invalid Bar value. 13 is unlucky!");

// Multi-member validation result:
Validation.For(foo)
    .When(x => 
    { 
        x.Bar = "dog";
        x.Baz = "cat";
    })
    .ShouldReturn.ErrorFor(x => x.Bar).AndFor(x => x.Baz)
        .WithMessage("Invalid Bar/Baz combination.");
``` 

**ShouldReturn.ErrrorFor(*expression*).ForValidationAttribute(<*TAttribute*>)** is used to assert that a ValidationResult was created by a specific **ValidationAttribute**:
```csharp
Validation.For(foo)
    .When(x => x.Bar = null)
    .ShouldReturn.ErrorFor(x => x.Bar).ForValidationAttribute<RequiredAttribute>();
```

There are also **ShouldReturn.** methods for standard **ValidationAttribute**s:
* [CreditCard]: ShouldReturn.**CreditCardErrorFor**(*x => x.Foo*);
* [EmailAddress]: ShouldReturn.**EmailAddressErrorFor**(*x => x.Foo*);
* [EnumDataType]: ShouldReturn.**EnumDataTypeErrorFor**(*x => x.Foo*);
* [MaxLength]: ShouldReturn.**MaxLengthErrorFor**(*x => x.Foo*);
* [MinLength]: ShouldReturn.**MinLengthErrorFor**(*x => x.Foo*);
* [Phone]: ShouldReturn.**PhoneErrorFor**(*x => x.Foo*);
* [Range]: ShouldReturn.**RangeErrorFor**(*x => x.Foo*);
* [RegularExpression]: ShouldReturn.**RegularExpressionErrorFor**(*x => x.Foo*);
* [Required]: ShouldReturn.**RequiredErrorFor**(*x => x.Foo*);
* [StringLength]: ShouldReturn.**StringLengthErrorFor**(*x => x.Foo*);

You don't have to specify the expected error message for **.ForValidationAttribute** method or for the methods listed above. The validation tester automatically determines the error message from the attribute.