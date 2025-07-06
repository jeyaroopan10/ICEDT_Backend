

using ICEDT.API.Data;
using ICEDT.API.Models;
using Microsoft.EntityFrameworkCore;

namespace ICEDT.API.Repositories.Interfaces

{
    public class LevelRepository : ILevelRepository
    {
        private readonly ApplicationDbContext _context;

        public LevelRepository(ApplicationDbContext context) => _context = context;

        public async Task<Level> GetByIdAsync(int id) =>
            await _context.Levels.FindAsync(id);

        public async Task<List<Level>> GetAllAsync() =>
            await _context.Levels.ToListAsync();

        public async Task AddAsync(Level level)
        {
            _context.Levels.Add(level);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Level level)
        {
            _context.Levels.Update(level);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var level = await _context.Levels.FindAsync(id);
            if (level != null)
            {
                _context.Levels.Remove(level);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Level> GetByIdWithLessonsAsync(int id)
        {
            return await _context.Levels
                .Include(l => l.Lessons)
                .FirstOrDefaultAsync(l => l.LevelId == id);
        }

        public async Task<List<Level>> GetAllWithLessonsAsync()
        {
            return await _context.Levels
                .Include(l => l.Lessons)
                .ToListAsync();
        }
    }
}