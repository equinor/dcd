using api.Exceptions;
using api.Features.Profiles;
using api.Features.Profiles.Dtos;

using Xunit;

namespace tests.Features.Profiles;

public class TimeSeriesDtoValidatorTests
{
    [Fact]
    public void Valid_TimeSeriesDto_does_not_throw()
    {
        var dto = new CreateTimeSeriesDto
        {
            ProfileType = ProfileTypes.Co2Emissions,
            StartYear = 0,
            Values = []
        };

        TimeSeriesDtoValidator.Validate(dto);
    }

    [Fact]
    public void Invalid_TimeSeriesDto_ProfileType_throws()
    {
        var dto = new CreateTimeSeriesDto
        {
            ProfileType = "foobar",
            StartYear = 0,
            Values = []
        };

        Assert.Throws<InvalidInputException>(() => TimeSeriesDtoValidator.Validate(dto));
    }

    [Fact]
    public void Invalid_TimeSeriesDto_Values_null_throws()
    {
        var dto = new CreateTimeSeriesDto
        {
            ProfileType = ProfileTypes.Co2Intensity,
            StartYear = 0,
            Values = null!
        };

        Assert.Throws<InvalidInputException>(() => TimeSeriesDtoValidator.Validate(dto));
    }
}
