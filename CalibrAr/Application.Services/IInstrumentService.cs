using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface IInstrumentService
    {
        Task<InstrumentDTO> AddAsync(InstrumentDTO dto);
        Task<bool> DeleteAsync(int id);
        Task<InstrumentDTO?> GetAsync(int id);
        Task<IEnumerable<InstrumentDTO>> GetAllAsync();
        Task<bool> UpdateAsync(InstrumentDTO dto);
        Task RecalculateCalibrationScheduleAsync(int instrumentId, DateTime? lastCalibrationDate);
        Task RecalculateScheduleForTypeAsync(int instrumentTypeId);
    }
}