using api.Dtos;
using api.Models;

namespace api.Adapters;

public static class ExplorationAdapter
{

    public static Exploration Convert(ExplorationDto explorationDto)
    {
        var exploration = new Exploration
        {
            Id = explorationDto.Id,
            ProjectId = explorationDto.ProjectId,
            Name = explorationDto.Name,
            RigMobDemob = explorationDto.RigMobDemob,
            Currency = explorationDto.Currency,
        };
        exploration.ExplorationWellCostProfile = Convert<ExplorationWellCostProfileDto, ExplorationWellCostProfile>(explorationDto.ExplorationWellCostProfile, exploration);
        exploration.AppraisalWellCostProfile = Convert<AppraisalWellCostProfileDto, AppraisalWellCostProfile>(explorationDto.AppraisalWellCostProfile, exploration);
        exploration.SidetrackCostProfile = Convert<SidetrackCostProfileDto, SidetrackCostProfile>(explorationDto.SidetrackCostProfile, exploration);
        exploration.GAndGAdminCost = Convert<GAndGAdminCostDto, GAndGAdminCost>(explorationDto.GAndGAdminCost, exploration);
        exploration.SeismicAcquisitionAndProcessing = Convert<SeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>(explorationDto.SeismicAcquisitionAndProcessing, exploration);
        exploration.CountryOfficeCost = Convert<CountryOfficeCostDto, CountryOfficeCost>(explorationDto.CountryOfficeCost, exploration);
        return exploration;
    }

    public static void ConvertExisting(Exploration existing, ExplorationDto explorationDto)
    {
        existing.Id = explorationDto.Id;
        existing.ProjectId = explorationDto.ProjectId;
        existing.Name = explorationDto.Name;
        existing.RigMobDemob = explorationDto.RigMobDemob;
        existing.Currency = explorationDto.Currency;
        existing.ExplorationWellCostProfile = Convert<ExplorationWellCostProfileDto, ExplorationWellCostProfile>(explorationDto.ExplorationWellCostProfile, existing);
        existing.AppraisalWellCostProfile = Convert<AppraisalWellCostProfileDto, AppraisalWellCostProfile>(explorationDto.AppraisalWellCostProfile, existing);
        existing.SidetrackCostProfile = Convert<SidetrackCostProfileDto, SidetrackCostProfile>(explorationDto.SidetrackCostProfile, existing);
        existing.GAndGAdminCost = Convert<GAndGAdminCostDto, GAndGAdminCost>(explorationDto.GAndGAdminCost, existing);
        existing.SeismicAcquisitionAndProcessing = Convert<SeismicAcquisitionAndProcessingDto, SeismicAcquisitionAndProcessing>(explorationDto.SeismicAcquisitionAndProcessing, existing);
        existing.CountryOfficeCost = Convert<CountryOfficeCostDto, CountryOfficeCost>(explorationDto.CountryOfficeCost, existing);
    }

    private static TModel? Convert<TDto, TModel>(TDto? dto, Exploration exploration)
        where TDto : TimeSeriesCostDto
        where TModel : TimeSeriesCost, IExplorationTimeSeries, new()
    {
        if (dto == null) { return new TModel(); }

        return new TModel
        {
            Id = dto.Id,
            StartYear = dto.StartYear,
            Currency = dto.Currency,
            EPAVersion = dto.EPAVersion,
            Values = dto.Values,
            Exploration = exploration,
        };
    }
}
