using api.Models;

namespace api.SampleData.Builders;

public class ExplorationBuilder : Exploration
{
    public ExplorationBuilder WithExplorationCostProfile(ExplorationCostProfile e)
    {
        e.Exploration = this;
        this.CostProfile = e;
        return this;
    }
    public ExplorationBuilder WithExplorationDrillingSchedule(ExplorationDrillingSchedule d)
    {
        d.Exploration = this;
        this.DrillingSchedule = d;
        return this;
    }

    public ExplorationBuilder WithGAndGAdminCost(GAndGAdminCost d)
    {
        d.Exploration = this;
        this.GAndGAdminCost = d;
        return this;
    }
}
