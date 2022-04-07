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

    public TopsideBuilder WithTopsideCessasionCostProfile(TopsideCessasionCostProfile topsideCessasionCostProfile)
    {
        topsideCessasionCostProfile.Topside = this;
        this.TopsideCessasionCostProfile = topsideCessasionCostProfile;
        return this;
    }
}
