using api.Models;

namespace api.SampleData.Builders;

public class TransportBuilder : Transport
{
    public TransportBuilder WithCostProfile(TransportCostProfile costProfile)
    {
        costProfile.Transport = this;
        this.CostProfile = costProfile;
        return this;
    }

    public TransportBuilder WithTransportCessationCostProfile(TransportCessationCostProfile transportCessationCostProfile)
    {
        transportCessationCostProfile.Transport = this;
        this.TransportCessationCostProfile = transportCessationCostProfile;
        return this;
    }
}
