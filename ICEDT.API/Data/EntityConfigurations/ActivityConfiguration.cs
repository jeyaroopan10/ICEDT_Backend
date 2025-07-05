

namespace ICEDT.API.Data.EntityConfigurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.ActivityId);
            builder.Property(a => a.Title).IsRequired();
            builder.Property(a => a.SequenceOrder).IsRequired();
            builder.Property(a => a.ContentJson).IsRequired();

            builder.HasOne(a => a.Lesson)
                   .WithMany(ls => ls.Activities)
                   .HasForeignKey(a => a.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.ActivityType)
                   .WithMany(at => at.Activities)
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 