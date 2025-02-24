using api.Exceptions;

namespace api.Features.Cases.Update;

public static class UpdateCaseDtoValidator
{
    public static void Validate(UpdateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new UnprocessableContentException($"{nameof(UpdateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }

        var earliestAllowedDgDate = new DateTime(2000, 1, 1);

        var dgDates = new List<DateTime>
        {
            dto.DG0Date,
            dto.DG1Date,
            dto.DG2Date,
            dto.DG3Date,
            dto.DG4Date,
            dto.DGADate,
            dto.DGBDate,
            dto.DGCDate
        }
        .Where(x => x != DateTime.MinValue)
        .ToList();

        if (dgDates.Any(x => x < earliestAllowedDgDate))
        {
            throw new UnprocessableContentException($"One of the provided DG dates before than {earliestAllowedDgDate}.");
        }
    }
}
