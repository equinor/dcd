using api.Context;

namespace api.SampleData.Generators;

public static class SaveSampleDataToDB
{
    public static void PopulateDb(DcdDbContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        var projectsBuilder = SampleAssetGenerator.initializeAssets();
        context.AddRange(projectsBuilder.Projects);
        context.SaveChanges();
        projectsBuilder = SampleCaseGenerator.initializeCases(projectsBuilder);
        foreach (var p in projectsBuilder.Projects)
        {
            context.AddRange(p.Cases!);
        }

        context.SaveChanges();
    }
}
