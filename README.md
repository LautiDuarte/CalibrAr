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
- **API.Clients**: clientes HTTP (uno por entidad) que usan tanto `WindowsForms` como `BlazorServer` para hablar con la WebAPI. No saben de dónde sale el token: se lo piden a un `ITokenProvider` que cada interfaz implementa a su manera. Así no hay nada global compartido entre usuarios.
- **API.Auth.WindowsForms**: implementación del servicio de autenticación específica para la app de escritorio (cachea el token, revisa expiración, expone los permisos del usuario logueado). Acá vive también el `AuthServiceProvider`, que es el "global" de la app de escritorio.
- **WindowsForms**: la interfaz de escritorio (login, pantalla principal con menú, formularios de alta/baja/modificación).
- **BlazorServer**: la interfaz web (Blazor Server con Bootstrap). Hace lo mismo que la de escritorio para las entidades principales, con login, logout y menú según permisos.

## Base de datos

La base **se autogenera** la primera vez que la WebAPI recibe un request real (usa `Database.EnsureCreated()`, no Migrations). No hace falta correr ningún script a mano.

⚠️ Como no se usan Migrations, si en algún momento cambia el modelo de datos y la base ya existe con el esquema viejo, hay que borrarla a mano para que se recree:

```sql
DROP DATABASE IF EXISTS CalibrAr;
```

Esto pasó en la Entrega 3 y le va a pasar a cualquiera que venga de una versión anterior: se quitó `NextCalibrationDate` de `Calibration` (ahora se calcula) y se pasaron a `decimal(18,4)` las columnas decimales (`MaxAllowedError`, `NominalValue`, `MeasuredValue` y `Error`). Si no borrás la base, crear una calibración falla porque la columna vieja es `NOT NULL`. Y con `decimal(18,2)` un valor como `0.004` se guardaba como `0` y después rompía la lista de calibraciones al leerlo.

## Cómo correr todo

1. **Levantar la WebAPI primero.** En Visual Studio, poné `WebAPI` como proyecto de inicio y F5 (perfil `https` o `http`). Se abre el navegador en `/swagger`. Entrar a Swagger no alcanza para crear la base — hacé al menos un request real (por ejemplo `GET /locations` desde Swagger) para disparar la creación.
2. **Levantar `WindowsForms`** con la API ya corriendo. La forma más cómoda: botón derecho sobre la solución → *Set Startup Projects* → *Multiple startup projects* → poner `WebAPI` y `WindowsForms` en "Start", así arrancan juntos con un solo F5.
3. Se abre la pantalla de **Login**. Usá alguna de las cuentas de prueba de la tabla de abajo.

Para la **web (Blazor)** es parecido: la WebAPI tiene que estar corriendo y se levanta `BlazorServer` con el perfil `http` (queda en `http://localhost:5249`). Se puede poner `WebAPI` y `BlazorServer` en "Start" en *Multiple startup projects*, o `WebAPI` + `WindowsForms` + `BlazorServer` si querés tener las dos interfaces a la vez. Los clientes buscan la WebAPI en `http://localhost:5031/`; si la tuya corre en otro lado, se puede cambiar con la variable de entorno `TPI_API_BASE_URL`.

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

- **Desde WindowsForms**: el token se maneja solo, no hay nada que hacer a mano. Se guarda en memoria mientras la app está abierta (nunca en disco).
- **Desde Blazor**: acá lo hicimos distinto, porque Blazor Server corre en el servidor y atiende a todos los usuarios en el mismo proceso, así que un token "global" lo compartirían todos. El login lo hace el servidor de Blazor contra `POST /auth/login`, y el token JWT queda **adentro de una cookie de sesión** (`HttpOnly`, encriptada), que el navegador no puede leer con JavaScript. En cada llamada a la WebAPI, Blazor saca el token de la cookie del usuario y lo manda como `Bearer`. Otras cosas que hace:
  - La cookie vence cuando vence el token, y sobrevive al F5 pero no a cerrar el navegador.
  - Si el token vence con la página abierta, a lo sumo en un minuto te manda al login.
  - Todas las páginas piden login por defecto, y cada pantalla y cada botón (Nuevo, Editar, Eliminar) se muestran según los permisos del usuario. Si alguien escribe la URL a mano sin permiso, ve "Acceso denegado".
  - El logout es un `POST` con token antiforgery, así que otro sitio no puede cerrarte la sesión.
  - Ocultar un botón es solo para que se vea prolijo: la seguridad de verdad la hace la WebAPI, que valida el token y el permiso en cada request.
- **Desde Swagger** (para probar la API directamente): hacé `Try it out` → `Execute` en `POST /auth/login`, copiá el valor de `"token"` de la respuesta, apretá el botón **Authorize** (arriba de todo, con un candado 🔒) y pegalo ahí (sin escribir "Bearer" adelante). De ahí en más, todos los "Try it out" que hagas van a mandar el token automáticamente.

## Qué entidades tienen pantalla en WindowsForms

Tienen pantalla completa (lista + alta/baja/modificación): `Location`, `Area`, `InstrumentType`, `Instrument`, `Procedure` y `Calibration`.

`Calibration` es el maestro/detalle de esta entrega: una calibración tiene sus mediciones (`CalibrationMeasurement`), que se cargan en la misma pantalla y se guardan todas juntas. Si algo falla, no se guarda nada. Al borrar una calibración se borran también sus mediciones.

El resto de las entidades (`ReferenceStandard`, `NonConformity`, `User`, `CalibrationMeasurement`, `InstrumentStatusHistory`) tienen el backend completo (repositorio, servicio y endpoints protegidos por JWT) pero todavía no tienen pantalla propia — se pueden probar por Swagger con cualquiera de los usuarios de la tabla de arriba.

## Qué entidades tienen pantalla en Blazor

`Location`, `Area`, `InstrumentType` e `Instrument`, las mismas cuatro que ya estaban en la primera versión de la app de escritorio. Cada una tiene su lista y sus pantallas de alta y de edición, y el borrado pide confirmación. Algunas reglas que tuvimos que respetar:

- Al **crear** un instrumento el estado siempre es "Activo", así que el estado solo se elige al editar. El **tipo** no se puede cambiar una vez creado, y las fechas de calibración las calcula el servicio.
- El **error máximo** y la **frecuencia** del instrumento son opcionales: si quedan vacíos se usan los del tipo de instrumento.
- El **responsable** de un área es obligatorio (la columna en la base es `NOT NULL`).
- Si el servidor rechaza algo (por ejemplo, borrar una ubicación que tiene áreas, o repetir un código de instrumento), el mensaje se muestra arriba en un cuadro rojo.

Los permisos funcionan igual que en escritorio. Por ejemplo, el auditor ve la lista de instrumentos pero no tiene botones para crear, editar ni borrar.

## Pendiente conocido

- En `WindowsForms`, si el token vence con una lista abierta, se muestra un error genérico en vez de mandarte directo al login (la excepción de sesión vencida se la "traga" cada pantalla). Hay que hacer logout desde el menú y volver a entrar.
- Hay unos 30 warnings de nulabilidad (CS86xx) en el código de `WindowsForms` y los servicios. No rompen nada pero hay que limpiarlos.
- Los mensajes que vienen del dominio (por ejemplo "Cannot delete location...") están en inglés y se muestran así en las dos interfaces.
- El dominio mezcla español e inglés (nombres de clases y algunos mensajes de validación en inglés, `UserRole` y otros mensajes en español) — es una decisión de idioma pendiente de unificar.
