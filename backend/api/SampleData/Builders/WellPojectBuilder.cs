using api.Models;

namespace api.SampleData.Builders;

public class WellProjectBuilder : WellProject
{
    public WellProjectBuilder WithWellProjectCostProfile(WellProjectCostProfile w)
    {
        CostProfile = w;
        return this;
    }
}
