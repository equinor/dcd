using api.Models;

namespace api.SampleData;

public class SubstructureBuilder : Substructure
{
    public SubstructureBuilder WithCostProfile(SubstructureCostProfileBuilder builder)
    {
        builder.Substructure = this;
        this.CostProfile = builder;
        return this;
    }

    public SubstructureBuilder WithDryWeight(SubstructureDryWeightBuilder builder)
    {
        builder.Substructure = this;
        this.DryWeight = builder;
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

public class SubstructureDryWeightBuilder : SubstructureDryWeight
{
    public SubstructureDryWeightBuilder WithValue(WeightUnit unit, double value)
    {
        this.Unit = unit;
        this.Value = value;
        return this;
    }
}
