using api.Features.Cases.Create;

using Xunit;

namespace tests.Features.Cases.Create;

public class CreateCaseServiceTests
{
    [Fact]
    public void DgDates__ShouldBeCalculatedFrom2030__WhenDtoDateIsDateTimeMinValue()
    {
        var dg4DateFromDto = DateTime.MinValue;

        var dgDates = CreateCaseService.CalculateDgDates(dg4DateFromDto);

        Assert.Equal(new DateTime(2030, 1, 1), dgDates.dg4);
        Assert.Equal(new DateTime(2027, 1, 1), dgDates.dg3);
        Assert.Equal(new DateTime(2026, 1, 1), dgDates.dg2);
        Assert.Equal(new DateTime(2025, 1, 1), dgDates.dg1);
        Assert.Equal(new DateTime(2024, 1, 1), dgDates.dg0);
    }

    [Fact]
    public void DgDates__ShouldBeCalculatedFromProvidedDate__WhenDtoDateIsNotDateTimeMinValue()
    {
        var dg4DateFromDto = new DateTime(2035, 6, 1);

        var dgDates = CreateCaseService.CalculateDgDates(dg4DateFromDto);

        Assert.Equal(new DateTime(2035, 6, 1), dgDates.dg4);
        Assert.Equal(new DateTime(2032, 6, 1), dgDates.dg3);
        Assert.Equal(new DateTime(2031, 6, 1), dgDates.dg2);
        Assert.Equal(new DateTime(2030, 6, 1), dgDates.dg1);
        Assert.Equal(new DateTime(2029, 6, 1), dgDates.dg0);
    }
}
