using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class ReferenceStandardEndpoints
    {
        public static void MapReferenceStandardEndpoints(this WebApplication app)
        {
            app.MapGet("/referencestandards/{id}", async (int id, IReferenceStandardService referenceStandardService) =>
            {
                ReferenceStandardDTO? dto = await referenceStandardService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetReferenceStandard")
            .Produces<ReferenceStandardDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/referencestandards", async (IReferenceStandardService referenceStandardService) =>
            {
                var dtos = await referenceStandardService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllReferenceStandards")
            .Produces<IEnumerable<ReferenceStandardDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/referencestandards", async (ReferenceStandardDTO dto, IReferenceStandardService referenceStandardService) =>
            {
                try
                {
                    ReferenceStandardDTO created = await referenceStandardService.AddAsync(dto);

                    return Results.Created($"/referencestandards/{created.Id}", created);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddReferenceStandard")
            .Produces<ReferenceStandardDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/referencestandards/{id}", async (int id, ReferenceStandardDTO dto, IReferenceStandardService referenceStandardService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await referenceStandardService.UpdateAsync(dto);

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
            .WithName("UpdateReferenceStandard")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/referencestandards/{id}", async (int id, IReferenceStandardService referenceStandardService) =>
            {
                var deleted = await referenceStandardService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteReferenceStandard")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}