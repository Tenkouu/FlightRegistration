using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FlightRegistration.Server.Migrations
{
    /// <inheritdoc />
    public partial class runaway : Migration
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
                    { 1, "FRANKFURT", new DateTime(2024, 11, 1, 9, 0, 0, 0, DateTimeKind.Utc), "ULAANBAATAR", new DateTime(2024, 11, 1, 3, 0, 0, 0, DateTimeKind.Utc), "MG101", 0, 20 },
                    { 2, "NEW YORK CITY", new DateTime(2024, 11, 2, 13, 0, 0, 0, DateTimeKind.Utc), "TOKYO", new DateTime(2024, 11, 2, 6, 0, 0, 0, DateTimeKind.Utc), "MG102", 0, 20 },
                    { 3, "LONDON", new DateTime(2024, 11, 3, 17, 0, 0, 0, DateTimeKind.Utc), "SEOUL", new DateTime(2024, 11, 3, 9, 0, 0, 0, DateTimeKind.Utc), "MG103", 0, 20 },
                    { 4, "PARIS", new DateTime(2024, 11, 4, 17, 0, 0, 0, DateTimeKind.Utc), "FRANKFURT", new DateTime(2024, 11, 4, 12, 0, 0, 0, DateTimeKind.Utc), "MG104", 0, 20 },
                    { 5, "SAN FRANCISCO", new DateTime(2024, 11, 5, 21, 0, 0, 0, DateTimeKind.Utc), "NEW YORK CITY", new DateTime(2024, 11, 5, 15, 0, 0, 0, DateTimeKind.Utc), "MG105", 0, 20 },
                    { 6, "LOS ANGELES", new DateTime(2024, 11, 7, 1, 0, 0, 0, DateTimeKind.Utc), "LONDON", new DateTime(2024, 11, 6, 18, 0, 0, 0, DateTimeKind.Utc), "MG106", 0, 20 },
                    { 7, "BEIJING", new DateTime(2024, 11, 8, 5, 0, 0, 0, DateTimeKind.Utc), "PARIS", new DateTime(2024, 11, 7, 21, 0, 0, 0, DateTimeKind.Utc), "MG107", 0, 20 },
                    { 8, "ULAANBAATAR", new DateTime(2024, 11, 9, 5, 0, 0, 0, DateTimeKind.Utc), "SAN FRANCISCO", new DateTime(2024, 11, 9, 0, 0, 0, 0, DateTimeKind.Utc), "MG108", 0, 20 },
                    { 9, "TOKYO", new DateTime(2024, 11, 10, 9, 0, 0, 0, DateTimeKind.Utc), "LOS ANGELES", new DateTime(2024, 11, 10, 3, 0, 0, 0, DateTimeKind.Utc), "MG109", 0, 20 },
                    { 10, "SEOUL", new DateTime(2024, 11, 11, 13, 0, 0, 0, DateTimeKind.Utc), "BEIJING", new DateTime(2024, 11, 11, 6, 0, 0, 0, DateTimeKind.Utc), "MG110", 0, 20 }
                });

            migrationBuilder.InsertData(
                table: "Passengers",
                columns: new[] { "Id", "FirstName", "LastName", "PassportNumber" },
                values: new object[,]
                {
                    { 1, "Nomin", "Batbold", "P000001" },
                    { 2, "Temuulen", "Ganbaatar", "P000002" },
                    { 3, "Anu", "Erdene", "P000003" },
                    { 4, "Khulan", "Dorj", "P000004" },
                    { 5, "Chingun", "Sukhbaatar", "P000005" },
                    { 6, "Solongo", "Purev", "P000006" },
                    { 7, "Ankhbayar", "Ankhiluun", "P000007" },
                    { 8, "Maral", "Tseren", "P000008" },
                    { 9, "Ganzorig", "Bold", "P000009" },
                    { 10, "Oyundari", "Munkhbat", "P000010" },
                    { 11, "Bat-Erdene", "Naran", "P000011" },
                    { 12, "Ujin", "Otgon", "P000012" },
                    { 13, "Enerel", "Baatar", "P000013" },
                    { 14, "Munkhzul", "Chimed", "P000014" },
                    { 15, "Saruul", "Jargal", "P000015" },
                    { 16, "Tuguldur", "Altangerel", "P000016" },
                    { 17, "Yesugen", "Zolboo", "P000017" },
                    { 18, "Altan", "Gerel", "P000018" },
                    { 19, "Nomin-Erdene", "Purevdorj", "P000019" },
                    { 20, "Emujin", "Dash", "P000020" },
                    { 21, "Odgerel", "Tuvshin", "P000021" },
                    { 22, "Zolzaya", "Enkhbold", "P000022" },
                    { 23, "Batzorig", "Ganbold", "P000023" },
                    { 24, "Delgermaa", "Demi", "P000024" },
                    { 25, "Khongorzul", "Sainbayar", "P000025" },
                    { 26, "Anar", "Oyun", "P000026" },
                    { 27, "Uuriintsolmon", "Bat-Orgil", "P000027" },
                    { 28, "Dulguun", "Erdenebat", "P000028" },
                    { 29, "Batnyam", "Gantulga", "P000029" },
                    { 30, "Bayasgalan", "Munkh-Erdene", "P000030" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AssignedSeatId", "BookingTime", "FlightId", "PassengerId" },
                values: new object[,]
                {
                    { 1, null, new DateTime(2024, 10, 1, 4, 0, 0, 0, DateTimeKind.Utc), 1, 1 },
                    { 3, null, new DateTime(2024, 10, 1, 3, 0, 0, 0, DateTimeKind.Utc), 1, 3 },
                    { 5, null, new DateTime(2024, 10, 1, 3, 0, 0, 0, DateTimeKind.Utc), 1, 5 },
                    { 6, null, new DateTime(2024, 10, 1, 6, 0, 0, 0, DateTimeKind.Utc), 2, 6 },
                    { 8, null, new DateTime(2024, 10, 1, 6, 0, 0, 0, DateTimeKind.Utc), 2, 8 },
                    { 9, null, new DateTime(2024, 10, 1, 6, 0, 0, 0, DateTimeKind.Utc), 2, 1 },
                    { 11, null, new DateTime(2024, 10, 1, 14, 0, 0, 0, DateTimeKind.Utc), 3, 11 },
                    { 12, null, new DateTime(2024, 10, 1, 12, 0, 0, 0, DateTimeKind.Utc), 3, 2 },
                    { 13, null, new DateTime(2024, 10, 1, 19, 0, 0, 0, DateTimeKind.Utc), 4, 15 },
                    { 14, null, new DateTime(2024, 10, 2, 0, 0, 0, 0, DateTimeKind.Utc), 4, 16 },
                    { 16, null, new DateTime(2024, 10, 4, 15, 0, 0, 0, DateTimeKind.Utc), 5, 18 },
                    { 17, null, new DateTime(2024, 10, 3, 3, 0, 0, 0, DateTimeKind.Utc), 5, 19 },
                    { 18, null, new DateTime(2024, 10, 5, 17, 0, 0, 0, DateTimeKind.Utc), 5, 20 },
                    { 19, null, new DateTime(2024, 10, 1, 21, 0, 0, 0, DateTimeKind.Utc), 6, 21 },
                    { 21, null, new DateTime(2024, 10, 1, 17, 0, 0, 0, DateTimeKind.Utc), 7, 23 },
                    { 23, null, new DateTime(2024, 10, 4, 3, 0, 0, 0, DateTimeKind.Utc), 7, 25 },
                    { 25, null, new DateTime(2024, 10, 1, 2, 0, 0, 0, DateTimeKind.Utc), 8, 27 },
                    { 26, null, new DateTime(2024, 10, 1, 14, 0, 0, 0, DateTimeKind.Utc), 8, 28 },
                    { 28, null, new DateTime(2024, 10, 5, 21, 0, 0, 0, DateTimeKind.Utc), 9, 30 },
                    { 30, null, new DateTime(2024, 10, 2, 14, 0, 0, 0, DateTimeKind.Utc), 10, 13 },
                    { 31, null, new DateTime(2024, 10, 2, 16, 0, 0, 0, DateTimeKind.Utc), 10, 14 }
                });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "FlightId", "IsReserved", "SeatNumber" },
                values: new object[,]
                {
                    { 1, 1, true, "1A" },
                    { 2, 1, false, "1B" },
                    { 3, 1, false, "1C" },
                    { 4, 1, false, "1D" },
                    { 5, 1, true, "2A" },
                    { 6, 1, false, "2B" },
                    { 7, 1, false, "2C" },
                    { 8, 1, false, "2D" },
                    { 9, 1, false, "3A" },
                    { 10, 1, false, "3B" },
                    { 11, 1, false, "3C" },
                    { 12, 1, false, "3D" },
                    { 13, 1, false, "4A" },
                    { 14, 1, false, "4B" },
                    { 15, 1, false, "4C" },
                    { 16, 1, false, "4D" },
                    { 17, 1, false, "5A" },
                    { 18, 1, false, "5B" },
                    { 19, 1, false, "5C" },
                    { 20, 1, false, "5D" },
                    { 21, 2, true, "1A" },
                    { 22, 2, false, "1B" },
                    { 23, 2, false, "1C" },
                    { 24, 2, false, "1D" },
                    { 25, 2, false, "2A" },
                    { 26, 2, false, "2B" },
                    { 27, 2, false, "2C" },
                    { 28, 2, false, "2D" },
                    { 29, 2, false, "3A" },
                    { 30, 2, false, "3B" },
                    { 31, 2, false, "3C" },
                    { 32, 2, false, "3D" },
                    { 33, 2, false, "4A" },
                    { 34, 2, false, "4B" },
                    { 35, 2, false, "4C" },
                    { 36, 2, false, "4D" },
                    { 37, 2, false, "5A" },
                    { 38, 2, false, "5B" },
                    { 39, 2, false, "5C" },
                    { 40, 2, false, "5D" },
                    { 41, 3, true, "1A" },
                    { 42, 3, false, "1B" },
                    { 43, 3, false, "1C" },
                    { 44, 3, false, "1D" },
                    { 45, 3, false, "2A" },
                    { 46, 3, false, "2B" },
                    { 47, 3, false, "2C" },
                    { 48, 3, false, "2D" },
                    { 49, 3, false, "3A" },
                    { 50, 3, false, "3B" },
                    { 51, 3, false, "3C" },
                    { 52, 3, false, "3D" },
                    { 53, 3, false, "4A" },
                    { 54, 3, false, "4B" },
                    { 55, 3, false, "4C" },
                    { 56, 3, false, "4D" },
                    { 57, 3, false, "5A" },
                    { 58, 3, false, "5B" },
                    { 59, 3, false, "5C" },
                    { 60, 3, false, "5D" },
                    { 61, 4, false, "1A" },
                    { 62, 4, false, "1B" },
                    { 63, 4, false, "1C" },
                    { 64, 4, false, "1D" },
                    { 65, 4, false, "2A" },
                    { 66, 4, false, "2B" },
                    { 67, 4, false, "2C" },
                    { 68, 4, false, "2D" },
                    { 69, 4, false, "3A" },
                    { 70, 4, false, "3B" },
                    { 71, 4, false, "3C" },
                    { 72, 4, false, "3D" },
                    { 73, 4, false, "4A" },
                    { 74, 4, false, "4B" },
                    { 75, 4, false, "4C" },
                    { 76, 4, false, "4D" },
                    { 77, 4, false, "5A" },
                    { 78, 4, false, "5B" },
                    { 79, 4, false, "5C" },
                    { 80, 4, false, "5D" },
                    { 81, 5, true, "1A" },
                    { 82, 5, false, "1B" },
                    { 83, 5, false, "1C" },
                    { 84, 5, false, "1D" },
                    { 85, 5, false, "2A" },
                    { 86, 5, false, "2B" },
                    { 87, 5, false, "2C" },
                    { 88, 5, false, "2D" },
                    { 89, 5, false, "3A" },
                    { 90, 5, false, "3B" },
                    { 91, 5, false, "3C" },
                    { 92, 5, false, "3D" },
                    { 93, 5, false, "4A" },
                    { 94, 5, false, "4B" },
                    { 95, 5, false, "4C" },
                    { 96, 5, false, "4D" },
                    { 97, 5, false, "5A" },
                    { 98, 5, false, "5B" },
                    { 99, 5, false, "5C" },
                    { 100, 5, false, "5D" },
                    { 101, 6, false, "1A" },
                    { 102, 6, false, "1B" },
                    { 103, 6, false, "1C" },
                    { 104, 6, false, "1D" },
                    { 105, 6, false, "2A" },
                    { 106, 6, false, "2B" },
                    { 107, 6, false, "2C" },
                    { 108, 6, true, "2D" },
                    { 109, 6, false, "3A" },
                    { 110, 6, false, "3B" },
                    { 111, 6, false, "3C" },
                    { 112, 6, false, "3D" },
                    { 113, 6, false, "4A" },
                    { 114, 6, false, "4B" },
                    { 115, 6, false, "4C" },
                    { 116, 6, false, "4D" },
                    { 117, 6, false, "5A" },
                    { 118, 6, false, "5B" },
                    { 119, 6, false, "5C" },
                    { 120, 6, false, "5D" },
                    { 121, 7, false, "1A" },
                    { 122, 7, false, "1B" },
                    { 123, 7, false, "1C" },
                    { 124, 7, true, "1D" },
                    { 125, 7, false, "2A" },
                    { 126, 7, false, "2B" },
                    { 127, 7, false, "2C" },
                    { 128, 7, false, "2D" },
                    { 129, 7, false, "3A" },
                    { 130, 7, false, "3B" },
                    { 131, 7, true, "3C" },
                    { 132, 7, false, "3D" },
                    { 133, 7, false, "4A" },
                    { 134, 7, false, "4B" },
                    { 135, 7, false, "4C" },
                    { 136, 7, false, "4D" },
                    { 137, 7, false, "5A" },
                    { 138, 7, false, "5B" },
                    { 139, 7, false, "5C" },
                    { 140, 7, false, "5D" },
                    { 141, 8, false, "1A" },
                    { 142, 8, false, "1B" },
                    { 143, 8, false, "1C" },
                    { 144, 8, false, "1D" },
                    { 145, 8, false, "2A" },
                    { 146, 8, false, "2B" },
                    { 147, 8, false, "2C" },
                    { 148, 8, false, "2D" },
                    { 149, 8, false, "3A" },
                    { 150, 8, false, "3B" },
                    { 151, 8, false, "3C" },
                    { 152, 8, false, "3D" },
                    { 153, 8, false, "4A" },
                    { 154, 8, false, "4B" },
                    { 155, 8, false, "4C" },
                    { 156, 8, false, "4D" },
                    { 157, 8, false, "5A" },
                    { 158, 8, false, "5B" },
                    { 159, 8, false, "5C" },
                    { 160, 8, false, "5D" },
                    { 161, 9, false, "1A" },
                    { 162, 9, true, "1B" },
                    { 163, 9, false, "1C" },
                    { 164, 9, false, "1D" },
                    { 165, 9, false, "2A" },
                    { 166, 9, false, "2B" },
                    { 167, 9, false, "2C" },
                    { 168, 9, false, "2D" },
                    { 169, 9, false, "3A" },
                    { 170, 9, false, "3B" },
                    { 171, 9, false, "3C" },
                    { 172, 9, false, "3D" },
                    { 173, 9, false, "4A" },
                    { 174, 9, false, "4B" },
                    { 175, 9, false, "4C" },
                    { 176, 9, false, "4D" },
                    { 177, 9, false, "5A" },
                    { 178, 9, false, "5B" },
                    { 179, 9, false, "5C" },
                    { 180, 9, false, "5D" },
                    { 181, 10, true, "1A" },
                    { 182, 10, false, "1B" },
                    { 183, 10, false, "1C" },
                    { 184, 10, false, "1D" },
                    { 185, 10, false, "2A" },
                    { 186, 10, false, "2B" },
                    { 187, 10, false, "2C" },
                    { 188, 10, false, "2D" },
                    { 189, 10, false, "3A" },
                    { 190, 10, false, "3B" },
                    { 191, 10, false, "3C" },
                    { 192, 10, false, "3D" },
                    { 193, 10, false, "4A" },
                    { 194, 10, false, "4B" },
                    { 195, 10, false, "4C" },
                    { 196, 10, false, "4D" },
                    { 197, 10, false, "5A" },
                    { 198, 10, false, "5B" },
                    { 199, 10, false, "5C" },
                    { 200, 10, false, "5D" }
                });

            migrationBuilder.InsertData(
                table: "Bookings",
                columns: new[] { "Id", "AssignedSeatId", "BookingTime", "FlightId", "PassengerId" },
                values: new object[,]
                {
                    { 2, 1, new DateTime(2024, 10, 1, 4, 0, 0, 0, DateTimeKind.Utc), 1, 2 },
                    { 4, 5, new DateTime(2024, 10, 1, 4, 0, 0, 0, DateTimeKind.Utc), 1, 4 },
                    { 7, 21, new DateTime(2024, 10, 1, 6, 0, 0, 0, DateTimeKind.Utc), 2, 7 },
                    { 10, 41, new DateTime(2024, 10, 1, 13, 0, 0, 0, DateTimeKind.Utc), 3, 10 },
                    { 15, 81, new DateTime(2024, 10, 4, 0, 0, 0, 0, DateTimeKind.Utc), 5, 17 },
                    { 20, 108, new DateTime(2024, 10, 4, 21, 0, 0, 0, DateTimeKind.Utc), 6, 22 },
                    { 22, 124, new DateTime(2024, 10, 2, 3, 0, 0, 0, DateTimeKind.Utc), 7, 24 },
                    { 24, 131, new DateTime(2024, 10, 5, 22, 0, 0, 0, DateTimeKind.Utc), 7, 26 },
                    { 27, 162, new DateTime(2024, 10, 4, 2, 0, 0, 0, DateTimeKind.Utc), 9, 29 },
                    { 29, 181, new DateTime(2024, 10, 2, 12, 0, 0, 0, DateTimeKind.Utc), 10, 12 }
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
