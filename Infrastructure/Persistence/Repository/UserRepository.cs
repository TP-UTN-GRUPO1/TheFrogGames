using Application.Abstraction;
using Application.Helpers;
using Contracts.User.Request;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            .FirstOrDefault(x => x.Email == request.Email && x.Password == HashHelper.ComputeHash(request.Password));
    }

    public bool UserEmailExist(string email)
    {
        return _context.Users.Any(x => x.Email == email);
    }

}
