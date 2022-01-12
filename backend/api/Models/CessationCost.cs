using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{
    public class CessationCost : TimeSeriesCost<double>
    {
        [ForeignKey("Case.Id")]
        public virtual Case Case { get; set; } = null!;
    }
}
