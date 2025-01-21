namespace api.Features.Projects.Exists;

public enum NoAccessReason
{
    ProjectDoesNotExist = 1,
    ClassificationRestricted = 2,
    ClassificationConfidential = 3
}
