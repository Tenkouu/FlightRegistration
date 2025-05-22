using FlightRegistration.Core.Models; // To access your Flight, Passenger, etc. models
using Microsoft.EntityFrameworkCore;
using System; // For DateTime

namespace FlightRegistration.Services.DataAccess
{
    public class AppDbContext : DbContext
    {
        // DbSet properties represent tables in your database
        public DbSet<Flight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // --- Entity Configurations ---

            // Passenger: Ensure PassportNumber is unique
            modelBuilder.Entity<Passenger>()
                .HasIndex(p => p.PassportNumber)
                .IsUnique();

            // Flight to Bookings (One-to-Many)
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Bookings)       // Flight has many Bookings
                .WithOne(b => b.Flight)         // Each Booking belongs to one Flight
                .HasForeignKey(b => b.FlightId) // Foreign key is Booking.FlightId
                .OnDelete(DeleteBehavior.Cascade); // If a Flight is deleted, its Bookings are also deleted.

            // Flight to Seats (One-to-Many)
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Seats)          // Flight has many Seats
                .WithOne(s => s.Flight)         // Each Seat belongs to one Flight
                .HasForeignKey(s => s.FlightId) // Foreign key is Seat.FlightId
                .OnDelete(DeleteBehavior.Cascade); // If a Flight is deleted, its Seats are also deleted.

            // Passenger to Bookings (One-to-Many)
            modelBuilder.Entity<Passenger>()
                .HasMany(p => p.Bookings)       // Passenger has many Bookings
                .WithOne(b => b.Passenger)      // Each Booking belongs to one Passenger
                .HasForeignKey(b => b.PassengerId) // Foreign key is Booking.PassengerId
                .OnDelete(DeleteBehavior.Restrict); // Prevent deleting a Passenger if they have Bookings.

            // Booking to Seat (One-to-One, optional, with Booking as the dependent)
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.AssignedSeat)        // Booking has one (optional) AssignedSeat
                .WithOne(s => s.ReservedByBooking)  // Seat is reserved by one (optional) Booking (inverse navigation)
                .HasForeignKey<Booking>(b => b.AssignedSeatId) // The foreign key is Booking.AssignedSeatId
                .IsRequired(false)                  // A Booking can exist without an AssignedSeat
                .OnDelete(DeleteBehavior.SetNull);   // If the Seat (principal) is deleted, set Booking.AssignedSeatId to null.


            // --- Seed Data ---

            // Define static DateTime values for seeding
            var seedTimeFlight1Departure = new DateTime(2024, 9, 1, 10, 30, 0, DateTimeKind.Utc);
            var seedTimeFlight1Arrival = seedTimeFlight1Departure.AddHours(5).AddMinutes(30); // e.g., 5h 30m flight

            var seedTimeFlight2Departure = new DateTime(2024, 9, 5, 14, 0, 0, DateTimeKind.Utc);
            var seedTimeFlight2Arrival = seedTimeFlight2Departure.AddHours(3); // e.g., 3h flight

            modelBuilder.Entity<Flight>().HasData(
                new Flight
                {
                    Id = 1,
                    FlightNumber = "MG101", // Changed to avoid potential conflict with your previous FL prefix
                    DepartureCity = "Ulaanbaatar (UBN)",
                    ArrivalCity = "Tokyo (NRT)",
                    DepartureTime = seedTimeFlight1Departure,
                    ArrivalTime = seedTimeFlight1Arrival,
                    Status = FlightStatus.Scheduled,
                    TotalSeats = 2 // Keeping small for easy seat testing
                },
                new Flight
                {
                    Id = 2,
                    FlightNumber = "MG202",
                    DepartureCity = "Seoul (ICN)",
                    ArrivalCity = "Ulaanbaatar (UBN)",
                    DepartureTime = seedTimeFlight2Departure,
                    ArrivalTime = seedTimeFlight2Arrival,
                    Status = FlightStatus.Scheduled,
                    TotalSeats = 150
                }
            );

            modelBuilder.Entity<Seat>().HasData(
                // Seats for Flight 1 (MG101)
                new Seat { Id = 1, FlightId = 1, SeatNumber = "1A", IsReserved = false },
                new Seat { Id = 2, FlightId = 1, SeatNumber = "1B", IsReserved = false },
                new Seat { Id = 3, FlightId = 1, SeatNumber = "2A", IsReserved = false }, // Added one more for variety
                new Seat { Id = 4, FlightId = 1, SeatNumber = "2B", IsReserved = false }, // And another

                // Seats for Flight 2 (MG202) - example, can add more
                new Seat { Id = 5, FlightId = 2, SeatNumber = "1A", IsReserved = false },
                new Seat { Id = 6, FlightId = 2, SeatNumber = "1B", IsReserved = false },
                new Seat { Id = 7, FlightId = 2, SeatNumber = "1C", IsReserved = false }
            );

            // You can also seed Passengers and Bookings if needed for initial testing scenarios
            // For example:
            // modelBuilder.Entity<Passenger>().HasData(
            //    new Passenger { Id = 1, PassportNumber = "P123456", FirstName = "John", LastName = "Doe" }
            // );
            // modelBuilder.Entity<Booking>().HasData(
            //    new Booking { Id = 1, FlightId = 1, PassengerId = 1, BookingTime = DateTime.UtcNow.AddDays(-10) } // Static time for BookingTime too!
            // );

            modelBuilder.Entity<Passenger>().HasData(
    new Passenger { Id = 1, PassportNumber = "P000001", FirstName = "Test", LastName = "PassengerA" },
    new Passenger { Id = 2, PassportNumber = "P000002", FirstName = "Another", LastName = "UserB" }
);

            // Static booking times
            var bookingTime1 = new DateTime(2024, 8, 1, 12, 0, 0, DateTimeKind.Utc);
            var bookingTime2 = new DateTime(2024, 8, 2, 15, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Booking>().HasData(
                // Passenger 1 booked on Flight 1 (MG101), no seat assigned yet
                new Booking { Id = 1, FlightId = 1, PassengerId = 1, BookingTime = bookingTime1, AssignedSeatId = null },
                // Passenger 2 booked on Flight 1 (MG101), no seat assigned yet
                new Booking { Id = 2, FlightId = 1, PassengerId = 2, BookingTime = bookingTime2, AssignedSeatId = null },
                // Passenger 1 also booked on Flight 2 (MG202), no seat assigned yet
                new Booking { Id = 3, FlightId = 2, PassengerId = 1, BookingTime = bookingTime1.AddDays(1), AssignedSeatId = null }
            );
        }
    }
}