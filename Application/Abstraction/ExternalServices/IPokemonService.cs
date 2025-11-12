using Contracts.PokeAPI.Response;
namespace Application.Abstraction.ExternalServices;

public interface IPokemonService
{
    Task<GetPokemonByIdResponse> GetPokemons(int id);
}