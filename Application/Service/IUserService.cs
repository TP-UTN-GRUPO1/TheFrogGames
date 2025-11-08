using Contracts.User.Request;
using Contracts.User.Response;

namespace Application.Service;

public interface IUserService
{
    UserResponse GetById(int id);
    bool Create(CreateUserRequest user);
    List<UserResponse> GetAll();
    bool CompleteUserInfo(int id, CompleteUserInfoRequest user);
    bool SoftDeleteUser(int id, SoftDeleteUserRequest request);
    bool ChangeRole(int id, ChangeRoleRequest request);
}