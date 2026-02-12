namespace Domain.Entities;

public class Favorite : BaseEntity
{
    public int UserId { get; set; }
    public User User { get; set; }

    public int GameId { get; set; }
    public Game Game { get; set; }
}