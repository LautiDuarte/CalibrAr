using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class ProcedureEndpoints
    {
        public static void MapProcedureEndpoints(this WebApplication app)
        {
            app.MapGet("/procedures/{id}", async (int id, IProcedureService procedureService) =>
            {
                ProcedureDTO? dto = await procedureService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetProcedure")
            .Produces<ProcedureDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("ProceduresRead");

            app.MapGet("/procedures", async (IProcedureService procedureService) =>
            {
                var dtos = await procedureService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllProcedures")
            .Produces<IEnumerable<ProcedureDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization("ProceduresRead");

            app.MapPost("/procedures", async (ProcedureDTO dto, IProcedureService procedureService) =>
            {
                try
                {
                    ProcedureDTO created = await procedureService.AddAsync(dto);

                    return Results.Created($"/procedures/{created.Id}", created);
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
            .WithName("AddProcedure")
            .Produces<ProcedureDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("ProceduresCreate");

            app.MapPut("/procedures/{id}", async (int id, ProcedureDTO dto, IProcedureService procedureService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await procedureService.UpdateAsync(dto);

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
            .WithName("UpdateProcedure")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("ProceduresUpdate");

            app.MapDelete("/procedures/{id}", async (int id, IProcedureService procedureService) =>
            {
                var deleted = await procedureService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteProcedure")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("ProceduresDelete");
        }
    }
}
