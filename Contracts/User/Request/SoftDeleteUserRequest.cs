namespace Contracts.User.Request;

public class SoftDeleteUserRequest
{
    public int Id { get; set; }
    public bool IsDeleted { get; set; }
}
