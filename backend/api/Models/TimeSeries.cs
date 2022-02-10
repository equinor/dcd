

using System.ComponentModel.DataAnnotations.Schema;

namespace api.Models
{

    public class TimeSeries<T>
    {
        public Guid Id { get; set; }
        public int StartYear { get; set; }
        public string InternalData { get; set; } = null!;
        [NotMapped]
        public T[] Values
        {
            get
            {
                string[] tab = this.InternalData.Split(';');
                return Array.ConvertAll(InternalData.Split(';'), new Converter<string, T>(convertStringToGeneric));
            }
            set
            {
                var _data = value;
                InternalData = String.Join(";", _data.Select(p => p!.ToString()).ToArray());
            }
        }

        public static T convertStringToGeneric(string pf)
        {
            return (T)Convert.ChangeType(pf, typeof(T));
        }

    }

    public class TimeSeriesVolume : TimeSeries<double>
    {

    }

    public class TimeSeriesMass : TimeSeries<double>
    {

    }

    public class TimeSeriesCost : TimeSeries<double>
    {
        public string EPAVersion { get; set; } = string.Empty;
        public Currency Currency { get; set; }
    }
    public enum Currency
    {
        USD,
        NOK
    }
    public class TimeSeriesSchedule : TimeSeries<int>
    {

    }
}

