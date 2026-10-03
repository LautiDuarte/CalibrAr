using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class CalibrationMeasurementEndpoints
    {
        public static void MapCalibrationMeasurementEndpoints(this WebApplication app)
        {
            app.MapGet("/calibrationmeasurements/{id}", async (int id, ICalibrationMeasurementService calibrationMeasurementService) =>
            {
                CalibrationMeasurementDTO? dto = await calibrationMeasurementService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetCalibrationMeasurement")
            .Produces<CalibrationMeasurementDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("CalibrationMeasurementsRead");

            app.MapGet("/calibrationmeasurements", async (ICalibrationMeasurementService calibrationMeasurementService) =>
            {
                var dtos = await calibrationMeasurementService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllCalibrationMeasurements")
            .Produces<IEnumerable<CalibrationMeasurementDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization("CalibrationMeasurementsRead");

            app.MapPost("/calibrationmeasurements", async (CalibrationMeasurementDTO dto, ICalibrationMeasurementService calibrationMeasurementService) =>
            {
                try
                {
                    CalibrationMeasurementDTO created = await calibrationMeasurementService.AddAsync(dto);

                    return Results.Created($"/calibrationmeasurements/{created.Id}", created);
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
            .WithName("AddCalibrationMeasurement")
            .Produces<CalibrationMeasurementDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("CalibrationMeasurementsCreate");

            app.MapPut("/calibrationmeasurements/{id}", async (int id, CalibrationMeasurementDTO dto, ICalibrationMeasurementService calibrationMeasurementService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await calibrationMeasurementService.UpdateAsync(dto);

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
            .WithName("UpdateCalibrationMeasurement")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("CalibrationMeasurementsUpdate");

            app.MapDelete("/calibrationmeasurements/{id}", async (int id, ICalibrationMeasurementService calibrationMeasurementService) =>
            {
                var deleted = await calibrationMeasurementService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteCalibrationMeasurement")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("CalibrationMeasurementsDelete");
        }
    }
}
