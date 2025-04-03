using api.Features.Cases.Recalculation.Types.StudyCostProfile;
using api.Features.Profiles;
using api.Models;

using Xunit;

namespace tests.Features.Cases.Recalculation.Calculators;

public class StudyCostProfileServiceTests
{
    [Fact]
    public void CalculateFeedStudies_ReturnsEqualCostForTwoCompleteYears()
    {
        var caseItem = new Case
        {
            Id = Guid.NewGuid(),
            Dg4Date = new DateTime(2030, 1, 1),
            Dg2Date = new DateTime(2025, 1, 1),
            Dg3Date = new DateTime(2027, 1, 1),
            CapexFactorFeedStudies = 1
        };

        const double sumFacilityCost = 1000.0;
        const double sumWellCost = 200.0;

        StudyCostProfileService.CalculateTotalFeedStudies(caseItem, sumFacilityCost, sumWellCost);

        var totalFeedStudiesProfile = caseItem.GetProfile(ProfileTypes.TotalFeedStudies);

        var expectedStartYear = caseItem.Dg2Date!.Value.Year - caseItem.Dg4Date.Year;

        Assert.Equal(-6, expectedStartYear);
        Assert.Equal(expectedStartYear, totalFeedStudiesProfile.StartYear);
        Assert.Equal(2, totalFeedStudiesProfile.Values.Length);
        Assert.Equal(600, totalFeedStudiesProfile.Values[0]);
        Assert.Equal(600, totalFeedStudiesProfile.Values[1]);
    }

    [Fact]
    public void CalculateFeedStudies_ReturnsDistributedCostsForOneAndAHalfYears()
    {
        var caseItem = new Case
        {
            Id = Guid.NewGuid(),
            Dg4Date = new DateTime(2030, 1, 1),
            Dg2Date = new DateTime(2025, 1, 1),
            Dg3Date = new DateTime(2026, 7, 1),
            CapexFactorFeedStudies = 1
        };

        const double sumFacilityCost = 1000.0;
        const double sumWellCost = 200.0;

        StudyCostProfileService.CalculateTotalFeedStudies(caseItem, sumFacilityCost, sumWellCost);

        var totalFeedStudiesProfile = caseItem.GetProfile(ProfileTypes.TotalFeedStudies);

        var expectedStartYear = caseItem.Dg2Date!.Value.Year - caseItem.Dg4Date.Year;

        Assert.Equal(-5, expectedStartYear);
        Assert.Equal(expectedStartYear, totalFeedStudiesProfile.StartYear);
        Assert.Equal(2, totalFeedStudiesProfile.Values.Length);
        var totalDays = (int)(caseItem.Dg3Date.Value - caseItem.Dg2Date.Value).TotalDays + 1; // Make total days inclusive
        var expectedFirstYear = 365 * (sumFacilityCost + sumWellCost) / totalDays;
        Assert.Equal(expectedFirstYear, totalFeedStudiesProfile.Values[0], 5);
        var expectedSecondYear = sumFacilityCost + sumWellCost - expectedFirstYear;
        Assert.Equal(expectedSecondYear, totalFeedStudiesProfile.Values[1], 5);
    }

    [Fact]
    public void CalculateFeedStudies_ReturnsDistributedCostsForStartYearAfterJanFirst()
    {
        var caseItem = new Case
        {
            Id = Guid.NewGuid(),
            Dg4Date = new DateTime(2030, 1, 1),
            Dg2Date = new DateTime(2025, 7, 1),
            Dg3Date = new DateTime(2026, 10, 15),
            CapexFactorFeedStudies = 1
        };

        const double sumFacilityCost = 1000.0;
        const double sumWellCost = 200.0;

        StudyCostProfileService.CalculateTotalFeedStudies(caseItem, sumFacilityCost, sumWellCost);

        var totalFeedStudiesProfile = caseItem.GetProfile(ProfileTypes.TotalFeedStudies);

        var expectedStartYear = caseItem.Dg2Date!.Value.Year - caseItem.Dg4Date.Year;

        Assert.Equal(-5, expectedStartYear);
        Assert.Equal(expectedStartYear, totalFeedStudiesProfile.StartYear);
        Assert.Equal(2, totalFeedStudiesProfile.Values.Length);
        var totalDays = (int)(caseItem.Dg3Date.Value - caseItem.Dg2Date.Value).TotalDays + 1; // Make total days inclusive
        var expectedFirstYear = (365 - caseItem.Dg2Date.Value.DayOfYear + 1) * (sumFacilityCost + sumWellCost) / totalDays;
        Assert.Equal(expectedFirstYear, totalFeedStudiesProfile.Values[0], 5);
        var expectedSecondYear = sumFacilityCost + sumWellCost - expectedFirstYear;
        Assert.Equal(expectedSecondYear, totalFeedStudiesProfile.Values[1], 5);
    }
}
