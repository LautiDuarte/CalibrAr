# CalibrAr

CalibrAr es un sistema de gestión de recursos de seguimiento y medición, diseñado para empresas del sector manufacturero que necesitan dar cumplimiento a los requisitos de la norma ISO 9001:2015, cláusula 7.1.5 (Recursos de seguimiento y medición).

## Integrantes

- Cardone, Juan Ignacio - Legajo: 50791 - Mail: juancardone8@gmail.com
- Donzino, Yamila - Legajo: 51004 - Mail: yamiladonzino51@gmail.com
- Duarte, Lautaro - Legajo: 51555 - Mail: lautaroduarte@outlook.es

## Requisitos

- .NET 8 SDK
- SQL Server local. El proyecto está configurado para conectarse a una instancia **SQL Server Express** llamada `SQLEXPRESS` (`Server=localhost\SQLEXPRESS`). Si tu instancia tiene otro nombre (o usás LocalDB, o una instancia por defecto sin nombre), editá `ConnectionStrings:DefaultConnection` en `CalibrAr/WebAPI/appsettings.json` antes de correr nada.
- Visual Studio 2022 (recomendado, sobre todo para correr `WindowsForms`, que es un proyecto de escritorio). También se puede compilar y correr la WebAPI sola con `dotnet` desde la terminal.

## Estructura de la solución

La solución vive en `CalibrAr/CalibrAr.sln` y está organizada en capas:

- **Domain.Model**: entidades del dominio (`Area`, `Location`, `Instrument`, `InstrumentType`, `Calibration`, `CalibrationMeasurement`, `NonConformity`, `InstrumentStatusHistory`, `Procedure`, `ReferenceStandard`, `User`, `Permission`, `PermissionGroup`), con validación propia (setters privados + métodos `SetX`).
- **Data**: `CalibrArContext` (DbContext de Entity Framework Core) + interfaces y repositorios de acceso a datos, uno por entidad. Acá también está `DemoDataSeeder`, que carga los datos de ejemplo.
- **DTOs**: objetos de transferencia usados entre la API y los servicios.
- **Application.Services**: lógica de negocio, mapea entre DTOs y entidades de dominio, incluye `AuthService` (login y generación de JWT) y los reportes (`ReportService` arma los datos y `ReportPdfGenerator` genera los PDF con QuestPDF).
- **WebAPI**: host de ASP.NET Core (Minimal APIs), expone los endpoints, la documentación Swagger, y valida los tokens JWT en cada request.
- **API.Clients**: clientes HTTP (uno por entidad) que usan tanto `WindowsForms` como `BlazorServer` para hablar con la WebAPI. No saben de dónde sale el token: se lo piden a un `ITokenProvider` que cada interfaz implementa a su manera. Así no hay nada global compartido entre usuarios.
- **API.Auth.WindowsForms**: implementación del servicio de autenticación específica para la app de escritorio (cachea el token, revisa expiración, expone los permisos del usuario logueado). Acá vive también el `AuthServiceProvider`, que es el "global" de la app de escritorio.
- **WindowsForms**: la interfaz de escritorio (login, pantalla principal con menú, formularios de alta/baja/modificación y reportes).
- **BlazorServer**: la interfaz web (Blazor Server con Bootstrap). Hace lo mismo que la de escritorio para todas las entidades, con login, logout, menú según permisos y reportes.

## Base de datos

La base **se crea sola** cuando arranca la WebAPI (usa `Database.EnsureCreated()`, no Migrations) y se carga con los usuarios de prueba y con datos de ejemplo. No hace falta correr ningún script a mano.

⚠️ Como no se usan Migrations, si en algún momento cambia el modelo de datos y la base ya existe con el esquema viejo, hay que borrarla a mano para que se recree:

```sql
DROP DATABASE IF EXISTS CalibrAr;
```

Esto pasó en la Entrega 3 y le va a pasar a cualquiera que venga de una versión anterior: se quitó `NextCalibrationDate` de `Calibration` (ahora se calcula) y se pasaron a `decimal(18,4)` las columnas decimales (`MaxAllowedError`, `NominalValue`, `MeasuredValue` y `Error`). Si no borrás la base, crear una calibración falla porque la columna vieja es `NOT NULL`. Y con `decimal(18,2)` un valor como `0.004` se guardaba como `0` y después rompía la lista de calibraciones al leerlo.

Con los reportes pasó lo mismo: el permiso nuevo `Reports.read` se carga junto con el modelo, así que una base vieja no lo tiene. Si venís de una versión anterior (o querés ver los datos de ejemplo, que solo se cargan cuando no hay instrumentos), borrá la base y dejá que se cree de nuevo. Lo mismo pasa con el link al documento de los procedimientos (`DocumentUrl`), que agregó una columna nueva.

## Cómo correr todo

1. **Levantar la WebAPI primero.** En Visual Studio, poné `WebAPI` como proyecto de inicio y F5 (perfil `https` o `http`). Se abre el navegador en `/swagger`. La base se crea sola al arrancar, así que la primera vez puede tardar unos segundos.
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
| Auditor (solo lectura de Instrumentos y Reportes) | `auditor@calibrar.com` | `auditor123` |

## Autenticación (JWT)

La clave con la que se firman los tokens **no está en el repositorio**. La WebAPI la busca en `JwtSettings:SecretKey`, que se puede definir con la variable de entorno `JwtSettings__SecretKey` o con user-secrets (`dotnet user-secrets set JwtSettings:SecretKey "<clave de al menos 32 caracteres>" --project CalibrAr/WebAPI`). Si no está definida y se corre en Development (lo normal al ejecutar desde Visual Studio), la API genera una clave al azar al arrancar, así que el proyecto corre sin configurar nada; la consecuencia es que al reiniciar la API los tokens anteriores dejan de valer y hay que volver a iniciar sesión. Fuera de Development, sin clave la API no arranca.

`POST /auth/login` devuelve un token JWT con los permisos del usuario codificados como claims (`"permission": ["Areas.read", "Areas.create", ...]`). Todos los demás endpoints exigen ese token en el header `Authorization: Bearer <token>`, salvo el propio login.

- **Desde WindowsForms**: el token se maneja solo, no hay nada que hacer a mano. Se guarda en memoria mientras la app está abierta (nunca en disco).
- **Desde Blazor**: acá lo hicimos distinto, porque Blazor Server corre en el servidor y atiende a todos los usuarios en el mismo proceso, así que un token "global" lo compartirían todos. El login lo hace el servidor de Blazor contra `POST /auth/login`, y el token JWT queda **adentro de una cookie de sesión** (`HttpOnly`, encriptada), que el navegador no puede leer con JavaScript. En cada llamada a la WebAPI, Blazor saca el token de la cookie del usuario y lo manda como `Bearer`. Otras cosas que hace:
  - La cookie vence cuando vence el token, y sobrevive al F5 pero no a cerrar el navegador.
  - Si el token vence con la página abierta, a lo sumo en un minuto te manda al login.
  - Todas las páginas piden login por defecto, y cada pantalla y cada botón (Nuevo, Editar, Eliminar) se muestran según los permisos del usuario. Si alguien escribe la URL a mano sin permiso, ve "Acceso denegado".
  - El logout es un `POST` con token antiforgery, así que otro sitio no puede cerrarte la sesión.
  - Ocultar un botón es solo para que se vea prolijo: la seguridad de verdad la hace la WebAPI, que valida el token y el permiso en cada request.
- **Desde Swagger** (para probar la API directamente): hacé `Try it out` → `Execute` en `POST /auth/login`, copiá el valor de `"token"` de la respuesta, apretá el botón **Authorize** (arriba de todo, con un candado 🔒) y pegalo ahí (sin escribir "Bearer" adelante). De ahí en más, todos los "Try it out" que hagas van a mandar el token automáticamente.

## Qué pantallas tiene cada interfaz

Las dos interfaces (WindowsForms y Blazor) tienen las diez entidades: Ubicaciones, Áreas, Tipos de instrumento, Instrumentos, Historial de estados, Procedimientos, Patrones de referencia, Calibraciones, No conformidades y Usuarios. Cada una tiene su lista con alta, modificación y baja (el historial de estados es solo de consulta: se llena solo cuando cambia el estado de un instrumento). Los permisos son los mismos en las dos, según el usuario con el que entres.

`Calibration` es el maestro/detalle: una calibración tiene sus mediciones (`CalibrationMeasurement`), que se cargan en la misma pantalla y se guardan todas juntas. Si algo falla, no se guarda nada, y al borrar una calibración se borran también sus mediciones. Una calibración se aprueba aparte: la tiene que aprobar un administrador o un responsable, y solo un administrador puede aprobar una que hizo él mismo.

Algunas cosas que hace la versión web (Blazor) y que tuvimos que respetar:

- Al **crear** un instrumento el estado siempre es "Activo". Después se cambia desde el detalle del instrumento (link "Cambiar estado", junto al estado actual), y cada cambio queda en el historial, que también se ve en el detalle del instrumento junto con sus calibraciones.
- En la lista de instrumentos, tocar una fila abre el detalle (con las calibraciones y el historial de estados); cada fila solo tiene los botones Editar y Eliminar.
- El **error máximo** y la **frecuencia** del instrumento son opcionales: si quedan vacíos se usan los del tipo de instrumento.
- Las ubicaciones, áreas, tipos de instrumento, procedimientos y patrones **inactivos** no se ofrecen al cargar algo nuevo, pero se conservan en lo que ya estaba cargado.
- En una calibración, el resultado se elige después de cargar las mediciones y se sugiere según ellas (si alguna está fuera de tolerancia avisa). En las internas el procedimiento es obligatorio y los patrones vencidos o inactivos no se pueden elegir. Una calibración aprobada queda en solo lectura hasta que se le quita la aprobación.
- Un procedimiento puede tener un link a su PDF. En la web se ve como "Ver" en la lista de procedimientos y como "Ver procedimiento" al elegirlo en una calibración. En WindowsForms se carga en el formulario del procedimiento y se abre con el botón "Ver documento", que está en la lista de procedimientos y también en la pantalla de la calibración, al lado del procedimiento elegido.
- Si el servidor rechaza algo (por ejemplo, borrar una ubicación que tiene áreas, o repetir un código de instrumento), el mensaje se muestra arriba en un cuadro rojo.

Algunas reglas que valida la API (valen para las dos interfaces):

- No se puede usar un patrón de referencia con el certificado vencido: se compara con la fecha de la calibración, así las calibraciones viejas se pueden seguir editando.
- No se puede eliminar una calibración que tiene una no conformidad asociada (hay que eliminar primero la no conformidad).
- Un procedimiento puede repetir el código, pero no la combinación de código y versión. Una versión nueva se carga como otro procedimiento con el mismo código, y el anterior se deja inactivo.
- La contraseña de un usuario nuevo tiene que tener entre 8 y 30 caracteres.
- El código de una no conformidad lo genera la API (NC-0001, NC-0002...) si no se manda uno, y no puede repetirse. En las dos interfaces se asigna solo al guardar y después no se edita.

## Reportes

Hay dos reportes, en las dos interfaces, los dos con botón para **descargar el PDF**:

1. **Calibraciones próximas y vencidas**: los instrumentos con la calibración vencida o por vencer en los próximos 30, 60 o 90 días.
2. **Resultados de calibraciones**: cuántas calibraciones dieron conforme, conforme con restricciones y no conforme en un período, con un gráfico de barras.

Todo lo hace la WebAPI (los datos y el PDF, que se genera con QuestPDF, licencia Community), así las dos interfaces muestran lo mismo y cada una solo se ocupa de dibujarlo a su manera. Los endpoints son `GET /reports/upcoming-calibrations` y `GET /reports/calibration-results`, y cada uno tiene su variante `/pdf`. Los ven el administrador, el responsable y el auditor (permiso `Reports.read`); el operador no. En Blazor están en el menú **Reportes**, y en WindowsForms en el item **Reportes** de la pantalla principal.

## Datos de ejemplo

Para poder probar y mostrar la aplicación, cuando la base está vacía la WebAPI carga datos de ejemplo (`Data/DemoDataSeeder.cs`): 4 áreas, 4 procedimientos (uno con una versión vieja inactiva), 4 patrones de referencia (uno vencido), 12 instrumentos en distintos estados, 19 calibraciones con fechas relativas a hoy (así siempre hay alguna vencida y alguna por vencer), 4 no conformidades y el historial de estados. Solo se cargan si todavía no hay instrumentos, así que no se duplican al reiniciar. Para volver a empezar de cero hay que borrar la base.

## Pendiente conocido

- En `WindowsForms`, si el token vence con una lista abierta, se muestra un error genérico en vez de mandarte directo al login (la excepción de sesión vencida se la "traga" cada pantalla). Hay que hacer logout desde el menú y volver a entrar.
- Hay unos 20 warnings de nulabilidad (CS86xx) en el código de `WindowsForms` y los servicios. No rompen nada pero hay que limpiarlos.
- Los mensajes que vienen del dominio (por ejemplo "Cannot delete location...") están en inglés y se muestran así en las dos interfaces.
- El dominio mezcla español e inglés (nombres de clases y algunos mensajes de validación en inglés, `UserRole` y otros mensajes en español) — es una decisión de idioma pendiente de unificar.

## Mejoras futuras

Cosas que charlamos y no llegamos a hacer:

- Relacionar los patrones de referencia con los tipos de instrumento, para que en una calibración solo se ofrezcan los que corresponden (hoy se ofrecen todos los vigentes). Lo ideal es una relación de muchos a muchos, porque un mismo patrón puede servir para varios tipos; una versión más simple sería que cada patrón tenga un solo tipo, opcional.
- Que la API también exija el procedimiento en las calibraciones internas (hoy lo controla solo la web).
- Sacar el "tipo de intervención" de la calibración: el mantenimiento no encaja con las mediciones y el resultado.
- Que cada usuario pueda cambiar su propia contraseña.
- Que las reglas de autoprotección de usuarios (no borrarse ni cambiarse el rol) y el usuario que registra un cambio de estado salgan del token y no de un dato que manda el cliente.
