namespace Contracts.Genre.Request
{
    public class UpdateGenreRequest
    {
        public int Id { get; set; }
        public string NewName { get; set; } = string.Empty;
    }
}
