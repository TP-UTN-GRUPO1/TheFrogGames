namespace Contracts.User.Response;

public class UserResponse
{
    public string Id { get; set; }
    public string CompleteName { get; set; }
    public string Email { get; set; }
    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
    public int RoleId { get; set; }
}
