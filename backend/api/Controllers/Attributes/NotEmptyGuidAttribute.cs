using System.ComponentModel.DataAnnotations;

namespace api.Controllers;

public class NotEmptyGuidAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        ErrorMessage = "The GUID must not be empty.";
        if (value is Guid guidValue)
        {
            return guidValue != Guid.Empty;
        }
        return false; // If the value is not a Guid, it's considered invalid.
    }
}
