using api.Adapters;
using api.Models;
using api.Dtos;
using api.Services;

namespace api.Utilities
{
    public class UnitConversion
    {
        private readonly ProjectService _projectService;

        private UnitConversion(ProjectService projectService)
        {
            _projectService = projectService;
        }

        public static double[] ValuesPerUnitProductionProfileNgl(double[] values, Guid id, ProductionProfileNGLDto? dto, DrainageStrategy drainageStrategy)
        {
            if (id != Guid.Empty && dto != null && drainageStrategy.ProductionProfileNGL != null)
            {
                bool currentValueIsOldValue = dto.Values.SequenceEqual(drainageStrategy.ProductionProfileNGL.Values);
                double oilfieldFactor = 6.29;

                // var project = _projectService.GetProject(drainageStrategy.ProjectId);
                // var physUnit = project.PhysicalUnit;

                // if (physUnit == PhysUnit.OilField && currentValueIsOldValue)
                // {
                //     values = Array.ConvertAll(values, x => x * oilfieldFactor);
                // }
                // else if (physUnit == PhysUnit.SI && currentValueIsOldValue)
                // {
                //     values = Array.ConvertAll(values, x => x / oilfieldFactor);
                // }
            }
            return values;
        }
    }
}