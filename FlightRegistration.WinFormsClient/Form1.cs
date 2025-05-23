// File: FlightRegistration.WinFormsClient/Form1.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
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
        // Use this single static instance for all HTTP calls from Form1
        // and make it accessible if FlightStatusForm (or other forms) need it.
        public static readonly HttpClient SharedHttpClient = new HttpClient();

        private BindingList<PassengerBookingDetailsDto> _currentBookingsBindingList;
        private PassengerBookingDetailsDto _selectedBooking;
        private List<SeatDto> _currentFlightSeats;
        private BoardingPassPrinter _boardingPassPrinter; // For printing

        private Font _dynamicSeatButtonFont;
        private Font _miracodeFont; // For dynamic labels like in ClearFlightDetails

        public static Form1 Instance { get; private set; } // For logging from other forms

        public Form1()
        {
            InitializeComponent(); // This loads your Form1.designer.cs content
            Instance = this;       // Set the static instance

            try
            {
                _dynamicSeatButtonFont = new Font("Miracode", 9F, FontStyle.Bold);
                _miracodeFont = new Font("Miracode", 9F);
            }
            catch (ArgumentException)
            {
                _dynamicSeatButtonFont = new Font("Consolas", 9F, FontStyle.Bold); // Fallback
                _miracodeFont = new Font("Consolas", 9F);
            }

            // Call DGV Styling method AFTER InitializeComponent
            StyleDataGridViewProgrammatically();

            _socketClient = new AgentSocketClient();
            _socketClient.OnLogMessage += (message) => AddLogMessage($"[Socket] {message}");
            _socketClient.OnDisconnected += HandleSocketDisconnected;
            _socketClient.OnSeatReservedUpdateReceived += HandleSeatReservedUpdate;
            _socketClient.OnFlightStatusUpdateReceived += HandleFlightStatusUpdate;

            SharedHttpClient.BaseAddress = new Uri("https://localhost:7134/"); // !!! ADJUST PORT IF NEEDED !!!

            if (btnDisconnectSocket != null) btnDisconnectSocket.Enabled = false;
            if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false;

            _currentBookingsBindingList = new BindingList<PassengerBookingDetailsDto>();
            InitializeDataGridViewColumns(); // Your method to set up DGV columns and event handlers
            ClearFlightDetails();

            _boardingPassPrinter = new BoardingPassPrinter(); // Initialize the printer
        }

        public void AddLogMessagePublic(string message) // For FlightStatusForm to log here
        {
            AddLogMessage(message);
        }

        private void StyleDataGridViewProgrammatically()
        {
            if (dgvBookings == null) return;
            Color dgvBackColor = ColorTranslator.FromHtml("#1e1e1e");
            Color dgvCellBackColor = ColorTranslator.FromHtml("#252526");
            Color dgvAltCellBackColor = ColorTranslator.FromHtml("#2d2d30");
            Color dgvTextColor = ColorTranslator.FromHtml("#eece59");
            Color dgvHeaderBackColor = ColorTranslator.FromHtml("#2a2d2e");
            Color dgvGridColor = ColorTranslator.FromHtml("#4a4a4a");
            Color dgvSelectionBackColor = Color.DarkSlateGray;
            Color dgvSelectionForeColor = Color.White;
            Font dgvCellFont = new Font("Miracode", 9F); // Assuming this is your desired cell font
            Font dgvHeaderFont = new Font("Miracode", 9F, FontStyle.Bold); // Assuming this for headers

            dgvBookings.SuspendLayout();
            dgvBookings.BackgroundColor = dgvBackColor;
            dgvBookings.GridColor = dgvGridColor;
            dgvBookings.BorderStyle = BorderStyle.FixedSingle;
            dgvBookings.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvBookings.RowHeadersVisible = false;
            dgvBookings.EnableHeadersVisualStyles = false;
            dgvBookings.ColumnHeadersDefaultCellStyle.BackColor = dgvHeaderBackColor;
            dgvBookings.ColumnHeadersDefaultCellStyle.ForeColor = dgvTextColor;
            dgvBookings.ColumnHeadersDefaultCellStyle.Font = dgvHeaderFont;
            dgvBookings.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvHeaderBackColor;
            dgvBookings.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvTextColor;
            dgvBookings.ColumnHeadersDefaultCellStyle.Padding = new Padding(4, 2, 4, 2);
            dgvBookings.ColumnHeadersHeight = 28;
            dgvBookings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvBookings.DefaultCellStyle.BackColor = dgvCellBackColor;
            dgvBookings.DefaultCellStyle.ForeColor = dgvTextColor;
            dgvBookings.DefaultCellStyle.Font = dgvCellFont;
            dgvBookings.DefaultCellStyle.SelectionBackColor = dgvSelectionBackColor;
            dgvBookings.DefaultCellStyle.SelectionForeColor = dgvSelectionForeColor;
            dgvBookings.DefaultCellStyle.Padding = new Padding(3);
            dgvBookings.AlternatingRowsDefaultCellStyle.BackColor = dgvAltCellBackColor;
            dgvBookings.AlternatingRowsDefaultCellStyle.ForeColor = dgvTextColor;
            dgvBookings.AlternatingRowsDefaultCellStyle.Font = dgvCellFont;
            dgvBookings.AlternatingRowsDefaultCellStyle.SelectionBackColor = dgvSelectionBackColor;
            dgvBookings.AlternatingRowsDefaultCellStyle.SelectionForeColor = dgvSelectionForeColor;
            dgvBookings.ResumeLayout();
        }

        private void InitializeDataGridViewColumns() // This was your InitializeDataGridView method
        {
            dgvBookings.AutoGenerateColumns = false;
            dgvBookings.Columns.Clear();
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "BookingIdCol", HeaderText = "BOOKING ID", DataPropertyName = "BookingId", Width = 130 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "PassengerNameCol", HeaderText = "PASSENGER", DataPropertyName = "PassengerName", AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "FlightNumberCol", HeaderText = "FLIGHT", DataPropertyName = "FlightNumber", Width = 100 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "CurrentSeatCol", HeaderText = "SEAT", DataPropertyName = "CurrentSeatNumber", Width = 100 });
            dgvBookings.Columns.Add(new DataGridViewTextBoxColumn { Name = "FlightStatusCol", HeaderText = "STATUS", DataPropertyName = "FlightStatus", Width = 150 });
            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.MultiSelect = false; dgvBookings.ReadOnly = true;
            dgvBookings.SelectionChanged += dgvBookings_SelectionChanged;
            dgvBookings.CellClick += dgvBookings_CellClick;
        }

        private void AddLogMessage(string message) { if (lstLogMessages.InvokeRequired) { lstLogMessages.Invoke(new Action<string>(AddLogMessage), message); } else { string logEntry = $"[{DateTime.Now:HH:mm:ss}] {message}"; lstLogMessages.Items.Insert(0, logEntry); if (lstLogMessages.Items.Count > 200) lstLogMessages.Items.RemoveAt(lstLogMessages.Items.Count - 1); } }
        private void HandleSocketDisconnected() { if (this.InvokeRequired) { this.Invoke(new Action(HandleSocketDisconnected)); return; } if (btnConnectSocket != null) btnConnectSocket.Enabled = true; if (btnDisconnectSocket != null) btnDisconnectSocket.Enabled = false; AddLogMessage("Socket connection lost or closed."); }
        private async void HandleSeatReservedUpdate(SeatReservedUpdatePayload payload) { AddLogMessage($"UI UPDATE (Seat Reserved): FlightID={payload.FlightId}, SeatNo='{payload.SeatNumber}', ReservedBy='{payload.ReservedByAgentId}'"); if (_selectedBooking != null && _selectedBooking.FlightId == payload.FlightId) { var seatToUpdate = _currentFlightSeats?.FirstOrDefault(s => s.Id == payload.SeatId); if (seatToUpdate != null) { seatToUpdate.IsReserved = true; seatToUpdate.ReservedForPassengerName = $"Agent: {payload.ReservedByAgentId}"; RenderSeatMapPlaceholder(); } } var affectedBookingInList = _currentBookingsBindingList.FirstOrDefault(b => b.FlightId == payload.FlightId && string.IsNullOrEmpty(b.CurrentSeatNumber)); if (affectedBookingInList != null && GetSeatNumberById(payload.FlightId, payload.SeatId) == payload.SeatNumber) { AddLogMessage($"Seat {payload.SeatNumber} on flight {payload.FlightId} taken. Grid may need manual refresh for specific booking."); } }
        private string GetSeatNumberById(int flightId, int seatId) => _currentFlightSeats?.FirstOrDefault(s => s.FlightId == flightId && s.Id == seatId)?.SeatNumber;
        private void HandleFlightStatusUpdate(FlightStatusUpdatePayload payload) { AddLogMessage($"UI UPDATE (Flight Status): FlightNo='{payload.FlightNumber}', NewStatus='{payload.NewStatus}'"); bool listUpdated = false; foreach (var booking in _currentBookingsBindingList.Where(b => b.FlightId == payload.FlightId)) { booking.FlightStatus = (FlightStatus)payload.NewStatusId; listUpdated = true; } if (listUpdated) { dgvBookings.Refresh(); AddLogMessage($"Flight status updated for flight {payload.FlightNumber}. Grid refreshed."); } if (_selectedBooking != null && _selectedBooking.FlightId == payload.FlightId) { _selectedBooking.FlightStatus = (FlightStatus)payload.NewStatusId; UpdateLabelSafe(lblSelectedFlightInfo, $"Flight: {_selectedBooking.FlightNumber} ({_selectedBooking.DepartureTime:g}) | Status: {_selectedBooking.FlightStatus}"); } }
        private void UpdateLabelSafe(Label lbl, string text) { if (lbl == null) return; if (lbl.InvokeRequired) { lbl.Invoke(new Action(() => lbl.Text = text)); } else { lbl.Text = text; } }
        private async void btnConnectSocket_Click(object sender, EventArgs e) { if (!_socketClient.IsConnected) { AddLogMessage("Connecting..."); btnConnectSocket.Enabled = false; btnDisconnectSocket.Enabled = false; await _socketClient.ConnectAsync(); if (_socketClient.IsConnected) { btnDisconnectSocket.Enabled = true; } else { btnConnectSocket.Enabled = true; } } else { AddLogMessage("Already connected."); } }
        private void btnDisconnectSocket_Click(object sender, EventArgs e) { if (_socketClient.IsConnected) _socketClient.Disconnect(); else AddLogMessage("Not connected."); }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e) { _socketClient?.Disconnect(); SharedHttpClient?.Dispose(); } // Use SharedHttpClient
        private async void btnSearchPassenger_Click(object sender, EventArgs e) { string passport = txtPassportNumber.Text.Trim(); string flightNo = txtFlightNumberSearch.Text.Trim(); if (string.IsNullOrWhiteSpace(passport) || string.IsNullOrWhiteSpace(flightNo)) { MessageBox.Show("Enter Passport & Flight No.", "Input Required", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; } _currentBookingsBindingList.Clear(); try { HttpResponseMessage resp = await SharedHttpClient.PostAsJsonAsync("api/checkin/search-passenger", new PassengerSearchRequestDto { PassportNumber = passport, FlightNumber = flightNo }); if (resp.IsSuccessStatusCode) { var res = await resp.Content.ReadFromJsonAsync<List<PassengerBookingDetailsDto>>(); if (res != null && res.Any()) res.ForEach(b => _currentBookingsBindingList.Add(b)); else MessageBox.Show("No bookings found.", "Search Result", MessageBoxButtons.OK, MessageBoxIcon.Information); } else { var errText = await resp.Content.ReadAsStringAsync(); try { var errDto = JsonSerializer.Deserialize<ErrorResponseDto>(errText); MessageBox.Show($"API Error: {errDto?.Message ?? resp.ReasonPhrase.ToString()}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } catch { MessageBox.Show($"API Error: {resp.ReasonPhrase} - {errText}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } } } catch (Exception ex) { MessageBox.Show($"App error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); } finally { RefreshBookingsGrid(); _selectedBooking = null; ClearAndDisableFlightControls(); } }
        private void ClearAndDisableFlightControls() { ClearFlightDetails(); if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false; }
        private void RefreshBookingsGrid() { if (dgvBookings.InvokeRequired) dgvBookings.Invoke(new Action(RefreshBookingsGrid)); else { dgvBookings.DataSource = null; dgvBookings.DataSource = _currentBookingsBindingList; } }
        private async void dgvBookings_CellClick(object sender, DataGridViewCellEventArgs e) { AddLogMessage($"CellClick: Row {e.RowIndex}"); if (e.RowIndex >= 0) { if (!dgvBookings.Rows[e.RowIndex].Selected) { dgvBookings.ClearSelection(); dgvBookings.Rows[e.RowIndex].Selected = true; } var item = dgvBookings.Rows[e.RowIndex].DataBoundItem as PassengerBookingDetailsDto; if (item != null && (_selectedBooking == null || _selectedBooking.BookingId != item.BookingId)) { _selectedBooking = item; await ProcessSelectedBookingAsync(); } else if (item != null && _selectedBooking != null && _selectedBooking.BookingId == item.BookingId) { await ProcessSelectedBookingAsync(); } } }
        private async void dgvBookings_SelectionChanged(object sender, EventArgs e) { AddLogMessage("SelectionChanged fired."); PassengerBookingDetailsDto newItem = null; if (dgvBookings.SelectedRows.Count > 0 && dgvBookings.SelectedRows[0].DataBoundItem != null) newItem = dgvBookings.SelectedRows[0].DataBoundItem as PassengerBookingDetailsDto; if (_selectedBooking != newItem) { _selectedBooking = newItem; await ProcessSelectedBookingAsync(); } }

        private async Task ProcessSelectedBookingAsync()
        {
            if (this.InvokeRequired) { this.Invoke(new Action(async () => await ProcessSelectedBookingAsync())); return; }
            AddLogMessage($"ProcessSelectedBooking: {(_selectedBooking != null ? "BookingID " + _selectedBooking.BookingId : "None")}");
            if (_selectedBooking != null)
            {
                if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = true;
                UpdateLabelSafe(lblSelectedSeatInfo, string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber) ? "Selected Seat: (None)" : $"Current Seat: {_selectedBooking.CurrentSeatNumber}");
                await LoadFlightDetailsForSelectedBooking();
            }
            else
            {
                if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false;
                ClearFlightDetails();
            }
        }
        private async Task LoadFlightDetailsForSelectedBooking() { AddLogMessage($"LoadFlightDetails: START for Booking {(_selectedBooking != null ? _selectedBooking.BookingId.ToString() : "NULL")}"); if (_selectedBooking == null) { ClearFlightDetails(); return; } if (this.InvokeRequired) { this.Invoke(new Action(async () => await LoadFlightDetailsForSelectedBooking())); return; } UpdateLabelSafe(lblSelectedFlightInfo, $"Flight: {_selectedBooking.FlightNumber} ({_selectedBooking.DepartureTime:g}) | Status: {_selectedBooking.FlightStatus}"); UpdateLabelSafe(lblSelectedSeatInfo, string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber) ? "Selected Seat: (None)" : $"Current Seat: {_selectedBooking.CurrentSeatNumber}"); try { HttpResponseMessage resp = await SharedHttpClient.GetAsync($"api/flights/{_selectedBooking.FlightId}"); if (resp.IsSuccessStatusCode) { var fd = await resp.Content.ReadFromJsonAsync<FlightDetailsDto>(); _currentFlightSeats = fd?.Seats; } else _currentFlightSeats = null; } catch { _currentFlightSeats = null; } finally { RenderSeatMapPlaceholder(); } }
        private void RenderSeatMapPlaceholder() { if (pnlSeatMapPlaceholder.InvokeRequired) { pnlSeatMapPlaceholder.Invoke(new Action(RenderSeatMapPlaceholder)); return; } pnlSeatMapPlaceholder.Controls.Clear(); AddLogMessage($"RenderSeatMap: _selectedBooking ID is {(_selectedBooking != null ? _selectedBooking.BookingId.ToString() : "BOOKING_IS_NULL")}, NeedsSeat: {(_selectedBooking != null ? string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber).ToString() : "N/A")}"); if (_currentFlightSeats == null || !_currentFlightSeats.Any()) { var lbl = new Label { Text = "No seat data.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = ColorTranslator.FromHtml("#eece59"), Font = _miracodeFont, BackColor = Color.Transparent }; pnlSeatMapPlaceholder.Controls.Add(lbl); return; } var flowPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoScroll = true, Padding = new Padding(5), BackColor = ColorTranslator.FromHtml("#1e1e1e") }; foreach (var seat in _currentFlightSeats.OrderBy(s => s.SeatNumber, new NaturalStringComparer())) { bool canAssign = _selectedBooking != null && string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber); bool btnEnabled = !seat.IsReserved && canAssign; Button seatBtn = new Button { Text = seat.SeatNumber, Tag = seat, Size = new Size(70, 35), Margin = new Padding(3), Enabled = btnEnabled, Font = _dynamicSeatButtonFont, FlatStyle = FlatStyle.Flat, TextAlign = ContentAlignment.MiddleCenter }; seatBtn.BackColor = seat.IsReserved ? ColorTranslator.FromHtml("#3e3e42") : Color.DarkSeaGreen; seatBtn.ForeColor = seat.IsReserved ? Color.DimGray : ColorTranslator.FromHtml("#eece59"); seatBtn.FlatAppearance.BorderColor = ColorTranslator.FromHtml("#eece59"); seatBtn.FlatAppearance.BorderSize = 1; seatBtn.Click += SeatButton_Click; flowPanel.Controls.Add(seatBtn); } pnlSeatMapPlaceholder.Controls.Add(flowPanel); }

        private async void SeatButton_Click(object sender, EventArgs e)
        {
            AddLogMessage("SeatButton_Click entered.");
            if (_selectedBooking == null) { MessageBox.Show("Please select a booking first before choosing a seat.", "No Booking Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            if (!string.IsNullOrEmpty(_selectedBooking.CurrentSeatNumber)) { MessageBox.Show($"Passenger {_selectedBooking.PassengerName} already has seat {_selectedBooking.CurrentSeatNumber} assigned. Cannot assign a new seat.", "Seat Already Assigned", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            Button clickedButton = sender as Button; if (clickedButton == null) { AddLogMessage("Error: Clicked sender was not a Button."); return; }
            SeatDto selectedSeat = clickedButton.Tag as SeatDto; if (selectedSeat == null) { AddLogMessage("Error: Button tag did not contain valid SeatDto."); return; }
            if (selectedSeat.IsReserved) { MessageBox.Show($"Seat {selectedSeat.SeatNumber} is already taken or reserved. Please refresh or choose another.", "Seat Taken", MessageBoxButtons.OK, MessageBoxIcon.Warning); await LoadFlightDetailsForSelectedBooking(); return; }
            UpdateLabelSafe(lblSelectedSeatInfo, $"Selected Seat: {selectedSeat.SeatNumber}");
            DialogResult confirmResult = MessageBox.Show($"Assign seat {selectedSeat.SeatNumber} to passenger '{_selectedBooking.PassengerName}' for flight {_selectedBooking.FlightNumber}?", "Confirm Seat Assignment", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirmResult == DialogResult.Yes)
            {
                var assignmentRequest = new SeatAssignmentRequestDto { BookingId = _selectedBooking.BookingId, SeatId = selectedSeat.Id };
                try
                {
                    AddLogMessage($"Attempting to assign seat {selectedSeat.SeatNumber} (ID: {selectedSeat.Id}) to booking {_selectedBooking.BookingId}");
                    HttpResponseMessage response = await SharedHttpClient.PostAsJsonAsync("api/checkin/assign-seat", assignmentRequest); // Use SharedHttpClient
                    if (response.IsSuccessStatusCode)
                    {
                        SeatAssignmentResponseDto apiResponse = null;
                        try { apiResponse = await response.Content.ReadFromJsonAsync<SeatAssignmentResponseDto>(); }
                        catch (JsonException jsonEx) { AddLogMessage($"Error deserializing seat assignment response: {jsonEx.Message}"); MessageBox.Show("Seat assigned, but response data could not be read.", "Assignment Partial Success", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                        AddLogMessage($"Seat assignment API success: {apiResponse?.Message ?? "No message from server."}");
                        if (apiResponse?.BoardingPass != null)
                        {
                            _boardingPassPrinter.PrintBoardingPass(apiResponse.BoardingPass, true); // Show preview
                        }
                        else { MessageBox.Show(apiResponse?.Message ?? "Seat assigned successfully! (Boarding pass details not available)", "Seat Assigned", MessageBoxButtons.OK, MessageBoxIcon.Information); AddLogMessage("Seat assigned, but no boarding pass data in API response."); }
                        _selectedBooking.CurrentSeatNumber = selectedSeat.SeatNumber; selectedSeat.IsReserved = true;
                        RefreshBookingsGrid(); RenderSeatMapPlaceholder(); UpdateLabelSafe(lblSelectedSeatInfo, $"Assigned: {selectedSeat.SeatNumber}");
                    }
                    else
                    {
                        string errorMessage = response.ReasonPhrase.ToString();
                        try { var errorDto = await response.Content.ReadFromJsonAsync<ErrorResponseDto>(); if (!string.IsNullOrEmpty(errorDto?.Message)) errorMessage = errorDto.Message; }
                        catch (JsonException) { errorMessage = await response.Content.ReadAsStringAsync(); }
                        catch (Exception readEx) { AddLogMessage($"Error reading error response content: {readEx.Message}"); }
                        AddLogMessage($"Failed to assign seat: {response.StatusCode} - {errorMessage}"); MessageBox.Show($"Failed to assign seat: {errorMessage}", "Assignment Failed", MessageBoxButtons.OK, MessageBoxIcon.Error); UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); await LoadFlightDetailsForSelectedBooking();
                    }
                }
                catch (HttpRequestException httpEx) { AddLogMessage($"Network error during seat assignment: {httpEx.Message}"); MessageBox.Show($"A network error occurred: {httpEx.Message}. Please check server connection.", "Network Error", MessageBoxButtons.OK, MessageBoxIcon.Error); UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); }
                catch (Exception ex) { AddLogMessage($"Unexpected error during seat assignment: {ex.Message}"); MessageBox.Show($"An application error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); }
            }
            else { UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); }
        }

        public class SeatAssignmentResponseDto { public string Message { get; set; } public BoardingPassDto BoardingPass { get; set; } }
        public class ErrorResponseDto { public string Message { get; set; } }
        private void ClearFlightDetails() { if (this.InvokeRequired) { this.Invoke(new Action(ClearFlightDetails)); return; } UpdateLabelSafe(lblSelectedFlightInfo, "Selected Flight: (None)"); if (pnlSeatMapPlaceholder != null) { pnlSeatMapPlaceholder.Controls.Clear(); var lblPrompt = new Label { Text = "Search for passenger and select booking.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, ForeColor = ColorTranslator.FromHtml("#eece59"), Font = _miracodeFont, BackColor = Color.Transparent }; pnlSeatMapPlaceholder.Controls.Add(lblPrompt); } UpdateLabelSafe(lblSelectedSeatInfo, "Selected Seat: (None)"); _currentFlightSeats = null; if (btnManageFlightStatus != null) btnManageFlightStatus.Enabled = false; }
        private void grpPassengerSearch_Enter(object sender, EventArgs e) { }
        private void txtFlightNumberSearch_TextChanged(object sender, EventArgs e) { }
        private void btnManageFlightStatus_Click(object sender, EventArgs e) { int? currentFlightId = _selectedBooking?.FlightId; string flightNumberForTitle = _selectedBooking?.FlightNumber ?? "N/A"; using (FlightStatusForm statusForm = new FlightStatusForm(currentFlightId)) { if (currentFlightId.HasValue) { statusForm.Text = $"Manage Status - Flight {flightNumberForTitle}"; } else { statusForm.Text = "Manage Flight Status"; } AddLogMessagePublic($"Opening Manage Flight Status window{(currentFlightId.HasValue ? " for flight ID " + currentFlightId.Value : " (no flight pre-selected)")}..."); statusForm.ShowDialog(this); AddLogMessagePublic("Manage Flight Status window closed."); } }
    }

    public class NaturalStringComparer : IComparer<string>
    {
        [System.Runtime.InteropServices.DllImport("shlwapi.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        private static extern int StrCmpLogicalW(string psz1, string psz2);
        public int Compare(string x, string y) { return StrCmpLogicalW(x, y); }
    }
}