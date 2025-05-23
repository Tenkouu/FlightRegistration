// File: FlightRegistration.WinFormsClient/Form1.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing; // Still needed for Size, Padding, and potentially dynamic button colors
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
// System.Text.StringBuilder is removed as ShowBoardingPass is removed
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using FlightRegistration.Core.DTOs;
using FlightRegistration.Core.Models;

namespace FlightRegistration.WinFormsClient
{
    public partial class Form1 : Form
    {
        private AgentSocketClient _socketClient;
        private static readonly HttpClient _httpClient = new HttpClient();

        private BindingList<PassengerBookingDetailsDto> _currentBookingsBindingList;
        private PassengerBookingDetailsDto _selectedBooking;
        private List<SeatDto> _currentFlightSeats;

        // Font for dynamically created seat buttons, if not set by designer inheritance
        private Font _dynamicSeatButtonFont;

        public Form1()
        {
            InitializeComponent(); // This loads your Form1.designer.cs content

            // Initialize dynamic font, attempting Miracode
            try
            {
                _dynamicSeatButtonFont = new Font("Miracode", 9F, FontStyle.Bold);
            }
            catch (ArgumentException)
            {
                _dynamicSeatButtonFont = new Font("Consolas", 9F, FontStyle.Bold); // Fallback
            }


            _socketClient = new AgentSocketClient();
            _socketClient.OnLogMessage += (message) => AddLogMessage($"[Socket] {message}");
            _socketClient.OnDisconnected += HandleSocketDisconnected;
            _socketClient.OnSeatReservedUpdateReceived += HandleSeatReservedUpdate;
            _socketClient.OnFlightStatusUpdateReceived += HandleFlightStatusUpdate;

            _httpClient.BaseAddress = new Uri("https://localhost:7134/"); // !!! ADJUST PORT !!!

            // Initial button states - these should match controls in your designer
            if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false;
            // btnAssignSeatPlaceholder is assumed GONE from your designer.
            // If you add btnManageFlightStatus later, initialize its state here.

            _currentBookingsBindingList = new BindingList<PassengerBookingDetailsDto>();
            InitializeDataGridView();
            ClearFlightDetails();
        }

        public void AddLogMessagePublic(string message)
        {
            AddLogMessage(message);
        }

        private void InitializeDataGridView()
        {
            dgvBookings.AutoGenerateColumns = false;
            dgvBookings.Columns.Clear();
            // Adjusted Widths:
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "BookingIdCol", HeaderText = "BOOKING ID", DataPropertyName = "BookingId", Width = 130 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "PassengerNameCol", HeaderText = "PASSENGER", DataPropertyName = "PassengerName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "FlightNumberCol", HeaderText = "FLIGHT", DataPropertyName = "FlightNumber", Width = 100 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "CurrentSeatCol", HeaderText = "SEAT", DataPropertyName = "CurrentSeatNumber", Width = 100 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "FlightStatusCol", HeaderText = "STATUS", DataPropertyName = "FlightStatus", Width = 150 });

            Color backColor = Color.FromArgb(42, 45, 46);
            Color foreColor = Color.FromArgb(241, 207, 71);
            Color gridColor = Color.FromArgb(60, 63, 65); // A slightly lighter gray for grid lines
            Color selectionBackColor = Color.FromArgb(70, 73, 75); // For selected rows

            dgvBookings.BackgroundColor = backColor; // This is also in your designer, ensures consistency
            dgvBookings.GridColor = gridColor;

            // Default Cell Style (for all data cells)
            dgvBookings.DefaultCellStyle.BackColor = backColor;
            dgvBookings.DefaultCellStyle.ForeColor = foreColor;
            dgvBookings.DefaultCellStyle.SelectionBackColor = selectionBackColor;
            dgvBookings.DefaultCellStyle.SelectionForeColor = foreColor;

            // Column Headers Style
            // IMPORTANT: For these header styles to fully apply, EnableHeadersVisualStyles must be false.
            dgvBookings.EnableHeadersVisualStyles = false;
            dgvBookings.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28); // Slightly darker header
            dgvBookings.ColumnHeadersDefaultCellStyle.ForeColor = foreColor;
            dgvBookings.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvBookings.ColumnHeadersDefaultCellStyle.BackColor; // Keep selection same as normal
            dgvBookings.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvBookings.ColumnHeadersDefaultCellStyle.ForeColor;
            dgvBookings.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single; // Or .None or .Raised

            // Row Headers Style (if visible, often they are not)
            dgvBookings.RowHeadersDefaultCellStyle.BackColor = Color.FromArgb(28, 28, 28);
            dgvBookings.RowHeadersDefaultCellStyle.ForeColor = foreColor;
            dgvBookings.RowHeadersDefaultCellStyle.SelectionBackColor = dgvBookings.RowHeadersDefaultCellStyle.BackColor;
            dgvBookings.RowHeadersDefaultCellStyle.SelectionForeColor = dgvBookings.RowHeadersDefaultCellStyle.ForeColor;
            dgvBookings.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            // Optional: Hide row headers if you don't need them
            dgvBookings.RowHeadersVisible = false;

            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.MultiSelect = false;
            dgvBookings.ReadOnly = true;

            dgvBookings.SelectionChanged -= dgvBookings_SelectionChanged; // Ensure only one subscription
            dgvBookings.SelectionChanged += dgvBookings_SelectionChanged;
            dgvBookings.CellClick -= dgvBookings_CellClick; // Ensure only one subscription
            dgvBookings.CellClick += dgvBookings_CellClick;
        }

        private void AddLogMessage(string message)
        {
            if (lstLogMessages.InvokeRequired) { lstLogMessages.Invoke(new Action<string>(AddLogMessage), message); }
            else { string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}"; lstLogMessages.Items.Insert(0, logEntry); if (lstLogMessages.Items.Count > 200) lstLogMessages.Items.RemoveAt(lstLogMessages.Items.Count - 1); }
        }

        private void HandleSocketDisconnected()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(HandleSocketDisconnected)); return; }
            if (btnConnectSocket != null) btnConnectSocket.Enabled = true;
            if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false;
            AddLogMessage("Socket connection lost or closed.");
        }

        private async void HandleSeatReservedUpdate(SeatReservedUpdatePayload payload)
        {
            AddLogMessage($"UI UPDATE (Seat Reserved): FlightID={payload.FlightId}, SeatNo='{payload.SeatNumber}', ReservedBy='{payload.ReservedByAgentId}'");
            if (_selectedBooking != null && _selectedBooking.FlightId == payload.FlightId)
            {
                var seatToUpdate = _currentFlightSeats?.FirstOrDefault(s => s.Id == payload.SeatId);
                if (seatToUpdate != null) { seatToUpdate.IsReserved = true; seatToUpdate.ReservedForPassengerName = $"Agent: {payload.ReservedByAgentId}"; RenderSeatMapPlaceholder(); }
            }
            var affectedBookingInList = _currentBookingsBindingList.FirstOrDefault(b => b.FlightId == payload.FlightId && string.IsNullOrEmpty(b.CurrentSeatNumber));
            if (affectedBookingInList != null && GetSeatNumberById(payload.FlightId, payload.SeatId) == payload.SeatNumber)
            {
                AddLogMessage($"Seat {payload.SeatNumber} on flight {payload.FlightId} taken. Grid may need manual refresh for specific booking.");
            }
        }
        private string GetSeatNumberById(int flightId, int seatId) => _currentFlightSeats?.FirstOrDefault(s => s.FlightId == flightId && s.Id == seatId)?.SeatNumber;

        private void HandleFlightStatusUpdate(FlightStatusUpdatePayload payload)
        {
            AddLogMessage($"UI UPDATE (Flight Status): FlightNo='{payload.FlightNumber}', NewStatus='{payload.NewStatus}'");
            bool listUpdated = false;
            foreach (var booking in _currentBookingsBindingList.Where(b => b.FlightId == payload.FlightId)) { booking.FlightStatus = (FlightStatus)payload.NewStatusId; listUpdated = true; }
            if (listUpdated) { dgvBookings.Refresh(); AddLogMessage($"Flight status updated for flight {payload.FlightNumber}. Grid refreshed."); }
            if (_selectedBooking != null && _selectedBooking.FlightId == payload.FlightId) { _selectedBooking.FlightStatus = (FlightStatus)payload.NewStatusId; UpdateLabelSafe(lblSelectedFlightInfo, $"Flight: {_selectedBooking.FlightNumber} ({_selectedBooking.DepartureTime:g}) | Status: {_selectedBooking.FlightStatus}"); }
        }

        private void UpdateLabelSafe(Label lbl, string text)
        {
            if (lbl == null) return;
            if (lbl.InvokeRequired) { lbl.Invoke(new Action(() => lbl.Text = text)); } else { lbl.Text = text; }
        }

        private async void btnConnectSocket_Click(object sender, EventArgs e)
        {
            if (!_socketClient.IsConnected) { AddLogMessage("Connecting..."); btnConnectSocket.Enabled = false; btnManageFlightStatus.Enabled = false; await _socketClient.ConnectAsync(); if (_socketClient.IsConnected) { btnManageFlightStatus.Enabled = true; } else { btnConnectSocket.Enabled = true; } } else { AddLogMessage("Already connected."); }
        }
        private void btnDisconnectSocket_Click(object sender, EventArgs e)
        {
            if (_socketClient != null && _socketClient.IsConnected)
            {
                AddLogMessage("Disconnecting from socket server...");
                _socketClient.Disconnect();
                // The OnDisconnected event handler (_socketClient.OnDisconnected += HandleSocketDisconnected;)
                // should take care of updating button states (like re-enabling btnConnectSocket
                // and disabling btnDisconnectSocket) and logging "Socket connection lost or closed."
            }
            else
            {
                AddLogMessage("Not currently connected.");
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) { _socketClient?.Disconnect(); _httpClient?.Dispose(); }

        private async void btnSearchPassenger_Click(object sender, EventArgs e)
        {
            string passport = txtPassportNumber.Text.Trim(); string flightNo = txtFlightNumberSearch.Text.Trim();
            if (string.IsNullOrWhiteSpace(passport) || string.IsNullOrWhiteSpace(flightNo)) { MessageBox.Show("Enter Passport & Flight No.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            _currentBookingsBindingList.Clear();
            try
            {
                HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("api/checkin/search-passenger", new PassengerSearchRequestDto { PassportNumber = passport, FlightNumber = flightNo });
                if (resp.IsSuccessStatusCode) { var res = await resp.Content.ReadFromJsonAsync<List<PassengerBookingDetailsDto>>(); if (res != null && res.Any()) res.ForEach(b => _currentBookingsBindingList.Add(b)); else MessageBox.Show("No bookings found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else { var err = await resp.Content.ReadFromJsonAsync<ErrorResponseDto>(); MessageBox.Show($"API Error: {err?.Message ?? resp.ReasonPhrase}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            catch (Exception ex) { MessageBox.Show($"App error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            finally { RefreshBookingsGrid(); _selectedBooking = null; ClearAndDisableFlightControls(); }
        }
        private void ClearAndDisableFlightControls()
        {
            ClearFlightDetails();
            if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false; // Ensure it's disabled
        }

        private void btnManageFlightStatus_Click(object sender, EventArgs e)
        {
            int? currentFlightId = _selectedBooking?.FlightId;

            // Assuming FlightStatusForm is in the same namespace or you have a using directive for it
            using (FlightStatusForm statusForm = new FlightStatusForm(currentFlightId))
            {
                AddLogMessage($"Opening Manage Flight Status window{(currentFlightId.HasValue ? " for flight ID " + currentFlightId.Value : " (no flight pre-selected)")}...");
                statusForm.ShowDialog(this); // Show as a modal dialog
                AddLogMessage("Manage Flight Status window closed.");
                // Form1's flight status display will update via socket messages triggered by server
                // when FlightStatusForm successfully updates a status.
            }
        }

        private void RefreshBookingsGrid() { if (dgvBookings.InvokeRequired) dgvBookings.Invoke(new Action(RefreshBookingsGrid)); else { dgvBookings.DataSource = null; dgvBookings.DataSource = _currentBookingsBindingList; } }

        private async void dgvBookings_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            AddLogMessage($"CellClick: Row {e.RowIndex}");
            if (e.RowIndex >= 0) { if (!dgvBookings.Rows[e.RowIndex].Selected) { dgvBookings.ClearSelection(); dgvBookings.Rows[e.RowIndex].Selected = true; } var item = dgvBookings.Rows[e.RowIndex].DataBoundItem as PassengerBookingDetailsDto; if (item != null && (_selectedBooking == null || _selectedBooking.BookingId != item.BookingId)) { _selectedBooking = item; await ProcessSelectedBookingAsync(); } else if (item != null && _selectedBooking != null && _selectedBooking.BookingId == item.BookingId) { await ProcessSelectedBookingAsync(); } }
        }
        private async void dgvBookings_SelectionChanged(object sender, EventArgs e)
        {
            AddLogMessage("SelectionChanged fired."); PassengerBookingDetailsDto newItem = null;
            if (dgvBookings.SelectedRows.Count > 0 && dgvBookings.SelectedRows[0].DataBoundItem != null) newItem = dgvBookings.SelectedRows[0].DataBoundItem as PassengerBookingDetailsDto;
            if (_selectedBooking != newItem) { _selectedBooking = newItem; await ProcessSelectedBookingAsync(); }
        }

        private async Task ProcessSelectedBookingAsync()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(async () => await ProcessSelectedBookingAsync())); return; }
            AddLogMessage($"ProcessSelectedBooking: {(_selectedBooking != null ? "BookingID " + _selectedBooking.BookingId : "None")}");
            if (_selectedBooking != null)
            {
                if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = true; // Enable the new button
                UpdateLabelSafe(lblSelectedSeatInfo, string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber) ? "Selected Seat: (None)" : $"Current Seat: {_selectedBooking.CurrentSeatNumber}");
                await LoadFlightDetailsForSelectedBooking();
            }
            else
            {
                if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false; // Disable the new button
                ClearFlightDetails();
            }
        }

        private async Task LoadFlightDetailsForSelectedBooking()
        {
            AddLogMessage($"LoadFlightDetails: START for Booking {(_selectedBooking != null ? _selectedBooking.BookingId.ToString() : "NULL")}");
            if (_selectedBooking == null) { ClearFlightDetails(); return; }
            if (this.InvokeRequired) { this.Invoke(new Action(async () => await LoadFlightDetailsForSelectedBooking())); return; }
            UpdateLabelSafe(lblSelectedFlightInfo, $"Flight: {_selectedBooking.FlightNumber} ({_selectedBooking.DepartureTime:g}) | Status: {_selectedBooking.FlightStatus}");
            UpdateLabelSafe(lblSelectedSeatInfo, string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber) ? "Selected Seat: (None)" : $"Current Seat: {_selectedBooking.CurrentSeatNumber}"); // FIX 5

            try
            {
                HttpResponseMessage resp = await _httpClient.GetAsync($"api/flights/{_selectedBooking.FlightId}");
                if (resp.IsSuccessStatusCode) { var fd = await resp.Content.ReadFromJsonAsync<FlightDetailsDto>(); _currentFlightSeats = fd?.Seats; } else _currentFlightSeats = null;
            }
            catch { _currentFlightSeats = null; }
            finally { RenderSeatMapPlaceholder(); }
        }

        private void RenderSeatMapPlaceholder()
        {
            if (pnlSeatMapPlaceholder.InvokeRequired) { pnlSeatMapPlaceholder.Invoke(new Action(RenderSeatMapPlaceholder)); return; }
            pnlSeatMapPlaceholder.Controls.Clear();
            AddLogMessage($"RenderSeatMap: _selectedBooking ID is {(_selectedBooking != null ? _selectedBooking.BookingId.ToString() : "BOOKING_IS_NULL")}, NeedsSeat: {(_selectedBooking != null ? string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber).ToString() : "N/A")}");
            if (_currentFlightSeats == null || !_currentFlightSeats.Any()) { var lbl = new Label { Text = "No seat data.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter }; /* Style with designer */ pnlSeatMapPlaceholder.Controls.Add(lbl); return; }

            var flowPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(5) };
            // flowPanel.BackColor = ...; // Set by designer or inherited

            foreach (var seat in _currentFlightSeats.OrderBy(s => s.SeatNumber, new NaturalStringComparer()))
            {
                bool canAssign = _selectedBooking != null && string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber);
                bool btnEnabled = !seat.IsReserved && canAssign;
                Button seatBtn = new Button
                {
                    Text = seat.SeatNumber,
                    Tag = seat,
                    Size = new Size(70, 35),
                    Margin = new Padding(3),
                    Enabled = btnEnabled,
                    Font = _dynamicSeatButtonFont, // Use defined font
                    FlatStyle = FlatStyle.Flat,
                    TextAlign = ContentAlignment.MiddleCenter // FIX 4
                };
                // Dynamic colors based on reservation status:
                seatBtn.BackColor = seat.IsReserved ? ColorTranslator.FromHtml("#3e3e42") : Color.DarkSeaGreen; // Example dark theme colors
                seatBtn.ForeColor = seat.IsReserved ? Color.DimGray : ColorTranslator.FromHtml("#eece59");
                seatBtn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eece59");
                seatBtn.FlatAppearance.BorderSize = 1;
                // REMOVED: if (seat.IsReserved) { seatBtn.Text += $"\n(Taken)"; } // FIX 3 (Remove "(Taken)")
                seatBtn.Click += SeatButton_Click;
                flowPanel.Controls.Add(seatBtn);
            }
            pnlSeatMapPlaceholder.Controls.Add(flowPanel);
        }

        private async void SeatButton_Click(object sender, EventArgs e)
        {
            AddLogMessage("SeatButton_Click entered.");
            if (_selectedBooking == null) { MessageBox.Show("Select a booking.", "No Booking", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber)) { MessageBox.Show($"Passenger already has seat {_selectedBooking.CurrentSeatNumber}.", "Seat Assigned", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            Button clickedBtn = sender as Button; SeatDto selSeat = clickedBtn?.Tag as SeatDto;
            if (selSeat == null) return; if (selSeat.IsReserved) { MessageBox.Show($"Seat {selSeat.SeatNumber} is taken.", "Seat Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning); await LoadFlightDetailsForSelectedBooking(); return; }
            UpdateLabelSafe(lblSelectedSeatInfo, $"Selected Seat: {selSeat.SeatNumber}");
            if (MessageBox.Show($"Assign {selSeat.SeatNumber} to {_selectedBooking.PassengerName}?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    HttpResponseMessage resp = await _httpClient.PostAsJsonAsync("api/checkin/assign-seat", new SeatAssignmentRequestDto { BookingId = _selectedBooking.BookingId, SeatId = selSeat.Id });
                    if (resp.IsSuccessStatusCode)
                    {
                        var res = await resp.Content.ReadFromJsonAsync<SeatAssignmentResponseDto>();
                        MessageBox.Show(res?.Message ?? "Seat Assigned!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information); // Boarding pass print removed
                        _selectedBooking.CurrentSeatNumber = selSeat.SeatNumber;
                        selSeat.IsReserved = true;
                        RefreshBookingsGrid();
                        RenderSeatMapPlaceholder();
                        // btnAssignSeatPlaceholder.Enabled = false; // This button is removed
                        UpdateLabelSafe(lblSelectedSeatInfo, $"Assigned: {selSeat.SeatNumber}");
                    }
                    else { var err = await resp.Content.ReadFromJsonAsync<ErrorResponseDto>(); MessageBox.Show($"Failed: {err?.Message ?? resp.ReasonPhrase}", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); await LoadFlightDetailsForSelectedBooking(); }
                }
                catch (Exception ex) { MessageBox.Show($"App error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
            else { UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); }
        }

        // ShowBoardingPass method is REMOVED

        public class SeatAssignmentResponseDto { public string Message { get; set; } public BoardingPassDto BoardingPass { get; set; } }
        public class ErrorResponseDto { public string Message { get; set; } }

        private void ClearFlightDetails()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(ClearFlightDetails)); return; }
            UpdateLabelSafe(lblSelectedFlightInfo, "Selected Flight: (None)");
            if (pnlSeatMapPlaceholder != null)
            {
                pnlSeatMapPlaceholder.Controls.Clear();
                var lblPrompt = new Label { Text = "Search for passenger and select booking.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter };
                // Assuming lblPrompt will inherit font/color from parent or designer
                pnlSeatMapPlaceholder.Controls.Add(lblPrompt);
            }
            UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)");
            _currentFlightSeats = null;
            // if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false; 
        }

        private void grpPassengerSearch_Enter(object sender, EventArgs e)
        {

        }

        private void txtFlightNumberSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }

    public class NaturalStringComparer : IComparer<string>
    {
        [System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);
        public int Compare(string x, string y) { return StrCmpLogicalW(x, y); }
    }
}