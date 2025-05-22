// File: FlightRegistration.Server/Program.cs

// Add these using statements at the top if not already present:
using FlightRegistration.Core.Interfaces;
using FlightRegistration.Services.DataAccess; // For AppDbContext
using FlightRegistration.Services.DataAccess.Interfaces;
using FlightRegistration.Services.DataAccess.Repositories;
using FlightRegistration.Services.BusinessLogic.Interfaces;
using FlightRegistration.Services.BusinessLogic.Implementations;
using FlightRegistration.Server.Services;
using FlightRegistration.Server.Sockets;
using FlightRegistration.Server.Hubs;
using Microsoft.EntityFrameworkCore; // For UseSqlite

var builder = WebApplication.CreateBuilder(args);

// --- CORS Policy Definition ---
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins"; // Define a policy name

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.WithOrigins("https://localhost:7272", "http://localhost:5151") // Add your Blazor app's origin(s)
                                .AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials(); // IMPORTANT for SignalR if using cookies/auth
                      });
});
// --- End CORS Policy Definition ---


// 1. Configure DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(connectionString, b => b.MigrationsAssembly("FlightRegistration.Server"))
);

// 2. Register your repositories for Dependency Injection
builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<IPassengerRepository, PassengerRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<ISeatRepository, SeatRepository>();

// 3. Register your Business Logic services
builder.Services.AddScoped<IFlightService, FlightService>();
builder.Services.AddScoped<ICheckInService, CheckInService>();

// 4. Register AgentNotifier as a Singleton
builder.Services.AddSingleton<AgentNotifier>();
builder.Services.AddSingleton<IAgentNotifier>(sp => sp.GetRequiredService<AgentNotifier>());

// 5. Register AgentSocketServer as a Hosted Service
builder.Services.AddHostedService<AgentSocketServer>();

// 6. Add SignalR Service
builder.Services.AddSignalR();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    // If your Blazor WASM app is hosted by this server project (it's not in your case, it's separate)
    // you might have app.UseWebAssemblyDebugging(); here.
}

app.UseHttpsRedirection();

// --- Middleware Order is Important ---
app.UseRouting(); // 1. Must come before UseCors and UseAuthorization

app.UseCors(MyAllowSpecificOrigins); // 2. APPLY THE CORS POLICY HERE - before Authorization and Endpoints

app.UseAuthorization(); // 3. If you have authorization

// 4. Map your endpoints
app.MapControllers();
app.MapHub<FlightDisplayHub>("/flightDisplayHub");
// If this server also hosts the Blazor app's static files (not in your current setup)
// app.UseBlazorFrameworkFiles();
// app.MapFallbackToFile("index.html");
// --- End Middleware Order ---

app.Run();