using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;

using Xunit;

namespace tests.Features.Profiles.TimeSeriesMerging;

public class TimeSeriesPaddingTests
{
    [Fact]
    public void PadTimeSeries_ShouldPadWithZeros_WhenTargetIsEmpty()
    {
        var target = new TimeSeries { StartYear = 2000, Values = [0] };
        var timeSeries = new TimeSeries { StartYear = 2000, Values = new double[10] };

        var result = TimeSeriesPadding.PadTimeSeries(target, timeSeries);

        Assert.Equal(2000, result.StartYear);
        Assert.Equal(10, result.Values.Length);
        Assert.All(result.Values, value => Assert.Equal(0, value));
    }

    [Fact]
    public void PadTimeSeriesStart_ShouldPadWithZeros_WhenStartYearDifferenceIsPositive()
    {
        var target = new TimeSeries { StartYear = 2005, Values = [1, 2, 3] };
        var timeSeries = new TimeSeries { StartYear = 2000, Values = new double[10] };

        // Act
        var result = TimeSeriesPadding.PadTimeSeriesStart(target, timeSeries);

        // Assert
        Assert.Equal(2000, result.StartYear);
        Assert.Equal(new double[] { 0, 0, 0, 0, 0, 1, 2, 3 }, result.Values);
    }

    [Fact]
    public void PadTimeSeriesEnd_ShouldPadWithZeros_WhenEndYearDifferenceIsPositive()
    {
        // Arrange
        var target = new TimeSeries { StartYear = 2000, Values = new double[] { 1, 2, 3 } };
        var timeSeries = new TimeSeries { StartYear = 2000, Values = new double[10] };

        // Act
        var result = TimeSeriesPadding.PadTimeSeriesEnd(target, timeSeries);

        // Assert
        Assert.Equal(2000, result.StartYear);
        Assert.Equal(new double[] { 1, 2, 3, 0, 0, 0, 0, 0, 0, 0 }, result.Values);
    }

    [Fact]
    public void PadTimeSeries_ShouldPadStartAndEndWithZeros_WhenEndAndStartDifferenceIsPositive()
    {
        var target = new TimeSeries { StartYear = 2005, Values = [1, 2, 3] };
        var timeSeries = new TimeSeries { StartYear = 2000, Values = new double[10] };

        var result = TimeSeriesPadding.PadTimeSeries(target, timeSeries);

        Assert.Equal(2000, result.StartYear);
        Assert.Equal(new double[] { 0, 0, 0, 0, 0, 1, 2, 3, 0, 0 }, result.Values);
    }
}
