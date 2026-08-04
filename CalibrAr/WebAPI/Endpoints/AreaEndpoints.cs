using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class AreaEndpoints
    {
        public static void MapAreaEndpoints(this WebApplication app)
        {
            app.MapGet("/areas/{id}", async (int id, IAreaService areaService) =>
            {
                AreaDTO? dto = await areaService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetArea")
            .Produces<AreaDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/areas", async (IAreaService areaService) =>
            {
                var dtos = await areaService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllAreas")
            .Produces<IEnumerable<AreaDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/areas", async (AreaDTO dto, IAreaService areaService) =>
            {
                try
                {
                    AreaDTO created = await areaService.AddAsync(dto);

                    return Results.Created($"/areas/{created.Id}", created);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("AddArea")
            .Produces<AreaDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapPut("/areas/{id}", async (int id, AreaDTO dto, IAreaService areaService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await areaService.UpdateAsync(dto);

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
                catch (KeyNotFoundException ex)
                {
                    return Results.NotFound(new { error = ex.Message });
                }
            })
            .WithName("UpdateArea")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/areas/{id}", async (int id, IAreaService areaService) =>
            {
                var deleted = await areaService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteArea")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}
