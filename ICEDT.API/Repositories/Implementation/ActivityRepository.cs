using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ICEDT.API.Models;
using ICEDT.API.Data;
using ICEDT.API.Repositories.Interfaces;

namespace ICEDT.API.Repositories.Implementation
{
    public class ActivityRepository : IActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityRepository(ApplicationDbContext context) => _context = context;

        public async Task<Activity> GetByIdAsync(int id) =>
            await _context.Activities.FindAsync(id);

        public async Task<List<Activity>> GetAllAsync() =>
            await _context.Activities
                .OrderBy(a => a.SequenceOrder)
                .ToListAsync();

        public async Task<List<Activity>> GetByLessonIdAsync(int lessonId) =>
            await _context.Activities
                .Where(a => a.LessonId == lessonId)
                .OrderBy(a => a.SequenceOrder)
                .ToListAsync();

        public async Task AddAsync(Activity activity)
        {
            _context.Activities.Add(activity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Activity activity)
        {
            _context.Activities.Update(activity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var activity = await _context.Activities.FindAsync(id);
            if (activity != null)
            {
                _context.Activities.Remove(activity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SequenceOrderExistsAsync(int sequenceOrder) =>
            await _context.Activities.AnyAsync(a => a.SequenceOrder == sequenceOrder);

        public async Task<bool> LessonExistsAsync(int lessonId) =>
            await _context.Lessons.AnyAsync(l => l.LessonId == lessonId);
    }
}