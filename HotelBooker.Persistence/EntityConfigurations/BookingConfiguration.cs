using HotelBooker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelBooker.Persistence.EntityConfigurations
{
    internal class BookingConfiguration
        : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder
                .ToTable("Booking")
                .HasKey(x => x.Id);

            builder
                .Property(x => x.ReferenceNumber)
                .HasMaxLength(128)
                .IsRequired();

            builder
                .HasIndex(x => x.ReferenceNumber)
                .IsUnique();

            builder
                .HasOne(x => x.Room)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.RoomId);

            builder
                .HasOne(x => x.Customer)
                .WithMany(x => x.Bookings)
                .HasForeignKey(x => x.CustomerId);
        }
    }
}
