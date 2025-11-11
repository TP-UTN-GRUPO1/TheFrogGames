using Application.Abstraction.ExternalServices;
using Contracts.User.Response;
using Infrastructure.ExternalServices.Models;
using System.Net.Http.Json;

namespace Infrastructure.ExternalServices;

public class PokemonService : IPokemonService
{
    private readonly HttpClient _pokeClient;

    public PokemonService(IHttpClientFactory httpClientFactory)
    {
        _pokeClient = httpClientFactory.CreateClient("pokeApiHttpClient");
        _pokeClient.BaseAddress = new Uri(_pokeClient.BaseAddress!, "pokemon/");
    }

    public async Task<GetPokemonByIdResponse> GetPokemons(int id)
    {
        var response = await _pokeClient.GetFromJsonAsync<GetPokemonByIdResponse>($"{id}");
        return response!;
    }
}