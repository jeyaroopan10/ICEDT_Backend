using ICEDT.API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ICEDT.API.Data.EntityConfigurations
{
    public class MainActivityTypeConfiguration : IEntityTypeConfiguration<MainActivityType>
    {
        public void Configure(EntityTypeBuilder<MainActivityType> builder)
        {
            builder.HasKey(m => m.MainActivityTypeId);
            builder.Property(m => m.Name).IsRequired().HasMaxLength(50);

            builder.HasMany(m => m.ActivityTypes)
                   .WithOne(t => t.MainActivityType)
                   .HasForeignKey(t => t.MainActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Activities linked via ActivityType (no direct foreign key)
            builder.HasMany(m => m.Activities)
                   .WithOne()
                   .HasForeignKey(a => a.ActivityTypeId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}