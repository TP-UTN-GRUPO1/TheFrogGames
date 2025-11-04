using Contracts.User.Request;
using Contracts.User.Response;

namespace Application.Service;

public interface IUserService
{
    UserResponse GetById(int id);
    bool Create(CreateUserRequest user);
    List<UserResponse> GetAll();
    bool UpdateUserStatus(ParcialUpdateUserRequest request);
    bool ParcialUpdateUser(int id, ParcialUpdateUserRequest user);
    bool Update(int id, UpdateUserRequest user);
    bool Delete(int id);
}