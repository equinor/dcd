
using api.Context;

namespace api.SampleData;

public static class SaveSampleDataToDB
{

    public static void PopulateDb(DcdDbContext context)
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }
        var projectsBuilder = SampleDataGenerator.initialize();
        context.AddRange(projectsBuilder.Projects);
        context.SaveChanges();
    }
}
