// In FlightRegistration.WinFormsClient/Form1.cs
using System;
using System.Drawing; // For Color, Font, Padding
using System.Windows.Forms;
using System.Threading.Tasks;

namespace FlightRegistration.WinFormsClient
{
    public partial class Form1 : Form
    {
        private AgentSocketClient _socketClient;

        public Form1()
        {
            InitializeComponent();
            InitializeCustomUIDesign(); // Call a method to apply some design aspects if not fully done in designer

            _socketClient = new AgentSocketClient(); // Default IP/Port

            // Subscribe to events from the socket client
            _socketClient.OnLogMessage += (message) => AddLogMessage($"[Socket] {message}");
            _socketClient.OnDisconnected += HandleSocketDisconnected;
            _socketClient.OnSeatReservedUpdateReceived += HandleSeatReservedUpdate;
            _socketClient.OnFlightStatusUpdateReceived += HandleFlightStatusUpdate;

            // Initial state for buttons
            btnDisconnectSocket.Enabled = false;
        }

        private void InitializeCustomUIDesign()
        {
            // This is where you could programmatically set properties if you prefer,
            // or ensure they are set if the designer part is tricky.
            this.Text = "Flight Check-in Agent Terminal ✈️";
            this.BackColor = Color.AliceBlue; // Light, pleasant background

            // Title Label
            // lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));
            // lblTitle.ForeColor = Color.SteelBlue;
            // lblTitle.Dock = DockStyle.Top;
            // lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // lblTitle.Padding = new Padding(0, 10, 0, 10);
            // lblTitle.Text = "Agent Terminal";


            // Connection Panel
            // pnlConnection.BackColor = Color.LightSteelBlue;
            // pnlConnection.Height = 40;
            // pnlConnection.Dock = DockStyle.Top;


            // Connect Button
            btnConnectSocket.Text = "🔗 Connect";
            btnConnectSocket.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btnConnectSocket.BackColor = Color.MediumSeaGreen;
            btnConnectSocket.ForeColor = Color.White;
            btnConnectSocket.FlatStyle = FlatStyle.Flat;
            btnConnectSocket.FlatAppearance.BorderSize = 0;

            // Disconnect Button
            btnDisconnectSocket.Text = "🔌 Disconnect";
            btnDisconnectSocket.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            btnDisconnectSocket.BackColor = Color.Tomato;
            btnDisconnectSocket.ForeColor = Color.White;
            btnDisconnectSocket.FlatStyle = FlatStyle.Flat;
            btnDisconnectSocket.FlatAppearance.BorderSize = 0;

            // Log ListBox
            // Assuming lstLogMessages is the name from your previous setup
            if (lstLogMessages != null)
            {
                try
                {
                    lstLogMessages.Font = new Font("Miracode", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                }
                catch (ArgumentException) // Miracode might not be installed
                {
                    lstLogMessages.Font = new Font("Consolas", 9F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
                    AddLogMessage("[UI Info] Miracode font not found, using Consolas for logs.");
                }
                lstLogMessages.BackColor = Color.Ivory;
                lstLogMessages.ForeColor = Color.DarkSlateGray;
                lstLogMessages.HorizontalScrollbar = true;
            }

            // Placeholder for GroupBoxes - you'd style these in the designer or here
            // grpPassengerSearch.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            // grpFlightStatus.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
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
                lstLogMessages.Items.Insert(0, logEntry); // Add to top for recent messages first
                if (lstLogMessages.Items.Count > 200) // Keep log list manageable
                {
                    lstLogMessages.Items.RemoveAt(lstLogMessages.Items.Count - 1);
                }
                // lstLogMessages.TopIndex = Math.Max(0, lstLogMessages.Items.Count - lstLogMessages.ClientSize.Height / lstLogMessages.ItemHeight); // Auto-scroll to bottom
                // For insert at top, no auto-scroll to bottom is needed, or scroll to top:
                // if (lstLogMessages.Items.Count > 0) lstLogMessages.TopIndex = 0;
            }
        }

        private void HandleSocketDisconnected()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(HandleSocketDisconnected));
                return;
            }
            btnConnectSocket.Enabled = true;
            btnDisconnectSocket.Enabled = false;
            AddLogMessage("Socket connection lost or closed.");
        }

        private void HandleSeatReservedUpdate(SeatReservedUpdatePayload payload)
        {
            AddLogMessage($"UI UPDATE (Seat Reserved): FlightID={payload.FlightId}, SeatNo='{payload.SeatNumber}', ReservedBy='{payload.ReservedByAgentId}'");
            // TODO: Actual UI update for seat map
            // Example: FindControl($"seat_{payload.FlightId}_{payload.SeatNumber}", true).BackColor = Color.Red;
        }

        private void HandleFlightStatusUpdate(FlightStatusUpdatePayload payload)
        {
            AddLogMessage($"UI UPDATE (Flight Status): FlightNo='{payload.FlightNumber}', NewStatus='{payload.NewStatus}'");
            // TODO: Actual UI update for flight status display
            // Example: Find flight in a DataGridView and update its status cell.
        }

        private async void btnConnectSocket_Click(object sender, EventArgs e)
        {
            if (!_socketClient.IsConnected)
            {
                AddLogMessage("Connecting to socket server...");
                btnConnectSocket.Enabled = false;
                btnDisconnectSocket.Enabled = false; // Also disable disconnect during attempt
                await _socketClient.ConnectAsync();
                if (_socketClient.IsConnected)
                {
                    btnDisconnectSocket.Enabled = true;
                    // btnConnectSocket remains disabled as it's connected
                }
                else
                {
                    btnConnectSocket.Enabled = true; // Re-enable if connection failed
                }
            }
            else
            {
                AddLogMessage("Already connected.");
            }
        }

        private void btnDisconnectSocket_Click(object sender, EventArgs e)
        {
            if (_socketClient.IsConnected)
            {
                _socketClient.Disconnect();
                // HandleSocketDisconnected will be called via event to update button states
            }
            else
            {
                AddLogMessage("Not currently connected.");
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            _socketClient?.Disconnect();
        }
    }
}