
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
        Surfs = new List<Surf>();
        Substructures = new List<Substructure>();
        Topsides = new List<Topside>();
        Transports = new List<Transport>();
    }

    public ProjectBuilder WithDrainageStrategy(DrainageStrategyBuilder d)
    {
        d.Project = this;
        DrainageStrategies.Add(d);
        return this;
    }

    public ProjectBuilder WithCase(CaseBuilder c)
    {
        c.Project = this;
        Cases.Add(c);
        return this;
    }

    public ProjectBuilder WithSurf(SurfBuilder s)
    {
        s.Project = this;
        Surfs.Add(s);
        return this;
    }

    public ProjectBuilder WithSubstructure(SubstructureBuilder s)
    {
        s.Project = this;
        Substructures.Add(s);
        return this;
    }

    public ProjectBuilder WithTopside(TopsideBuilder t)
    {
        t.Project = this;
        Topsides.Add(t);
        return this;
    }

    public ProjectBuilder WithTransport(TransportBuilder t)
    {
        t.Project = this;
        Transports.Add(t);
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
    public CaseBuilder WithSurf(string surfName, Project project)
    {
        var surf = project.Surfs.FirstOrDefault(d => d.Name.Equals(surfName));
        if (surf == null)
        {
            throw new Exception(string.Format("Surf {0} not found.", surfName));
        }
        SurfLink = surf.Id;
        return this;
    }

    public CaseBuilder WithSubstructure(string substructureName, Project project)
    {
        var substructure = project.Substructures.FirstOrDefault(d => d.Name.Equals(substructureName));
        if (substructure == null)
        {
            throw new Exception(string.Format("Substructure {0} not found.", substructureName));
        }
        SubstructureLink = substructure.Id;
        return this;
    }

    public CaseBuilder WithTopside(string topsideName, Project project)
    {
        var topside = project.Topsides.FirstOrDefault(d => d.Name.Equals(topsideName));
        if (topside == null)
        {
            throw new Exception(string.Format("Surf {0} not found.", topsideName));
        }
        TopsideLink = topside.Id;
        return this;
    }

    public CaseBuilder WithTransport(string transportName, Project project)
    {
        var transport = project.Transports.FirstOrDefault(d => d.Name.Equals(transportName));
        if (transport == null)
        {
            throw new Exception(string.Format("Transport {0} not found.", transportName));
        }
        TransportLink = transport.Id;
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
