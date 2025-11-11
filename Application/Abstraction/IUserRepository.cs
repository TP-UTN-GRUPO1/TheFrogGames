using Contracts.User.Request;
using Domain.Entities;

namespace Application.Abstraction;

public interface IUserRepository : IBaseRepository<User>
{
    User GetUserByEmailAndPassword(LoginUserRequest request);

    bool UserEmailExist(string email);
}
