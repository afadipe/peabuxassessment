using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Peabux.Domain.Entities;

public class User : IdentityUser
{
    [PersonalData]
    [Required]
    [StringLength(60)]
    public string? FirstName { get; set; }
    [PersonalData]
    [Required]
    [StringLength(60)]
    public string? Surname { get; set; }
    [PersonalData]
    [Required]
    [StringLength(40)]
    public string NationalIdNumber { get; set; }
    [PersonalData]
    [Required]
    public DateTime DateOfBirth { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    [NotMapped]
    public string FullName
    {
        get { return $"{this.Surname}, {this.FirstName}"; }
    }
}
