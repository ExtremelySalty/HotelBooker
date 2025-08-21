using HotelBooker.CommonTests;
using HotelBooker.Domain.Models;
using Shouldly;

namespace HotelBooker.Domain.Tests.Models
{
    [TestFixture]
    [Category(TestingCategories.Unit)]
    public class RoomTests
    {
        [Test]
        public void IsAvailable_WhenRoomHasNoBookings_ShouldReturnTrue()
        {
            // Arrange
            var room = new Room();
            var startDate = new DateTime(2025, 10, 1);
            var endDate = new DateTime(2025, 10, 10);

            // Act
            var isAvailable = room.IsAvailable(startDate, endDate);

            // Assert
            isAvailable.ShouldBeTrue();
        }

        [Test]
        public void IsAvailable_WhenRoomHasSpaceForBooking_ShouldReturnTrue()
        {
            // Arrange
            var room = new Room();
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 9, 1),
                EndDateUtc = new DateTime(2025, 9, 10),
            });
            var requestedStartDate = new DateTime(2025, 10, 1);
            var requestedEndDate = new DateTime(2025, 10, 10);

            // Act
            var isAvailable = room.IsAvailable(requestedStartDate, requestedEndDate);

            // Assert
            isAvailable.ShouldBeTrue();
        }

        [Test]
        public void IsAvailable_WhenRoomHasBookingThatOverlaps_ShouldReturnFalse()
        {
            // Arrange
            var room = new Room();
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 9, 5),
                EndDateUtc = new DateTime(2025, 9, 15),
            });

            var requestedStartDate = new DateTime(2025, 9, 10);
            var requestedEndDate = new DateTime(2025, 9, 20);

            // Act
            var isAvailable = room.IsAvailable(requestedStartDate, requestedEndDate);

            // Assert
            isAvailable.ShouldBeFalse();
        }

        [Test]
        public void IsAvailable_WhenRoomHasBookingThatStartsBeforeAndEndsAfter_ShouldReturnFalse()
        {
            // Arrange
            var room = new Room();
            room.Bookings.Add(new Booking
            {
                StartDateUtc = new DateTime(2025, 9, 1),
                EndDateUtc = new DateTime(2025, 9, 30),
            });

            var requestedStartDate = new DateTime(2025, 9, 15);
            var requestedEndDate = new DateTime(2025, 9, 20);

            // Act
            var isAvailable = room.IsAvailable(requestedStartDate, requestedEndDate);

            // Assert
            isAvailable.ShouldBeFalse();
        }
    }
}
