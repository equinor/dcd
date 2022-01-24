using api.Models;

namespace api.SampleData;

public class WellProjectBuilder : WellProject
{
    public WellProjectBuilder() { }
    public WellProjectBuilder WithWellProjectCostProfile(WellProjectCostProfileBuilder w)
    {
        this.CostProfile = w;
        return this;
    }
    public WellProjectBuilder WithDrillingSchedule(DrillingScheduleBuilder d)
    {
        this.DrillingSchedule = d;
        return this;
    }
}

public class WellProjectCostProfileBuilder : WellProjectCostProfile
{
    public WellProjectCostProfileBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public WellProjectCostProfileBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class DrillingScheduleBuilder : DrillingSchedule
{
    public DrillingScheduleBuilder()
    {
        YearValues = new List<YearValue<int>>();
    }
    public DrillingScheduleBuilder WithYearValue(int year, int numberOfWells)
    {
        this.YearValues.Add(new YearValue<int>(year, numberOfWells));
        return this;
    }
}
