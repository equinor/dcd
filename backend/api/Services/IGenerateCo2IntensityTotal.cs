namespace api.Services
{
    public interface IGenerateCo2IntensityTotal
    {
        double Calculate(Guid caseId);
    }
}
