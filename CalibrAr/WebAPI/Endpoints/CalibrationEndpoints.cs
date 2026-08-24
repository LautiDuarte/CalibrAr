using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class CalibrationEndpoints
    {
        public static void MapCalibrationEndpoints(this WebApplication app)
        {
            app.MapGet("/calibrations/{id}", async (int id, ICalibrationService calibrationService) =>
            {
                CalibrationDTO? dto = await calibrationService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetCalibration")
            .Produces<CalibrationDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/calibrations", async (ICalibrationService calibrationService) =>
            {
                var dtos = await calibrationService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllCalibrations")
            .Produces<IEnumerable<CalibrationDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/calibrations", async (CalibrationDTO dto, ICalibrationService calibrationService) =>
            {
                try
                {
                    CalibrationDTO created = await calibrationService.AddAsync(dto);

                    return Results.Created($"/calibrations/{created.Id}", created);
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
            .WithName("AddCalibration")
            .Produces<CalibrationDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/calibrations/{id}", async (int id, CalibrationDTO dto, ICalibrationService calibrationService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await calibrationService.UpdateAsync(dto);

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
            .WithName("UpdateCalibration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/calibrations/{id}", async (int id, ICalibrationService calibrationService) =>
            {
                var deleted = await calibrationService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteCalibration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}