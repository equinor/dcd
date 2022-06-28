using api.Models;

namespace api.SampleData.Builders;

public class WellsBuilder
{
    public List<WellBuilder> Wells { get; set; } = new List<WellBuilder>();

    public WellsBuilder WithProject(WellBuilder p)
    {
        Wells.Add(p);
        return this;
    }

    public WellsBuilder WithCase(WellBuilder p)
    {
        Wells.Add(p);
        return this;
    }

    public WellBuilder ForProject(string wellName)
    {
        var wellBuilder = Wells.FirstOrDefault(p => p.Name!.Equals(wellName));
        if (wellBuilder == null)
        {
            throw new Exception(string.Format("Cannot find project {0}", wellName));
        }
        return wellBuilder;
    }
}
public class WellBuilder : Well
{
    public WellBuilder()
    {
        // WellType = new List<WellType>();
    }

    public WellBuilder WithWellType(WellTypeBuilder d)
    {
        return this;
    }
}

public class ExplorationWellTypeBuilder : ExplorationWellType
{
    public ExplorationWellTypeBuilder()
    {

    }

    public ExplorationWellTypeBuilder WithExplorationWellType(ExplorationWellTypeBuilder d)
    {
        return this;
    }
}

