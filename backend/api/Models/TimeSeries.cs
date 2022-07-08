
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


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
                if (this.InternalData == null)
                {
                    return Array.Empty<T>();
                    // throw new Exception("Timeseries contains no values");
                }
                string[] tab = this.InternalData.Split(';');
                return Array.ConvertAll(InternalData.Split(';'), new Converter<string, T>(convertStringToGeneric));
            }
            set
            {
                var _data = value;
                InternalData = String.Join(";", _data.Select(p => p!.ToString()).ToArray());
            }
        }


        private static T convertStringToGeneric(string pf)
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

    public class TimeSeriesSchedule : TimeSeries<int>
    {

    }
}

