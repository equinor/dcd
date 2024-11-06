using api.Models;

namespace api.SampleData.Builders;

public class CaseBuilder : Case
{
    public CaseBuilder WithDrainageStrategy(string drainageStrategyName, Project project)
    {

        var drainageStrategy = project.DrainageStrategies!.FirstOrDefault(d => d.Name.Equals(drainageStrategyName));
        if (drainageStrategy == null)
        {
            throw new Exception($"Drainage strategy {drainageStrategyName} not found");
        }
        DrainageStrategyLink = drainageStrategy.Id;
        return this;
    }
    public CaseBuilder WithWellProject(string wellProjectName, Project project)
    {
        var wellProject = project.WellProjects!.FirstOrDefault(d => d.Name.Equals(wellProjectName));
        if (wellProject == null)
        {
            throw new Exception($"Drainage strategy {wellProjectName} not found");
        }
        WellProjectLink = wellProject.Id;
        return this;
    }
    public CaseBuilder WithSurf(string surfName, Project project)
    {
        var surf = project.Surfs!.FirstOrDefault(d => d.Name.Equals(surfName));
        if (surf == null)
        {
            throw new Exception($"Surf {surfName} not found");
        }
        SurfLink = surf.Id;
        return this;
    }
    public CaseBuilder WithSubstructure(string substructureName, Project project)
    {
        var substructure = project.Substructures!.FirstOrDefault(d => d.Name.Equals(substructureName));
        if (substructure == null)
        {
            throw new Exception($"Substructure {substructureName} not found");
        }
        SubstructureLink = substructure.Id;
        return this;
    }
    public CaseBuilder WithTopside(string topsideName, Project project)
    {
        var topside = project.Topsides!.FirstOrDefault(d => d.Name.Equals(topsideName));
        if (topside == null)
        {
            throw new Exception($"Topside {topsideName} not found");
        }
        TopsideLink = topside.Id;
        return this;
    }
    public CaseBuilder WithTransport(string transportName, Project project)
    {
        var transport = project.Transports!.FirstOrDefault(d => d.Name.Equals(transportName));
        if (transport == null)
        {
            throw new Exception($"Transport {transportName} not found");
        }
        TransportLink = transport.Id;
        return this;
    }
    public CaseBuilder WithExploration(string explorationName, Project project)
    {
        var transport = project.Explorations!.FirstOrDefault(d => d.Name.Equals(explorationName));
        if (transport == null)
        {
            throw new Exception($"Exploration {explorationName} not found");
        }
        ExplorationLink = transport.Id;
        return this;
    }
}
