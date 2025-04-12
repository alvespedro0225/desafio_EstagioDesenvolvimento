using System.Net;
using System.Text.Json;
using pokeAPI.Models;
using pokeAPI.Services.Cache;
using pokeAPI.Services.HttpClient;

namespace pokeAPI.Endpoints;

public static class Pokemon
{
    public static void MapPokemonEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("pokemon");
        group.MapGet("{pokemonName}", GetPokemon);
        group.MapGet("", GetPokemons);
    }

    public static async Task<IResult> GetPokemon
        (IPokemonClient httpClient, ICache<string, string, PokemonModel> cache, string pokemonName)
    {
        string? data;
        try
        {
            data = cache.GetData(pokemonName);
        }
        catch (KeyNotFoundException)
        {
            data = null;
        }
        if (data != null) return Results.Content(data, "application/json");
        
        var response = await httpClient.GetPokemonData(pokemonName);
        
        if (!ValidateResponse(response, out var statusCode)) 
            return Results.StatusCode((int?) statusCode ?? 500);
        
        data = await response.Content.ReadAsStringAsync();
        cache.SetData(pokemonName, data);
        return Results.Content(data, "application/json");

    }

    public static async Task<IResult> GetPokemons
        (IPokemonClient httpClient, ICache<string, string, PokemonModel> cache, int limit = 20, int offset = 0)
    {
        PokemonModel[]? data;
        try
        {
           data = cache.GetArrayData(limit, offset);
        }
        catch (Exception e)
        {
            if (e is KeyNotFoundException or ArgumentOutOfRangeException)
            {
                data = null;
            }
            else throw;
        }

        if (data is not null) return Results.Ok(data);
        
        var response = await httpClient.GetPokemonsData(limit, offset);
        
        if (!ValidateResponse(response, out var statusCode)) 
            return Results.StatusCode((int?) statusCode ?? 500);
        
        var jsonData = await response.Content.ReadAsStringAsync();
        var results = JsonSerializer.Deserialize<PokemonResultModel>(jsonData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        
        if (results is null) 
            throw new NullReferenceException();
        
        
        data = results.Results ?? throw new NullReferenceException();
        cache.SetArrayData(data, limit, offset, results.Count);
        return Results.Ok(data);
    }
    
    private static bool ValidateResponse(HttpResponseMessage response, out HttpStatusCode? statusCode)
    {
        try
        {
            response.EnsureSuccessStatusCode();
        }
        catch (HttpRequestException ex)
        {
            // TODO Add Logging
            statusCode = ex.StatusCode;
            return false;
        }
        statusCode = null;
        return true;
    }
}