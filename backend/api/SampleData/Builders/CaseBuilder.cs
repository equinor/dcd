using api.Models;

namespace api.SampleData.Builders;

public class CaseBuilder : Case
{
    public CaseBuilder WithWell(string wellName, Project project)
    {

        var well = project.Wells!.FirstOrDefault(d => d.Name!.Equals(wellName));
        if (well == null)
        {
            throw new Exception(string.Format("Well {0} not found", wellName));
        }
        WellsLink = well.Id;
        return this;
    }
    public CaseBuilder WithDrainageStrategy(string drainageStrategyName, Project project)
    {

        var drainageStrategy = project.DrainageStrategies!.FirstOrDefault(d => d.Name.Equals(drainageStrategyName));
        if (drainageStrategy == null)
        {
            throw new Exception(string.Format("Drainage strategy {0} not found", drainageStrategyName));
        }
        DrainageStrategyLink = drainageStrategy.Id;
        return this;
    }
    public CaseBuilder WithWellProject(string wellProjectName, Project project)
    {
        var wellProject = project.WellProjects!.FirstOrDefault(d => d.Name.Equals(wellProjectName));
        if (wellProject == null)
        {
            throw new Exception(string.Format("Drainage strategy {0} not found", wellProjectName));
        }
        WellProjectLink = wellProject.Id;
        return this;
    }
    public CaseBuilder WithSurf(string surfName, Project project)
    {
        var surf = project.Surfs!.FirstOrDefault(d => d.Name.Equals(surfName));
        if (surf == null)
        {
            throw new Exception(string.Format("Surf {0} not found", surfName));
        }
        SurfLink = surf.Id;
        return this;
    }
    public CaseBuilder WithSubstructure(string substructureName, Project project)
    {
        var substructure = project.Substructures!.FirstOrDefault(d => d.Name.Equals(substructureName));
        if (substructure == null)
        {
            throw new Exception(string.Format("Substructure {0} not found", substructureName));
        }
        SubstructureLink = substructure.Id;
        return this;
    }
    public CaseBuilder WithTopside(string topsideName, Project project)
    {
        var topside = project.Topsides!.FirstOrDefault(d => d.Name.Equals(topsideName));
        if (topside == null)
        {
            throw new Exception(string.Format("Topside {0} not found", topsideName));
        }
        TopsideLink = topside.Id;
        return this;
    }
    public CaseBuilder WithTransport(string transportName, Project project)
    {
        var transport = project.Transports!.FirstOrDefault(d => d.Name.Equals(transportName));
        if (transport == null)
        {
            throw new Exception(string.Format("Transport {0} not found", transportName));
        }
        TransportLink = transport.Id;
        return this;
    }
    public CaseBuilder WithExploration(string explorationName, Project project)
    {
        var transport = project.Explorations!.FirstOrDefault(d => d.Name.Equals(explorationName));
        if (transport == null)
        {
            throw new Exception(string.Format("Exploration {0} not found", explorationName));
        }
        ExplorationLink = transport.Id;
        return this;
    }
}
