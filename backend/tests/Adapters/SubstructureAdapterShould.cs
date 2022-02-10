using System;
using System.Collections.Generic;

using api.Adapters;
using api.Dtos;
using api.Models;

using Xunit;

namespace tests
{
    [Collection("Database collection")]
    public class SubstructureAdapterShould
    {
        [Fact]
        public void ConvertSubstructureDtoToDataModel()
        {
            // Arrange
            var projectId = new Guid();
            var substructureAdapter = new SubstructureAdapter();
            var substructureDto = CreateSubstructureDto(projectId);

            // Act
            var result = substructureAdapter.Convert(substructureDto);

            // Assert
            Assert.Equal(substructureDto.ProjectId, result.ProjectId);
            Assert.Equal(substructureDto.Name, result.Name);
            Assert.Equal(substructureDto.DryWeight, result.DryWeight);
            Assert.Equal(substructureDto.Maturity, result.Maturity);
            TestHelper.CompareCosts(substructureDto.CostProfile, result.CostProfile);
        }

        private SubstructureDto CreateSubstructureDto(Guid projectId)
        {
            return new SubstructureDto
            {
                ProjectId = projectId,
                Name = "Test substructure",
                CostProfile = new SubstructureCostProfileDto
                {
                    EPAVersion = "one version",
                    Currency = Currency.NOK,
                    StartYear = 2010,
                    Values = new double[] { 3.4564, 18.9, 62.3 }
                },
            };
        }
    }
}
