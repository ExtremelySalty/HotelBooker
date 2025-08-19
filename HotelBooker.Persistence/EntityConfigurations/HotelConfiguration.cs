using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooker.Persistence.EntityConfigurations
{
    internal class HotelConfiguration
        : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder
                .ToTable("Hotel")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Code)
                .HasMaxLength(20)
                .IsRequired();

            builder
                .HasIndex(x => x.Code)
                .IsUnique();

            builder
                .HasMany(x => x.Rooms)
                .WithOne(x => x.Hotel)
                .HasForeignKey(x => x.HotelId);
        }
    }
}
