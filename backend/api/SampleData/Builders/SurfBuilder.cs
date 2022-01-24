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

    public SurfBuilder WithInfieldPipelineSystemLength(InfieldPipelineSystemLengthBuilder builder)
    {
        builder.Surf = this;
        this.InfieldPipelineSystemLength = builder;
        return this;
    }

    public SurfBuilder WithUmbilicalSystemLength(UmbilicalSystemLengthBuilder builder)
    {
        builder.Surf = this;
        this.UmbilicalSystemLength = builder;
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

public class InfieldPipelineSystemLengthBuilder : InfieldPipelineSystemLength
{
    public InfieldPipelineSystemLengthBuilder WithValue(LengthUnit unit, double value)
    {
        this.Unit = unit;
        this.Value = value;
        return this;
    }
}

public class UmbilicalSystemLengthBuilder : UmbilicalSystemLength
{
    public UmbilicalSystemLengthBuilder WithValue(LengthUnit unit, double value)
    {
        this.Unit = unit;
        this.Value = value;
        return this;
    }
}
