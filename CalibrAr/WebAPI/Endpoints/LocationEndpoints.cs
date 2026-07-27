using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class LocationEndpoints
    {
        public static void MapLocationEndpoints(this WebApplication app)
        {
            app.MapGet("/locations/{id}", async (int id, ILocationService locationService) =>
            {
                LocationDTO? dto = await locationService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetLocation")
            .Produces<LocationDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/locations", async (ILocationService locationService) =>
            {
                var dtos = await locationService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllLocations")
            .Produces<IEnumerable<LocationDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/locations", async (LocationDTO dto, ILocationService locationService) =>
            {
                try
                {
                    LocationDTO created = await locationService.AddAsync(dto);

                    return Results.Created($"/locations/{created.Id}", created);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddLocation")
            .Produces<LocationDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/locations/{id}", async (int id, LocationDTO dto, ILocationService locationService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await locationService.UpdateAsync(dto);

                    if (!updated)
                    {
                        return Results.NotFound();
                    }

                    return Results.NoContent();
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("UpdateLocation")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/locations/{id}", async (int id, ILocationService locationService) =>
            {
                try
                {
                    var deleted = await locationService.DeleteAsync(id);

                    if (!deleted)
                    {
                        return Results.NotFound();
                    }

                    return Results.NoContent();
                }
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("DeleteLocation")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi();
        }
    }
}
