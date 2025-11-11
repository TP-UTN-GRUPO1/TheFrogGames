using Contracts.User.Response;
namespace Application.Abstraction.ExternalServices
{
    public interface IPokemonService
    {
        Task<GetPokemonByIdResponse> GetPokemons(int id);
    }
}
