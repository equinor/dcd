using api.Models;

namespace api.Context
{
    public static class InitContent
    {
        public static readonly List<Project> Projects = GetProjects();

        private static List<Project> GetProjects()
        {
            var project1 = new Project
            {
                Id = "1",
                ProjectName = "Project No 1",
                CreateDate = DateTimeOffset.UtcNow
            };
            var project2 = new Project
            {
                Id = "2",
                ProjectName = "Project No 2",
                CreateDate = DateTimeOffset.UtcNow
            };
            var project3 = new Project
            {
                Id = "3",
                ProjectName = "Project No 3",
                CreateDate = DateTimeOffset.UtcNow
            };

            List<Project> projects = new List<Project>(new Project[] { project1, project2, project3 });

            return projects;
        }

        public static void PopulateDb(DCDDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.AddRange(Projects);

            context.SaveChanges();
        }
    }
}
