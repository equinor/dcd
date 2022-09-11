using api.Models;

namespace api.SampleData.Builders;

public class ExplorationBuilder : Exploration
{
    public ExplorationBuilder WithExplorationCostProfile(ExplorationCostProfile e)
    {
        e.Exploration = this;
        CostProfile = e;
        return this;
    }

    public ExplorationBuilder WithGAndGAdminCost(GAndGAdminCost d)
    {
        d.Exploration = this;
        GAndGAdminCost = d;
        return this;
    }
}
