using ICEDT.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT.API.Data.EntityConfigurations
{
    public class ActivityTypeConfiguration : IEntityTypeConfiguration<ActivityType>
    {
        public void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            builder.HasKey(t => t.ActivityTypeId);
            builder.Property(t => t.Name).IsRequired().HasMaxLength(50);
            builder.Property(t => t.MainActivityTypeId).IsRequired();

            builder.HasOne(t => t.MainActivityType)
                   .WithMany(m => m.ActivityTypes)
                   .HasForeignKey(t => t.MainActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.Activities)
                   .WithOne(a => a.ActivityType)
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}