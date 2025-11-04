namespace Domain.Entities;

public class Game : BaseEntity
{
  
    public string? Title { get; set; }
    public decimal Price { get; set; }
    public string? Developer { get; set; }
    public string? ImageUrl { get; set; }
    public float Rating { get; set; }
    public Boolean Available { get; set; }
    public int Sold { get; set; }

    public ICollection<Platform> Platforms { get; set; } = new List<Platform>();
    public ICollection<Genre>Genres { get; set; } = new List<Genre>();
}
