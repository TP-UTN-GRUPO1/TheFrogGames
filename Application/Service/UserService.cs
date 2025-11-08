using Application.Abstraction;
using Application.Helpers;
using Contracts.User.Request;
using Contracts.User.Response;
using Domain.Entities;

namespace Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }
    public UserResponse GetById(int id)
    {
        var user = _userRepository.GetById(id);

        var completeName = $"{user.Name} {user.LastName}";

        return new UserResponse
        {
            CompleteName = completeName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            IsDeleted = user.IsDeleted
        };
    }
    public bool Create(CreateUserRequest user)
    {
        string hashedPassword = HashHelper.ComputeHash(user.Password);
        var newUser = new User
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Password = hashedPassword,
            RoleId = 3
        };
        return _userRepository.Create(newUser);
    }

    public List<UserResponse> GetAll()
    {

        var userList = _userRepository
            .GetAll()
            .Where(u => !u.IsDeleted)
            .Select(u => new UserResponse
            {
                Id = u.Id.ToString(),
                CompleteName = $"{u.Name} {u.LastName}",
                Email = u.Email,
                BirthDate = u.BirthDate,
                IsDeleted = u.IsDeleted,
                RoleId = u.RoleId,

            }).ToList();
        return userList;
    }

    //public bool UpdateUserStatus(ParcialUpdateUserRequest request)
    //{
    //    var user = _userRepository.GetById(request.Id);

    //    if (user == null)
    //    {
    //        return false;
    //    }

    //    user.IsDeleted = !user.IsDeleted;

    //    return _userRepository.UpdateUserStatus(user);
    //}

    public bool ParcialUpdateUser(int id, ParcialUpdateUserRequest user)
    {
        var ExistingUser = _userRepository.GetById(id);

        if (ExistingUser == null)
        {
            return false;
        }
        ExistingUser.Name = user.Name ?? ExistingUser.Name;
        ExistingUser.LastName = user.LastName ?? ExistingUser.LastName;

        return _userRepository.ParcialUpdateUser(ExistingUser);
    }
    public bool Update(int id, UpdateUserRequest user)
    {
        var ExistingUser = _userRepository.GetById(id);
        if (ExistingUser == null)
        {
            return false;
        }
        ExistingUser.Name = user.Name;
        ExistingUser.LastName = user.LastName;
        ExistingUser.Email = user.Email;
        return _userRepository.Update(ExistingUser);
    }
    public bool SoftDeleteUser(int id, SoftDeleteUserRequest request)
    {
        var user = _userRepository.GetById(id, trackChanges: true);
        if (user == null)
        {
            return false;
            throw new Exception($"Usuario con ID {id} no encontrado.");
        }
        user.IsDeleted = request.IsDeleted;

        bool success = _userRepository.Update(user);

        if (!success)
        {
            throw new ApplicationException("Error al actualizar el estado del usuario.");
        }
        return true;
    }

}
