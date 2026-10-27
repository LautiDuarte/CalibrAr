using System.Security.Claims;
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
            .WithOpenApi()
            .RequireAuthorization("CalibrationsRead");

            app.MapGet("/calibrations", async (ICalibrationService calibrationService) =>
            {
                var dtos = await calibrationService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllCalibrations")
            .Produces<IEnumerable<CalibrationDTO>>(StatusCodes.Status200OK)
            .WithOpenApi()
            .RequireAuthorization("CalibrationsRead");

            // Búsqueda con filtros combinables: los que no se mandan, no filtran
            app.MapGet("/calibrations/search", async (DateTime? from, DateTime? to, string? result, int? instrumentId,
                string? text, string? interventionType, bool? approved, ICalibrationService calibrationService) =>
            {
                try
                {
                    var dtos = await calibrationService.SearchAsync(from, to, result, instrumentId, text, interventionType, approved);

                    return Results.Ok(dtos);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("SearchCalibrations")
            .Produces<IEnumerable<CalibrationDTO>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi()
            .RequireAuthorization("CalibrationsRead");

            app.MapPost("/calibrations", async (CalibrationDTO dto, ClaimsPrincipal user, ICalibrationService calibrationService) =>
            {
                int? userId = user.GetUserId();
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                try
                {
                    CalibrationDTO created = await calibrationService.AddAsync(dto, userId.Value);

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
            .WithOpenApi()
            .RequireAuthorization("CalibrationsCreate");

            app.MapPut("/calibrations", async (CalibrationDTO dto, ClaimsPrincipal user, ICalibrationService calibrationService) =>
            {
                int? userId = user.GetUserId();
                if (userId == null)
                {
                    return Results.Unauthorized();
                }

                try
                {
                    var updated = await calibrationService.UpdateAsync(dto, userId.Value);

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
            .WithName("UpdateCalibration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi()
            .RequireAuthorization("CalibrationsUpdate");

            app.MapDelete("/calibrations/{id}", async (int id, ICalibrationService calibrationService) =>
            {
                try
                {
                    var deleted = await calibrationService.DeleteAsync(id);

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
            .WithName("DeleteCalibration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi()
            .RequireAuthorization("CalibrationsDelete");

            app.MapPut("/calibrations/{id}/approve", async (int id, ClaimsPrincipal user, ICalibrationService calibrationService) =>
            {
                int? approverUserId = user.GetUserId();
                if (approverUserId == null)
                {
                    return Results.Unauthorized();
                }

                try
                {
                    var approved = await calibrationService.ApproveAsync(id, approverUserId.Value);
                    return approved ? Results.NoContent() : Results.NotFound();
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
            .WithName("ApproveCalibration")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status409Conflict)
            .WithOpenApi()
            .RequireAuthorization("CalibrationsUpdate");
        }
    }
}
