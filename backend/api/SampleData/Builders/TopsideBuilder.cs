using api.Models;

namespace api.SampleData.Builders;

public class TopsideBuilder : Topside
{
    public TopsideBuilder WithCostProfile(TopsideCostProfileBuilder builder)
    {
        builder.Topside = this;
        this.CostProfile = builder;
        return this;
    }
}

public class TopsideCostProfileBuilder : TopsideCostProfile
{
    public TopsideCostProfileBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }

    public TopsideCostProfileBuilder WithYearValue(int year, double value)
    {
        this.YearValues.Add(new YearValue<double>(year, value));
        return this;
    }
}
