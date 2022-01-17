using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class Co2Emissions : TimeSeriesMass<double>
    {
        [ForeignKey("DrainageStrategy.Id")]
        public virtual DrainageStrategy DrainageStrategy { get; set; } = null!;
    }
}
