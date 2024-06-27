namespace api.Services.GenerateCostProfiles
{
    public interface ICo2IntensityTotalService
    {
        Task<double> Calculate(Guid caseId);
    }
}
