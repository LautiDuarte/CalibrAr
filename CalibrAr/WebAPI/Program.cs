using Application.Services;
using Data;
using Microsoft.EntityFrameworkCore;
using WebAPI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.Run();
