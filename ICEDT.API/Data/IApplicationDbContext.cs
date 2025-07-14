using ICEDT.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace ICEDT.API.Data
{
    public interface IApplicationDbContext
    {
        DbSet<Level> Levels { get; set; }
        DbSet<Lesson> Lessons { get; set; }
        DbSet<Activity> Activities { get; set; }
        DbSet<ActivityType> ActivityTypes { get; set; }
        DbSet<MainActivityType> MainActivityTypes { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
} 