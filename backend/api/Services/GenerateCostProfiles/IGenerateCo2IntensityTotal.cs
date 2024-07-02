namespace api.Services.GenerateCostProfiles
{
    public interface IGenerateCo2IntensityTotal
    {
        Task<double> Calculate(Guid caseId);
    }
}
