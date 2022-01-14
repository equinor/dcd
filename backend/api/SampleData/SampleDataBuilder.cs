
using api.Models;

namespace api.SampleData;

public class ProjectsBuilder
{
    public List<Project> Projects { get; set; } = new List<Project>();

    public ProjectsBuilder WithProject(ProjectBuilder p)
    {
        Projects.Add(p);
        return this;
    }
}
public class ProjectBuilder : Project
{
    public ProjectBuilder()
    {
        Cases = new List<Case>();
    }

    public ProjectBuilder WithCase(CaseBuilder c)
    {
        c.Project = this;
        Cases.Add(c);
        return this;
    }
}

public class CaseBuilder : Case
{
    public CaseBuilder WithDrainageStrategy(DrainageStrategyBuilder d)
    {
        d.Case = this;
        this.DrainageStrategy = d;
        return this;
    }
    public CaseBuilder WithCessationCost(CessationCostBuilder c)
    {
        c.Case = this;
        this.CessationCost = c;
        return this;
    }
    public CaseBuilder WithExploration(ExplorationBuilder b)
    {
        b.Case = this;
        this.Exploration = b;
        return this;
    }
}

public class CessationCostBuilder : CessationCost
{
    public CessationCostBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public CessationCostBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

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

public class ExplorationBuilder : Exploration
{
    public ExplorationBuilder WithCost(ExplorationCostBuilder b)
    {
        this.Cost = b;
        return this;
    }
    public ExplorationBuilder WithDrillingSchedule(DrillingScheduleBuilder b)
    {
        b.Exploration = this;
        this.DrillingSchedule = b;
        return this;
    }
    public ExplorationBuilder WithGGAndAdminCost(GGAndAdminCostBuilder b)
    {
        b.Exploration = this;
        this.GGAndAdminCost = b;
        return this;
    }
}

public class ExplorationCostBuilder : ExplorationCost<double>
{
    public ExplorationCostBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public ExplorationCostBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}
public class DrillingScheduleBuilder : DrillingSchedule
{
    public DrillingScheduleBuilder()
    {
        YearValues = new List<YearValue<int>>();
    }
    public DrillingScheduleBuilder WithYearValue(int y, int v)
    {
        this.YearValues.Add(new YearValue<int>(y, v));
        return this;
    }
}

public class GGAndAdminCostBuilder : GGAndAdminCost<double>
{
    public GGAndAdminCostBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public GGAndAdminCostBuilder WithYearValue(int y, double v)
    {
        this.YearValues.Add(new YearValue<double>(y, v));
        return this;
    }
}

