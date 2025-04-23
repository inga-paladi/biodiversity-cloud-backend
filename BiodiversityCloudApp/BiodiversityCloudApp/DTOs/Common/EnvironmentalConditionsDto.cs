namespace BiodiversityCloudApp.DTOs.Common
{
    public class EnvironmentalConditionsDto
    {
        public float? Temperature { get; set; } // Temperature in Celsius
        public float? Humidity { get; set; } // Humidity percentage
        public float? WindSpeed { get; set; } // Wind speed in meters per second
        public string? AdditionalDetails { get; set; }
    }
}