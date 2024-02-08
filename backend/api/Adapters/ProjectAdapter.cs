using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ProjectAdapter
{
    public static Project Convert(ProjectDto projectDto)
    {
        var project = new Project
        {
            Name = projectDto.Name,
            CommonLibraryId = projectDto.CommonLibraryId,
            CommonLibraryName = projectDto.CommonLibraryName,
            FusionProjectId = projectDto.FusionProjectId,
            ReferenceCaseId = projectDto.ReferenceCaseId,
            Description = projectDto.Description,
            Country = projectDto.Country,
            ProjectCategory = projectDto.ProjectCategory,
            ProjectPhase = projectDto.ProjectPhase,
            Currency = projectDto.Currency,
            PhysicalUnit = projectDto.PhysUnit,
            SharepointSiteUrl = projectDto.SharepointSiteUrl,
            ExplorationOperationalWellCosts =
                ExplorationOperationalWellCostsAdapter.Convert(projectDto.ExplorationOperationalWellCosts),
            DevelopmentOperationalWellCosts =
                DevelopmentOperationalWellCostsAdapter.Convert(projectDto.DevelopmentOperationalWellCosts),
            CO2RemovedFromGas = projectDto.CO2RemovedFromGas,
            CO2EmissionFromFuelGas = projectDto.CO2EmissionFromFuelGas,
            FlaredGasPerProducedVolume = projectDto.FlaredGasPerProducedVolume,
            CO2EmissionsFromFlaredGas = projectDto.CO2EmissionsFromFlaredGas,
            CO2Vented = projectDto.CO2Vented,
            DailyEmissionFromDrillingRig = projectDto.DailyEmissionFromDrillingRig,
            AverageDevelopmentDrillingDays = projectDto.AverageDevelopmentDrillingDays,
        };

        return project;
    }

    public static void ConvertExisting(Project existing, ProjectDto projectDto)
    {
        existing.Name = projectDto.Name;
        existing.CommonLibraryName = projectDto.CommonLibraryName;
        existing.Description = projectDto.Description;
        existing.Country = projectDto.Country;
        existing.Currency = projectDto.Currency;
        existing.PhysicalUnit = projectDto.PhysUnit;
        existing.SharepointSiteUrl = projectDto.SharepointSiteUrl;
        existing.CO2RemovedFromGas = projectDto.CO2RemovedFromGas;
        existing.CO2EmissionFromFuelGas = projectDto.CO2EmissionFromFuelGas;
        existing.FlaredGasPerProducedVolume = projectDto.FlaredGasPerProducedVolume;
        existing.CO2EmissionsFromFlaredGas = projectDto.CO2EmissionsFromFlaredGas;
        existing.CO2Vented = projectDto.CO2Vented;
        existing.DailyEmissionFromDrillingRig = projectDto.DailyEmissionFromDrillingRig;
        existing.AverageDevelopmentDrillingDays = projectDto.AverageDevelopmentDrillingDays;
        existing.ReferenceCaseId = projectDto.ReferenceCaseId;
    }

    public static void ConvertExistingFromProjectMaster(Project existing, ProjectDto projectDto)
    {
        existing.Name = projectDto.Name;
        existing.CommonLibraryId = projectDto.CommonLibraryId;
        existing.CommonLibraryName = projectDto.CommonLibraryName;
        existing.Country = projectDto.Country;
        existing.Id = projectDto.Id;
        existing.ProjectCategory = projectDto.ProjectCategory;
        existing.ProjectPhase = projectDto.ProjectPhase;
    }
}
