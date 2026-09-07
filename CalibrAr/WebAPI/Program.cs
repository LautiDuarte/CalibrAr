using System.Text;
using Application.Services;
using Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Habilita el boton "Authorize" en Swagger UI para pegar el token de /auth/login.
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegar el token que devuelve POST /auth/login (sin el prefijo 'Bearer ')."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<CalibrArContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IInstrumentTypeRepository, InstrumentTypeRepository>();
builder.Services.AddScoped<IReferenceStandardRepository, ReferenceStandardRepository>();
builder.Services.AddScoped<IInstrumentRepository, InstrumentRepository>();
builder.Services.AddScoped<IProcedureRepository, ProcedureRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICalibrationRepository, CalibrationRepository>();
builder.Services.AddScoped<ICalibrationMeasurementRepository, CalibrationMeasurementRepository>();
builder.Services.AddScoped<INonConformityRepository, NonConformityRepository>();
builder.Services.AddScoped<IInstrumentStatusHistoryRepository, InstrumentStatusHistoryRepository>();

builder.Services.AddScoped<IAreaService, AreaService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IInstrumentTypeService, InstrumentTypeService>();
builder.Services.AddScoped<IReferenceStandardService, ReferenceStandardService>();
builder.Services.AddScoped<IInstrumentService, InstrumentService>();
builder.Services.AddScoped<IProcedureService, ProcedureService>();
builder.Services.AddScoped<ICalibrationService, CalibrationService>();
builder.Services.AddScoped<ICalibrationMeasurementService, CalibrationMeasurementService>();
builder.Services.AddScoped<INonConformityService, NonConformityService>();
builder.Services.AddScoped<IInstrumentStatusHistoryService, InstrumentStatusHistoryService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<AuthService>();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var jwtSecretKey = jwtSettings["SecretKey"]
    ?? throw new InvalidOperationException("Falta configurar JwtSettings:SecretKey en appsettings.json.");

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwtSettings["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization(options =>
{
    // Una política por cada permiso sembrado en CalibrArContext, formato "Categoria.accion".
    string[] categories =
    [
        "Locations", "Areas", "Instruments", "Calibrations", "Users", "InstrumentTypes",
        "Procedures", "NonConformities", "ReferenceStandards", "InstrumentStatusHistory",
        "CalibrationMeasurements"
    ];
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

    // Cualquier endpoint sin política explícita ni [AllowAnonymous] igual exige estar autenticado.
    options.FallbackPolicy = options.DefaultPolicy;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapAreaEndpoints();
app.MapLocationEndpoints();
app.MapInstrumentTypeEndpoints();
app.MapReferenceStandardEndpoints();
app.MapInstrumentEndpoints();
app.MapProcedureEndpoints();
app.MapCalibrationEndpoints();
app.MapCalibrationMeasurementEndpoints();
app.MapNonConformityEndpoints();
app.MapInstrumentStatusHistoryEndpoints();
app.MapUserEndpoints();
app.MapAuthEndpoints();

app.Run();
