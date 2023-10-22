
using FluentValidation;
using Peabux.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Peabux.Domain.Dtos;

public record RegistrationDto
{
    //[Required(ErrorMessage = "FirstName is required")]
    public string? FirstName { get; init; }
    //[Required(ErrorMessage = "Surname is required")]
    public string? Surname { get; init; }
    //[Required(ErrorMessage = "Password is required")]
    public string Password { get; init; }
    public DateTime DateOfBirth { get; init; }
    //[Required(ErrorMessage = "NationalIdNumber is required")]
    public string NationalIdNumber { get; set; }
    public string Role { get; init; }
    //student
    public string StudentNumber { get; set; }
    //teacher
    public EnumTitle? Title { get; set; }
    public string TeacherNumber { get; set; }
    public decimal? Salary { get; set; }
}



public class RegistrationValidator : AbstractValidator<RegistrationDto>
{

    public RegistrationValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().NotNull();
        RuleFor(x => x.Surname).NotEmpty().NotNull();
        RuleFor(x => x.Password).NotEmpty().NotNull();
        RuleFor(x => x.NationalIdNumber).NotEmpty().NotNull();
        RuleFor(x => x.Role).NotEmpty().NotNull();
        RuleFor(x => x.Password).NotEmpty().NotNull();

        When(x => x.Role.Trim().ToUpper() == "STUDENT", () =>
        {
            RuleFor(x => x.StudentNumber).NotEmpty().NotNull();
            RuleFor(x => x.DateOfBirth)
           .Must(x => DateTime.Now.Year - x.Year < 21).WithMessage("Age must be less than 21 year");
        });

        When(x => x.Role.Trim().ToUpper() == "TEACHER", () =>
        {
            RuleFor(x => x.TeacherNumber).NotEmpty().NotNull();
            RuleFor(x => x.Title).IsInEnum().NotEmpty().NotNull();
            RuleFor(x => x.Salary).NotEmpty().NotNull().ScalePrecision(1, 5);
            RuleFor(x => x.DateOfBirth)
           .Must(x => DateTime.Now.Year - x.Year >= 22).WithMessage("Age must be greater than or equal to 22 year");
        });
    }

}





