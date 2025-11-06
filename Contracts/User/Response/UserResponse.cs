namespace Contracts.User.Response;

public class UserResponse
{
    public string Id { get; set; }
    public string CompleteName { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public bool IsDeleted { get; set; }
    public int RoleId { get; set; }
}
