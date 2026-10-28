using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class InstrumentTypeEndpoints
    {
        public static void MapInstrumentTypeEndpoints(this WebApplication app)
        {
            app.MapGet("/instrumenttypes/{id}", async (int id, IInstrumentTypeService instrumentTypeService) =>
            {
                InstrumentTypeDTO? dto = await instrumentTypeService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetInstrumentType")
            .Produces<InstrumentTypeDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("InstrumentTypesRead");

            app.MapGet("/instrumenttypes", async (IInstrumentTypeService instrumentTypeService) =>
            {
                var dtos = await instrumentTypeService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllInstrumentTypes")
            .Produces<IEnumerable<InstrumentTypeDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization("InstrumentTypesRead");

            app.MapPost("/instrumenttypes", async (InstrumentTypeDTO dto, IInstrumentTypeService instrumentTypeService) =>
            {
                try
                {
                    InstrumentTypeDTO created = await instrumentTypeService.AddAsync(dto);

                    return Results.Created($"/instrumenttypes/{created.Id}", created);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddInstrumentType")
            .Produces<InstrumentTypeDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("InstrumentTypesCreate");

            app.MapPut("/instrumenttypes", async (InstrumentTypeDTO dto, IInstrumentTypeService instrumentTypeService) =>
            {
                try
                {
                    var updated = await instrumentTypeService.UpdateAsync(dto);

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
            .WithName("UpdateInstrumentType")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("InstrumentTypesUpdate");

            app.MapDelete("/instrumenttypes/{id}", async (int id, IInstrumentTypeService instrumentTypeService) =>
            {
                try
                {
                    var deleted = await instrumentTypeService.DeleteAsync(id);

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
            .WithName("DeleteInstrumentType")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("InstrumentTypesDelete");
        }
    }
}
