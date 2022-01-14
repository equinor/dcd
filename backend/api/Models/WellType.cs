using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public class WellType
    {
        [ForeignKey("Exploration.Id")]
        public int DrillingDays { get; set; }
        public double WellCost { get; set; }
        public Currency Currency { get; set; }
        public WellTypeName WellTypeName { get; set; }
    }

    public enum WellTypeName
    {
        OilProducer,
        GasProducer
    }
}
