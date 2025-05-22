// File: FlightRegistration.BlazorDisplay/Services/FlightDisplayService.cs
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;
using System.Net.Http;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration; // For IConfiguration

namespace FlightRegistration.BlazorDisplay.Services
{
    public class FlightDisplayService : IAsyncDisposable
    {
        private HubConnection? _hubConnection;
        // We won't inject HttpClient directly into the constructor of this singleton.
        // Instead, we'll create one when needed for the initial load, or use IHttpClientFactory if preferred.
        // For simplicity in WASM for a one-off initial load, creating one is okay.
        private readonly IConfiguration _configuration;
        private readonly string _hubUrl;
        private readonly string _apiBaseUrl;

        public List<FlightDetailsDto> CurrentFlights { get; private set; } = new List<FlightDetailsDto>();
        public event Action? OnFlightsUpdated;
        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public FlightDisplayService(IConfiguration configuration) // Removed HttpClient from constructor
        {
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiBaseUrl"] ?? "https://localhost:7134"; // !!! ADJUST IF NEEDED !!!
            _hubUrl = $"{_apiBaseUrl.TrimEnd('/')}/flightDisplayHub";
        }

        public async Task InitializeAsync()
        {
            await LoadInitialFlightsAsync();

            _hubConnection = new HubConnectionBuilder()
                .WithUrl(_hubUrl)
                .WithAutomaticReconnect()
                .Build();

            _hubConnection.On<int, string, string, int>("ReceiveFlightStatusUpdate", (flightId, flightNumber, newStatusString, newStatusId) =>
            {
                var flightToUpdate = CurrentFlights.FirstOrDefault(f => f.Id == flightId);
                if (flightToUpdate != null)
                {
                    flightToUpdate.Status = (FlightStatus)newStatusId;
                    Console.WriteLine($"SignalR: Flight {flightNumber} status updated to {newStatusString}");
                    OnFlightsUpdated?.Invoke();
                }
            });
            // ... other hub handlers ...

            try
            {
                Console.WriteLine($"Attempting to connect to SignalR hub at {_hubUrl}");
                await _hubConnection.StartAsync();
                Console.WriteLine("SignalR connection established.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error starting SignalR connection: {ex.Message}");
            }
        }

        public async Task LoadInitialFlightsAsync()
        {
            // Create a new HttpClient instance just for this operation.
            // In Blazor WASM, this is generally acceptable for services that are singletons
            // and need to make initial calls. The browser handles the underlying connections.
            using var httpClient = new HttpClient();
            try
            {
                string apiUrl = $"{_apiBaseUrl.TrimEnd('/')}/api/flights";
                Console.WriteLine($"Loading initial flights from {apiUrl}");
                var flights = await httpClient.GetFromJsonAsync<List<FlightDetailsDto>>(apiUrl);
                if (flights != null)
                {
                    CurrentFlights = flights;
                    OnFlightsUpdated?.Invoke();
                    Console.WriteLine($"Loaded {CurrentFlights.Count} initial flights.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading initial flights: {ex.Message}");
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnection != null)
            {
                await _hubConnection.DisposeAsync();
            }
        }
    }
}