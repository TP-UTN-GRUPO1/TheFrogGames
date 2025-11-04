using Contracts.User.Request;
using Domain.Entities;

namespace Application.Abstraction;

public interface IUserRepository : IBaseRepository<User>
{
    bool UpdateUserStatus(User user);
    bool ParcialUpdateUser(User user);
    User GetUserByEmailAndPassword(LoginUserRequest request);

}
