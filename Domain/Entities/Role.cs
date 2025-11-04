namespace Domain.Entities;

public class Role : BaseEntity
{
    public string Name { get; set; }

}
public enum TypeRole
{
    SysAdmin = 1,
    Admin,
    User
}