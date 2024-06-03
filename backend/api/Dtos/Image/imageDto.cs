using System;
using System.ComponentModel.DataAnnotations;

namespace api.Dtos;

public class ImageDto
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    [Url]
    public required string Url { get; set; }

    [Required]
    public DateTimeOffset CreateTime { get; set; }

    public string? Description { get; set; }

    [Required]
    public Guid CaseId { get; set; }
}