namespace Contracts.Favorites.Response;

public class FavoriteResponse
{
    public int GameId { get; set; }
    public string Title { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public bool Available { get; set; }
}
