
using FluentValidation;
using Peabux.Domain.Enums;

namespace Peabux.Domain.Dtos;

public record RegistrationDto
{
    public string? FirstName { get; init; }
    public string? Surname { get; init; }
    public string Password { get; init; }
    public DateTime DateOfBirth { get; init; }
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
        RuleFor(x => x.FirstName).NotEmpty().NotNull().WithMessage("FirstName is required");
        RuleFor(x => x.Surname).NotEmpty().NotNull().WithMessage("Surname is required");
        RuleFor(x => x.Password).NotEmpty().NotNull().WithMessage("Password is required");
        RuleFor(x => x.NationalIdNumber).NotEmpty().NotNull().WithMessage("NationalIdNumber is required");
        RuleFor(x => x.Role).NotEmpty().NotNull().WithMessage("Role is required");
        RuleFor(x => x.Password).NotEmpty().NotNull();

        When(x => x.Role.Trim().ToUpper() == "STUDENT", () =>
        {
            RuleFor(x => x.StudentNumber).NotEmpty().NotNull().WithMessage("StudentNumber is required");
            RuleFor(x => x.DateOfBirth)
           .Must(x => DateTime.Now.Year - x.Year < 21).WithMessage("Student age must be less than 21 year");
        });

        When(x => x.Role.Trim().ToUpper() == "TEACHER", () =>
        {
            RuleFor(x => x.TeacherNumber).NotEmpty().NotNull().WithMessage("TeacherNumber is required");
            RuleFor(x => x.Title).IsInEnum().NotEmpty().NotNull().WithMessage("Teacher Title is required");
            RuleFor(x => x.Salary).NotEmpty().NotNull().WithMessage("Teacher salary is required");
            RuleFor(x => x.DateOfBirth)
           .Must(x => DateTime.Now.Year - x.Year >= 22).WithMessage("Teacher age must be greater than or equal to 22 year");
        });
    }

}





