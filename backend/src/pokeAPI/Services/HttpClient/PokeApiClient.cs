namespace pokeAPI.Services.HttpClient;

public class PokeApiClient(IHttpClientFactory httpClientFactory) : IPokemonClient
{
    
    public async Task<HttpResponseMessage> GetPokemonData(string name)
     {
        using var httpClient = httpClientFactory.CreateClient("PokeAPI");
        return await httpClient.GetAsync($"pokemon/{name}");
     }

    public async Task<HttpResponseMessage> GetPokemonsData(int limit, int offset)
    {
        using var httpClient = httpClientFactory.CreateClient("PokeAPI");
        return await httpClient.GetAsync($"pokemon/?limit={limit}&offset={offset}");
    }
}