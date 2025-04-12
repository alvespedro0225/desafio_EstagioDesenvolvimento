namespace pokeAPI.Services.HttpClient;

/// <summary>
/// Wrapper on HttpClient for easily changing consuming APIs or behaviour
/// </summary>

public interface IPokemonClient
{
    public Task<HttpResponseMessage> GetPokemonData(string name); 
    public Task<HttpResponseMessage> GetPokemonsData(int limit, int offset);
}