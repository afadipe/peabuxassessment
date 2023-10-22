using System.ComponentModel.DataAnnotations.Schema;

namespace Peabux.Domain.Entities;

public class Student : BaseEntity
{
    public string StudentNumber { get; set; }
    [ForeignKey(nameof(User))]
    public Guid UserId { get; set; }
    public User User { get; set; }
}
