using api.Models;

namespace api.SampleData.Builders;

public class TransportBuilder : Transport
{
    public TransportBuilder WithCostProfile(TransportCostProfileBuilder builder)
    {
        builder.Transport = this;
        this.CostProfile = builder;
        return this;
    }
}

public class TransportCostProfileBuilder : TransportCostProfile
{
    public TransportCostProfileBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }

    public TransportCostProfileBuilder WithYearValue(int year, double value)
    {
        this.YearValues.Add(new YearValue<double>(year, value));
        return this;
    }
}
