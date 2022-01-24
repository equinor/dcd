using api.Models;

namespace api.SampleData;

public class TopsideBuilder : Topside
{
    public TopsideBuilder WithCostProfile(TopsideCostProfileBuilder builder)
    {
        builder.Topside = this;
        this.CostProfile = builder;
        return this;
    }

    public TopsideBuilder WithDryWeight(TopsideDryWeightBuilder builder)
    {
        builder.Topside = this;
        this.DryWeight = builder;
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

public class TopsideDryWeightBuilder : TopsideDryWeight
{
    public TopsideDryWeightBuilder WithValue(WeightUnit unit, double value)
    {
        this.Unit = unit;
        this.Value = value;
        return this;
    }
}
