using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class UserRepository : IUserRepository
    {
        private readonly CalibrArContext context;

        public UserRepository(CalibrArContext context)
        {
            this.context = context;
        }

        //public UserRepository()
        //{
        //}

        public async Task AddAsync(User user)
        {
            context.Users.Add(user);
            await context.SaveChangesAsync();
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                context.Users.Remove(user);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<User?> GetAsync(int id)
        {
            return await context.Users.Include(u => u.Group).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await context.Users
                .Include(u => u.Group)
                .ThenInclude(g => g!.Permissions)
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<IEnumerable<User>> GetAllAsync()
        {
            return await context.Users.Include(u => u.Group).OrderBy(u => u.FullName).ToListAsync();
        }

        public async Task<bool> UpdateAsync(User user)
        {
            var existing = await context.Users.FindAsync(user.Id);
            if (existing != null)
            {
                existing.SetFullName(user.FullName);
                existing.SetEmail(user.Email);
                existing.SetRole(user.Role);
                existing.SetIsActive(user.IsActive);
                existing.SetGroup(user.Group);
                await context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> ExistsByEmailAsync(string email, int? excludeId = null)
        {
            return await context.Users.AnyAsync(u => u.Email == email && (excludeId == null || u.Id != excludeId));
        }

        public async Task<int> CountOtherActiveAdministratorsAsync(int userId)
        {
            return await context.Users.CountAsync(u => u.Role == UserRole.Administrador && u.IsActive && u.Id != userId);
        }

        // Usuario referenciado por calibraciones, no conformidades o historial de estados
        public async Task<bool> IsReferencedAsync(int id)
        {
            return await context.Calibrations.AnyAsync(c => c.PerformedByUserId == id || c.ApprovedByUserId == id)
                || await context.NonConformities.AnyAsync(n => n.DetectedByUserId == id || n.ClosedByUserId == id)
                || await context.InstrumentStatusHistories.AnyAsync(h => h.ChangedByUserId == id);
        }

        public async Task<PermissionGroup?> GetGroupByNameAsync(string name)
        {
            return await context.PermissionGroups.FirstOrDefaultAsync(g => g.Name == name);
        }

        public async Task UpdateLastLoginAsync(int userId, DateTime lastLoginAt)
        {
            var existing = await context.Users.FindAsync(userId);
            if (existing != null)
            {
                existing.SetLastLoginAt(lastLoginAt);
                await context.SaveChangesAsync();
            }
        }
    }
}