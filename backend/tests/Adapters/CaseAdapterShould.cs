using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;

using Xunit;

namespace tests
{
    [Collection("Database collection")]
    public class CaseAdapterShould
    {
        [Fact]
        public void ConvertDrainageStrategyDtoToDataModel()
        {
            // Arrange
            var projectId = new Guid();
            var caseAdapter = new CaseAdapter();
            var caseDto = CreateCaseDto(projectId);

            // Act
            var result = caseAdapter.Convert(caseDto);

            // Assert
            Assert.Equal(caseDto.ProjectId, result.ProjectId);
            Assert.Equal(caseDto.Name, result.Name);
            Assert.Equal(caseDto.Description, result.Description);
            Assert.Equal(caseDto.DG4Date, result.DG4Date);
            Assert.Equal(caseDto.CreateTime, result.CreateTime);
            Assert.Equal(caseDto.ModifyTime, result.ModifyTime);


        }

        private CaseDto CreateCaseDto(Guid projectId)
        {
            return new CaseDto
            {
                ProjectId = projectId,
                Name = "Test case adapter",
                Description = "Case test description",
                DG4Date = DateTimeOffset.Now.AddYears(4),
                CreateTime = DateTimeOffset.Now.AddDays(-1),
                ModifyTime = DateTimeOffset.Now.AddDays(-3)
            };
        }
    }
}
