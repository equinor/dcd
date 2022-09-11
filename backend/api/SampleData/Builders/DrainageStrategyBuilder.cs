using api.Models;

namespace api.SampleData.Builders;

public class DrainageStrategyBuilder : DrainageStrategy
{
    public DrainageStrategyBuilder WithProductionProfileOil(ProductionProfileOil p)
    {
        p.DrainageStrategy = this;
        ProductionProfileOil = p;
        return this;
    }

    public DrainageStrategyBuilder WithProductionProfileGas(ProductionProfileGas p)
    {
        p.DrainageStrategy = this;
        ProductionProfileGas = p;
        return this;
    }

    public DrainageStrategyBuilder WithProductionProfileWater(ProductionProfileWater p)
    {
        p.DrainageStrategy = this;
        ProductionProfileWater = p;
        return this;
    }

    public DrainageStrategyBuilder WithProductionProfileWaterInjection(ProductionProfileWaterInjection p)
    {
        p.DrainageStrategy = this;
        ProductionProfileWaterInjection = p;
        return this;
    }

    public DrainageStrategyBuilder WithProductionProfileNGL(ProductionProfileNGL p)
    {
        p.DrainageStrategy = this;
        ProductionProfileNGL = p;
        return this;
    }

    public DrainageStrategyBuilder WithFuelFlaringAndLosses(FuelFlaringAndLosses p)
    {
        p.DrainageStrategy = this;
        FuelFlaringAndLosses = p;
        return this;
    }

    public DrainageStrategyBuilder WithNetSalesGas(NetSalesGas p)
    {
        p.DrainageStrategy = this;
        NetSalesGas = p;
        return this;
    }

    public DrainageStrategyBuilder WithCo2Emissions(Co2Emissions p)
    {
        p.DrainageStrategy = this;
        Co2Emissions = p;
        return this;
    }
}
