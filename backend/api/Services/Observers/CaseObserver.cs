
using api.Models;
using api.Services;

namespace api.Services.Observers;

public interface ICaseObserver
{
    void Update(Case caseEntity, string propertyName, object oldValue, object newValue);
}


public class CaseObserver : ICaseObserver
{
    private readonly Lazy<IStudyCostProfileService> _studyCostProfileService;
    

    public CaseObserver(Lazy<IStudyCostProfileService> studyCostProfileService)
    {
        _studyCostProfileService = studyCostProfileService;
    }

    public void Update(Case caseEntity, string propertyName, object oldValue, object newValue)
    {
        Console.WriteLine("GenerateStudyCostPfile: Property {0} changed from {1} to {2}", propertyName, oldValue, newValue);

        // Assuming you want to recalculate StudyCost when the Name property changes
        if (propertyName == nameof(Case.Name))
        {
            _studyCostProfileService.Value.Generate(caseEntity);
        }
    }
}
