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

            // --- Entity Configurations (Keep these as they were) ---
            modelBuilder.Entity<Passenger>()
                .HasIndex(p => p.PassportNumber)
                .IsUnique();
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Bookings)
                .WithOne(b => b.Flight)
                .HasForeignKey(b => b.FlightId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Flight>()
                .HasMany(f => f.Seats)
                .WithOne(s => s.Flight)
                .HasForeignKey(s => s.FlightId)
                .OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<Passenger>()
                .HasMany(p => p.Bookings)
                .WithOne(b => b.Passenger)
                .HasForeignKey(b => b.PassengerId)
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.AssignedSeat)
                .WithOne(s => s.ReservedByBooking)
                .HasForeignKey<Booking>(b => b.AssignedSeatId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.SetNull);

            // --- NEW Seed Data (Corrected) ---

            // Passengers
            modelBuilder.Entity<Passenger>().HasData(
                new Passenger { Id = 1, PassportNumber = "P000001", FirstName = "Alice", LastName = "Wonder" },
                new Passenger { Id = 2, PassportNumber = "P000002", FirstName = "Bob", LastName = "Builder" },
                new Passenger { Id = 3, PassportNumber = "P000003", FirstName = "Charlie", LastName = "Chaplin" },
                new Passenger { Id = 4, PassportNumber = "P000004", FirstName = "Diana", LastName = "Prince" },
                new Passenger { Id = 5, PassportNumber = "P000005", FirstName = "Edward", LastName = "Elric" }
            );

            // Flights
            var flight1Dep = new DateTime(2024, 10, 10, 10, 0, 0, DateTimeKind.Utc);
            var flight2Dep = new DateTime(2024, 10, 10, 14, 30, 0, DateTimeKind.Utc);
            var flight3Dep = new DateTime(2024, 10, 11, 8, 0, 0, DateTimeKind.Utc);

            modelBuilder.Entity<Flight>().HasData(
                new Flight { Id = 1, FlightNumber = "MG101", DepartureCity = "UBN", ArrivalCity = "TYO", DepartureTime = flight1Dep, ArrivalTime = flight1Dep.AddHours(5), Status = FlightStatus.Scheduled, TotalSeats = 6 },
                new Flight { Id = 2, FlightNumber = "MG202", DepartureCity = "SEL", ArrivalCity = "UBN", DepartureTime = flight2Dep, ArrivalTime = flight2Dep.AddHours(3.5), Status = FlightStatus.Scheduled, TotalSeats = 4 },
                new Flight { Id = 3, FlightNumber = "MG303", DepartureCity = "UBN", ArrivalCity = "FRA", DepartureTime = flight3Dep, ArrivalTime = flight3Dep.AddHours(10), Status = FlightStatus.Scheduled, TotalSeats = 8 }
            );

            // Seats
            // We need to know which Seat IDs will be pre-assigned to set IsReserved correctly.
            // Pre-assigned seats:
            // Booking ID 2 (Bob) -> Flight 1 (MG101), Seat 1A (this will be Seat ID 1)
            // Booking ID 5 (Edward) -> Flight 2 (MG202), Seat 1A (this will be Seat ID 7: 6 from F1 + 1st from F2)
            // Booking ID 7 (Charlie) -> Flight 3 (MG303), Seat 1A (this will be Seat ID 11: 6 from F1 + 4 from F2 + 1st from F3)

            int seatIdCounter = 1;
            // Flight 1 (MG101) - 6 seats
            for (int row = 1; row <= 2; row++)
            {
                foreach (char col in new[] { 'A', 'B', 'C' })
                {
                    bool isReserved = (seatIdCounter == 1); // Seat ID 1 is pre-assigned
                    modelBuilder.Entity<Seat>().HasData(new Seat { Id = seatIdCounter, FlightId = 1, SeatNumber = $"{row}{col}", IsReserved = isReserved });
                    seatIdCounter++;
                }
            }
            // Flight 2 (MG202) - 4 seats
            for (int row = 1; row <= 2; row++)
            {
                foreach (char col in new[] { 'A', 'B' })
                {
                    bool isReserved = (seatIdCounter == 7); // Seat ID 7 is pre-assigned
                    modelBuilder.Entity<Seat>().HasData(new Seat { Id = seatIdCounter, FlightId = 2, SeatNumber = $"{row}{col}", IsReserved = isReserved });
                    seatIdCounter++;
                }
            }
            // Flight 3 (MG303) - 8 seats
            for (int row = 1; row <= 2; row++)
            {
                foreach (char col in new[] { 'A', 'B', 'C', 'D' })
                {
                    bool isReserved = (seatIdCounter == 11); // Seat ID 11 is pre-assigned
                    modelBuilder.Entity<Seat>().HasData(new Seat { Id = seatIdCounter, FlightId = 3, SeatNumber = $"{row}{col}", IsReserved = isReserved });
                    seatIdCounter++;
                }
            }

            // Bookings
            var bookingTime = new DateTime(2024, 9, 1, 10, 0, 0, DateTimeKind.Utc);
            modelBuilder.Entity<Booking>().HasData(
                // Flight MG101
                new Booking { Id = 1, FlightId = 1, PassengerId = 1, BookingTime = bookingTime, AssignedSeatId = null },         // Alice, NO SEAT
                new Booking { Id = 2, FlightId = 1, PassengerId = 2, BookingTime = bookingTime.AddHours(1), AssignedSeatId = 1 },  // Bob, SEAT ID 1 on MG101
                new Booking { Id = 3, FlightId = 1, PassengerId = 3, BookingTime = bookingTime.AddHours(2), AssignedSeatId = null }, // Charlie, NO SEAT

                // Flight MG202
                new Booking { Id = 4, FlightId = 2, PassengerId = 4, BookingTime = bookingTime.AddHours(3), AssignedSeatId = null },         // Diana, NO SEAT
                new Booking { Id = 5, FlightId = 2, PassengerId = 5, BookingTime = bookingTime.AddHours(4), AssignedSeatId = 7 },  // Edward, SEAT ID 7 on MG202

                // Flight MG303
                new Booking { Id = 6, FlightId = 3, PassengerId = 1, BookingTime = bookingTime.AddHours(5), AssignedSeatId = null },          // Alice, NO SEAT
                new Booking { Id = 7, FlightId = 3, PassengerId = 3, BookingTime = bookingTime.AddHours(6), AssignedSeatId = 11 }, // Charlie, SEAT ID 11 on MG303
                new Booking { Id = 8, FlightId = 3, PassengerId = 4, BookingTime = bookingTime.AddHours(7), AssignedSeatId = null }   // Diana, NO SEAT
            );
        }
    }
}