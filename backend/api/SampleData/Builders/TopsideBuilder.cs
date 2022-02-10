using api.Models;

namespace api.SampleData.Builders;

public class TopsideBuilder : Topside
{
    public TopsideBuilder WithCostProfile(TopsideCostProfile topside)
    {
        topside.Topside = this;
        this.CostProfile = topside;
        return this;
    }
}
