namespace Contracts.User.Request;

public class ParcialUpdateUserRequest
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public bool IsDeleted { get; set; }



}
