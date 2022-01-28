using api.Models;

namespace api.SampleData.Builders;

public class SubstructureBuilder : Substructure
{
    public SubstructureBuilder WithCostProfile(SubstructureCostProfileBuilder builder)
    {
        builder.Substructure = this;
        this.CostProfile = builder;
        return this;
    }
}

public class SubstructureCostProfileBuilder : SubstructureCostProfile
{
    public SubstructureCostProfileBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }

    public SubstructureCostProfileBuilder WithYearValue(int year, double value)
    {
        this.YearValues.Add(new YearValue<double>(year, value));
        return this;
    }
}
