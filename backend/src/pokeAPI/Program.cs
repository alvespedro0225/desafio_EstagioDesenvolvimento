using System.Collections.Concurrent;
using System.Net.Http.Headers;
using pokeAPI.Endpoints;
using pokeAPI.Models;
using pokeAPI.Services.Cache;
using pokeAPI.Services.HttpClient;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpClient("PokeAPI", client =>
{
    client.BaseAddress = new Uri("https://pokeapi.co/api/v2/");
    client.DefaultRequestHeaders.UserAgent.Add( new ProductInfoHeaderValue("PokemonAPI", "1.0"));
});
builder.Services.AddSingleton<IDictionary<string, string>, ConcurrentDictionary<string, string>>();
builder.Services.AddSingleton<ICache<string,string, PokemonModel>, DictionaryCache<string,string, PokemonModel>>();
builder.Services.AddSingleton<IPokemonClient, PokeApiClient>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.MapPokemonEndpoints();
app.Run();

