using ICEDT.API.Data;
using ICEDT.API.DTO.Request;
using ICEDT.API.DTO.Response;
using ICEDT.API.Middleware;
using ICEDT.API.Models;
using ICEDT.API.Repositories.Interfaces;
using ICEDT.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;



namespace ICEDT.API.Services.Implementation
{
 
    public class ActivityService : IActivityService
    {
        private readonly IActivityRepository _activityRepo;
        private readonly IActivityTypeRepository _typeRepo;
        private readonly ApplicationDbContext _context;

        public ActivityService(IActivityRepository activityRepo, IActivityTypeRepository typeRepo, ApplicationDbContext context)
        {
            _activityRepo = activityRepo;
            _typeRepo = typeRepo;
            _context = context;
        }

        // Activity CRUD
        public async Task<ActivityResponseDto> GetActivityAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Invalid Activity ID.");
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null) throw new NotFoundException("Activity not found.");
            return MapToActivityResponseDto(activity);
        }

        public async Task<List<ActivityResponseDto>> GetAllActivitiesAsync()
        {
            var activities = await _activityRepo.GetAllAsync();
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<List<ActivityResponseDto>> GetActivitiesByLessonIdAsync(int lessonId)
        {
            if (lessonId <= 0) throw new BadRequestException("Invalid Lesson ID.");
            var lessonExists = await _context.Lessons.AnyAsync(l => l.LessonId == lessonId);
            if (!lessonExists) throw new NotFoundException("Lesson not found.");
            var activities = await _activityRepo.GetByLessonIdAsync(lessonId);
            return activities.Select(MapToActivityResponseDto).ToList();
        }

        public async Task<ActivityResponseDto> AddActivityAsync(ActivityRequestDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new BadRequestException("Title is required.");
            if (dto.LessonId <= 0) throw new BadRequestException("Invalid Lesson ID.");
            if (dto.ActivityTypeId <= 0) throw new BadRequestException("Invalid Activity Type ID.");

            var lessonExists = await _context.Lessons.AnyAsync(l => l.LessonId == dto.LessonId);
            if (!lessonExists) throw new NotFoundException("Lesson not found.");
            var typeExists = await _context.ActivityTypes.AnyAsync(t => t.ActivityTypeId == dto.ActivityTypeId);
            if (!typeExists) throw new NotFoundException("Activity Type not found.");

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
            if (id <= 0) throw new BadRequestException("Invalid Activity ID.");
            if (dto.LessonId <= 0) throw new BadRequestException("Invalid Lesson ID.");
            if (dto.ActivityTypeId <= 0) throw new BadRequestException("Invalid Activity Type ID.");
            if (string.IsNullOrWhiteSpace(dto.Title)) throw new BadRequestException("Title is required.");

            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null) throw new NotFoundException("Activity not found.");

            var lessonExists = await _context.Lessons.AnyAsync(l => l.LessonId == dto.LessonId);
            if (!lessonExists) throw new NotFoundException("Lesson not found.");
            var typeExists = await _context.ActivityTypes.AnyAsync(t => t.ActivityTypeId == dto.ActivityTypeId);
            if (!typeExists) throw new NotFoundException("Activity Type not found.");

            activity.LessonId = dto.LessonId;
            activity.ActivityTypeId = dto.ActivityTypeId;
            activity.Title = dto.Title;
            activity.SequenceOrder = dto.SequenceOrder;
            activity.ContentJson = dto.ContentJson;
            await _activityRepo.UpdateAsync(activity);
        }

        public async Task DeleteActivityAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Invalid Activity ID.");
            var activity = await _activityRepo.GetByIdAsync(id);
            if (activity == null) throw new NotFoundException("Activity not found.");
            await _activityRepo.DeleteAsync(id);
        }

        // ActivityType CRUD
        public async Task<ActivityTypeResponseDto> GetActivityTypeAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Invalid ActivityType ID.");
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
            if (string.IsNullOrWhiteSpace(dto.ActivityName)) throw new BadRequestException("ActivityType name is required.");
            var type = new ActivityType { Name = dto.ActivityName };
            await _typeRepo.AddAsync(type);
            return MapToActivityTypeResponseDto(type);
        }

        public async Task UpdateActivityTypeAsync(int id, ActivityTypeRequestDto dto)
        {
            if (id <= 0) throw new BadRequestException("Invalid ActivityType ID.");
            if (string.IsNullOrWhiteSpace(dto.ActivityName)) throw new BadRequestException("ActivityType name is required.");
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null) throw new NotFoundException("ActivityType not found.");
            type.Name = dto.ActivityName;
            await _typeRepo.UpdateAsync(type);
        }

        public async Task DeleteActivityTypeAsync(int id)
        {
            if (id <= 0) throw new BadRequestException("Invalid ActivityType ID.");
            var type = await _typeRepo.GetByIdAsync(id);
            if (type == null) throw new NotFoundException("ActivityType not found.");
            var hasActivities = await _context.Activities.AnyAsync(a => a.ActivityTypeId == id);
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
                ActivityName = type.Name
            };
        }
    }
}