using Contracts.User.Request;

namespace Application.Abstraction.ExternalServices;

public interface IAuthService
{
    string Login(LoginUserRequest request);
}
