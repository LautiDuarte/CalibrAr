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
        }
    }
}
