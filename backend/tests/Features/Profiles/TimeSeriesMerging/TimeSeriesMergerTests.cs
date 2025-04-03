using api.Features.Profiles.Dtos;
using api.Features.Profiles.TimeSeriesMerging;
using api.Models;

using Xunit;

namespace tests.Features.Profiles.TimeSeriesMerging;

public class TimeSeriesMergerTests
{
    [Fact]
    public void Creating_time_series_from_null__should_generate_default_time_series()
    {
        TimeSeriesProfile timeSeriesProfile = null;
        // ReSharper disable once ExpressionIsAlwaysNull
        var timeSeries = new TimeSeries(timeSeriesProfile);

        Assert.Equal(0, timeSeries.StartYear);
        Assert.NotNull(timeSeries.Values);
        Assert.Empty(timeSeries.Values);
    }

    [Fact]
    public void Merging_time_series__should_add_values_when_start_year_and_length_is_the_same()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3]
        };

        var merged = TimeSeriesMerger.MergeTimeSeries(series1, series2);

        Assert.Equal(2020, merged.StartYear);

        Assert.Equal(3, merged.Values.Length);

        Assert.Equal(2, merged.Values[0]);
        Assert.Equal(4, merged.Values[1]);
        Assert.Equal(6, merged.Values[2]);
    }

    [Fact]
    public void Merging_time_series__should_add_values_when_start_year_is_same_but_series1_length_is_longer()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3, 4]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3]
        };

        var merged = TimeSeriesMerger.MergeTimeSeries(series1, series2);

        Assert.Equal(2020, merged.StartYear);

        Assert.Equal(4, merged.Values.Length);

        Assert.Equal(2, merged.Values[0]);
        Assert.Equal(4, merged.Values[1]);
        Assert.Equal(6, merged.Values[2]);
        Assert.Equal(4, merged.Values[3]);
    }

    [Fact]
    public void Merging_time_series__should_add_values_when_start_year_is_same_but_series2_length_is_longer()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3, 4]
        };

        var merged = TimeSeriesMerger.MergeTimeSeries(series1, series2);

        Assert.Equal(2020, merged.StartYear);

        Assert.Equal(4, merged.Values.Length);

        Assert.Equal(2, merged.Values[0]);
        Assert.Equal(4, merged.Values[1]);
        Assert.Equal(6, merged.Values[2]);
        Assert.Equal(4, merged.Values[3]);
    }

    [Fact]
    public void Merging_time_series__should_add_all_values_from_multiple_series()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2020,
            Values = [1, 2, 3, 4]
        };

        var series3 = new TimeSeries
        {
            StartYear = 2021,
            Values = [1, 2, 3, 4]
        };

        var merged = TimeSeriesMerger.MergeTimeSeries(series1, series2, series3);

        Assert.Equal(2019, merged.StartYear);

        Assert.Equal(6, merged.Values.Length);

        Assert.Equal(1, merged.Values[0]); // 2019
        Assert.Equal(3, merged.Values[1]); // 2020
        Assert.Equal(6, merged.Values[2]); // 2021
        Assert.Equal(5, merged.Values[3]); // 2022
        Assert.Equal(7, merged.Values[4]); // 2023
        Assert.Equal(4, merged.Values[5]); // 2024
    }

    [Fact]
    public void Merging_time_series_with_subtraction__should_subtract_all_values_from_multiple_series()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3, 4]
        };

        var series3 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3, 4]
        };

        var merged = TimeSeriesMerger.MergeTimeSeriesWithSubtraction(series1, series2, series3);

        Assert.Equal(2019, merged.StartYear);

        Assert.Equal(4, merged.Values.Length);

        Assert.Equal(-1, merged.Values[0]); // 2019
        Assert.Equal(-2, merged.Values[1]); // 2020
        Assert.Equal(-3, merged.Values[2]); // 2021
        Assert.Equal(-8, merged.Values[3]); // 2022
    }

    [Fact]
    public void Merging_time_series_with_multiplication__should_multiply_all_values_from_multiple_series()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3, 4]
        };

        var series3 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3, 4]
        };

        var merged = TimeSeriesMerger.MergeTimeSeriesWithMultiplication(series1, series2, series3);

        Assert.Equal(2019, merged.StartYear);

        Assert.Equal(4, merged.Values.Length);

        Assert.Equal(1, merged.Values[0]); // 2019
        Assert.Equal(8, merged.Values[1]); // 2020
        Assert.Equal(27, merged.Values[2]); // 2021
        Assert.Equal(0, merged.Values[3]); // 2022
    }

    [Fact]
    public void Merging_time_series_with_division__should_divide_all_values_from_multiple_series()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3, 4]
        };

        var series3 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1, 2, 3, 4]
        };

        var merged = TimeSeriesMerger.MergeTimeSeriesWithDivision(series1, series2, series3);

        Assert.Equal(2019, merged.StartYear);

        Assert.Equal(4, merged.Values.Length);

        Assert.Equal(1, merged.Values[0]); // 2019
        Assert.Equal(0.5, merged.Values[1]); // 2020
        Assert.Equal(0.333, merged.Values[2], 3); // 2021
        Assert.Equal(0, merged.Values[3]); // 2022
    }

    [Fact]
    public void Merging_time_series_with_division__should_throw_error_when_dividing_by_0()
    {
        var series1 = new TimeSeries
        {
            StartYear = 2019,
            Values = [1]
        };

        var series2 = new TimeSeries
        {
            StartYear = 2019,
            Values = [0]
        };

        Assert.Throws<Exception>(() => TimeSeriesMerger.MergeTimeSeriesWithDivision(series1, series2));
    }
}
