namespace Contracts.Platform.Request
{
    public class UpdatePlatformRequest
    {
        public int Id { get; set; }
        public string NewName { get; set; } = string.Empty;
    }
}
