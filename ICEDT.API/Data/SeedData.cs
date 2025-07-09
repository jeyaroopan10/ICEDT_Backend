using ICEDT.API.Data;
using ICEDT.API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace ICEDT.API.Data
{
    public static class SeedData
    {
        public static async Task Initialize(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            // Ensure database is created
            await context.Database.EnsureCreatedAsync();

            // Seed MainActivityTypes
            if (!await context.MainActivityTypes.AnyAsync())
            {
                var mainActivityTypes = new[]
                {
                    new MainActivityType { Name = "Primary Type" },
                    new MainActivityType { Name = "Secondary Type" },
                    new MainActivityType { Name = "Interactive Type" }
                };
                context.MainActivityTypes.AddRange(mainActivityTypes);
                await context.SaveChangesAsync();
            }

            // Seed ActivityTypes
            if (!await context.ActivityTypes.AnyAsync())
            {
                var mainActivityTypes = await context.MainActivityTypes.ToListAsync();
                var activityTypes = new[]
                {
                    new ActivityType
                    {
                        Name = "Quiz",
                        MainActivityTypeId = mainActivityTypes.First(m => m.Name == "Primary Type").MainActivityTypeId
                    },
                    new ActivityType
                    {
                        Name = "Video",
                        MainActivityTypeId = mainActivityTypes.First(m => m.Name == "Primary Type").MainActivityTypeId
                    },
                    new ActivityType
                    {
                        Name = "Assignment",
                        MainActivityTypeId = mainActivityTypes.First(m => m.Name == "Secondary Type").MainActivityTypeId
                    },
                    new ActivityType
                    {
                        Name = "Discussion",
                        MainActivityTypeId = mainActivityTypes.First(m => m.Name == "Interactive Type").MainActivityTypeId
                    }
                };
                context.ActivityTypes.AddRange(activityTypes);
                await context.SaveChangesAsync();
            }

            // Seed Levels
            if (!await context.Levels.AnyAsync())
            {
                var levels = new[]
                {
                    new Level { LevelName = "Level 1", SequenceOrder = 1 },
                    new Level { LevelName = "Level 2", SequenceOrder = 2 }
                };
                context.Levels.AddRange(levels);
                await context.SaveChangesAsync();
            }

            // Seed Lessons
            if (!await context.Lessons.AnyAsync())
            {
                var levels = await context.Levels.ToListAsync();
                var lessons = new[]
                {
                    new Lesson { LessonName = "Introduction to Algebra", Description = "Basic Algebra Concepts", SequenceOrder = 1, LevelId = levels.First(l => l.LevelName == "Level 1").LevelId },
                    new Lesson { LessonName = "Basic Geometry", Description = "Fundamentals of Geometry", SequenceOrder = 2, LevelId = levels.First(l => l.LevelName == "Level 1").LevelId },
                    new Lesson { LessonName = "Interactive Coding", Description = "Hands-on Coding Activities", SequenceOrder = 1, LevelId = levels.First(l => l.LevelName == "Level 2").LevelId }
                };
                context.Lessons.AddRange(lessons);
                await context.SaveChangesAsync();
            }

            // Seed Activities
            if (!await context.Activities.AnyAsync())
            {
                var activityTypes = await context.ActivityTypes.ToListAsync();
                var lessons = await context.Lessons.ToListAsync();
                var activities = new[]
                {
                    new Activity
                    {
                        LessonId = lessons.First(l => l.LessonName == "Introduction to Algebra").LessonId,
                        ActivityTypeId = activityTypes.First(t => t.Name == "Quiz").ActivityTypeId,
                        Title = "Algebra Quiz 1",
                        SequenceOrder = 1,
                        ContentJson = "{\"type\": \"quiz\", \"questions\": []}"
                    },
                    new Activity
                    {
                        LessonId = lessons.First(l => l.LessonName == "Introduction to Algebra").LessonId,
                        ActivityTypeId = activityTypes.First(t => t.Name == "Video").ActivityTypeId,
                        Title = "Algebra Video Tutorial",
                        SequenceOrder = 2,
                        ContentJson = "{\"type\": \"video\", \"url\": \"https://example.com/video1\"}"
                    },
                    new Activity
                    {
                        LessonId = lessons.First(l => l.LessonName == "Basic Geometry").LessonId,
                        ActivityTypeId = activityTypes.First(t => t.Name == "Assignment").ActivityTypeId,
                        Title = "Geometry Assignment",
                        SequenceOrder = 1,
                        ContentJson = "{\"type\": \"assignment\", \"tasks\": []}"
                    },
                    new Activity
                    {
                        LessonId = lessons.First(l => l.LessonName == "Interactive Coding").LessonId,
                        ActivityTypeId = activityTypes.First(t => t.Name == "Discussion").ActivityTypeId,
                        Title = "Coding Discussion",
                        SequenceOrder = 1,
                        ContentJson = "{\"type\": \"discussion\", \"topic\": \"coding basics\"}"
                    }
                };
                context.Activities.AddRange(activities);
                await context.SaveChangesAsync();
            }

            // Seed Users
            if (!await userManager.Users.AnyAsync())
            {
                var user = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    FirstName = "Admin",
                    LastName = "User"
                };

                var result = await userManager.CreateAsync(user, "Password123");
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException($"Failed to create user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
    }
}