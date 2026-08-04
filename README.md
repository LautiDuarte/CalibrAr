# CalibrAr

CalibrAr es un sistema de gestión de recursos de seguimiento y medición, diseñado para empresas del sector manufacturero que necesitan dar cumplimiento a los requisitos de la norma ISO 9001:2015, cláusula 7.1.5 (Recursos de seguimiento y medición).

## Requisitos

- .NET 8 SDK

## Estructura de la solución

La solución vive en `CalibrAr/CalibrAr.sln` y está organizada en capas:

- **Domain.Model**: entidades del dominio (`Area`, `Location`, `InstrumentType`, `ReferenceStandard`, `User`, `Instrument`, `Calibration`, etc.), con validación propia (setters privados + métodos `SetX`).
- **Data**: interfaces y repositorios de acceso a datos.
- **DTOs**: objetos de transferencia usados entre la API y los servicios.
- **Application.Services**: lógica de negocio, mapea entre DTOs y entidades de dominio.
- **WebAPI**: host de ASP.NET Core (Minimal APIs), expone los endpoints y la documentación Swagger.

## Cómo compilar y correr

Desde la carpeta `CalibrAr/` (donde está el `.sln`):

```
dotnet build CalibrAr.sln
dotnet run --project WebAPI/WebAPI.csproj
```

Con la app corriendo en modo Development, Swagger queda disponible en `/swagger`.

Para compilar un solo proyecto, apuntá directo a su `.csproj`, por ejemplo:

```
dotnet build Application.Services/Application.Services.csproj
```
