using api.Models;

namespace api.SampleData.Builders;

public class ExplorationBuilder : Exploration
{
    public ExplorationBuilder WithExplorationDrillingSchedule(ExplorationDrillingScheduleBuilder d)
    {
        this.DrillingSchedule = d;
        return this;
    }

    public ExplorationBuilder WithGAndGAdminCost(WithGAndGAdminCostBuilder d)
    {
        this.GAndGAdminCost = d;
        return this;
    }
}

public class ExplorationDrillingScheduleBuilder : ExplorationDrillingSchedule
{
    public ExplorationDrillingScheduleBuilder()
    {
        YearValues = new List<YearValue<int>>();
    }
    public ExplorationDrillingScheduleBuilder WithYearValue(int year, int numberOfWells)
    {
        this.YearValues.Add(new YearValue<int>(year, numberOfWells));
        return this;
    }
}

public class WithGAndGAdminCostBuilder : GAndGAdminCost
{
    public WithGAndGAdminCostBuilder()
    {
        YearValues = new List<YearValue<double>>();
    }
    public WithGAndGAdminCostBuilder WithYearValue(int year, double cost)
    {
        this.YearValues.Add(new YearValue<double>(year, cost));
        return this;
    }
}

