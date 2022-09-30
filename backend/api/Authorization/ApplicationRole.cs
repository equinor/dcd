using System.ComponentModel.DataAnnotations;

namespace Api.Authorization;

public enum ApplicationRole 
{
    /// <summary>
    /// For everyone not associated with the application.
    /// </summary>
    [Display(Name = "None")]
    None = 0,

    /// <summary>
    /// ...
    /// </summary>
    [Display(Name = "User")]
    External,

    /// <summary>
    /// ...
    /// </summary>
    [Display(Name = "ReadOnly")]
    Consultant,

    /// <summary>
    /// ...
    /// </summary>
    [Display(Name = "Admin")]
    EquinorEmployee,
}