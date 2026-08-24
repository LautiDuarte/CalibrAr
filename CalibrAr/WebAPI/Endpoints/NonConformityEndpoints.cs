using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class NonConformityEndpoints
    {
        public static void MapNonConformityEndpoints(this WebApplication app)
        {
            app.MapGet("/nonconformities/{id}", async (int id, INonConformityService nonConformityService) =>
            {
                NonConformityDTO? dto = await nonConformityService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetNonConformity")
            .Produces<NonConformityDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/nonconformities", async (INonConformityService nonConformityService) =>
            {
                var dtos = await nonConformityService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllNonConformities")
            .Produces<IEnumerable<NonConformityDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/nonconformities", async (NonConformityDTO dto, INonConformityService nonConformityService) =>
            {
                try
                {
                    NonConformityDTO created = await nonConformityService.AddAsync(dto);

                    return Results.Created($"/nonconformities/{created.Id}", created);
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
            .WithName("AddNonConformity")
            .Produces<NonConformityDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/nonconformities/{id}", async (int id, NonConformityDTO dto, INonConformityService nonConformityService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await nonConformityService.UpdateAsync(dto);

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
            .WithName("UpdateNonConformity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/nonconformities/{id}", async (int id, INonConformityService nonConformityService) =>
            {
                var deleted = await nonConformityService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteNonConformity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}