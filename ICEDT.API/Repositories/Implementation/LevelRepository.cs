using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ICEDT.API.Models;
using ICEDT.API.Data;
using ICEDT.API.Repositories.Interfaces;

namespace ICEDT.API.Repositories.Implementation
{
    public class LevelRepository : ILevelRepository
    {
        private readonly IApplicationDbContext _context;

        public LevelRepository(IApplicationDbContext context) => _context = context;

        public async Task<Level> GetByIdAsync(int id) =>
            await _context.Levels
                .Include(l => l.Lessons)
                .FirstOrDefaultAsync(l => l.LevelId == id);

        public async Task<List<Level>> GetAllAsync() =>
            await _context.Levels
                .Include(l => l.Lessons)
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();

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
                .Where(l => l.LevelId == id)
                .OrderBy(l => l.SequenceOrder)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Level>> GetAllWithLessonsAsync()
        {
            var levels = await _context.Levels
                .Include(l => l.Lessons)
                .OrderBy(l => l.SequenceOrder)
                .ToListAsync();
            // Sort child lessons in-memory
            foreach (var level in levels)
            {
                if (level.Lessons != null)
                    level.Lessons = level.Lessons.OrderBy(ls => ls.SequenceOrder).ToList();
            }
            return levels;
        }

        public async Task<bool> SequenceOrderExistsAsync(int sequenceOrder) =>
            await _context.Levels.AnyAsync(l => l.SequenceOrder == sequenceOrder);
    }
}