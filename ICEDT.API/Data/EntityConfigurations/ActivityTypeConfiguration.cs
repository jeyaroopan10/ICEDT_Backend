

namespace ICEDT.API.Data.EntityConfigurations
{
    public class ActivityTypeConfiguration : IEntityTypeConfiguration<ActivityType>
    {
        public void Configure(EntityTypeBuilder<ActivityType> builder)
        {
            builder.HasKey(at => at.ActivityTypeId);
            builder.Property(at => at.ActivityName).IsRequired();

            builder.HasMany(at => at.Activities)
                   .WithOne(a => a.ActivityType)
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
} 