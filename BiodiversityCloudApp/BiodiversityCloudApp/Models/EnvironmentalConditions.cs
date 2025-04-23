namespace BiodiversityCloudApp.Models;
public class EnvironmentalConditions
{
    public float? Temperature { get; set; } // Temperature in Celsius
    public float? Humidity { get; set; } // Humidity percentage
    public float? WindSpeed { get; set; } // Wind speed in meters per second
    public string? AdditionalDetails { get; set; }
}
