// File: FlightRegistration.WinFormsClient/Form1.cs
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json; // For ReadFromJsonAsync, PostAsJsonAsync (needs System.Net.Http.Json NuGet)
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlightRegistration.Core.DTOs; // For DTOs
using FlightRegistration.Core.Models; // For FlightStatus enum
using System.ComponentModel; // For BindingList

namespace FlightRegistration.WinFormsClient
{
    public partial class Form1 : Form
    {
        private AgentSocketClient _socketClient;
        private static readonly HttpClient _httpClient = new HttpClient();

        private BindingList<PassengerBookingDetailsDto> _currentBookingsBindingList; // Use BindingList
        private PassengerBookingDetailsDto _selectedBooking;
        private List<SeatDto> _currentFlightSeats;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomUIDesign();

            _socketClient = new AgentSocketClient();
            _socketClient.OnLogMessage += (message) => AddLogMessage($"[Socket] {message}");
            _socketClient.OnDisconnected += HandleSocketDisconnected;
            _socketClient.OnSeatReservedUpdateReceived += HandleSeatReservedUpdate;
            _socketClient.OnFlightStatusUpdateReceived += HandleFlightStatusUpdate;

            _httpClient.BaseAddress = new Uri("https://localhost:7134/"); // !!! ADJUST PORT !!!

            btnDisconnectSocket.Enabled = false;
            btnAssignSeatPlaceholder.Enabled = false;

            _currentBookingsBindingList = new BindingList<PassengerBookingDetailsDto>(); // Initialize
            InitializeDataGridView();
            ClearFlightDetails();
        }

        private void InitializeDataGridView()
        {
            dgvBookings.AutoGenerateColumns = false;
            dgvBookings.Columns.Clear();

            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "BookingIdCol", HeaderText = "Booking ID", DataPropertyName = "BookingId", Width = 80 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "PassengerNameCol", HeaderText = "Passenger", DataPropertyName = "PassengerName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "FlightNumberCol", HeaderText = "Flight", DataPropertyName = "FlightNumber", Width = 70 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "CurrentSeatCol", HeaderText = "Seat", DataPropertyName = "CurrentSeatNumber", Width = 50 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "FlightStatusCol", HeaderText = "Status", DataPropertyName = "FlightStatus", Width = 100 });

            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.MultiSelect = false;
            dgvBookings.ReadOnly = true;

            // Event Handlers
            dgvBookings.SelectionChanged -= dgvBookings_SelectionChanged; // Prevent multiple subscriptions
            dgvBookings.SelectionChanged += dgvBookings_SelectionChanged;

            dgvBookings.CellClick -= dgvBookings_CellClick; // Prevent multiple subscriptions
            dgvBookings.CellClick += dgvBookings_CellClick; // Add CellClick for diagnostics
        }

        private void InitializeCustomUIDesign()
        {
            this.Text = "Flight Check-in Agent Terminal ✈️";
            this.BackColor = Color.AliceBlue;

            if (this.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblTitle") is Label lblTitle)
            {
                lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
                lblTitle.ForeColor = Color.SteelBlue;
                lblTitle.Dock = DockStyle.Top;
                lblTitle.TextAlign = ContentAlignment.MiddleCenter;
                lblTitle.Padding = new Padding(0, 10, 0, 10);
                lblTitle.Text = "Agent Terminal";
            }
            if (this.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "pnlConnection") is Panel pnlConnection)
            {
                pnlConnection.BackColor = Color.LightSteelBlue;
            }

            btnConnectSocket.Text = "🔗 Connect";
            btnConnectSocket.Font = new Font("Segoe UI", 9F);
            btnConnectSocket.BackColor = Color.MediumSeaGreen;
            btnConnectSocket.ForeColor = Color.White;
            btnConnectSocket.FlatStyle = FlatStyle.Flat;
            btnConnectSocket.FlatAppearance.BorderSize = 0;

            btnDisconnectSocket.Text = "🔌 Disconnect";
            btnDisconnectSocket.Font = new Font("Segoe UI", 9F);
            btnDisconnectSocket.BackColor = Color.Tomato;
            btnDisconnectSocket.ForeColor = Color.White;
            btnDisconnectSocket.FlatStyle = FlatStyle.Flat;
            btnDisconnectSocket.FlatAppearance.BorderSize = 0;

            btnSearchPassenger.BackColor = Color.CornflowerBlue;
            btnSearchPassenger.ForeColor = Color.White;
            btnSearchPassenger.FlatStyle = FlatStyle.Flat;
            btnSearchPassenger.FlatAppearance.BorderSize = 0;
            btnSearchPassenger.Font = new Font("Segoe UI", 9F);

            if (lstLogMessages != null)
            {
                try { lstLogMessages.Font = new Font("Miracode", 9F); }
                catch { lstLogMessages.Font = new Font("Consolas", 9F); AddLogMessage("[UI Info] Miracode font not found, using Consolas."); }
                lstLogMessages.BackColor = Color.Ivory;
                lstLogMessages.ForeColor = Color.DarkSlateGray;
                lstLogMessages.HorizontalScrollbar = true;
            }
        }

        private void AddLogMessage(string message)
        {
            if (lstLogMessages.InvokeRequired)
            {
                lstLogMessages.Invoke(new Action<string>(AddLogMessage), message);
            }
            else
            {
                string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}";
                lstLogMessages.Items.Insert(0, logEntry);
                if (lstLogMessages.Items.Count > 200) { lstLogMessages.Items.RemoveAt(lstLogMessages.Items.Count - 1); }
            }
        }

        private void HandleSocketDisconnected()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(HandleSocketDisconnected)); return; }
            btnConnectSocket.Enabled = true;
            btnDisconnectSocket.Enabled = false;
            AddLogMessage("Socket connection lost or closed.");
        }

        private async void HandleSeatReservedUpdate(SeatReservedUpdatePayload payload)
        {
            AddLogMessage($"UI UPDATE (Seat Reserved): FlightID={payload.FlightId}, SeatNo='{payload.SeatNumber}', ReservedBy='{payload.ReservedByAgentId}'");
            if (_selectedBooking != null && _selectedBooking.FlightId == payload.FlightId)
            {
                var seatToUpdate = _currentFlightSeats?.FirstOrDefault(s => s.Id == payload.SeatId);
                if (seatToUpdate != null)
                {
                    seatToUpdate.IsReserved = true;
                    seatToUpdate.ReservedForPassengerName = $"Agent: {payload.ReservedByAgentId}";
                    AddLogMessage($"Seat {seatToUpdate.SeatNumber} on current flight marked as reserved in local data.");
                    RenderSeatMapPlaceholder();
                }
            }
            // If a booking in the grid was affected, its CurrentSeatNumber might need updating.
            // This is hard to do without BookingId in payload. A full refresh might be needed.
            var affectedBookingInList = _currentBookingsBindingList.FirstOrDefault(b => b.FlightId == payload.FlightId && string.IsNullOrEmpty(b.CurrentSeatNumber));
            if (affectedBookingInList != null)
            {
                AddLogMessage($"A seat on flight {payload.FlightId} was reserved. Consider re-searching to update booking details in the grid if necessary.");
                // For now, we don't automatically update the grid row here as we don't know *which* booking got the seat.
            }
        }

        private void HandleFlightStatusUpdate(FlightStatusUpdatePayload payload)
        {
            AddLogMessage($"UI UPDATE (Flight Status): FlightNo='{payload.FlightNumber}', NewStatus='{payload.NewStatus}'");
            bool listUpdated = false;
            foreach (var booking in _currentBookingsBindingList.Where(b => b.FlightId == payload.FlightId))
            {
                booking.FlightStatus = (FlightStatus)payload.NewStatusId;
                listUpdated = true;
            }
            // BindingList should auto-refresh the DGV if properties of items change (if DTOs implement INotifyPropertyChanged)
            // Since PassengerBookingDetailsDto is a simple DTO, we might need to help the grid refresh.
            if (listUpdated)
            {
                dgvBookings.Refresh(); // Try this to repaint
                AddLogMessage($"Flight status updated in booking list for flight {payload.FlightNumber}. Grid refreshed.");
            }

            if (_selectedBooking != null && _selectedBooking.FlightId == payload.FlightId)
            {
                _selectedBooking.FlightStatus = (FlightStatus)payload.NewStatusId;
                UpdateLabelSafe(lblSelectedFlightInfo, $"Selected Flight: {_selectedBooking.FlightNumber} ({_selectedBooking.DepartureTime:g}) - Status: {_selectedBooking.FlightStatus}");
            }
        }

        private void UpdateLabelSafe(Label lbl, string text)
        {
            if (lbl.InvokeRequired) { lbl.Invoke(new Action(() => lbl.Text = text)); }
            else { lbl.Text = text; }
        }


        private async void btnConnectSocket_Click(object sender, EventArgs e)
        {
            if (!_socketClient.IsConnected)
            {
                AddLogMessage("Connecting to socket server...");
                btnConnectSocket.Enabled = false;
                btnDisconnectSocket.Enabled = false;
                await _socketClient.ConnectAsync();
                if (_socketClient.IsConnected) { btnDisconnectSocket.Enabled = true; }
                else { btnConnectSocket.Enabled = true; }
            }
            else { AddLogMessage("Already connected."); }
        }

        private void btnDisconnectSocket_Click(object sender, EventArgs e)
        {
            if (_socketClient.IsConnected) { _socketClient.Disconnect(); }
            else { AddLogMessage("Not currently connected."); }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _socketClient?.Disconnect();
            _httpClient?.Dispose();
        }

        private async void btnSearchPassenger_Click(object sender, EventArgs e)
        {
            string passportNumber = txtPassportNumber.Text.Trim();
            string flightNumber = txtFlightNumberSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(passportNumber) || string.IsNullOrWhiteSpace(flightNumber))
            {
                MessageBox.Show("Please enter both Passport Number and Flight Number.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var searchRequest = new PassengerSearchRequestDto { PassportNumber = passportNumber, FlightNumber = flightNumber };
            _currentBookingsBindingList.Clear(); // Clear previous results

            try
            {
                AddLogMessage($"Searching for bookings: Passport={passportNumber}, Flight={flightNumber}");
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/checkin/search-passenger", searchRequest);

                if (response.IsSuccessStatusCode)
                {
                    var results = await response.Content.ReadFromJsonAsync<List<PassengerBookingDetailsDto>>();
                    if (results != null && results.Any())
                    {
                        AddLogMessage($"Found {results.Count} booking(s).");
                        results.ForEach(b => _currentBookingsBindingList.Add(b));
                    }
                    else
                    {
                        AddLogMessage("No bookings found for the criteria.");
                        MessageBox.Show("No bookings found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    string errorContent = await response.Content.ReadAsStringAsync();
                    AddLogMessage($"Error searching bookings: {response.StatusCode} - {errorContent}");
                    MessageBox.Show($"Error: {response.ReasonPhrase}\n{errorContent}", "API Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"Exception during passenger search: {ex.Message}");
                MessageBox.Show($"An application error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                RefreshBookingsGrid(); // Bind the BindingList
                _selectedBooking = null;
                btnAssignSeatPlaceholder.Enabled = false;
                ClearFlightDetails();
            }
        }

        private void RefreshBookingsGrid()
        {
            if (dgvBookings.InvokeRequired) { dgvBookings.Invoke(new Action(RefreshBookingsGrid)); }
            else
            {
                dgvBookings.DataSource = null; // Important to unbind before rebind if not using BindingList properly or for full refresh
                dgvBookings.DataSource = _currentBookingsBindingList; // Bind to BindingList
            }
        }

        // THIS IS THE NEW EVENT HANDLER
        private void dgvBookings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AddLogMessage($"dgvBookings_CellClick fired. RowIndex: {e.RowIndex}, ColumnIndex: {e.ColumnIndex}");
            if (e.RowIndex >= 0) // Ensure it's not a header click
            {
                // Manually ensure the row is selected, as SelectionChanged might be unreliable
                if (!dgvBookings.Rows[e.RowIndex].Selected)
                {
                    dgvBookings.ClearSelection();
                    dgvBookings.Rows[e.RowIndex].Selected = true;
                    // Forcing SelectionChanged event or manually updating _selectedBooking
                    // The SelectionChanged event *should* fire after this.
                    // If it doesn't, we might need to manually extract the DataBoundItem here.
                    AddLogMessage($"Manually selected row {e.RowIndex} in CellClick. Waiting for SelectionChanged or updating manually.");

                    // If SelectionChanged is truly broken, do the work here:
                    var selectedItem = dgvBookings.Rows[e.RowIndex].DataBoundItem as PassengerBookingDetailsDto;
                    if (selectedItem != null && _selectedBooking != selectedItem) // Check if it's a new selection
                    {
                        _selectedBooking = selectedItem;
                        AddLogMessage($"_selectedBooking updated directly in CellClick. Booking ID: {_selectedBooking.BookingId}");
                        HandleBookingSelection(); // Call the consolidated logic
                    }
                    else if (selectedItem != null && _selectedBooking == selectedItem)
                    {
                        AddLogMessage($"CellClick on already selected booking. Booking ID: {_selectedBooking.BookingId}");
                        // Potentially still call HandleBookingSelection if you want to re-process (e.g. reload seat map)
                        // HandleBookingSelection(); 
                    }
                }
                else
                {
                    AddLogMessage($"Row {e.RowIndex} was already selected according to dgv.");
                    // It's possible SelectionChanged didn't fire if the selection didn't "change" from the DGV's perspective
                    // but our _selectedBooking might be out of sync. Let's ensure it's set.
                    var currentBoundItem = dgvBookings.Rows[e.RowIndex].DataBoundItem as PassengerBookingDetailsDto;
                    if (currentBoundItem != null && _selectedBooking != currentBoundItem)
                    {
                        _selectedBooking = currentBoundItem;
                        AddLogMessage($"_selectedBooking synced in CellClick (was already selected). Booking ID: {_selectedBooking.BookingId}");
                        HandleBookingSelection();
                    }
                    else if (_selectedBooking == null && currentBoundItem != null)
                    { // If _selectedBooking was null but a row is selected
                        _selectedBooking = currentBoundItem;
                        AddLogMessage($"_selectedBooking initialized in CellClick. Booking ID: {_selectedBooking.BookingId}");
                        HandleBookingSelection();
                    }
                }
            }
        }

        private async void dgvBookings_SelectionChanged(object sender, EventArgs e)
        {
            AddLogMessage("dgvBookings_SelectionChanged fired.");
            if (dgvBookings.SelectedRows.Count > 0 && dgvBookings.SelectedRows[0].DataBoundItem != null)
            {
                var newlySelectedItem = dgvBookings.SelectedRows[0].DataBoundItem as PassengerBookingDetailsDto;
                if (_selectedBooking != newlySelectedItem) // Process only if selection actually changed
                {
                    _selectedBooking = newlySelectedItem;
                    AddLogMessage($"_selectedBooking updated in SelectionChanged. Booking ID: {_selectedBooking?.BookingId}");
                    await HandleBookingSelection();
                }
                else
                {
                    AddLogMessage("SelectionChanged fired, but _selectedBooking is the same. No action.");
                }
            }
            else if (dgvBookings.SelectedRows.Count == 0 && _selectedBooking != null) // Selection cleared
            {
                AddLogMessage("Selection cleared in dgvBookings.");
                _selectedBooking = null;
                await HandleBookingSelection(); // This will call ClearFlightDetails
            }
        }

        // New method to consolidate what happens after a booking is selected (or deselected)
        private async Task HandleBookingSelection()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(async () => await HandleBookingSelection())); return; }

            if (_selectedBooking != null)
            {
                AddLogMessage($"Handling selection for Booking ID: {_selectedBooking.BookingId}");
                btnAssignSeatPlaceholder.Enabled = string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber);
                await LoadFlightDetailsForSelectedBooking();
            }
            else
            {
                AddLogMessage("HandleBookingSelection: No booking selected or selection cleared.");
                btnAssignSeatPlaceholder.Enabled = false;
                ClearFlightDetails();
            }
        }


        private async Task LoadFlightDetailsForSelectedBooking()
        {
            if (_selectedBooking == null) { ClearFlightDetails(); return; }
            if (this.InvokeRequired) { this.Invoke(new Action(async () => await LoadFlightDetailsForSelectedBooking())); return; }

            UpdateLabelSafe(lblSelectedFlightInfo, $"Selected Flight: {_selectedBooking.FlightNumber} ({_selectedBooking.DepartureTime:g}) - Status: {_selectedBooking.FlightStatus}");
            UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)");

            try
            {
                AddLogMessage($"Loading flight details for Flight ID: {_selectedBooking.FlightId}");
                HttpResponseMessage response = await _httpClient.GetAsync($"api/flights/{_selectedBooking.FlightId}");
                if (response.IsSuccessStatusCode)
                {
                    var flightDetails = await response.Content.ReadFromJsonAsync<FlightDetailsDto>();
                    _currentFlightSeats = flightDetails?.Seats; // Can be null if no seats
                    AddLogMessage($"Loaded {flightDetails?.Seats?.Count ?? 0} seats for flight {flightDetails?.FlightNumber}.");
                }
                else
                {
                    AddLogMessage($"Failed to load seat map for flight {_selectedBooking.FlightId}: {response.StatusCode}");
                    _currentFlightSeats = null;
                }
            }
            catch (Exception ex)
            {
                AddLogMessage($"Exception loading seat map: {ex.Message}");
                _currentFlightSeats = null;
            }
            finally
            {
                RenderSeatMapPlaceholder(); // Always render, even if empty or error
            }
        }

        private void RenderSeatMapPlaceholder()
        {
            if (pnlSeatMapPlaceholder.InvokeRequired) { pnlSeatMapPlaceholder.Invoke(new Action(RenderSeatMapPlaceholder)); return; }

            pnlSeatMapPlaceholder.Controls.Clear();
            if (_currentFlightSeats == null || !_currentFlightSeats.Any())
            {
                var lblNoSeats = new Label { Text = "No seat data loaded or available for this flight.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.DimGray };
                pnlSeatMapPlaceholder.Controls.Add(lblNoSeats);
                return;
            }

            var flowPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(5), BackColor = Color.White };
            foreach (var seat in _currentFlightSeats.OrderBy(s => s.SeatNumber, new NaturalStringComparer()))
            {
                Button seatButton = new Button
                {
                    Text = seat.SeatNumber,
                    Tag = seat,
                    Size = new Size(60, 40),
                    Margin = new Padding(3),
                    BackColor = seat.IsReserved ? Color.LightCoral : Color.LightGreen,
                    Enabled = !seat.IsReserved && (_selectedBooking != null && string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber)),
                    Font = new Font("Segoe UI", 8f)
                };
                if (seat.IsReserved) { seatButton.Text += $"\n(Taken)"; seatButton.Font = new Font(seatButton.Font, FontStyle.Italic); }
                seatButton.Click += SeatButton_Click;
                flowPanel.Controls.Add(seatButton);
            }
            pnlSeatMapPlaceholder.Controls.Add(flowPanel);
        }

        private async void SeatButton_Click(object sender, EventArgs e)
        {
            if (_selectedBooking == null)
            {
                MessageBox.Show("Please select a booking first.", "No Booking Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning); return;
            }
            if (!string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber))
            {
                MessageBox.Show($"Passenger {_selectedBooking.PassengerName} already has seat {_selectedBooking.CurrentSeatNumber} assigned.", "Seat Already Assigned", MessageBoxButtons.OK, MessageBoxIcon.Information); return;
            }

            Button clickedButton = sender as Button;
            SeatDto selectedSeat = clickedButton?.Tag as SeatDto;

            if (selectedSeat == null) return;
            if (selectedSeat.IsReserved) { MessageBox.Show($"Seat {selectedSeat.SeatNumber} is already reserved.", "Seat Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning); await LoadFlightDetailsForSelectedBooking(); return; }

            UpdateLabelSafe(lblSelectedSeatInfo, $"Selected Seat: {selectedSeat.SeatNumber} (ID: {selectedSeat.Id})");
            var confirmResult = MessageBox.Show($"Assign seat {selectedSeat.SeatNumber} to {_selectedBooking.PassengerName} on flight {_selectedBooking.FlightNumber}?", "Confirm Seat Assignment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmResult == DialogResult.Yes)
            {
                var assignmentRequest = new SeatAssignmentRequestDto { BookingId = _selectedBooking.BookingId, SeatId = selectedSeat.Id };
                try
                {
                    AddLogMessage($"Attempting to assign seat {selectedSeat.SeatNumber} to booking {_selectedBooking.BookingId}");
                    HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/checkin/assign-seat", assignmentRequest);
                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadFromJsonAsync<SeatAssignmentResponseDto>();
                        AddLogMessage($"Seat assigned successfully: {result?.Message}");
                        MessageBox.Show(result?.Message ?? "Seat assigned!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        _selectedBooking.CurrentSeatNumber = selectedSeat.SeatNumber; // Update local DTO
                        selectedSeat.IsReserved = true; // Update local seat DTO
                        RefreshBookingsGrid();      // Update grid
                        RenderSeatMapPlaceholder(); // Update seat map
                        btnAssignSeatPlaceholder.Enabled = false;
                        UpdateLabelSafe(lblSelectedSeatInfo, $"Assigned: {selectedSeat.SeatNumber}");
                    }
                    else
                    {
                        var errorResult = await response.Content.ReadFromJsonAsync<ErrorResponseDto>();
                        string errorMessage = errorResult?.Message ?? await response.Content.ReadAsStringAsync();
                        AddLogMessage($"Failed to assign seat: {response.StatusCode} - {errorMessage}");
                        MessageBox.Show($"Failed to assign seat: {errorMessage}", "Assignment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)");
                        await LoadFlightDetailsForSelectedBooking();
                    }
                }
                catch (Exception ex)
                {
                    AddLogMessage($"Exception during seat assignment: {ex.Message}");
                    MessageBox.Show($"An application error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else { UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); }
        }

        public class SeatAssignmentResponseDto { public string Message { get; set; } public BoardingPassDto BoardingPass { get; set; } }
        public class ErrorResponseDto { public string Message { get; set; } }

        private void ClearFlightDetails()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(ClearFlightDetails)); return; }
            UpdateLabelSafe(lblSelectedFlightInfo, "Selected Flight: (None)");
            pnlSeatMapPlaceholder.Controls.Clear();
            var lblPrompt = new Label { Text = "Search for a passenger and select a booking to see flight details.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = Color.DimGray };
            pnlSeatMapPlaceholder.Controls.Add(lblPrompt);
            UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)");
            _currentFlightSeats = null;
        }
    }

    public class NaturalStringComparer : IComparer<string>
    {
        [System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);
        public int Compare(string x, string y) { return StrCmpLogicalW(x, y); }
    }
}