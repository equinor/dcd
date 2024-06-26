
using api.Models;
using api.Services;
using api.Services.GenerateCostProfiles;

namespace api.Services.Observers;

public interface IExplorationObserver
{
    void Update(Case caseEntity, string propertyName, object oldValue, object newValue);
}


public class ExplorationObserver : IExplorationObserver
{
    private readonly Lazy<IStudyCostProfileService> _studyCostProfileService;
    private readonly Lazy<ICo2EmissionsProfileService> _co2EmissionsProfileService;
    private readonly Lazy<IGenerateGAndGAdminCostProfile> _gAndGAdminCostProfileService;
    private readonly Lazy<IFuelFlaringLossesProfileService> _fuelFlaringLossesProfileService;
    private readonly Lazy<IImportedElectricityProfileService> _importedElectricityProfileService;



    public ExplorationObserver(
        Lazy<IStudyCostProfileService> studyCostProfileService,
        Lazy<ICo2EmissionsProfileService> co2EmissionsProfileService,
        Lazy<IGenerateGAndGAdminCostProfile> gAndGAdminCostProfileService,
        Lazy<IFuelFlaringLossesProfileService> fuelFlaringLossesProfileService,
        Lazy<IImportedElectricityProfileService> importedElectricityProfileService
        )
    {
        _studyCostProfileService = studyCostProfileService;
        _co2EmissionsProfileService = co2EmissionsProfileService;
        _gAndGAdminCostProfileService = gAndGAdminCostProfileService;
        _fuelFlaringLossesProfileService = fuelFlaringLossesProfileService;
        _importedElectricityProfileService = importedElectricityProfileService;
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
