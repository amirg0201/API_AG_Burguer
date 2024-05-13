using System;
using System.Collections.Generic;
using API_AG_Burguer.Data;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.EntityFrameworkCore;

namespace API_AG_Burguer.Data.Models;

public partial class Burger
{
    public int Burgerid { get; set; }

    public string Name { get; set; } = null!;

    public bool WithCheese { get; set; }

    public decimal Precio { get; set; }

    public virtual ICollection<Promo> Promos { get; set; } = new List<Promo>();
}


public static class BurgerEndpoints
{
	public static void MapBurgerEndpoints (this IEndpointRouteBuilder routes)
    {
        var group = routes.MapGroup("/api/Burger").WithTags(nameof(Burger));

        group.MapGet("/", async (AmirGarciaDbContext db) =>
        {
            return await db.Burgers.ToListAsync();
        })
        .WithName("GetAllBurgers")
        .WithOpenApi();

        group.MapGet("/{id}", async Task<Results<Ok<Burger>, NotFound>> (int burgerid, AmirGarciaDbContext db) =>
        {
            return await db.Burgers.AsNoTracking()
                .FirstOrDefaultAsync(model => model.Burgerid == burgerid)
                is Burger model
                    ? TypedResults.Ok(model)
                    : TypedResults.NotFound();
        })
        .WithName("GetBurgerById")
        .WithOpenApi();

        group.MapPut("/{id}", async Task<Results<Ok, NotFound>> (int burgerid, Burger burger, AmirGarciaDbContext db) =>
        {
            var affected = await db.Burgers
                .Where(model => model.Burgerid == burgerid)
                .ExecuteUpdateAsync(setters => setters
                  .SetProperty(m => m.Burgerid, burger.Burgerid)
                  .SetProperty(m => m.Name, burger.Name)
                  .SetProperty(m => m.WithCheese, burger.WithCheese)
                  .SetProperty(m => m.Precio, burger.Precio)
                  );
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("UpdateBurger")
        .WithOpenApi();

        group.MapPost("/", async (Burger burger, AmirGarciaDbContext db) =>
        {
            db.Burgers.Add(burger);
            await db.SaveChangesAsync();
            return TypedResults.Created($"/api/Burger/{burger.Burgerid}",burger);
        })
        .WithName("CreateBurger")
        .WithOpenApi();

        group.MapDelete("/{id}", async Task<Results<Ok, NotFound>> (int burgerid, AmirGarciaDbContext db) =>
        {
            var affected = await db.Burgers
                .Where(model => model.Burgerid == burgerid)
                .ExecuteDeleteAsync();
            return affected == 1 ? TypedResults.Ok() : TypedResults.NotFound();
        })
        .WithName("DeleteBurger")
        .WithOpenApi();
    }
}