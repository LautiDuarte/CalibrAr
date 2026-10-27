using System.Security.Claims;
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
            .WithOpenApi()
            .RequireAuthorization("NonConformitiesRead");

            app.MapGet("/nonconformities", async (INonConformityService nonConformityService) =>
            {
                var dtos = await nonConformityService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllNonConformities")
            .Produces<IEnumerable<NonConformityDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization("NonConformitiesRead");

            app.MapPost("/nonconformities", async (NonConformityDTO dto, ClaimsPrincipal user, INonConformityService nonConformityService) =>
            {
                int? userId = user.GetUserId();
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                try
                {
                    NonConformityDTO created = await nonConformityService.AddAsync(dto, userId.Value);

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
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("AddNonConformity")
            .Produces<NonConformityDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("NonConformitiesCreate");

            app.MapPut("/nonconformities", async (NonConformityDTO dto, INonConformityService nonConformityService) =>
            {
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
                catch (InvalidOperationException ex)
                {
                    return Results.Conflict(new { error = ex.Message });
                }
            })
            .WithName("UpdateNonConformity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("NonConformitiesUpdate");

            app.MapPut("/nonconformities/{id}/close", async (int id, ClaimsPrincipal user, INonConformityService nonConformityService) =>
            {
                int? closerUserId = user.GetUserId();
                if (closerUserId == null)
                {
                    return Results.Unauthorized();
                }

                try
                {
                    var closed = await nonConformityService.CloseAsync(id, closerUserId.Value);
                    return closed ? Results.NoContent() : Results.NotFound();
                }
                catch (ArgumentException ex) { return Results.BadRequest(new { error = ex.Message }); }
                catch (KeyNotFoundException ex) { return Results.NotFound(new { error = ex.Message }); }
                catch (InvalidOperationException ex) { return Results.Conflict(new { error = ex.Message }); }
            })
            .WithName("CloseNonConformity")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi()
            .RequireAuthorization("NonConformitiesUpdate");

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
            .WithOpenApi()
            .RequireAuthorization("NonConformitiesDelete");
        }
    }
}
