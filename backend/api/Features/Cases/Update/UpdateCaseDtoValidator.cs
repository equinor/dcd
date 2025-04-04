using api.Exceptions;

namespace api.Features.Cases.Update;

public static class UpdateCaseDtoValidator
{
    public static readonly DateTime MinAllowedDgDate = new(2000, 1, 1);
    public static readonly DateTime MaxAllowedDbDate = new(2150, 1, 1);

    public static void Validate(UpdateCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            throw new UnprocessableContentException($"{nameof(UpdateCaseDto)}.{nameof(dto.Name)} is required and cannot be white space only.");
        }

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

        if (dgDates.Any(x => x < MinAllowedDgDate))
        {
            throw new UnprocessableContentException($"One of the provided DG dates is before {MinAllowedDgDate}.");
        }

        if (dgDates.Any(x => x > MaxAllowedDbDate))
        {
            throw new UnprocessableContentException($"One of the provided DG dates is after {MaxAllowedDbDate}.");
        }

        if (dgDates.Any(x => x!.Value.TimeOfDay != TimeSpan.Zero))
        {
            throw new UnprocessableContentException("One of the provided DG dates are not in UTC time.");
        }
    }
}
