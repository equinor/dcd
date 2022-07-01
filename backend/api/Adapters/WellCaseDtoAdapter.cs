using api.Dtos;
using api.Models;

namespace api.Adapters
{
    public static class WellCaseDtoAdapter
    {
        public static WellCaseDto Convert(WellCase wellCase)
        {
            var wellDto = new WellCaseDto
            {
                CaseId = wellCase.CaseId,
                WellId = wellCase.WellId,
                Count = wellCase.Count,
                DrillingSchedule = wellCase.DrillingSchedule,
            };
            return wellDto;
        }
    }
}
