using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlightRegistration.Server.Migrations
{
    /// <inheritdoc />
    public partial class SeedPassengersAndBookings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "Id", "FirstName", "LastName", "PassportNumber" },
                values: new object[,]
                {
                    { 1, "Test", "PassengerA", "P000001" },
                    { 2, "Another", "UserB", "P000002" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AssignedSeatId", "BookingTime", "FlightId", "PassengerId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 8, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 2, null, new DateTime(2024, 8, 2, 15, 0, 0, 0, DateTimeKind.Utc), 1, 2 },
                    { 3, null, new DateTime(2024, 8, 2, 12, 0, 0, 0, DateTimeKind.Utc), 2, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Bookings",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Passengers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Passengers",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
