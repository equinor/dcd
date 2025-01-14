using api.Context;
using api.Context.Extensions;
using api.Features.Assets.CaseAssets.Transports.Dtos;
using api.Features.Cases.Recalculation;
using api.Models;

using Microsoft.EntityFrameworkCore;

namespace api.Features.Assets.CaseAssets.Transports.Update;

public class UpdateTransportService(DcdDbContext context, IRecalculationService recalculationService)
{
    public async Task ResetTransport(Guid projectId, Guid caseId, Guid transportId)
    {
        var existing = await context.Transports.SingleAsync(x => x.ProjectId == projectId && x.Id == transportId);

        existing.GasExportPipelineLength = 0;
        existing.OilExportPipelineLength = 0;
        existing.Currency = Currency.NOK;
        existing.CostYear = 0;
        existing.DG3Date = null;
        existing.DG4Date = null;
        existing.Source = Source.ConceptApp;
        existing.ProspVersion = null;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task UpdateTransportFromProsp(Guid projectId, Guid caseId, Guid transportId, ProspUpdateTransportDto updatedTransportDto)
    {
        var existing = await context.Transports.SingleAsync(x => x.ProjectId == projectId && x.Id == transportId);

        existing.GasExportPipelineLength = updatedTransportDto.GasExportPipelineLength;
        existing.OilExportPipelineLength = updatedTransportDto.OilExportPipelineLength;
        existing.Currency = updatedTransportDto.Currency;
        existing.CostYear = updatedTransportDto.CostYear;
        existing.DG3Date = updatedTransportDto.DG3Date;
        existing.DG4Date = updatedTransportDto.DG4Date;
        existing.Source = updatedTransportDto.Source;
        existing.ProspVersion = updatedTransportDto.ProspVersion;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);
    }

    public async Task<TransportDto> UpdateTransport(Guid projectId, Guid caseId, Guid transportId, UpdateTransportDto dto)
    {
        var existing = await context.Transports.SingleAsync(x => x.ProjectId == projectId && x.Id == transportId);

        existing.GasExportPipelineLength = dto.GasExportPipelineLength;
        existing.OilExportPipelineLength = dto.OilExportPipelineLength;
        existing.Currency = dto.Currency;
        existing.CostYear = dto.CostYear;
        existing.DG3Date = dto.DG3Date;
        existing.DG4Date = dto.DG4Date;
        existing.Source = dto.Source;
        existing.Maturity = dto.Maturity;
        existing.LastChangedDate = DateTime.UtcNow;

        await context.UpdateCaseModifyTime(caseId);
        await recalculationService.SaveChangesAndRecalculateAsync(caseId);

        return new TransportDto
        {
            Id = existing.Id,
            Name = existing.Name,
            ProjectId = existing.ProjectId,
            Maturity = existing.Maturity,
            GasExportPipelineLength = existing.GasExportPipelineLength,
            OilExportPipelineLength = existing.OilExportPipelineLength,
            Currency = existing.Currency,
            LastChangedDate = existing.LastChangedDate,
            CostYear = existing.CostYear,
            Source = existing.Source,
            ProspVersion = existing.ProspVersion,
            DG3Date = existing.DG3Date,
            DG4Date = existing.DG4Date
        };
    }
}
