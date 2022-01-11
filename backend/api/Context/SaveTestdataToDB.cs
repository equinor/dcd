
namespace api.Context
{
    public static class SaveTestdataToDB
    {

        public static void PopulateDb(DcdDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var projectsBuilder = TestDataGenerator.initialize();
            context.AddRange(projectsBuilder.Projects);
            context.SaveChanges();
        }
    }
}
