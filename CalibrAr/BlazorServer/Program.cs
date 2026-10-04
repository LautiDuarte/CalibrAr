using API.Clients;
using BlazorServer.Auth;
using BlazorServer.Components;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components.Authorization;

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
        options.AccessDeniedPath = "/acceso-denegado"; // logueado pero sin el permiso → redirige acá
        options.Cookie.Name = "CalibrAr.Auth";
        options.Cookie.HttpOnly = true;            // JavaScript no puede leerla
        options.Cookie.SameSite = SameSiteMode.Strict;
        options.SlidingExpiration = false;         // no renovar: la cookie no puede vivir más que el JWT
    });

builder.Services.AddAuthorization(options =>
{
    // Mismo esquema que la WebAPI: la política "LocationsRead" exige el claim permission "Locations.read".
    // Solo las entidades que tienen pantalla en Blazor.
    string[] categories = ["Locations", "Areas", "Instruments", "InstrumentTypes"];
    (string Suffix, string Action)[] actions =
    [
        ("Read", "read"), ("Create", "create"), ("Update", "update"), ("Delete", "delete")
    ];

    foreach (var category in categories)
    {
        foreach (var (suffix, action) in actions)
        {
            options.AddPolicy($"{category}{suffix}", policy => policy.RequireClaim("permission", $"{category}.{action}"));
        }
    }
});
builder.Services.AddCascadingAuthenticationState();

// Reemplaza al proveedor por defecto (que nunca revisa al usuario) por uno que detecta el
// vencimiento del JWT mientras el circuito está abierto. Scoped = uno por circuito/usuario.
builder.Services.AddScoped<AuthenticationStateProvider, TokenExpirationAuthenticationStateProvider>();

// Clientes de la WebAPI. Scoped = uno por circuito: cada usuario usa su propio token,
// que DI les inyecta a través de ITokenProvider (nunca un estático compartido).
builder.Services.AddScoped<ITokenProvider, BlazorTokenProvider>();
builder.Services.AddScoped<AuthApiClient>();
builder.Services.AddScoped<LocationApiClient>();
builder.Services.AddScoped<AreaApiClient>();
builder.Services.AddScoped<InstrumentTypeApiClient>();
builder.Services.AddScoped<InstrumentApiClient>();

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
