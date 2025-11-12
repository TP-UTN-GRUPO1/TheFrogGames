using Application.Abstraction;
using Application.Abstraction.ExternalServices;
using Application.Helpers;
using Contracts.User.Request;
using Contracts.User.Response;
using Domain.Entities;

namespace Application.Service;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IPokemonService _pokemonService;
    public UserService(IUserRepository userRepository, IPokemonService pokemonService)
    {
        _userRepository = userRepository;
        _pokemonService = pokemonService;
    }
    public UserResponse GetById(int id)
    {
        var user = _userRepository.GetById(id);

        var completeName = $"{user.Name} {user.LastName}";

        return new UserResponse
        {
            Id = user.Id.ToString(),
            CompleteName = completeName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            IsDeleted = user.IsDeleted,
            RoleId = user.RoleId,
            PokemonName = user.PokemonName
        };
    }
    public async Task<bool> Create(CreateUserRequest user)
    {
        if (_userRepository.UserEmailExist(user.Email))
        {
            return false;
        }

        string hashedPassword = HashHelper.ComputeHash(user.Password);
        
        int randomPokemonId = new Random().Next(1, 1000);
        var pokemon = await _pokemonService.GetPokemons(randomPokemonId);

        var newUser = new User
        {
            Name = user.Name,
            LastName = user.LastName,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Password = hashedPassword,
            RoleId = 3,
            PokemonName = pokemon?.Name

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
                PokemonName = u.PokemonName
            }).ToList();
        return userList;
    }

    public bool CompleteUserInfo(int id, CompleteUserInfoRequest user)
    {
        var ExistingUser = _userRepository.GetById(id);

        if (ExistingUser == null || ExistingUser.IsDeleted)
        {
            return false;
        }
        ExistingUser.Name = PatchHelper.KeepIfEmpty(ExistingUser.Name, user.Name);
        ExistingUser.LastName = PatchHelper.KeepIfEmpty(ExistingUser.LastName, user.LastName);
        ExistingUser.Address =PatchHelper.KeepIfEmpty(ExistingUser.Address, user.Address);
        ExistingUser.Country = PatchHelper.KeepIfEmpty(ExistingUser.Country, user.Country); ;
        ExistingUser.Province = PatchHelper.KeepIfEmpty(ExistingUser.Province, user.Province); ;
        ExistingUser.City = PatchHelper.KeepIfEmpty(ExistingUser.City, user.City);

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

    public bool ChangeRole(int id, ChangeRoleRequest request)
    {
        var user = _userRepository.GetById(id);
        if (user == null)
        {
            return false;
        }
        user.RoleId = request.RoleId;
        return _userRepository.Update(user);
    }

}
