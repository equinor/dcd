namespace api.Models
{

    public class YearValue<T>
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public T Value { get; set; } = default(T)!;

        public YearValue(int year, T value)
        {
            this.Year = year;
            this.Value = value;
        }
    }

}
