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

        // In FlightRegistration.Services/DataAccess/AppDbContext.cs

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

            // --- RICH SEED DATA with Mongolian Names ---

            // Passengers (30)
            var passengers = new List<Passenger>
    {
        new Passenger { Id = 1, PassportNumber = "P000001", FirstName = "Nomin", LastName = "Batbold" },
        new Passenger { Id = 2, PassportNumber = "P000002", FirstName = "Temuulen", LastName = "Ganbaatar" },
        new Passenger { Id = 3, PassportNumber = "P000003", FirstName = "Anu", LastName = "Erdene" },
        new Passenger { Id = 4, PassportNumber = "P000004", FirstName = "Khulan", LastName = "Dorj" },
        new Passenger { Id = 5, PassportNumber = "P000005", FirstName = "Chingun", LastName = "Sukhbaatar" },
        new Passenger { Id = 6, PassportNumber = "P000006", FirstName = "Solongo", LastName = "Purev" },
        new Passenger { Id = 7, PassportNumber = "P000007", FirstName = "Ankhbayar", LastName = "Ankhiluun" },
        new Passenger { Id = 8, PassportNumber = "P000008", FirstName = "Maral", LastName = "Tseren" },
        new Passenger { Id = 9, PassportNumber = "P000009", FirstName = "Ganzorig", LastName = "Bold" },
        new Passenger { Id = 10, PassportNumber = "P000010", FirstName = "Oyundari", LastName = "Munkhbat" },
        new Passenger { Id = 11, PassportNumber = "P000011", FirstName = "Bat-Erdene", LastName = "Naran" },
        new Passenger { Id = 12, PassportNumber = "P000012", FirstName = "Ujin", LastName = "Otgon" },
        new Passenger { Id = 13, PassportNumber = "P000013", FirstName = "Enerel", LastName = "Baatar" },
        new Passenger { Id = 14, PassportNumber = "P000014", FirstName = "Munkhzul", LastName = "Chimed" },
        new Passenger { Id = 15, PassportNumber = "P000015", FirstName = "Saruul", LastName = "Jargal" },
        new Passenger { Id = 16, PassportNumber = "P000016", FirstName = "Tuguldur", LastName = "Altangerel" },
        new Passenger { Id = 17, PassportNumber = "P000017", FirstName = "Yesugen", LastName = "Zolboo" },
        new Passenger { Id = 18, PassportNumber = "P000018", FirstName = "Altan", LastName = "Gerel" },
        new Passenger { Id = 19, PassportNumber = "P000019", FirstName = "Nomin-Erdene", LastName = "Purevdorj" },
        new Passenger { Id = 20, PassportNumber = "P000020", FirstName = "Emujin", LastName = "Dash" },
        new Passenger { Id = 21, PassportNumber = "P000021", FirstName = "Odgerel", LastName = "Tuvshin" },
        new Passenger { Id = 22, PassportNumber = "P000022", FirstName = "Zolzaya", LastName = "Enkhbold" },
        new Passenger { Id = 23, PassportNumber = "P000023", FirstName = "Batzorig", LastName = "Ganbold" },
        new Passenger { Id = 24, PassportNumber = "P000024", FirstName = "Delgermaa", LastName = "Demi" },
        new Passenger { Id = 25, PassportNumber = "P000025", FirstName = "Khongorzul", LastName = "Sainbayar" },
        new Passenger { Id = 26, PassportNumber = "P000026", FirstName = "Anar", LastName = "Oyun" },
        new Passenger { Id = 27, PassportNumber = "P000027", FirstName = "Uuriintsolmon", LastName = "Bat-Orgil" },
        new Passenger { Id = 28, PassportNumber = "P000028", FirstName = "Dulguun", LastName = "Erdenebat" },
        new Passenger { Id = 29, PassportNumber = "P000029", FirstName = "Batnyam", LastName = "Gantulga" },
        new Passenger { Id = 30, PassportNumber = "P000030", FirstName = "Bayasgalan", LastName = "Munkh-Erdene" }
    };
            modelBuilder.Entity<Passenger>().HasData(passengers);

            // Flights (10)
            var flights = new List<Flight>();
            string[] cities = { "ULAANBAATAR", "TOKYO", "SEOUL", "FRANKFURT", "NEW YORK CITY", "LONDON", "PARIS", "SAN FRANCISCO", "LOS ANGELES", "BEIJING" };
            var baseDateTime = new DateTime(2024, 11, 1, 0, 0, 0, DateTimeKind.Utc);
            for (int i = 1; i <= 10; i++)
            {
                flights.Add(new Flight
                {
                    Id = i,
                    FlightNumber = $"MG{100 + i}",
                    DepartureCity = cities[i - 1],
                    ArrivalCity = cities[(i + 2) % cities.Length], // Ensure different arrival city
                    DepartureTime = baseDateTime.AddDays(i - 1).AddHours(i * 3), // Spaced out departures
                    ArrivalTime = baseDateTime.AddDays(i - 1).AddHours(i * 3 + (i % 4 + 5)), // Varying durations
                    Status = FlightStatus.Scheduled,
                    TotalSeats = 20
                });
            }
            modelBuilder.Entity<Flight>().HasData(flights);

            // Seats (20 per flight = 200 total)
            var seats = new List<Seat>();
            int seatIdCounter = 1;
            char[] seatLetters = { 'A', 'B', 'C', 'D' };
            for (int flightId = 1; flightId <= 10; flightId++)
            {
                for (int row = 1; row <= 5; row++)
                {
                    foreach (char letter in seatLetters)
                    {
                        seats.Add(new Seat { Id = seatIdCounter++, FlightId = flightId, SeatNumber = $"{row}{letter}", IsReserved = false });
                    }
                }
            }
            // Note: IsReserved will be updated below based on bookings.

            // Bookings
            var bookings = new List<Booking>();
            int bookingIdCounter = 1;
            var bookingTimeBase = new DateTime(2024, 10, 1, 0, 0, 0, DateTimeKind.Utc);
            Random random = new Random(123); // Seeded random for consistent "randomness"

            // Assign some seats randomly for pre-booked passengers
            // Flight 1 (MG101) - Seats 1-20. Passengers 1-10
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 1, PassengerId = 1, BookingTime = bookingTimeBase.AddHours(random.Next(1, 5)), AssignedSeatId = null }); // Nomin, no seat
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 1, PassengerId = 2, BookingTime = bookingTimeBase.AddHours(random.Next(1, 5)), AssignedSeatId = 1 });    // Temuulen, Seat 1A (ID=1)
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 1, PassengerId = 3, BookingTime = bookingTimeBase.AddHours(random.Next(1, 5)), AssignedSeatId = null }); // Anu, no seat
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 1, PassengerId = 4, BookingTime = bookingTimeBase.AddHours(random.Next(1, 5)), AssignedSeatId = 5 });    // Khulan, Seat 2A (ID=5)
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 1, PassengerId = 5, BookingTime = bookingTimeBase.AddHours(random.Next(1, 5)), AssignedSeatId = null }); // Chingun, no seat

            // Flight 2 (MG102) - Seats 21-40. Passengers 6-15
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 2, PassengerId = 6, BookingTime = bookingTimeBase.AddHours(random.Next(6, 10)), AssignedSeatId = null }); // Solongo
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 2, PassengerId = 7, BookingTime = bookingTimeBase.AddHours(random.Next(6, 10)), AssignedSeatId = 21 });   // Bilguun, Seat 1A (ID=21)
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 2, PassengerId = 8, BookingTime = bookingTimeBase.AddHours(random.Next(6, 10)), AssignedSeatId = null }); // Maral
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 2, PassengerId = 1, BookingTime = bookingTimeBase.AddHours(random.Next(6, 10)), AssignedSeatId = null }); // Nomin again

            // Flight 3 (MG103) - Seats 41-60. Passengers 10-20
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 3, PassengerId = 10, BookingTime = bookingTimeBase.AddHours(random.Next(11, 15)), AssignedSeatId = 41 }); // Oyundari, Seat 1A (ID=41)
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 3, PassengerId = 11, BookingTime = bookingTimeBase.AddHours(random.Next(11, 15)), AssignedSeatId = null });// Bat-Erdene
            bookings.Add(new Booking { Id = bookingIdCounter++, FlightId = 3, PassengerId = 2, BookingTime = bookingTimeBase.AddHours(random.Next(11, 15)), AssignedSeatId = null }); // Temuulen again

            // Populate more bookings to ensure most passengers have at least one, and some have multiple
            // And ensure a good number of unassigned seats on various flights
            int passengerIndex = 15; // Start from passenger 16
            for (int flightId = 4; flightId <= 10; flightId++)
            {
                int bookingsForThisFlight = random.Next(2, 5); // 2 to 4 bookings per flight
                for (int k = 0; k < bookingsForThisFlight; k++)
                {
                    if (passengerIndex > 30) passengerIndex = random.Next(1, 15); // Reuse some early passengers if we run out

                    // Decide if this booking gets a pre-assigned seat (e.g., 30% chance)
                    int? assignedSeatForThisBooking = null;
                    if (random.Next(1, 100) <= 30)
                    {
                        // Try to assign an available seat on this flight
                        int flightSeatStartIndex = (flightId - 1) * 20 + 1;
                        int randomSeatOffset = random.Next(0, 19);
                        int potentialSeatId = flightSeatStartIndex + randomSeatOffset;

                        // Check if this seat is already taken by another booking in *this current bookings list*
                        if (!bookings.Any(b => b.AssignedSeatId == potentialSeatId))
                        {
                            assignedSeatForThisBooking = potentialSeatId;
                        }
                    }
                    bookings.Add(new Booking
                    {
                        Id = bookingIdCounter++,
                        FlightId = flightId,
                        PassengerId = passengerIndex,
                        BookingTime = bookingTimeBase.AddDays(random.Next(0, 5)).AddHours(random.Next(0, 23)),
                        AssignedSeatId = assignedSeatForThisBooking
                    });
                    passengerIndex++;
                }
            }
            modelBuilder.Entity<Booking>().HasData(bookings);

            // Update IsReserved flag for all seats based on the final bookings list
            var allPreAssignedSeatIds = bookings.Where(b => b.AssignedSeatId.HasValue).Select(b => b.AssignedSeatId.Value).ToHashSet();
            foreach (var seat in seats)
            {
                if (allPreAssignedSeatIds.Contains(seat.Id))
                {
                    seat.IsReserved = true;
                }
            }
            // Re-seed the seats list with updated IsReserved flags
            // EF Core HasData is smart enough to handle this as updates if PKs match
            modelBuilder.Entity<Seat>().HasData(seats);
        }
    }
}