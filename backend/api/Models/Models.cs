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
        public Project(string Id, string ProjectName, ICollection<Case> Cases)
        {
            this.Id = Id;
            this.ProjectName = ProjectName;
            this.CreateDate = DateTimeOffset.UtcNow;
            this.Cases = Cases;
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
        public long Capex { get; set; }
        public long? Drillex { get; set; }
        public long? UR { get; set; }
        [Required]
        public DateTimeOffset CreateDate { get; set; }

        public Case(string Id, string Name, long Capex, long? Drillex = null, long? UR = null)
        {
            this.Id = Id;
            this.Name = Name;
            this.Capex = Capex;
            this.Drillex = Drillex;
            this.UR = UR;
        }
    }

}
