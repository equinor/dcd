using api.Models;

namespace api.SampleData.Builders;

public class ExplorationBuilder : Exploration
{

    public ExplorationBuilder WithGAndGAdminCost(GAndGAdminCost d)
    {
        d.Exploration = this;
        this.GAndGAdminCost = d;
        return this;
    }
}
