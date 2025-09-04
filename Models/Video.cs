namespace StarFetch.Models;

public class Video
{
    public string? Iso639Part1 { get; set; }
    public string? Iso3166Part1 { get; set; }
    public string? Name { get; set; }
    public string? Key { get; set; }
    public string? Site { get; set; }
    public int Size { get; set; }
    public string? Type { get; set; }
    public bool Official { get; set; }
    public DateTime? PublishedAt { get; set; }
    public string? Id { get; set; }
}
