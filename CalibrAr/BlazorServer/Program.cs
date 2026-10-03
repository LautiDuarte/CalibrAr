using BlazorServer.Components;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Autenticación por cookie (patrón BFF): el navegador solo tiene la cookie,
// el JWT para la WebAPI viaja adentro de ella, encriptado y en el servidor.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";              // sin cookie → redirige acá
        options.Cookie.Name = "CalibrAr.Auth";
        options.Cookie.HttpOnly = true;            // JavaScript no puede leerla
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.SlidingExpiration = false;         // no renovar: la cookie no puede vivir más que el JWT
    });

builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

// HttpClient para hablar con la WebAPI (por ahora solo lo usa el login).
builder.Services.AddHttpClient("WebAPI", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["WebApi:BaseUrl"]
        ?? throw new InvalidOperationException("Falta configurar WebApi:BaseUrl en appsettings.json."));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Logout por POST (no GET) para que un link o una imagen de otro sitio no pueda cerrar la sesión.
// El token antiforgery garantiza que el formulario salió de esta misma app.
app.MapPost("/logout", async (HttpContext context, IAntiforgery antiforgery) =>
{
    if (!await antiforgery.IsRequestValidAsync(context))
        return Results.BadRequest();

    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.LocalRedirect("/login");
});

app.Run();
