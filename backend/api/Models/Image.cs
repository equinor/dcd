using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models;

public class Image
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string Url { get; set; } = string.Empty!;
    public DateTimeOffset CreateTime { get; set; }

    public string? Description { get; set; }

    [ForeignKey("Case")]
    public Guid CaseId { get; set; }
}
