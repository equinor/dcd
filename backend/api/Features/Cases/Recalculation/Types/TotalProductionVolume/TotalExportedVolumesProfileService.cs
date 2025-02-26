
using api.Features.Cases.Recalculation.Calculators.GenerateCo2Intensity;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

public static class TotalExportedVolumesProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var oilProductionProfile = Co2IntensityProfileService.GetOilProfile(caseItem);

        var nglProduction = GetNglProduction(caseItem);
        nglProduction.Values = nglProduction.Values.Select(v => v / 1_000_000 * 1.9).ToArray();

        var condensateProduction = GetCondensateProduction(caseItem);
        condensateProduction.Values = condensateProduction.Values.Select(v => v / 1_000_000).ToArray();

        var netSalesGas = getNetSalesGas(caseItem);
        netSalesGas.Values = netSalesGas.Values.Select(v => v / 1E9).ToArray();

        var totalExportedVolumes = TimeSeriesMerger.MergeTimeSeries(oilProductionProfile, nglProduction, condensateProduction, netSalesGas);
        totalExportedVolumes.Values = totalExportedVolumes.Values.Select(v => v * 1_000_000 * 6.29).ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalExportedVolumes);
        profile.Values = totalExportedVolumes.Values;
        profile.StartYear = totalExportedVolumes.StartYear;
    }

    public static TimeSeries GetCondensateProduction(Case caseItem)
    {
        var CondensateProduction = caseItem.GetProfileOrNull(ProfileTypes.CondensateProduction);
        var CondensateProductionOverride = caseItem.GetProfileOrNull(ProfileTypes.CondensateProductionOverride);
        return CondensateProductionOverride?.Override == true
            ? new TimeSeries(CondensateProductionOverride)
            : new TimeSeries(CondensateProduction);
    }

    public static TimeSeries GetNglProduction(Case caseItem)
    {
        var NglProduction = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNgl);
        var NglProductionOverride = caseItem.GetProfileOrNull(ProfileTypes.ProductionProfileNglOverride);
        return NglProductionOverride?.Override == true
            ? new TimeSeries(NglProductionOverride)
            : new TimeSeries(NglProduction);
    }

    public static TimeSeries getNetSalesGas(Case caseItem)
    {
        var NetSalesGas = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGas);
        var NetSalesGasOverride = caseItem.GetProfileOrNull(ProfileTypes.NetSalesGasOverride);
        return NetSalesGasOverride?.Override == true
            ? new TimeSeries(NetSalesGasOverride)
            : new TimeSeries(NetSalesGas);
    }

    public static TimeSeries GetTotalExportedVolumes(Case caseItem)
    {
        var totalExportedVolumes = caseItem.GetProfileOrNull(ProfileTypes.TotalExportedVolumes);
        var totalExportedVolumesOverride = caseItem.GetProfileOrNull(ProfileTypes.TotalExportedVolumesOverride);
        return totalExportedVolumesOverride?.Override == true
            ? new TimeSeries(totalExportedVolumesOverride)
            : new TimeSeries(totalExportedVolumes);
    }
}
