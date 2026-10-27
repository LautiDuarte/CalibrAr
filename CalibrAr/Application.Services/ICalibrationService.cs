using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTOs;

namespace Application.Services
{
    public interface ICalibrationService
    {
        Task<CalibrationDTO> AddAsync(CalibrationDTO dto, int userId);
        Task<bool> DeleteAsync(int id);
        Task<CalibrationDTO?> GetAsync(int id);
        Task<IEnumerable<CalibrationDTO>> GetAllAsync();
        Task<IEnumerable<CalibrationDTO>> SearchAsync(DateTime? from, DateTime? to, string? result, int? instrumentId,
            string? text, string? interventionType, bool? approved);
        Task<bool> UpdateAsync(CalibrationDTO dto, int userId);
        Task<bool> ApproveAsync(int id, int approverUserId);
    }
}