using Microsoft.EntityFrameworkCore;
using Application.Abstraction;
using Contracts.User.Request;
using Domain.Entities;

namespace Infrastructure.Persistence.Repository;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    private readonly TheFrogGamesDbContext _context;
    public UserRepository(TheFrogGamesDbContext context) : base(context)
    {
        _context = context;
    }
    public User? GetUserByEmailAndPassword(LoginUserRequest request)
    {
        return _context.Users
            .Include(x => x.Role)
            .FirstOrDefault(x => x.Email == request.Email && x.Password == request.Password);
    }
    public bool UpdateUserStatus(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return true;
    }

    public bool ParcialUpdateUser(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
        return true;
    }
}
