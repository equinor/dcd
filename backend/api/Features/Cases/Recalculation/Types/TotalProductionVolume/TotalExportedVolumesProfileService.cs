using api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

namespace api.Features.Cases.Recalculation.Types.TotalProductionVolume;

public static class TotalExportedVolumesProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var oilProductionProfile = Co2IntensityProfileService.GetOilProfile(caseItem);

        var nglProduction = GetNglProduction(caseItem);
        nglProduction.Values = nglProduction.Values.Select(v => v / 1_000_000 * 1.9).ToArray();

        var condensateProduction = GetCondensateProduction(caseItem);
        condensateProduction.Values = condensateProduction.Values.Select(v => v / 1_000_000).ToArray();

        var netSalesGas = GetNetSalesGas(caseItem);
        netSalesGas.Values = netSalesGas.Values.Select(v => v / 1E9).ToArray();

        var totalExportedVolumes = TimeSeriesMerger.MergeTimeSeries(oilProductionProfile, nglProduction, condensateProduction, netSalesGas);
        totalExportedVolumes.Values = totalExportedVolumes.Values.Select(v => v * 1_000_000 * VolumeConstants.BarrelsPerCubicMeter).ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalExportedVolumes);
        profile.Values = totalExportedVolumes.Values;
        profile.StartYear = totalExportedVolumes.StartYear;
    }

    public static TimeSeries GetTotalExportedVolumes(Case caseItem) =>
        new (caseItem.GetOverrideProfileOrProfile(ProfileTypes.TotalExportedVolumes));

    public static TimeSeries GetNglProduction(Case caseItem) =>
        new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ProductionProfileNgl));

    private static TimeSeries GetCondensateProduction(Case caseItem) =>
        new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CondensateProduction));

    private static TimeSeries GetNetSalesGas(Case caseItem) =>
        new(caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas));
}
