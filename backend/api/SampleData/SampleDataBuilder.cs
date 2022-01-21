
using api.Models;

namespace api.SampleData;

public class ProjectsBuilder
{
    public List<ProjectBuilder> Projects { get; set; } = new List<ProjectBuilder>();

    public ProjectsBuilder WithProject(ProjectBuilder p)
    {
        Projects.Add(p);
        return this;
    }

    public ProjectBuilder ForProject(string projectName)
    {
        var projectBuilder = Projects.FirstOrDefault(p => p.ProjectName.Equals(projectName));
        if (projectBuilder == null)
        {
            throw new Exception(string.Format("Cannot find project %s"));
        }
        return projectBuilder;
    }
}

public class ProjectBuilder : Project
{
    public ProjectBuilder()
    {
        Cases = new List<Case>();
        DrainageStrategies = new List<DrainageStrategy>();
        WellProjects = new List<WellProject>();
    }

    public ProjectBuilder WithDrainageStrategy(DrainageStrategyBuilder d)
    {
        d.Project = this;
        DrainageStrategies.Add(d);
        return this;
    }

    public ProjectBuilder WithWellProject(WellProjectBuilder w)
    {
        w.Project = this;
        WellProjects.Add(w);
        return this;
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

    public CaseBuilder WithDrainageStrategy(string drainageStrategyName, Project project)
    {

        var drainageStrategy = project.DrainageStrategies.FirstOrDefault(d => d.Name.Equals(drainageStrategyName));
        if (drainageStrategy == null)
        {
            throw new Exception(string.Format("Drainage strategy %s not found", drainageStrategyName));
        }
        DrainageStrategyLink = drainageStrategy.Id;
        return this;
    }
    public CaseBuilder WithCessationCost(CessationCostBuilder c)
    {
        c.Case = this;
        this.CessationCost = c;
        return this;
    }
    public CaseBuilder WithWellProject(string wellProjectName, Project project)
    {
        var wellProject = project.WellProjects.FirstOrDefault(d => d.Name.Equals(wellProjectName));
        if (wellProject == null)
        {
            throw new Exception(string.Format("Drainage strategy %s not found", wellProjectName));
        }
        WellProjectLink = wellProject.Id;
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

public class WellProjectBuilder : WellProject
{
    public WellProjectBuilder() { }
    public WellProjectBuilder WithWellProjectCostProfile(WellProjectCostProfileBuilder w)
    {
        this.CostProfile = w;
        return this;
    }
    public WellProjectBuilder WithDrillingSchedule(DrillingScheduleBuilder d)
    {
        this.DrillingSchedule = d;
        return this;
    }
}

public class WellProjectCostProfileBuilder : WellProjectCostProfile
{
    public WellProjectCostProfileBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public WellProjectCostProfileBuilder WithYearValue(int y, double v)
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
    public DrillingScheduleBuilder WithYearValue(int year, int numberOfWells)
    {
        this.YearValues.Add(new YearValue<int>(year, numberOfWells));
        return this;
    }
}

