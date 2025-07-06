using ICEDT.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT.API.Data.EntityConfigurations
{
    public class ActivityConfiguration : IEntityTypeConfiguration<Activity>
    {
        public void Configure(EntityTypeBuilder<Activity> builder)
        {
            builder.HasKey(a => a.ActivityId);
            builder.Property(a => a.LessonId).IsRequired();
            builder.Property(a => a.ActivityTypeId).IsRequired();
            builder.Property(a => a.Title).IsRequired().HasMaxLength(100);
            builder.Property(a => a.SequenceOrder).IsRequired();
            builder.Property(a => a.ContentJson).IsRequired();

            builder.HasOne(a => a.Lesson)
                   .WithMany(l => l.Activities)
                   .HasForeignKey(a => a.LessonId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.ActivityType)
                   .WithMany(t => t.Activities)
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.SequenceOrder).IsUnique();
        }
    }
}