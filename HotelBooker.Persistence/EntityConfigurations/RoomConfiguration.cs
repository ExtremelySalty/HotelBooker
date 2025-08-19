using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooker.Persistence.EntityConfigurations
{
    internal class RoomConfiguration
        : IEntityTypeConfiguration<Room>
    {
        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder
                .ToTable("Room")
                .HasKey(x => x.Id);


            builder
                .HasOne(x => x.Hotel)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.HotelId);

            builder
                .HasOne(x => x.RoomType)
                .WithMany(x => x.Rooms)
                .HasForeignKey(x => x.RoomTypeId);

            builder
                .HasMany(x => x.Bookings)
                .WithOne(x => x.Room)
                .HasForeignKey(x => x.RoomId);
        }
    }
}
