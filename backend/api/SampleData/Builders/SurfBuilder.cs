using api.Models;

namespace api.SampleData.Builders;

public class SurfBuilder : Surf
{
    public SurfBuilder WithCostProfile(SurfCostProfile costProfile)
    {
        costProfile.Surf = this;
        this.CostProfile = costProfile;
        return this;
    }

    public SurfBuilder WithSurfCessationCostProfile(SurfCessationCostProfile surfCessationCostProfile)
    {
        surfCessationCostProfile.Surf = this;
        this.CessationCostProfile = surfCessationCostProfile;
        return this;
    }
}
