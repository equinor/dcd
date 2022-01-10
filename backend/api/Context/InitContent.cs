using System.Collections;

using api.Models;

using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;

using Microsoft.EntityFrameworkCore;

namespace api.Context
{
    public static class InitContent
    {
        public static readonly List<Project> Projects = GetProjects();

        public static readonly List<Case> Cases = GetCases();

        public static readonly List<DrainageStrategy> DrainageStrategies = GetDrainageStrategies();

        public static readonly List<ProductionProfileOil> ProductionProfileOil = GetProductionProfileOil();

        public static readonly List<ProductionProfileGas> ProductionProfileGas = GetProductionProfileGas();

        private static List<Project> GetProjects()
        {

            var project1 = new Project
            {
                ProjectName = "P1",
                ProjectPhase = ProjectPhase.DG1,
                CreateDate = DateTimeOffset.UtcNow
            };
            var project2 = new Project
            {
                ProjectName = "P2",
                ProjectPhase = ProjectPhase.DG2,
                CreateDate = DateTimeOffset.UtcNow
            };
            var project3 = new Project
            {
                ProjectName = "P3",
                ProjectPhase = ProjectPhase.DG2,
                CreateDate = DateTimeOffset.UtcNow
            };

            List<Project> projects = new List<Project>(new Project[] { project1, project2, project3 });

            return projects;
        }

        private static List<Case> GetCases()
        {
            var case1 = new Case
            {
                Name = "Case 1",
                Description = "Description 1",
                Project = Projects[0],
            };
            var case2 = new Case
            {
                Name = "Case 2",
                Description = "Description 2",
                Project = Projects[0],
            };

            List<Case> cases = new List<Case>(new Case[] { case1, case2 });
            return cases;
        }

        private static List<DrainageStrategy> GetDrainageStrategies()
        {
            var drainageStrategy1 = new DrainageStrategy
            {
                NGLYield = 0.2,
                Case = Cases[0]
            };
            var drainageStrategy2 = new DrainageStrategy
            {
                NGLYield = 0.3,
                Case = Cases[1]
            };
            var drainageStrategies = new List<DrainageStrategy>(new DrainageStrategy[] { drainageStrategy1, drainageStrategy2 });
            return drainageStrategies;
        }

        private static List<ProductionProfileOil> GetProductionProfileOil()
        {
            var productionProfileOil1 = new ProductionProfileOil
            {
                YearValues = new List<YearValue<double>>() { new YearValue<double>(2030, 6.2), new YearValue<double>(2031, 5.3), new YearValue<double>(2032, 4.4) },
                DrainageStrategy = DrainageStrategies[0]
            };
            var productionProfiles = new List<ProductionProfileOil>(new ProductionProfileOil[] { productionProfileOil1 });
            return productionProfiles;
        }

        private static List<ProductionProfileGas> GetProductionProfileGas()
        {
            var productionProfileGas1 = new ProductionProfileGas
            {
                YearValues = new List<YearValue<double>>() { new YearValue<double>(2030, 2.2), new YearValue<double>(2031, 2.3), new YearValue<double>(2032, 2.4) },
                DrainageStrategy = DrainageStrategies[0]
            };

            var productionProfileGas2 = new ProductionProfileGas
            {
                YearValues = new List<YearValue<double>>() { new YearValue<double>(2040, 3.2), new YearValue<double>(2041, 3.3) },
                DrainageStrategy = DrainageStrategies[1]
            };
            var productionProfiles = new List<ProductionProfileGas>(new ProductionProfileGas[] { productionProfileGas1, productionProfileGas2 });
            return productionProfiles;
        }

        public static void PopulateDb(DcdDbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            context.AddRange(Projects);
            context.AddRange(Cases);
            context.AddRange(DrainageStrategies);
            context.AddRange(ProductionProfileOil);
            context.AddRange(ProductionProfileGas);
            context.SaveChanges();
        }
    }
}
