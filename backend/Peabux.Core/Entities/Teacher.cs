
using Peabux.Domain.Enums;
using SmartAnalyzers.CSharpExtensions.Annotations;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.ComponentModel.DataAnnotations.Schema;

[assembly: InitRequiredForNotNull]
namespace Peabux.Domain.Entities;
//https://cezarypiatek.github.io/post/better-non-nullable-handling/
public class Teacher : BaseEntity
{
    public EnumTitle Title { get; set; }
    public string TeacherNumber { get; set; }
    public decimal Salary { get; set; }
    [ForeignKey(nameof(User))]
    public string UserId { get; set; }
    public User User { get; set; }
}
