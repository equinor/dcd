using System.ComponentModel.DataAnnotations;

namespace api.AppInfrastructure.Authorization;

public enum ApplicationRole
{
    [Display(Name = "None")] None = 0,
    [Display(Name = "User")] User,
    [Display(Name = "ReadOnly")] ReadOnly,
    [Display(Name = "Admin")] Admin
}
