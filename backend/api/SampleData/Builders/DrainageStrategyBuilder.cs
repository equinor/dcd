using api.Models;

namespace api.SampleData.Builders;

public class DrainageStrategyBuilder : DrainageStrategy
{
    public DrainageStrategyBuilder WithProductionProfileOil(ProductionProfileOilBuilder p)
    {
        p.DrainageStrategy = this;
        this.ProductionProfileOil = p;
        return this;
    }

    public DrainageStrategyBuilder WithProductionProfileGas(ProductionProfileGasBuilder p)
    {
        p.DrainageStrategy = this;
        this.ProductionProfileGas = p;
        return this;
    }
    public DrainageStrategyBuilder WithProductionProfileWater(ProductionProfileWaterBuilder p)
    {
        p.DrainageStrategy = this;
        this.ProductionProfileWater = p;
        return this;
    }
    public DrainageStrategyBuilder WithProductionProfileWaterInjection(ProductionProfileWaterInjectionBuilder p)
    {
        p.DrainageStrategy = this;
        this.ProductionProfileWaterInjection = p;
        return this;
    }
    public DrainageStrategyBuilder WithFuelFlaringAndLosses(FuelFlaringAndLossesBuilder p)
    {
        p.DrainageStrategy = this;
        this.FuelFlaringAndLosses = p;
        return this;
    }
    public DrainageStrategyBuilder WithNetSalesGas(NetSalesGasBuilder p)
    {
        p.DrainageStrategy = this;
        this.NetSalesGas = p;
        return this;
    }
    public DrainageStrategyBuilder WithCo2Emissions(Co2EmissionsBuilder p)
    {
        p.DrainageStrategy = this;
        this.Co2Emissions = p;
        return this;
    }
}

public class ProductionProfileOilBuilder : ProductionProfileOil
{
    public ProductionProfileOilBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public ProductionProfileOilBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class ProductionProfileGasBuilder : ProductionProfileGas
{
    public ProductionProfileGasBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public ProductionProfileGasBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class ProductionProfileWaterBuilder : ProductionProfileWater
{
    public ProductionProfileWaterBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public ProductionProfileWaterBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class ProductionProfileWaterInjectionBuilder : ProductionProfileWaterInjection
{
    public ProductionProfileWaterInjectionBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public ProductionProfileWaterInjectionBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class FuelFlaringAndLossesBuilder : FuelFlaringAndLosses
{
    public FuelFlaringAndLossesBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public FuelFlaringAndLossesBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class NetSalesGasBuilder : NetSalesGas
{
    public NetSalesGasBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public NetSalesGasBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

public class Co2EmissionsBuilder : Co2Emissions
{
    public Co2EmissionsBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public Co2EmissionsBuilder WithYearValue(int year, double value)
    {
        this.YearValues.Add(new YearValue<double>(year, value));
        return this;
    }
}
