
using System.ComponentModel.DataAnnotations;

namespace Peabux.Domain.Dtos;

public record RegistrationDto
{
    [Required(ErrorMessage = "Username is required")]
    public string? FirstName { get; init; }
    [Required(ErrorMessage = "Username is required")]
    public string? Surname { get; init; }
    [Required(ErrorMessage = "Password is required")]
    public string? Password { get; init; }
    public string? DateOfBirth { get; init; }
    public string? NationalIdNumber { get; set; }
    public string Role { get; init; }

    public string StudentNumber { get; set; }
    public string Title { get; set; }
    public string TeacherNumber { get; set; }
    public decimal? Salary { get; set; }
}


