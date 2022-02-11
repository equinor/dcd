using api.Adapters;

using api.Services;

using Microsoft.IdentityModel.Tokens;

namespace api.Dtos
{
    public class OffshoreFacilitiesDtoBuilder
    {
        private OffshoreFacilitiesCostProfileDto offshoreFacilitiesDto = null!;
        private TopsideCostProfileDto topsideCostProfile = null!;
        private SurfCostProfileDto surfCostProfile = null!;
        public TransportCostProfileDto transportCostProfile = null!;
        public SubstructureCostProfileDto substructureCostProfile = null!;
        private List<Int32> years = new List<int>();
        private List<Int32> lengths = new List<int>();
        public OffshoreFacilitiesDtoBuilder()
        {
            offshoreFacilitiesDto = new OffshoreFacilitiesCostProfileDto();
        }
        public OffshoreFacilitiesDtoBuilder WithTopside(TopsideService topsideService, Guid topsideLink)
        {
            if (topsideLink != Guid.Empty)
            {
                topsideCostProfile = TopsideDtoAdapter.Convert(topsideService.GetTopside(topsideLink)).CostProfile;
                years.Add(topsideCostProfile.StartYear);
                lengths.Add(topsideCostProfile.Values.Length);
            }
            return this;
        }

        public OffshoreFacilitiesDtoBuilder WithSurf(SurfService surfService, Guid surfLink)
        {
            if (surfLink != Guid.Empty)
            {
                surfCostProfile = SurfDtoAdapter.Convert(surfService.GetSurf(surfLink)).CostProfile;
                years.Add(surfCostProfile.StartYear);
                lengths.Add(surfCostProfile.Values.Length);
            }
            return this;
        }

        public OffshoreFacilitiesDtoBuilder WithTransport(TransportService transportService, Guid transportLink)
        {
            if (transportLink != Guid.Empty)
            {
                transportCostProfile = TransportDtoAdapter.Convert(transportService.GetTransport(transportLink)).CostProfile;
                years.Add(transportCostProfile.StartYear);
                lengths.Add(transportCostProfile.Values.Length);
            }
            return this;
        }
        public OffshoreFacilitiesDtoBuilder WithSubstructure(SubstructureService substructureService, Guid substructureLink)
        {
            if (substructureLink != Guid.Empty)
            {
                substructureCostProfile = SubstructureDtoAdapter.Convert(substructureService.GetSubstructure(substructureLink)).CostProfile;
                years.Add(substructureCostProfile.StartYear);
                lengths.Add(substructureCostProfile.Values.Length);
            }
            return this;
        }
        public OffshoreFacilitiesCostProfileDto Build()
        {
            if (lengths.IsNullOrEmpty())
                return offshoreFacilitiesDto;
            int length = lengths.Max() + years.Max() - years.Min();
            var values = new double[length];
            for (int i = 0; i < length; i++)
            {
                values[i] = 0;
            }
            offshoreFacilitiesDto.StartYear = years.Min();

            AddToValues(values, topsideCostProfile);
            AddToValues(values, substructureCostProfile);
            AddToValues(values, transportCostProfile);
            AddToValues(values, surfCostProfile);
            offshoreFacilitiesDto.Values = values;
            return offshoreFacilitiesDto;
        }

        private void AddToValues(double[] values, TimeSeriesCostDto timeSeriesCost)
        {
            if (timeSeriesCost == null)
            {
                return;
            }
            int startIndex = timeSeriesCost.StartYear - offshoreFacilitiesDto.StartYear;
            for (int i = 0; i < timeSeriesCost.Values.Length; i++)
            {
                values[startIndex + i] += timeSeriesCost.Values[i];
            }
        }

    }
}
