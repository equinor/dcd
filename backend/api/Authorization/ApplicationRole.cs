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
    /// Project user role
    /// </summary>
    [Display(Name = "User")]
    User,

    /// <summary>
    /// Observer role
    /// </summary>
    [Display(Name = "ReadOnly")]
    ReadOnly,

    /// <summary>
    /// Admin role
    /// </summary>
    [Display(Name = "Admin")]
    Admin,
}
