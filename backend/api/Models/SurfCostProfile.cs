using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class SurfCostProfile : TimeSeriesCost<double>
    {
        [ForeignKey("Surf.Id")]
        public virtual Surf Surf { get; set; } = null!;
    }
}
