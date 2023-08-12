using FluentValidation;
using HomeWork14.Models;
using System;

namespace HomeWork14.Validation
{
    public class PersonValidator : AbstractValidator<Person>
    {
        public PersonValidator() {
            RuleFor(person => person.FirstName).NotEmpty().WithMessage("FirstName must not be empty!")
                .Length(1, 50).WithMessage("FirstName length must be between 1 and 50 chars!");
            RuleFor(person => person.LastName).NotEmpty().WithMessage("LastName must not be empty!")
                .Length(1, 50).WithMessage("LastName length must be between 1 and 50 chars!");
            RuleFor(person => person.JobPosition).NotEmpty().WithMessage("JobPosition must not be empty!")
                .Length(1, 50).WithMessage("JobPosition length must be between 1 and 50 chars!");
            RuleFor(person => person.Salary).NotNull().InclusiveBetween(1, 10000).WithMessage("Salary must be between 1 and 10,000 GEL!");
            RuleFor(person => person.WorkExperience).NotNull().WithMessage("WorkExperience must not be empty!");
            RuleFor(person => person.Email).NotEmpty().EmailAddress().WithMessage("Enter your valid E-Mail address!");
            RuleFor(person => person.CreateDate).GreaterThanOrEqualTo(DateTime.Today).WithMessage("Created Date must not be greater than today!");
            RuleFor(person => person.PersonAddress).SetValidator(new AddressValidator());
        }
    }
}
