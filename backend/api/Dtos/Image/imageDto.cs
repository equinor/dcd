using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class ImageDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Url]
    public string Url { get; set; } = null!;

    [Required]
    public DateTimeOffset CreateTime { get; set; }

    public string? Description { get; set; }

    [Required]
    public Guid? CaseId { get; set; }

    [Required]
    public string ProjectName { get; set; } = null!;

    [Required]
    public Guid ProjectId { get; set; }
}
