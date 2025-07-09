using ICEDT.API.Models;
using ICEDT.API.Middleware;
using ICEDT.API.Repositories.Interfaces;
using ICEDT.API.Services.Interfaces;
using ICEDT.API.DTO.Request;
using ICEDT.API.DTO.Response;

namespace ICEDT.API.Services.Implementation
{
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IActivityTypeRepository _typeRepo;

        public ActivityService(IActivityRepository activityRepo, IActivityTypeRepository typeRepo)
        {
            _activityRepo = activityRepo;
            _typeRepo = typeRepo;
        }

        // Activity CRUD
        public async Task<ActivityResponseDto> GetActivityAsync(int id)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null) throw new NotFoundException("Activity not found.");
            return MapToActivityResponseDto(activity);
        }

        public async Task<List<ActivityResponseDto>> GetAllActivitiesAsync()
        {
            var activities = await _activityRepo.GetAllAsync();
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<List<ActivityResponseDto>> GetActivitiesByLessonIdAsync(int lessonId, int? activitytypeid, int? mainactivitytypeid)
        {
            var lessonExists = await _activityRepo.LessonExistsAsync(lessonId);
            if (!lessonExists) throw new NotFoundException("Lesson not found.");
            var activities = await _activityRepo.GetByLessonIdAsync(lessonId, activitytypeid, mainactivitytypeid);
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<ActivityResponseDto> AddActivityAsync(ActivityRequestDto dto)
        {
            var lessonExists = await _activityRepo.LessonExistsAsync(dto.LessonId);
            if (!lessonExists) throw new NotFoundException("Lesson not found.");
            var typeExists = await _typeRepo.ActivityTypeExistsAsync(dto.ActivityTypeId);
            if (!typeExists) throw new NotFoundException("Activity Type not found.");

            
            if (await _activityRepo.SequenceOrderExistsAsync(dto.SequenceOrder))
                throw new BadRequestException($"Sequence order {dto.SequenceOrder} is already in use.");

            /* 
            var existingActivities = await _activityRepo.GetAllAsync();
            if (existingActivities.Any(a => a.SequenceOrder == dto.SequenceOrder))
            {
                foreach (var activity in existingActivities.Where(a => a.SequenceOrder >= dto.SequenceOrder))
                {
                    activity.SequenceOrder++;
                    await _activityRepo.UpdateAsync(activity);
                }
            }
            */

            var activity = new Activity
            {
                LessonId = dto.LessonId,
                ActivityTypeId = dto.ActivityTypeId,
                Title = dto.Title,
                SequenceOrder = dto.SequenceOrder,
                ContentJson = dto.ContentJson
            };
            await _activityRepo.AddAsync(activity);
            return MapToActivityResponseDto(activity);
        }

        public async Task UpdateActivityAsync(int id, ActivityRequestDto dto)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null) throw new NotFoundException("Activity not found.");

            var lessonExists = await _activityRepo.LessonExistsAsync(dto.LessonId);
            if (!lessonExists) throw new NotFoundException("Lesson not found.");
            var typeExists = await _typeRepo.ActivityTypeExistsAsync(dto.ActivityTypeId);
            if (!typeExists) throw new NotFoundException("Activity Type not found.");

     
            if (activity.SequenceOrder != dto.SequenceOrder)
            {
                if (await _activityRepo.SequenceOrderExistsAsync(dto.SequenceOrder))
                    throw new BadRequestException($"Sequence order {dto.SequenceOrder} is already in use.");
            }

            /*
            if (activity.SequenceOrder != dto.SequenceOrder)
            {
                var existingActivities = await _activityRepo.GetAllAsync();
                if (existingActivities.Any(a => a.SequenceOrder == dto.SequenceOrder && a.ActivityId != id))
                {
                    foreach (var existingActivity in existingActivities.Where(a => a.SequenceOrder >= dto.SequenceOrder && a.ActivityId != id))
                    {
                        existingActivity.SequenceOrder++;
                        await _activityRepo.UpdateAsync(existingActivity);
                    }
                }
            }
            */

            activity.LessonId = dto.LessonId;
            activity.ActivityTypeId = dto.ActivityTypeId;
            activity.Title = dto.Title;
            activity.SequenceOrder = dto.SequenceOrder;
            activity.ContentJson = dto.ContentJson;
            await _activityRepo.UpdateAsync(activity);
        }

        public async Task DeleteActivityAsync(int id)
        {
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null) throw new NotFoundException("Activity not found.");
            await _activityRepo.DeleteAsync(id);
        }

        // ActivityType CRUD
        public async Task<ActivityTypeResponseDto> GetActivityTypeAsync(int id)
        {
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null) throw new NotFoundException("ActivityType not found.");
            return MapToActivityTypeResponseDto(type);
        }

        public async Task<List<ActivityTypeResponseDto>> GetAllActivityTypesAsync()
        {
            var types = await _typeRepo.GetAllAsync();
            return types.Select(MapToActivityTypeResponseDto).ToList();
        }

        public async Task<ActivityTypeResponseDto> AddActivityTypeAsync(ActivityTypeRequestDto dto)
        {
            var type = new ActivityType { Name = dto.ActivityName };
            await _typeRepo.AddAsync(type);
            return MapToActivityTypeResponseDto(type);
        }

        public async Task UpdateActivityTypeAsync(int id, ActivityTypeRequestDto dto)
        {
          
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null) throw new NotFoundException("ActivityType not found.");
            type.Name = dto.ActivityName;
            await _typeRepo.UpdateAsync(type);
        }

        public async Task DeleteActivityTypeAsync(int id)
        {
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null) throw new NotFoundException("ActivityType not found.");
            var hasActivities = await _activityRepo.GetAllAsync().ContinueWith(t => t.Result.Any(a => a.ActivityTypeId == id));
            if (hasActivities) throw new BadRequestException("Cannot delete ActivityType with associated Activities.");
            await _typeRepo.DeleteAsync(id);
        }

        // Mapping helpers
        private ActivityResponseDto MapToActivityResponseDto(Activity activity)
        {
          
            return new ActivityResponseDto
            {
                ActivityId = activity.ActivityId,
                LessonId = activity.LessonId,
                ActivityTypeId = activity.ActivityTypeId,
                MainActivityTypeId = activity.ActivityType != null ? activity.ActivityType.MainActivityTypeId : 0,
                Title = activity.Title,
                SequenceOrder = activity.SequenceOrder,
                ContentJson = activity.ContentJson
            };
        }

        private ActivityTypeResponseDto MapToActivityTypeResponseDto(ActivityType type)
        {
            return new ActivityTypeResponseDto
            {
                ActivityTypeId = type.ActivityTypeId,
                ActivityName = type.Name,
                MainActivityTypeId = type.MainActivityTypeId
            };
        }
    }
}