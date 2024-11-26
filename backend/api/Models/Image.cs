using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using api.Models.Interfaces;

namespace api.Models;

public class Image : IHasProjectId, IChangeTrackable
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public Guid ProjectId { get; set; }

    [ForeignKey("Case")]
    public Guid? CaseId { get; set; }

    [Required]
    public string ProjectName { get; set; } = null!;

    [Required]
    public string Url { get; set; } = null!;
    public DateTimeOffset CreateTime { get; set; }
    public string? Description { get; set; }
}
