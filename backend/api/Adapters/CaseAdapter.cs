using api.Dtos;
using api.Models;
using api.Services;

namespace api.Adapters
{
    public class CaseAdapter
    {
        public Case Convert(CaseDto caseDto)
        {
            return new Case
            {
                ProjectId = caseDto.ProjectId,
                Name = caseDto.Name,
                Description = caseDto.Description,
                ReferenceCase = caseDto.ReferenceCase,
                DG4Date = caseDto.DG4Date,
                CreateTime = caseDto.CreateTime,
                ModifyTime = caseDto.ModifyTime
            };
        }


    }
}

