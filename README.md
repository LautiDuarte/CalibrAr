# CalibrAr

CalibrAr es un sistema de gestión de recursos de seguimiento y medición, diseñado para empresas del sector manufacturero que necesitan dar cumplimiento a los requisitos de la norma ISO 9001:2015, cláusula 7.1.5 (Recursos de seguimiento y medición).

## Requisitos

- .NET 8 SDK
- SQL Server local. El proyecto está configurado para conectarse a una instancia **SQL Server Express** llamada `SQLEXPRESS` (`Server=localhost\SQLEXPRESS`). Si tu instancia tiene otro nombre (o usás LocalDB, o una instancia por defecto sin nombre), editá `ConnectionStrings:DefaultConnection` en `CalibrAr/WebAPI/appsettings.json` antes de correr nada.
- Visual Studio 2022 (recomendado, sobre todo para correr `WindowsForms`, que es un proyecto de escritorio). También se puede compilar y correr la WebAPI sola con `dotnet` desde la terminal.

## Estructura de la solución

La solución vive en `CalibrAr/CalibrAr.sln` y está organizada en capas:

- **Domain.Model**: entidades del dominio (`Area`, `Location`, `Instrument`, `InstrumentType`, `Calibration`, `CalibrationMeasurement`, `NonConformity`, `InstrumentStatusHistory`, `Procedure`, `ReferenceStandard`, `User`, `Permission`, `PermissionGroup`), con validación propia (setters privados + métodos `SetX`).
- **Data**: `CalibrArContext` (DbContext de Entity Framework Core) + interfaces y repositorios de acceso a datos, uno por entidad.
- **DTOs**: objetos de transferencia usados entre la API y los servicios.
- **Application.Services**: lógica de negocio, mapea entre DTOs y entidades de dominio, incluye `AuthService` (login y generación de JWT).
- **WebAPI**: host de ASP.NET Core (Minimal APIs), expone los endpoints, la documentación Swagger, y valida los tokens JWT en cada request.
- **API.Clients**: clientes HTTP (uno por entidad) que consume `WindowsForms` para hablar con la WebAPI, más el manejo del token de sesión.
- **API.Auth.WindowsForms**: implementación del servicio de autenticación específica para la app de escritorio (cachea el token, revisa expiración, expone los permisos del usuario logueado).
- **WindowsForms**: la interfaz de escritorio (login, pantalla principal con menú, formularios de alta/baja/modificación).

## Base de datos

La base **se autogenera** la primera vez que la WebAPI recibe un request real (usa `Database.EnsureCreated()`, no Migrations). No hace falta correr ningún script a mano.

⚠️ Como no se usan Migrations, si en algún momento cambia el modelo de datos y la base ya existe con el esquema viejo, hay que borrarla a mano para que se recree:

```sql
DROP DATABASE IF EXISTS CalibrAr;
```

## Cómo correr todo

1. **Levantar la WebAPI primero.** En Visual Studio, poné `WebAPI` como proyecto de inicio y F5 (perfil `https` o `http`). Se abre el navegador en `/swagger`. Entrar a Swagger no alcanza para crear la base — hacé al menos un request real (por ejemplo `GET /locations` desde Swagger) para disparar la creación.
2. **Levantar `WindowsForms`** con la API ya corriendo. La forma más cómoda: botón derecho sobre la solución → *Set Startup Projects* → *Multiple startup projects* → poner `WebAPI` y `WindowsForms` en "Start", así arrancan juntos con un solo F5.
3. Se abre la pantalla de **Login**. Usá alguna de las cuentas de prueba de la tabla de abajo.

Por consola (solo para la WebAPI, sin interfaz de escritorio), desde la carpeta `CalibrAr/`:

```
dotnet build CalibrAr.sln
dotnet run --project WebAPI/WebAPI.csproj
```

## Usuarios de prueba (seed)

La base se siembra con 4 usuarios, uno por rol, cada uno con un conjunto de permisos distinto (ver `CalibrArContext.SeedInitialData`):

| Rol | Email | Password |
|---|---|---|
| Administrador (todos los permisos) | `admin@calibrar.com` | `admin123` |
| Responsable | `responsible@calibrar.com` | `responsible123` |
| Operador | `operator@calibrar.com` | `operator123` |
| Auditor (solo lectura de Instruments) | `auditor@calibrar.com` | `auditor123` |

## Autenticación (JWT)

`POST /auth/login` devuelve un token JWT con los permisos del usuario codificados como claims (`"permission": ["Areas.read", "Areas.create", ...]`). Todos los demás endpoints exigen ese token en el header `Authorization: Bearer <token>`, salvo el propio login.

- **Desde WindowsForms**: el token se maneja solo, no hay nada que hacer a mano.
- **Desde Swagger** (para probar la API directamente): hacé `Try it out` → `Execute` en `POST /auth/login`, copiá el valor de `"token"` de la respuesta, apretá el botón **Authorize** (arriba de todo, con un candado 🔒) y pegalo ahí (sin escribir "Bearer" adelante). De ahí en más, todos los "Try it out" que hagas van a mandar el token automáticamente.

## Qué entidades tienen pantalla en WindowsForms

Tienen pantalla completa (lista + alta/baja/modificación): `Location`, `Area`, `InstrumentType`, `Instrument`.

El resto de las entidades (`ReferenceStandard`, `Procedure`, `Calibration`, `NonConformity`, `User`, `CalibrationMeasurement`, `InstrumentStatusHistory`) tienen el backend completo (repositorio, servicio y endpoints protegidos por JWT) pero todavía no tienen pantalla propia en `WindowsForms` — se pueden probar por Swagger con cualquiera de los usuarios de la tabla de arriba.

## Pendiente conocido

- Falta un botón/menú de **logout** explícito en `WindowsForms` (hoy la sesión se cierra sola solo cuando expira el token).
- El dominio mezcla español e inglés (nombres de clases y algunos mensajes de validación en inglés, `UserRole` y otros mensajes en español) — es una decisión de idioma pendiente de unificar.
