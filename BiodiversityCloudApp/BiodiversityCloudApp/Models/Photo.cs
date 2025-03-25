using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Photo
{
    public Photo()
    {
    }

    public Photo(string url, string description, Guid observationId, Observation observation)
        : this()
    {
        Url = url;
        Description = description;
        ObservationId = observationId;
        Observation = observation;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Url { get; set; }
    public string Description { get; set; }
    public Guid ObservationId { get; set; }
    public Observation Observation { get; set; }
}
