using api.Features.Cases.Create;

using Xunit;

namespace tests.Features.Cases.Create;

public class CreateCaseServiceTests
{
    [Fact]
    public void DgDates__ShouldBeCalculatedFromProvidedDate__WhenDtoDateIsCloseToMinDate()
    {
        var dg4DateFromDto = new DateTime(2001, 1, 1);

        var dgDates = CreateCaseService.CalculateDgDates(dg4DateFromDto);

        Assert.Equal(new DateTime(2001, 1, 1), dgDates.dg4);
        Assert.Null(dgDates.dg3);
        Assert.Null(dgDates.dg2);
        Assert.Null(dgDates.dg1);
        Assert.Null(dgDates.dg0);
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
