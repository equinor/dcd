using api.Models;

namespace api.SampleData.Builders;

public class SubstructureBuilder : Substructure
{
    public SubstructureBuilder WithCostProfile(SubstructureCostProfile costProfile)
    {
        costProfile.Substructure = this;
        this.CostProfile = costProfile;
        return this;
    }

    public SubstructureBuilder WithSubStructureCessasionCostProfile(SubstructureCessasionCostProfile substructureCessasionCostProfile)
    {
        substructureCessasionCostProfile.Substructure = this;
        this.SubstructureCessasionCostProfile = substructureCessasionCostProfile;
        return this;
    }
}
