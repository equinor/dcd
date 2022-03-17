using api.Models;

namespace api.SampleData.Builders;

public class WellProjectBuilder : WellProject
{
    public WellProjectBuilder WithWellProjectCostProfile(WellProjectCostProfile w)
    {
        this.CostProfile = w;
        return this;
    }
    public WellProjectBuilder WithDrillingSchedule(DrillingSchedule d)
    {
        this.DrillingSchedule = d;
        return this;
    }
}
