using api.Models;

namespace api.SampleData;

public class TransportBuilder : Transport
{
    public TransportBuilder WithCostProfile(TransportCostProfileBuilder builder)
    {
        builder.Transport = this;
        this.CostProfile = builder;
        return this;
    }

    public TransportBuilder WithGasExportPipelineLength(GasExportPipelineLengthBuilder builder) 
    {
        builder.Transport = this;
        this.GasExportPipelineLength = builder;
        return this;
    }

    public TransportBuilder WithOilExportPipelineLength(OilExportPipelineLengthBuilder builder) 
    {
        builder.Transport = this;
        this.OilExportPipelineLength = builder;
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

public class GasExportPipelineLengthBuilder : GasExportPipelineLength
{
    public GasExportPipelineLengthBuilder WithValue(LengthUnit unit, double value)
    {
        this.Unit = unit;
        this.Value = value;
        return this;
    }
}

public class OilExportPipelineLengthBuilder : OilExportPipelineLength
{
    public OilExportPipelineLengthBuilder WithValue(LengthUnit unit, double value)
    {
        this.Unit = unit;
        this.Value = value;
        return this;
    }
}
