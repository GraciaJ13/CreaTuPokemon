using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<Pokemon> PokemonDb = new List<Pokemon>();


app.MapPost("/api/v1/pokemon", (Pokemon pokemon) => {
    PokemonDb.Add(pokemon);
    return Results.Ok("Pokémon creado correctamente");
});

app.MapPost("/api/v1/pokemon/batch", (List<Pokemon> pokemons) => {
    PokemonDb.AddRange(pokemons);
    return Results.Ok("Pokémon creados correctamente");
});

app.MapPut("/api/v1/pokemon/{id:int}", (int id, Pokemon pokemon) => {
    var existingPokemon = PokemonDb.Find(p => p.Id == id);
    if (existingPokemon != null) {
        existingPokemon.Name = pokemon.Name;
        existingPokemon.Type = pokemon.Type;
        existingPokemon.Abilities = pokemon.Abilities;
        existingPokemon.Defense = pokemon.Defense;
        return Results.Ok("Pokémon actualizado");
    } else {
        return Results.NotFound($"No se encontró ningún Pokémon con el ID {id}");
    }
});

app.MapDelete("/api/v1/pokemon/{id:int}", (int id) => {
    var pokemonToRemove = PokemonDb.Find(p => p.Id == id);
    if (pokemonToRemove != null) {
        PokemonDb.Remove(pokemonToRemove);
        return Results.Ok($"Pokémon eliminado correctamente (ID: {id})");
    } else {
        return Results.NotFound($"No se encontró ningún Pokémon con el ID {id}");
    }
});


app.MapGet("/api/v1/pokemon/{id:int}", (int id) => {
    var pokemon = PokemonDb.Find(p => p.Id == id);
    if (pokemon != null) {
        return Results.Ok(pokemon);
    } else {
        return Results.NotFound($"No se encontró ningún Pokémon con el ID {id}");
    }
});


app.MapGet("/api/v1/pokemon/Type/{Type}", (string Type) => {
    var pokemonOfType = PokemonDb.FindAll(p => p.Type.Equals(type, StringComparison.OrdinalIgnoreCase));
    if (pokemonOfType.Count > 0) {
        return Results.Ok(pokemonOfType);
    } else {
        return Results.NotFound($"No se encontraron Pokémon del tipo '{Type}'");
    }
});


app.MapGet("/api/v1/pokemon/abilities/{value:int}", (int value) => {
    List<Pokemon> filteredPokemon = new List<Pokemon>();
    foreach (var pokemon in PokemonDb) {
        foreach (var abilityValue in pokemon.Abilities.Values) {
            if (abilityValue > value) {
                filteredPokemon.Add(pokemon);
                break; 
            }
        }
    }
    return Results.Ok(filteredPokemon);
});

app.Run();

class Pokemon 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public Dictionary<int> Abilities { get; set; } // Nombre de habilidad -> valor
    public double Defense { get; set; }
}

