using Application.Services;
using DTOs;

namespace WebAPI
{
    public static class UserEndpoints
    {
        public static void MapUserEndpoints(this WebApplication app)
        {
            app.MapGet("/users/{id}", async (int id, IUserService userService) =>
            {
                UserDTO? dto = await userService.GetAsync(id);

                if (dto == null)
                {
                    return Results.NotFound();
                }

                return Results.Ok(dto);
            })
            .WithName("GetUser")
            .Produces<UserDTO>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();

            app.MapGet("/users", async (IUserService userService) =>
            {
                var dtos = await userService.GetAllAsync();

                return Results.Ok(dtos);
            })
            .WithName("GetAllUsers")
            .Produces<IEnumerable<UserDTO>>(StatusCodes.Status200OK)
            .WithOpenApi();

            app.MapPost("/users", async (UserDTO dto, IUserService userService) =>
            {
                try
                {
                    UserDTO created = await userService.AddAsync(dto);

                    return Results.Created($"/users/{created.Id}", created);
                }
                catch (ArgumentException ex)
                {
                    return Results.BadRequest(new { error = ex.Message });
                }
            })
            .WithName("AddUser")
            .Produces<UserDTO>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapPut("/users/{id}", async (int id, UserDTO dto, IUserService userService) =>
            {
                dto.Id = id;
                try
                {
                    var updated = await userService.UpdateAsync(dto);

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
            .WithName("UpdateUser")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status400BadRequest)
            .WithOpenApi();

            app.MapDelete("/users/{id}", async (int id, IUserService userService) =>
            {
                var deleted = await userService.DeleteAsync(id);

                if (!deleted)
                {
                    return Results.NotFound();
                }

                return Results.NoContent();
            })
            .WithName("DeleteUser")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status404NotFound)
            .WithOpenApi();
        }
    }
}