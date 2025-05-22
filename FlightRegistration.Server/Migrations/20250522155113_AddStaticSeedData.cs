using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlightRegistration.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddStaticSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "ArrivalCity", "ArrivalTime", "DepartureCity", "DepartureTime", "FlightNumber", "Status", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "Tokyo (NRT)", new DateTime(2024, 9, 1, 16, 0, 0, 0, DateTimeKind.Utc), "Ulaanbaatar (UBN)", new DateTime(2024, 9, 1, 10, 30, 0, 0, DateTimeKind.Utc), "MG101", 0, 2 },
                    { 2, "Ulaanbaatar (UBN)", new DateTime(2024, 9, 5, 17, 0, 0, 0, DateTimeKind.Utc), "Seoul (ICN)", new DateTime(2024, 9, 5, 14, 0, 0, 0, DateTimeKind.Utc), "MG202", 0, 150 }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "FlightId", "IsReserved", "SeatNumber" },
                values: new object[,]
                {
                    { 1, 1, false, "1A" },
                    { 2, 1, false, "1B" },
                    { 3, 1, false, "2A" },
                    { 4, 1, false, "2B" },
                    { 5, 2, false, "1A" },
                    { 6, 2, false, "1B" },
                    { 7, 2, false, "1C" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Seats",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Flights",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
