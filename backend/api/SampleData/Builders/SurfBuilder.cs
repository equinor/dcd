using api.Models;

namespace api.SampleData.Builders;

public class SurfBuilder : Surf
{
    public SurfBuilder WithCostProfile(SurfCostProfileBuilder builder)
    {
        builder.Surf = this;
        this.CostProfile = builder;
        return this;
    }
}

public class SurfCostProfileBuilder : SurfCostProfile
{
    public SurfCostProfileBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }

    public SurfCostProfileBuilder WithYearValue(int year, double value)
    {
        this.YearValues.Add(new YearValue<double>(year, value));
        return this;
    }
}
