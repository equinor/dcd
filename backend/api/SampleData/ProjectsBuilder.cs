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
            throw new Exception(string.Format("Cannot find project {0}", projectName));
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
