namespace pokeAPI.Models;

public class PokemonResultModel
{
    public required int Count {get; set; }
    public string? Next {get; set; }
    public string? Previous {get; set; }
    public PokemonModel[]? Results { get; set; }
}