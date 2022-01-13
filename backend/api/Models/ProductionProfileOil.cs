using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class ProductionProfileOil : TimeSeriesVolume<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
}
