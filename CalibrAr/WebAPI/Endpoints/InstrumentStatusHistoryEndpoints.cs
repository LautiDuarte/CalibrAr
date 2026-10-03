using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class InstrumentStatusHistoryEndpoints
    {
        public static void MapInstrumentStatusHistoryEndpoints(this WebApplication app)
        {
            app.MapGet("/instrumentstatushistories/{id}", async (int id, IInstrumentStatusHistoryService instrumentStatusHistoryService) =>
            {
                InstrumentStatusHistoryDTO? dto = await instrumentStatusHistoryService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetInstrumentStatusHistory")
            .Produces<InstrumentStatusHistoryDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("InstrumentStatusHistoryRead");

            app.MapGet("/instrumentstatushistories", async (IInstrumentStatusHistoryService instrumentStatusHistoryService) =>
            {
                var dtos = await instrumentStatusHistoryService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllInstrumentStatusHistories")
            .Produces<IEnumerable<InstrumentStatusHistoryDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization("InstrumentStatusHistoryRead");

            app.MapPost("/instrumentstatushistories", async (InstrumentStatusHistoryDTO dto, IInstrumentStatusHistoryService instrumentStatusHistoryService) =>
            {
                try
                {
                    InstrumentStatusHistoryDTO created = await instrumentStatusHistoryService.AddAsync(dto);

                    return Results.Created($"/instrumentstatushistories/{created.Id}", created);
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
            .WithName("AddInstrumentStatusHistory")
            .Produces<InstrumentStatusHistoryDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("InstrumentStatusHistoryCreate");

            app.MapPut("/instrumentstatushistories/{id}", async (int id, InstrumentStatusHistoryDTO dto, IInstrumentStatusHistoryService instrumentStatusHistoryService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await instrumentStatusHistoryService.UpdateAsync(dto);

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
            .WithName("UpdateInstrumentStatusHistory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("InstrumentStatusHistoryUpdate");

            app.MapDelete("/instrumentstatushistories/{id}", async (int id, IInstrumentStatusHistoryService instrumentStatusHistoryService) =>
            {
                var deleted = await instrumentStatusHistoryService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteInstrumentStatusHistory")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("InstrumentStatusHistoryDelete");
        }
    }
}
