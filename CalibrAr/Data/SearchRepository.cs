using System.Data;
using Domain.Model;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Data
{
    // Acceso a datos con ADO.NET (SqlConnection/SqlCommand), sin Entity Framework.
    // El WHERE se arma solo con los filtros que llegaron y cada valor viaja como SqlParameter,
    // nunca concatenado en el texto del SQL.
    public class SearchRepository : ISearchRepository
    {
        private readonly string connectionString;

        public SearchRepository(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Falta configurar ConnectionStrings:DefaultConnection.");
        }

        public async Task<List<int>> SearchInstrumentIdsAsync(string? text, InstrumentStatus? status, int? areaId, bool hideDecommissioned)
        {
            using var command = new SqlCommand();
            var conditions = new List<string>();

            if (!string.IsNullOrWhiteSpace(text))
            {
                conditions.Add("(i.Code LIKE @text ESCAPE '!' OR i.Name LIKE @text ESCAPE '!' OR i.Brand LIKE @text ESCAPE '!' OR i.Model LIKE @text ESCAPE '!')");
                command.Parameters.Add(new SqlParameter("@text", SqlDbType.NVarChar, 200) { Value = LikePattern(text) });
            }

            if (status.HasValue)
            {
                conditions.Add("i.Status = @status");
                command.Parameters.Add(new SqlParameter("@status", SqlDbType.Int) { Value = (int)status.Value });
            }

            if (areaId.HasValue)
            {
                conditions.Add("i.AreaId = @areaId");
                command.Parameters.Add(new SqlParameter("@areaId", SqlDbType.Int) { Value = areaId.Value });
            }

            if (hideDecommissioned)
            {
                conditions.Add("i.Status <> @decommissioned");
                command.Parameters.Add(new SqlParameter("@decommissioned", SqlDbType.Int) { Value = (int)InstrumentStatus.Decommissioned });
            }

            command.CommandText = "SELECT i.Id FROM Instruments i" + WhereClause(conditions) + " ORDER BY i.Name, i.Id";
            return await ReadIdsAsync(command);
        }

        public async Task<List<int>> SearchCalibrationIdsAsync(string? text, DateTime? from, DateTime? to, Result? result,
            InterventionType? interventionType, bool? approved, int? instrumentId)
        {
            using var command = new SqlCommand();
            var conditions = new List<string>();

            if (!string.IsNullOrWhiteSpace(text))
            {
                conditions.Add("(i.Code LIKE @text ESCAPE '!' OR c.CertificateNumber LIKE @text ESCAPE '!' OR c.ExternalLab LIKE @text ESCAPE '!')");
                command.Parameters.Add(new SqlParameter("@text", SqlDbType.NVarChar, 200) { Value = LikePattern(text) });
            }

            if (from.HasValue)
            {
                conditions.Add("c.CalibrationDate >= @from");
                command.Parameters.Add(new SqlParameter("@from", SqlDbType.DateTime2) { Value = from.Value.Date });
            }

            if (to.HasValue)
            {
                // "hasta" incluye todo ese día
                conditions.Add("c.CalibrationDate < @toExclusive");
                command.Parameters.Add(new SqlParameter("@toExclusive", SqlDbType.DateTime2) { Value = to.Value.Date.AddDays(1) });
            }

            if (result.HasValue)
            {
                conditions.Add("c.Result = @result");
                command.Parameters.Add(new SqlParameter("@result", SqlDbType.Int) { Value = (int)result.Value });
            }

            if (interventionType.HasValue)
            {
                conditions.Add("c.InterventionType = @interventionType");
                command.Parameters.Add(new SqlParameter("@interventionType", SqlDbType.Int) { Value = (int)interventionType.Value });
            }

            if (approved.HasValue)
            {
                conditions.Add(approved.Value ? "c.ApprovedByUserId IS NOT NULL" : "c.ApprovedByUserId IS NULL");
            }

            if (instrumentId.HasValue)
            {
                conditions.Add("c.InstrumentId = @instrumentId");
                command.Parameters.Add(new SqlParameter("@instrumentId", SqlDbType.Int) { Value = instrumentId.Value });
            }

            command.CommandText = "SELECT c.Id FROM Calibrations c INNER JOIN Instruments i ON i.Id = c.InstrumentId"
                + WhereClause(conditions) + " ORDER BY c.CalibrationDate DESC, c.Id DESC";
            return await ReadIdsAsync(command);
        }

        private static string WhereClause(List<string> conditions) =>
            conditions.Count == 0 ? string.Empty : " WHERE " + string.Join(" AND ", conditions);

        // Los comodines de LIKE (% _ [) que escriba el usuario se buscan como texto común,
        // anteponiendo el carácter de escape "!" que se declara con ESCAPE '!' en el SQL
        private static string LikePattern(string text)
        {
            var escaped = text.Trim()
                .Replace("!", "!!")
                .Replace("%", "!%")
                .Replace("_", "!_")
                .Replace("[", "![");
            return $"%{escaped}%";
        }

        private async Task<List<int>> ReadIdsAsync(SqlCommand command)
        {
            var ids = new List<int>();

            using var connection = new SqlConnection(connectionString);
            command.Connection = connection;
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                ids.Add(reader.GetInt32(0));
            }

            return ids;
        }
    }
}
