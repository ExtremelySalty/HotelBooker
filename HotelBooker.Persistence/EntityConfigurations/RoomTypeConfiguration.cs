using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooker.Persistence.EntityConfigurations
{
    internal class RoomTypeConfiguration
        : IEntityTypeConfiguration<RoomType>
    {
        public void Configure(EntityTypeBuilder<RoomType> builder)
        {
            builder
                .ToTable("RoomType")
                .HasKey(x => x.Id);

            builder
                .HasMany(x => x.Rooms)
                .WithOne(x => x.RoomType)
                .HasForeignKey(x => x.RoomTypeId);
        }
    }
}
