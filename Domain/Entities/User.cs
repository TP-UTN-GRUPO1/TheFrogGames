using System.Net.Sockets;

namespace Domain.Entities;

public class User : BaseEntity 
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Password { get; set; }
    public string? Address { get; set; }
    public string? Country { get; set; }
    public string? Province { get; set; }
    public string? City { get; set; }
    public int RoleId { get; set; }
    public bool IsDeleted { get; set; } = false;
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public string? PokemonName { get; set; }
    public Role Role { get; set; }


}
