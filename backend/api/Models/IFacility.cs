namespace api.Models
{
    public interface IFacility
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }
        public Maturity Maturity { get; set; }
    }
}
