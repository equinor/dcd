using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class CaseAdapter
    {
        public Case Convert(CaseDto caseDto)
        {
            var case_ = new Case();
            case_.ProjectId = caseDto.ProjectId;
            case_.Name = caseDto.Name;
            case_.Description = caseDto.Description;
            case_.ReferenceCase = caseDto.ReferenceCase;
            case_.DG4Date = caseDto.DG4Date;
            case_.CreateTime = caseDto.CreateTime;
            case_.ModifyTime = caseDto.ModifyTime;

            return case_;
        }
    }

}

