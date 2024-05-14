using System.ComponentModel.DataAnnotations;

namespace api.Authorization;

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
    User,

    /// <summary>
    /// ...
    /// </summary>
    [Display(Name = "ReadOnly")]
    ReadOnly,

    /// <summary>
    /// ...
    /// </summary>
    [Display(Name = "Admin")]
    Admin,
}
