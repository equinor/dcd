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

        var minAllowedDgDate = new DateTime(2000, 1, 1);
        var maxAllowedDbDate = new DateTime(2150, 1, 1);

        var dgDates = new List<DateTime?>
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
            .Where(x => x != null)
            .Where(x => x != DateTime.MinValue)
            .ToList();

        if (dgDates.Any(x => x < minAllowedDgDate))
        {
            throw new UnprocessableContentException($"One of the provided DG dates is before {minAllowedDgDate}.");
        }

        if (dgDates.Any(x => x > maxAllowedDbDate))
        {
            throw new UnprocessableContentException($"One of the provided DG dates is after {maxAllowedDbDate}.");
        }
    }
}
