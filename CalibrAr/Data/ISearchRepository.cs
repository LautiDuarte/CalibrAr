using Domain.Model;

namespace Data
{
    // Búsquedas con filtros combinables resueltas en la base con ADO.NET.
    // Devuelven solo los ids, ya ordenados; las entidades las carga el repositorio de cada una.
    public interface ISearchRepository
    {
        Task<List<int>> SearchInstrumentIdsAsync(string? text, InstrumentStatus? status, int? areaId, bool hideDecommissioned);

        Task<List<int>> SearchCalibrationIdsAsync(string? text, DateTime? from, DateTime? to, Result? result,
            InterventionType? interventionType, bool? approved, int? instrumentId);
    }
}
