using api.Adapters;
using api.Dtos;
using api.Helpers;
using api.Models;

namespace api.Services.GenerateCostProfiles;

public class GenerateFuelFlaringLossesProfile
{
    private readonly CaseService _caseService;
    private readonly DrainageStrategyService _drainageStrategyService;
    private readonly ProjectService _projectService;
    private readonly TopsideService _topsideService;

    public GenerateFuelFlaringLossesProfile(IServiceProvider serviceProvider)
    {
        _caseService = serviceProvider.GetRequiredService<CaseService>();
        _projectService = serviceProvider.GetRequiredService<ProjectService>();
        _topsideService = serviceProvider.GetRequiredService<TopsideService>();
        _drainageStrategyService = serviceProvider.GetRequiredService<DrainageStrategyService>();
    }

    public FuelFlaringAndLossesDto Generate(Guid caseId)
    {
        var caseItem = _caseService.GetCase(caseId);
        var topside = _topsideService.GetTopside(caseItem.TopsideLink);
        var project = _projectService.GetProjectWithoutAssets(caseItem.ProjectId);
        var drainageStrategy = _drainageStrategyService.GetDrainageStrategy(caseItem.DrainageStrategyLink);
        // fuel consumption = fuel gas consumption * total use of power (GSm3)
        // fuel gas consumption = PE * FCM * cd / 1000 (Divide by1000 to convert from million M to billion G)
        // total use of power = total use of power oil + total use of power gas + total use of power wi  (percentage)

        // fuel consumption = PE * FCM * cd  * (OSP*OOM + OR*OSP*(1-OOM) + GSP*OOM + GR*GSP*(1-GOM) + WSP*WOM + WR*WSP*(1-WOM)) *1E6 [Sm3/yr] (New store in base unit)
        // fuel consumption = facilities availability * Fuel gas consumption max * calender days * (CO2 share gas profile * CO2 on max oil + )
        var part1 = caseItem.FacilitiesAvailability * topside.FuelConsumption /* check if correct */ * 365 * 1_000_000;
        // OSP*OOM 
        // CO2 share oil profile * CO2 on max oil
        var part2 = topside.CO2ShareOilProfile * topside.CO2OnMaxOilProfile;
        //  OR*OSP*(1-OOM)
        // gas rate, from production profile gas * CO2 share oil profile * (1 - CO2 on max oil)
        var part3 = drainageStrategy.ProductionProfileOil?.Values.Select(v => v * topside.CO2ShareOilProfile * (1 - topside.CO2OnMaxOilProfile));
        var part3a = drainageStrategy.ProductionProfileOil?.Values.Select(v => part2 + (v * topside.CO2ShareOilProfile * (1 - topside.CO2OnMaxOilProfile))).ToArray() ?? Array.Empty<double>();
        var part3ts = new TimeSeries<double>
        {
            Values = part3a,
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
        };

        // GSP*OOM
        // CO2 share gas profile * CO2 on max oil <- Is this correct?
        var part4 = topside.CO2ShareGasProfile * topside.CO2OnMaxGasProfile;

        // GR*GSP*(1-GOM)
        // gas rate, from production profile gas * CO2 share gas profile * (1 - CO2 on max gas)
        var part5 = drainageStrategy.ProductionProfileGas?.Values.Select(v => v * topside.CO2ShareGasProfile * (1 - topside.CO2OnMaxGasProfile));
        var part5a = drainageStrategy.ProductionProfileGas?.Values.Select(v => part4 + (v * topside.CO2ShareGasProfile * (1 - topside.CO2OnMaxGasProfile))).ToArray() ?? Array.Empty<double>();
        var part5ts = new TimeSeries<double>
        {
            Values = part5a,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };

        // WSP*WOM
        // CO2 share wi profile * CO2 on max wi
        var part6 = topside.CO2ShareWaterInjectionProfile * topside.CO2OnMaxWaterInjectionProfile;

        // WR*WSP*(1-WOM)
        // water injection rate, from injection profile water * CO2 share wi profile * (1 - CO2 on max wi)
        var part7 = drainageStrategy.ProductionProfileWaterInjection?.Values.Select(v => v * topside.CO2ShareWaterInjectionProfile * (1 - topside.CO2OnMaxWaterInjectionProfile));
        var part7a = drainageStrategy.ProductionProfileWaterInjection?.Values.Select(v => part6 + (v * topside.CO2ShareWaterInjectionProfile * (1 - topside.CO2OnMaxWaterInjectionProfile))).ToArray() ?? Array.Empty<double>();
        var part7ts = new TimeSeries<double>
        {
            Values = part7a,
            StartYear = drainageStrategy.ProductionProfileWaterInjection?.StartYear ?? 0,
        };

        // fuel consumption = PE * FCM * cd  * (OSP*OOM + OR*OSP*(1-OOM) + GSP*OOM + GR*GSP*(1-GOM) + WSP*WOM + WR*WSP*(1-WOM)) *1E6 [Sm3/yr] (New store in base unit)

        var fuelConsumptions = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> { part3ts, part5ts, part7ts });

        // var fuelConsumptions =
        //     EmissionCalculationHelper.CalculateTotalFuelConsumptions(caseItem, topside, drainageStrategy);

        // flaring = (OR + GR/1000) * FGV  [Sm3/yr]  (NEW store in base unit)
        // (oil rate, from production profile oil
        var part8 = drainageStrategy.ProductionProfileOil?.Values.Select(v => v).ToArray() ?? Array.Empty<double>();
        var part8ts = new TimeSeries<double>
        {
            Values = part8,
            StartYear = drainageStrategy.ProductionProfileOil?.StartYear ?? 0,
        };

        // gas rate, from production profile gas/1000
        var part8a = drainageStrategy.ProductionProfileGas?.Values.Select(v => v / 1000).ToArray() ?? Array.Empty<double>();
        var part8ats = new TimeSeries<double>
        {
            Values = part8a,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };
        // * Flared gas per produced volume
        var part8bts = TimeSeriesCost.MergeCostProfiles(part8ats, part8ts);
        var flaringValues = part8bts.Values.Select(v => v * 1.321).ToArray() ?? Array.Empty<double>(); // project.flaredgasperproducedvolume
        var flaring = new TimeSeries<double>
        {
            Values = flaringValues,
            StartYear = part8bts.StartYear,
        };

        // var flarings = EmissionCalculationHelper.CalculateFlaring(project, drainageStrategy);

        // losses = GR * CRG [Sm3/yr] (NEW store in base unit)
        var part9 = drainageStrategy.ProductionProfileGas?.Values.Select(v => v * project.CO2RemovedFromGas).ToArray() ?? Array.Empty<double>();
        var losses = new TimeSeries<double>
        {
            Values = part9,
            StartYear = drainageStrategy.ProductionProfileGas?.StartYear ?? 0,
        };

        // var losses = EmissionCalculationHelper.CalculateLosses(project, drainageStrategy);

        // Total profile = fuel consumption + flaring + losses
        // Total profile = PE * FCM * cd  * (OSP*OOM + OR*OSP*(1-OOM) + GSP*OOM + GR*GSP*(1-GOM) + WSP*WOM + WR*WSP*(1-WOM)) *1E6 [Sm3/yr] + (OR + GR/1000) * FGV + GR * CRG [GSm3/yr]
        // var totalProfile =
        //     TimeSeriesCost.MergeCostProfiles(TimeSeriesCost.MergeCostProfiles(fuelConsumptions, flarings), losses);

        var total = TimeSeriesCost.MergeCostProfilesList(new List<TimeSeries<double>> { fuelConsumptions, flaring, losses });

        var fuelFlaringLosses = new FuelFlaringAndLosses
        {
            StartYear = total.StartYear,
            Values = total.Values,
        };

        var dto = DrainageStrategyDtoAdapter.Convert(fuelFlaringLosses, project.PhysicalUnit);
        return dto ?? new FuelFlaringAndLossesDto();
    }
}
