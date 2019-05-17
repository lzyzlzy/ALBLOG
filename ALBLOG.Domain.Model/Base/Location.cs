namespace ALBLOG.Domain.Model
{
    public class Location
    {
        public Location()
        {
        }

        public Location(double longitude, double latitude)
        {
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        // 经度
        public double Longitude { get; set; }

        // 纬度
        public double Latitude { get; set; }

        public string ToDisplayString()
        {
            return $"{this.Longitude},{this.Latitude}";
        }
    }
}