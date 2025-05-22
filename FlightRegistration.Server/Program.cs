using FlightRegistration.Services.DataAccess; // For AppDbContext
using Microsoft.EntityFrameworkCore;         // For UseSqlite and AddDbContext
using FlightRegistration.Services.DataAccess.Interfaces; // For repository interfaces
using FlightRegistration.Services.DataAccess.Repositories; // For repository implementations
using FlightRegistration.Services.BusinessLogic.Interfaces; // For service interfaces
using FlightRegistration.Services.BusinessLogic.Implementations; // For service implementations




var builder = WebApplication.CreateBuilder(args);

// 1. Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("FlightRegistration.Server"))
);
// The MigrationsAssembly option tells EF Core to look for migrations history and create new migrations
// in the FlightRegistration.Server project. This is common when DbContext is in a separate library.

// Add services to the container.
// 2. Register your repositories for Dependency Injection
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
// Add other repositories here as you create them:
// builder.Services.AddScoped<ISeatRepository, SeatRepository>();
// builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
// builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Later, you'll register your Business Logic services here too
// builder.Services.AddScoped<IFlightService, FlightService>();
// builder.Services.AddScoped<ICheckInService, CheckInService>();


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IFlightService, FlightService>();
// ...
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();
// ...

builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();

// builder.Services.AddScoped<ICheckInService, CheckInService>(); // You'll add this later
// Later, you'll add SignalR and other services here
// builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Later, you'll map your SignalR hub here
// app.MapHub<FlightHub>("/flightHub");

app.Run();