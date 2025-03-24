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
                dto.Dg0Date,
                dto.Dg1Date,
                dto.Dg2Date,
                dto.Dg3Date,
                dto.Dg4Date,
                dto.DgaDate,
                dto.DgbDate,
                dto.DgcDate,
                dto.ApboDate,
                dto.BorDate,
                dto.VpboDate
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

        if (dgDates.Any(x => x!.Value.TimeOfDay != TimeSpan.Zero))
        {
            throw new UnprocessableContentException("One of the provided DG dates are not in UTC time.");
        }
    }
}
