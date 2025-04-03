using api.Features.Profiles;
using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;
using static api.Features.Profiles.VolumeConstants;

namespace api.Features.Cases.Recalculation.Types.TotalProductionVolume;

public static class TotalExportedVolumesProfileService
{
    public static void RunCalculation(Case caseItem)
    {
        var oilProduction = caseItem.GetProductionAndAdditionalProduction(ProfileTypes.ProductionProfileOil);

        var nglProduction = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.ProductionProfileNgl));
        nglProduction.Values = nglProduction.Values.Select(v => v * NglToOilEnergyRatio).ToArray();

        var condensateProduction = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.CondensateProduction));

        var netSalesGas = new TimeSeries(caseItem.GetOverrideProfileOrProfile(ProfileTypes.NetSalesGas));
        netSalesGas.Values = netSalesGas.Values.Select(v => v / OilToGasEnergyRatio).ToArray();

        var totalExportedVolumes = TimeSeriesMerger.MergeTimeSeries(oilProduction, nglProduction, condensateProduction, netSalesGas);
        totalExportedVolumes.Values = totalExportedVolumes.Values.Select(v => v * BarrelsPerCubicMeter).ToArray();

        var profile = caseItem.CreateProfileIfNotExists(ProfileTypes.TotalExportedVolumes);
        profile.Values = totalExportedVolumes.Values;
        profile.StartYear = totalExportedVolumes.StartYear;
    }
}
