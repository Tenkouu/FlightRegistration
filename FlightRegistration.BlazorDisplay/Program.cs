using Microsoft.AspNetCore.Components.Web; // May not be needed if HeadOutlet is only in App.razor
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using FlightRegistration.BlazorDisplay; // For App.razor
using FlightRegistration.BlazorDisplay.Services;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

// This assumes App.razor is your root component.
builder.RootComponents.Add<App>("#app");
// If your App.razor will contain <HeadOutlet />, you don't need to add it here.
// builder.RootComponents.Add<HeadOutlet>("head::after"); 

// In Program.cs
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) }); builder.Services.AddSingleton<FlightDisplayService>(); // Ensure FlightDisplayService.cs exists and is correct

await builder.Build().RunAsync();