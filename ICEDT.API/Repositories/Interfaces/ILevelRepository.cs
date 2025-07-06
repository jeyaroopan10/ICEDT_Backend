

using ICEDT.API.Models;

namespace ICEDT.API.Repositories.Interfaces
{
    public interface ILevelRepository
    {
        Task<Level> GetByIdAsync(int id);
        Task<List<Level>> GetAllAsync();
        Task AddAsync(Level level);
        Task UpdateAsync(Level level);
        Task DeleteAsync(int id);
        Task<Level> GetByIdWithLessonsAsync(int id);
        Task<List<Level>> GetAllWithLessonsAsync();
    }
}