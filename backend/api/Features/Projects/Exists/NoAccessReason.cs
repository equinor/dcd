namespace api.Features.Projects.Exists;

public enum NoAccessReason
{
    ProjectDoesNotExist = 1,
    ClassificationInternal = 2,
    ClassificationRestricted = 3,
    ClassificationConfidential = 4
}
