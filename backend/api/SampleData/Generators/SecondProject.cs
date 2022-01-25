using api.Models;
using api.SampleData.Builders;
namespace api.SampleData.Generators;

public class SecondProject : IProject
{
    public ProjectBuilder buildCases()
    {
        return new ProjectBuilder()
        {
            ProjectName = "P2",
            CreateDate = DateTimeOffset.UtcNow
        }
            .WithDrainageStrategy(new DrainageStrategyBuilder()
            {
                Name = "Drainage Strategy 1",
                NGLYield = 0.56
            }
                .WithProductionProfileGas(new ProductionProfileGasBuilder()
                    .WithYearValue(2040, 10.45)
                    .WithYearValue(2041, 13.23)
                    .WithYearValue(2042, 34.21)
                )
            );
    }
}