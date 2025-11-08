using System.ComponentModel.DataAnnotations;

namespace Contracts.User.Request;

public class CompleteUserInfoRequest
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }

}
