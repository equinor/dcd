using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Project
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Id { get; set; }
        [Required]
        public string ProjectName { get; set; }
        [Required]
        public DateTimeOffset CreateDate { get; set; }
        [Required]
        public ICollection<Case>? Cases { get; set; }

        public Project(string Id, string ProjectName)
        {
            this.Id = Id;
            this.ProjectName = ProjectName;
            this.CreateDate = DateTimeOffset.UtcNow;
        }
    }

    public class Case
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        public string Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Capex { get; set; }
        [Required]
        public string? Drillex { get; set; }
        [Required]
        public string? UR { get; set; }
        [Required]
        public DateTimeOffset CreateDate { get; set; }

        public Case(string id, string name)
        {
            this.Id = id;
            this.Name = name;
        }
    }

}
