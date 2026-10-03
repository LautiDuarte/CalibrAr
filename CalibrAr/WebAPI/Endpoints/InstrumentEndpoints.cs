using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class InstrumentEndpoints
    {
        public static void MapInstrumentEndpoints(this WebApplication app)
        {
            app.MapGet("/instruments/{id}", async (int id, IInstrumentService instrumentService) =>
            {
                InstrumentDTO? dto = await instrumentService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetInstrument")
            .Produces<InstrumentDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/instruments", async (IInstrumentService instrumentService) =>
            {
                var dtos = await instrumentService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllInstruments")
            .Produces<IEnumerable<InstrumentDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/instruments", async (InstrumentDTO dto, IInstrumentService instrumentService) =>
            {
                try
                {
                    InstrumentDTO created = await instrumentService.AddAsync(dto);

                    return Results.Created($"/instruments/{created.Id}", created);
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
            .WithName("AddInstrument")
            .Produces<InstrumentDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/instruments", async (InstrumentDTO dto, IInstrumentService instrumentService) =>
            {
                try
                {
                    var updated = await instrumentService.UpdateAsync(dto);

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
            .WithName("UpdateInstrument")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/instruments/{id}", async (int id, IInstrumentService instrumentService) =>
            {
                var deleted = await instrumentService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteInstrument")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}