using System.Collections.Generic;
using System.Threading.Tasks;
using ICEDT.API.Models;

namespace ICEDT.API.Repositories.Interfaces
{
    public interface IActivityTypeRepository
    {
        Task<ActivityType> GetByIdAsync(int id);
        Task<List<ActivityType>> GetAllAsync();
        Task AddAsync(ActivityType activityType);
        Task UpdateAsync(ActivityType activityType);
        Task DeleteAsync(int id);
        Task<bool> ActivityTypeExistsAsync(int activityTypeId);
    }
}