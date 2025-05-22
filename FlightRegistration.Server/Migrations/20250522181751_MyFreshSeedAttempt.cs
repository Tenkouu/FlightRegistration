using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlightRegistration.Server.Migrations
{
    /// <inheritdoc />
    public partial class MyFreshSeedAttempt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightNumber = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureCity = table.Column<string>(type: "TEXT", nullable: false),
                    ArrivalCity = table.Column<string>(type: "TEXT", nullable: false),
                    DepartureTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ArrivalTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalSeats = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Passengers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PassportNumber = table.Column<string>(type: "TEXT", nullable: false),
                    FirstName = table.Column<string>(type: "TEXT", nullable: false),
                    LastName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Passengers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatNumber = table.Column<string>(type: "TEXT", nullable: false),
                    IsReserved = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FlightId = table.Column<int>(type: "INTEGER", nullable: false),
                    PassengerId = table.Column<int>(type: "INTEGER", nullable: false),
                    BookingTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AssignedSeatId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Bookings_Passengers_PassengerId",
                        column: x => x.PassengerId,
                        principalTable: "Passengers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Bookings_Seats_AssignedSeatId",
                        column: x => x.AssignedSeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Flights",
                columns: new[] { "Id", "ArrivalCity", "ArrivalTime", "DepartureCity", "DepartureTime", "FlightNumber", "Status", "TotalSeats" },
                values: new object[,]
                {
                    { 1, "TYO", new DateTime(2024, 10, 10, 15, 0, 0, 0, DateTimeKind.Utc), "UBN", new DateTime(2024, 10, 10, 10, 0, 0, 0, DateTimeKind.Utc), "MG101", 0, 6 },
                    { 2, "UBN", new DateTime(2024, 10, 10, 18, 0, 0, 0, DateTimeKind.Utc), "SEL", new DateTime(2024, 10, 10, 14, 30, 0, 0, DateTimeKind.Utc), "MG202", 0, 4 },
                    { 3, "FRA", new DateTime(2024, 10, 11, 18, 0, 0, 0, DateTimeKind.Utc), "UBN", new DateTime(2024, 10, 11, 8, 0, 0, 0, DateTimeKind.Utc), "MG303", 0, 8 }
                });

            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "Id", "FirstName", "LastName", "PassportNumber" },
                values: new object[,]
                {
                    { 1, "Alice", "Wonder", "P000001" },
                    { 2, "Bob", "Builder", "P000002" },
                    { 3, "Charlie", "Chaplin", "P000003" },
                    { 4, "Diana", "Prince", "P000004" },
                    { 5, "Edward", "Elric", "P000005" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AssignedSeatId", "BookingTime", "FlightId", "PassengerId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 9, 1, 10, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 3, null, new DateTime(2024, 9, 1, 12, 0, 0, 0, DateTimeKind.Utc), 1, 3 },
                    { 4, null, new DateTime(2024, 9, 1, 13, 0, 0, 0, DateTimeKind.Utc), 2, 4 },
                    { 6, null, new DateTime(2024, 9, 1, 15, 0, 0, 0, DateTimeKind.Utc), 3, 1 },
                    { 8, null, new DateTime(2024, 9, 1, 17, 0, 0, 0, DateTimeKind.Utc), 3, 4 }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "FlightId", "IsReserved", "SeatNumber" },
                values: new object[,]
                {
                    { 1, 1, true, "1A" },
                    { 2, 1, false, "1B" },
                    { 3, 1, false, "1C" },
                    { 4, 1, false, "2A" },
                    { 5, 1, false, "2B" },
                    { 6, 1, false, "2C" },
                    { 7, 2, true, "1A" },
                    { 8, 2, false, "1B" },
                    { 9, 2, false, "2A" },
                    { 10, 2, false, "2B" },
                    { 11, 3, true, "1A" },
                    { 12, 3, false, "1B" },
                    { 13, 3, false, "1C" },
                    { 14, 3, false, "1D" },
                    { 15, 3, false, "2A" },
                    { 16, 3, false, "2B" },
                    { 17, 3, false, "2C" },
                    { 18, 3, false, "2D" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AssignedSeatId", "BookingTime", "FlightId", "PassengerId" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2024, 9, 1, 11, 0, 0, 0, DateTimeKind.Utc), 1, 2 },
                    { 5, 7, new DateTime(2024, 9, 1, 14, 0, 0, 0, DateTimeKind.Utc), 2, 5 },
                    { 7, 11, new DateTime(2024, 9, 1, 16, 0, 0, 0, DateTimeKind.Utc), 3, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_AssignedSeatId",
                table: "Bookings",
                column: "AssignedSeatId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FlightId",
                table: "Bookings",
                column: "FlightId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_PassengerId",
                table: "Bookings",
                column: "PassengerId");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_PassportNumber",
                table: "Passengers",
                column: "PassportNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_FlightId",
                table: "Seats",
                column: "FlightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "Passengers");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "Flights");
        }
    }
}
