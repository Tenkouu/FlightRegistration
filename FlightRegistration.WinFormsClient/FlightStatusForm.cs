// File: FlightRegistration.WinFormsClient/FlightStatusForm.cs
using System;
using System.Drawing; // Still needed for Font object if used for dynamic elements (none here)
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json; // For JsonSerializer (if needed for error parsing)
using System.Threading.Tasks;
using System.Windows.Forms;
using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;

namespace FlightRegistration.WinFormsClient
{
    public partial class FlightStatusForm : Form
    {
        private readonly HttpClient _httpClient; // Each instance of this form gets its own HttpClient
        private FlightDetailsDto _currentFlightDetails;

        public FlightStatusForm() // Removed initialFlightId, it's fully standalone
        {
            InitializeComponent(); // Loads YOUR designer settings

            // HttpClient for this form instance
            _httpClient = new HttpClient { BaseAddress = new Uri("https://localhost:7134/") }; // !!! ADJUST PORT !!!

            PopulateFlightStatusComboBox();
            grpUpdateStatusControls.Enabled = false; // Disable update section initially
        }

        private void PopulateFlightStatusComboBox()
        {
            cmbNewStatus.DataSource = Enum.GetValues(typeof(FlightStatus));
            cmbNewStatus.SelectedIndex = -1; // No initial selection
        }

        private async void btnSearchFlight_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtFlightIdSearch.Text.Trim(), out int flightId) || flightId <= 0)
            {
                MessageBox.Show("Please enter a valid numeric Flight ID.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearFlightDisplay();
                return;
            }

            ClearFlightDisplay(); // Clear previous details
            Console.WriteLine($"FlightStatusForm: Searching for flight ID {flightId}"); // Simple console log

            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync($"api/flights/{flightId}");
                if (response.IsSuccessStatusCode)
                {
                    _currentFlightDetails = await response.Content.ReadFromJsonAsync<FlightDetailsDto>();
                    if (_currentFlightDetails != null)
                    {
                        DisplayFlightDetails();
                        grpUpdateStatusControls.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show($"Flight with ID {flightId} not found.", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show($"Error fetching flight: {response.ReasonPhrase} (Status: {response.StatusCode})", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An application error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ClearFlightDisplay();
            }
        }

        private void DisplayFlightDetails()
        {
            if (_currentFlightDetails == null) { ClearFlightDisplay(); return; }

            lblLoadedFlightNumberValue.Text = _currentFlightDetails.FlightNumber;
            // Assuming you have these labels in your designer:
            if (lblLoadedDepartureStatic != null) lblLoadedDepartureValue.Text = _currentFlightDetails.DepartureCity; // Just city
            if (lblLoadedDepTimeStatic != null) lblLoadedDepTimeValue.Text = _currentFlightDetails.DepartureTime.ToString("g"); // Date and Time

            if (lblLoadedArrivalStatic != null) lblLoadedArrivalValue.Text = _currentFlightDetails.ArrivalCity; // Just city
            if (lblLoadedArrTimeStatic != null) lblLoadedArrTimeValue.Text = _currentFlightDetails.ArrivalTime.ToString("g"); // Date and Time

            lblLoadedCurrentStatusValue.Text = _currentFlightDetails.Status.ToString();
            cmbNewStatus.SelectedItem = _currentFlightDetails.Status;
        }

        private void ClearFlightDisplay()
        {
            _currentFlightDetails = null;
            lblLoadedFlightNumberValue.Text = "N/A";
            lblLoadedDepartureValue.Text = "N/A";
            if (lblLoadedDepTimeValue != null) lblLoadedDepTimeValue.Text = "N/A";
            lblLoadedArrivalValue.Text = "N/A";
            if (lblLoadedArrTimeValue != null) lblLoadedArrTimeValue.Text = "N/A";
            lblLoadedCurrentStatusValue.Text = "N/A";
            cmbNewStatus.SelectedIndex = -1;
            grpUpdateStatusControls.Enabled = false;
        }

        private async void btnUpdateStatusAction_Click(object sender, EventArgs e)
        {
            if (_currentFlightDetails == null)
            {
                MessageBox.Show("Please search and load a flight first.", "No Flight Loaded", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbNewStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a new status for the flight.", "Status Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            FlightStatus newStatus = (FlightStatus)cmbNewStatus.SelectedItem;
            if (newStatus == _currentFlightDetails.Status)
            {
                MessageBox.Show("New status is the same as the current status. No change made.", "No Change", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmResult = MessageBox.Show($"Change status of flight {_currentFlightDetails.FlightNumber} from '{_currentFlightDetails.Status}' to '{newStatus}'?",
                                                "Confirm Flight Status Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var statusUpdateRequest = new FlightStatusUpdateRequestDto
                {
                    FlightId = _currentFlightDetails.Id,
                    NewStatus = newStatus
                };
                Console.WriteLine($"FlightStatusForm: Attempting update for flight {_currentFlightDetails.FlightNumber} to {newStatus}");
                try
                {
                    HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/flights/{_currentFlightDetails.Id}/status", statusUpdateRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        MessageBox.Show($"Flight {_currentFlightDetails.FlightNumber} status successfully updated to {newStatus}.\nOther displays will update via real-time connection.", "Status Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _currentFlightDetails.Status = newStatus; // Update local DTO
                        DisplayFlightDetails(); // Refresh displayed current status on this form
                        // No DialogResult needed as it's standalone and not returning to a caller expecting it.
                    }
                    else
                    {
                        // Attempt to parse a structured error, fallback to string content
                        string errorMessage = response.ReasonPhrase.ToString(); // Default message
                        if (response.Content != null &&
                            response.Content.Headers.ContentType?.MediaType == "application/json")
                        {
                            try
                            {
                                var errorDto = await response.Content.ReadFromJsonAsync<Form1.ErrorResponseDto>(); // Assuming ErrorResponseDto is accessible
                                if (!string.IsNullOrEmpty(errorDto?.Message))
                                {
                                    errorMessage = errorDto.Message;
                                }
                            }
                            catch (JsonException)
                            { /* Failed to parse, use ReasonPhrase or full content */
                                errorMessage = await response.Content.ReadAsStringAsync();
                            }
                        }
                        else if (response.Content != null)
                        {
                            errorMessage = await response.Content.ReadAsStringAsync();
                        }
                        MessageBox.Show($"Failed to update status: {errorMessage}", "Update Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"An application error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnFSFClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFlightIdSearch_TextChanged(object sender, EventArgs e)
        {

        }

        // If Form1.ErrorResponseDto is not accessible (e.g. because it's a nested public class in Form1)
        // and you want to keep FlightStatusForm truly separate, define ErrorResponseDto in Core.DTOs
        // or define a local one here if it's only for this form's error parsing.
        // For example, if needed locally:
        // public class ErrorResponseDto { public string Message { get; set; } }
    }
}