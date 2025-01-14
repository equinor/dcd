using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Surfs.Profiles.Dtos;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Surfs.Update;

public class UpdateSurfService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task ResetSurf(Guid projectId, Guid caseId, Guid surfId)
    {
        var existingSurf = await context.Surfs.SingleAsync(x => x.ProjectId == projectId && x.Id == surfId);

        existingSurf.Source = Source.ConceptApp;
        existingSurf.ProspVersion = null;
        existingSurf.CessationCost = 0;
        existingSurf.InfieldPipelineSystemLength = 0;
        existingSurf.UmbilicalSystemLength = 0;
        existingSurf.ArtificialLift = ArtificialLift.NoArtificialLift;
        existingSurf.RiserCount = 0;
        existingSurf.TemplateCount = 0;
        existingSurf.ProducerCount = 0;
        existingSurf.GasInjectorCount = 0;
        existingSurf.WaterInjectorCount = 0;
        existingSurf.ProductionFlowline = ProductionFlowline.No_production_flowline;
        existingSurf.Currency = Currency.NOK;
        existingSurf.CostYear = 0;
        existingSurf.ApprovedBy = "";
        existingSurf.DG3Date = null;
        existingSurf.DG4Date = null;
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateSurfFromProsp(Guid projectId, Guid caseId, Guid surfId, ProspUpdateSurfDto dto)
    {
        var existingSurf = await context.Surfs.SingleAsync(x => x.ProjectId == projectId && x.Id == surfId);

        existingSurf.Source = dto.Source;
        existingSurf.ProspVersion = dto.ProspVersion;
        existingSurf.CessationCost = dto.CessationCost;
        existingSurf.InfieldPipelineSystemLength = dto.InfieldPipelineSystemLength;
        existingSurf.UmbilicalSystemLength = dto.UmbilicalSystemLength;
        existingSurf.ArtificialLift = dto.ArtificialLift;
        existingSurf.RiserCount = dto.RiserCount;
        existingSurf.TemplateCount = dto.TemplateCount;
        existingSurf.ProducerCount = dto.ProducerCount;
        existingSurf.GasInjectorCount = dto.GasInjectorCount;
        existingSurf.WaterInjectorCount = dto.WaterInjectorCount;
        existingSurf.ProductionFlowline = dto.ProductionFlowline;
        existingSurf.Currency = dto.Currency;
        existingSurf.CostYear = dto.CostYear;
        existingSurf.ApprovedBy = dto.ApprovedBy;
        existingSurf.DG3Date = dto.DG3Date;
        existingSurf.DG4Date = dto.DG4Date;
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task<SurfDto> UpdateSurf(Guid projectId, Guid caseId, Guid surfId, UpdateSurfDto dto)
    {
        var existingSurf = await context.Surfs.SingleAsync(x => x.ProjectId == projectId && x.Id == surfId);

        existingSurf.Maturity = dto.Maturity;
        existingSurf.CessationCost = dto.CessationCost;
        existingSurf.InfieldPipelineSystemLength = dto.InfieldPipelineSystemLength;
        existingSurf.UmbilicalSystemLength = dto.UmbilicalSystemLength;
        existingSurf.ArtificialLift = dto.ArtificialLift;
        existingSurf.RiserCount = dto.RiserCount;
        existingSurf.TemplateCount = dto.TemplateCount;
        existingSurf.ProducerCount = dto.ProducerCount;
        existingSurf.GasInjectorCount = dto.GasInjectorCount;
        existingSurf.WaterInjectorCount = dto.WaterInjectorCount;
        existingSurf.ProductionFlowline = dto.ProductionFlowline;
        existingSurf.Currency = dto.Currency;
        existingSurf.CostYear = dto.CostYear;
        existingSurf.Source = dto.Source;
        existingSurf.ApprovedBy = dto.ApprovedBy;
        existingSurf.DG3Date = dto.DG3Date;
        existingSurf.DG4Date = dto.DG4Date;
        existingSurf.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new SurfDto
        {
            Id = existingSurf.Id,
            Name = existingSurf.Name,
            ProjectId = existingSurf.ProjectId,
            CessationCost = existingSurf.CessationCost,
            Maturity = existingSurf.Maturity,
            InfieldPipelineSystemLength = existingSurf.InfieldPipelineSystemLength,
            UmbilicalSystemLength = existingSurf.UmbilicalSystemLength,
            ArtificialLift = existingSurf.ArtificialLift,
            RiserCount = existingSurf.RiserCount,
            TemplateCount = existingSurf.TemplateCount,
            ProducerCount = existingSurf.ProducerCount,
            GasInjectorCount = existingSurf.GasInjectorCount,
            WaterInjectorCount = existingSurf.WaterInjectorCount,
            ProductionFlowline = existingSurf.ProductionFlowline,
            Currency = existingSurf.Currency,
            LastChangedDate = existingSurf.LastChangedDate.Value,
            CostYear = existingSurf.CostYear,
            Source = existingSurf.Source,
            ProspVersion = existingSurf.ProspVersion,
            ApprovedBy = existingSurf.ApprovedBy,
            DG3Date = existingSurf.DG3Date,
            DG4Date = existingSurf.DG4Date
        };
    }
}
