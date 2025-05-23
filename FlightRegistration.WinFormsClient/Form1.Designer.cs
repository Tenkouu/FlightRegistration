// File: FlightRegistration.WinFormsClient/Form1.designer.cs
namespace FlightRegistration.WinFormsClient
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lstLogMessages = new ListBox();
            pnlConnection = new Panel();
            btnManageFlightStatus = new Button();
            btnConnectSocket = new Button();
            btnDisconnectSocket = new Button();
            lblTitle = new Label();
            grpPassengerSearch = new GroupBox();
            dgvBookings = new DataGridView();
            btnSearchPassenger = new Button();
            txtFlightNumberSearch = new TextBox();
            lblFlightNumberSearch = new Label();
            txtPassportNumber = new TextBox();
            lblPassportNumber = new Label();
            grpFlightDetails = new GroupBox();
            lblSelectedSeatInfo = new Label();
            pnlSeatMapPlaceholder = new Panel();
            lblSelectedFlightInfo = new Label();
            pnlConnection.SuspendLayout();
            grpPassengerSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBookings).BeginInit();
            grpFlightDetails.SuspendLayout();
            SuspendLayout();
            // 
            // lstLogMessages
            // 
            lstLogMessages.BackColor = Color.FromArgb(42, 45, 46);
            lstLogMessages.Dock = DockStyle.Bottom;
            lstLogMessages.ForeColor = Color.FromArgb(241, 207, 71);
            lstLogMessages.FormattingEnabled = true;
            lstLogMessages.HorizontalScrollbar = true;
            lstLogMessages.ItemHeight = 21;
            lstLogMessages.Location = new Point(0, 627);
            lstLogMessages.Name = "lstLogMessages";
            lstLogMessages.Size = new Size(786, 130);
            lstLogMessages.TabIndex = 2;
            // 
            // pnlConnection
            // 
            pnlConnection.BackColor = Color.FromArgb(42, 45, 46);
            pnlConnection.Controls.Add(btnManageFlightStatus);
            pnlConnection.Controls.Add(btnConnectSocket);
            pnlConnection.Controls.Add(btnDisconnectSocket);
            pnlConnection.Dock = DockStyle.Top;
            pnlConnection.ForeColor = Color.FromArgb(241, 207, 71);
            pnlConnection.Location = new Point(0, 71);
            pnlConnection.Name = "pnlConnection";
            pnlConnection.Size = new Size(786, 52);
            pnlConnection.TabIndex = 4;
            // 
            // btnManageFlightStatus
            // 
            btnManageFlightStatus.Anchor = AnchorStyles.Top;
            btnManageFlightStatus.BackColor = Color.FromArgb(42, 45, 46);
            btnManageFlightStatus.Enabled = false;
            btnManageFlightStatus.FlatAppearance.BorderSize = 2;
            btnManageFlightStatus.FlatStyle = FlatStyle.Flat;
            btnManageFlightStatus.Font = new Font("Miracode", 9F, FontStyle.Bold);
            btnManageFlightStatus.ForeColor = Color.White;
            btnManageFlightStatus.Location = new Point(311, 9);
            btnManageFlightStatus.Name = "btnManageFlightStatus";
            btnManageFlightStatus.Size = new Size(180, 37);
            btnManageFlightStatus.TabIndex = 2;
            btnManageFlightStatus.Text = "Status";
            btnManageFlightStatus.UseVisualStyleBackColor = false;
            btnManageFlightStatus.Click += btnManageFlightStatus_Click;
            // 
            // btnConnectSocket
            // 
            btnConnectSocket.BackColor = Color.DarkGreen;
            btnConnectSocket.FlatAppearance.BorderSize = 0;
            btnConnectSocket.FlatStyle = FlatStyle.Flat;
            btnConnectSocket.Font = new Font("Miracode", 9F, FontStyle.Bold);
            btnConnectSocket.ForeColor = Color.Transparent;
            btnConnectSocket.Location = new Point(13, 8);
            btnConnectSocket.Name = "btnConnectSocket";
            btnConnectSocket.Size = new Size(175, 37);
            btnConnectSocket.TabIndex = 0;
            btnConnectSocket.Text = "Connect";
            btnConnectSocket.UseVisualStyleBackColor = false;
            btnConnectSocket.Click += btnConnectSocket_Click;
            // 
            // btnDisconnectSocket
            // 
            btnDisconnectSocket.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnDisconnectSocket.BackColor = Color.Maroon;
            btnDisconnectSocket.FlatAppearance.BorderColor = Color.DarkRed;
            btnDisconnectSocket.FlatStyle = FlatStyle.Flat;
            btnDisconnectSocket.Font = new Font("Miracode", 9F, FontStyle.Bold);
            btnDisconnectSocket.ForeColor = Color.White;
            btnDisconnectSocket.Location = new Point(593, 8);
            btnDisconnectSocket.Name = "btnDisconnectSocket";
            btnDisconnectSocket.Size = new Size(180, 37);
            btnDisconnectSocket.TabIndex = 1;
            btnDisconnectSocket.Text = "Disconnect";
            btnDisconnectSocket.UseVisualStyleBackColor = false;
            btnDisconnectSocket.Click += btnDisconnectSocket_Click;
            // 
            // lblTitle
            // 
            lblTitle.BackColor = Color.FromArgb(42, 45, 46);
            lblTitle.Dock = DockStyle.Top;
            lblTitle.Font = new Font("Miracode", 16F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(241, 207, 71);
            lblTitle.Location = new Point(0, 0);
            lblTitle.Name = "lblTitle";
            lblTitle.Padding = new Padding(0, 10, 0, 10);
            lblTitle.Size = new Size(786, 71);
            lblTitle.TabIndex = 3;
            lblTitle.Text = "Agent Terminal";
            lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // grpPassengerSearch
            // 
            grpPassengerSearch.BackColor = Color.FromArgb(42, 45, 46);
            grpPassengerSearch.BackgroundImageLayout = ImageLayout.None;
            grpPassengerSearch.Controls.Add(dgvBookings);
            grpPassengerSearch.Controls.Add(btnSearchPassenger);
            grpPassengerSearch.Controls.Add(txtFlightNumberSearch);
            grpPassengerSearch.Controls.Add(lblFlightNumberSearch);
            grpPassengerSearch.Controls.Add(txtPassportNumber);
            grpPassengerSearch.Controls.Add(lblPassportNumber);
            grpPassengerSearch.Dock = DockStyle.Top;
            grpPassengerSearch.Font = new Font("Miracode", 10F, FontStyle.Bold);
            grpPassengerSearch.ForeColor = Color.FromArgb(241, 207, 71);
            grpPassengerSearch.Location = new Point(0, 123);
            grpPassengerSearch.Name = "grpPassengerSearch";
            grpPassengerSearch.Padding = new Padding(10);
            grpPassengerSearch.Size = new Size(786, 221);
            grpPassengerSearch.TabIndex = 5;
            grpPassengerSearch.TabStop = false;
            grpPassengerSearch.Text = "🔍 Passenger Search, Seat Assignment";
            grpPassengerSearch.Enter += grpPassengerSearch_Enter;
            // 
            // dgvBookings
            // 
            dgvBookings.AllowUserToAddRows = false;
            dgvBookings.AllowUserToDeleteRows = false;
            dgvBookings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvBookings.BackgroundColor = Color.FromArgb(42, 45, 46);
            dgvBookings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvBookings.Location = new Point(13, 70);
            dgvBookings.MultiSelect = false;
            dgvBookings.Name = "dgvBookings";
            dgvBookings.ReadOnly = true;
            dgvBookings.RowHeadersWidth = 51;
            dgvBookings.RowTemplate.Height = 25;
            dgvBookings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvBookings.Size = new Size(760, 112);
            dgvBookings.TabIndex = 5;
            // 
            // btnSearchPassenger
            // 
            btnSearchPassenger.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchPassenger.BackColor = Color.FromArgb(241, 207, 71);
            btnSearchPassenger.FlatStyle = FlatStyle.Flat;
            btnSearchPassenger.Font = new Font("Miracode", 9F, FontStyle.Bold);
            btnSearchPassenger.ForeColor = Color.FromArgb(42, 45, 46);
            btnSearchPassenger.Location = new Point(654, 24);
            btnSearchPassenger.Name = "btnSearchPassenger";
            btnSearchPassenger.Size = new Size(120, 37);
            btnSearchPassenger.TabIndex = 4;
            btnSearchPassenger.Text = "Search";
            btnSearchPassenger.UseVisualStyleBackColor = false;
            btnSearchPassenger.Click += btnSearchPassenger_Click;
            // 
            // txtFlightNumberSearch
            // 
            txtFlightNumberSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            txtFlightNumberSearch.BackColor = Color.FromArgb(42, 45, 46);
            txtFlightNumberSearch.BorderStyle = BorderStyle.FixedSingle;
            txtFlightNumberSearch.Font = new Font("Miracode", 9F);
            txtFlightNumberSearch.ForeColor = Color.FromArgb(241, 207, 71);
            txtFlightNumberSearch.Location = new Point(422, 38);
            txtFlightNumberSearch.Name = "txtFlightNumberSearch";
            txtFlightNumberSearch.Size = new Size(160, 24);
            txtFlightNumberSearch.TabIndex = 3;
            txtFlightNumberSearch.TextChanged += txtFlightNumberSearch_TextChanged;
            // 
            // lblFlightNumberSearch
            // 
            lblFlightNumberSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblFlightNumberSearch.AutoSize = true;
            lblFlightNumberSearch.Font = new Font("Miracode", 9F, FontStyle.Bold);
            lblFlightNumberSearch.ForeColor = Color.FromArgb(241, 207, 71);
            lblFlightNumberSearch.Location = new Point(336, 40);
            lblFlightNumberSearch.Name = "lblFlightNumberSearch";
            lblFlightNumberSearch.Size = new Size(87, 21);
            lblFlightNumberSearch.TabIndex = 2;
            lblFlightNumberSearch.Text = "Flight:";
            // 
            // txtPassportNumber
            // 
            txtPassportNumber.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtPassportNumber.BackColor = Color.FromArgb(42, 45, 46);
            txtPassportNumber.BorderStyle = BorderStyle.FixedSingle;
            txtPassportNumber.Font = new Font("Miracode", 9F);
            txtPassportNumber.ForeColor = Color.FromArgb(241, 207, 71);
            txtPassportNumber.Location = new Point(116, 40);
            txtPassportNumber.Name = "txtPassportNumber";
            txtPassportNumber.Size = new Size(160, 24);
            txtPassportNumber.TabIndex = 1;
            // 
            // lblPassportNumber
            // 
            lblPassportNumber.AutoSize = true;
            lblPassportNumber.Font = new Font("Miracode", 9F, FontStyle.Bold);
            lblPassportNumber.ForeColor = Color.FromArgb(241, 207, 71);
            lblPassportNumber.Location = new Point(10, 40);
            lblPassportNumber.Name = "lblPassportNumber";
            lblPassportNumber.Size = new Size(109, 21);
            lblPassportNumber.TabIndex = 0;
            lblPassportNumber.Text = "Passport:";
            // 
            // grpFlightDetails
            // 
            grpFlightDetails.BackColor = Color.FromArgb(42, 45, 46);
            grpFlightDetails.Controls.Add(lblSelectedSeatInfo);
            grpFlightDetails.Controls.Add(pnlSeatMapPlaceholder);
            grpFlightDetails.Controls.Add(lblSelectedFlightInfo);
            grpFlightDetails.Dock = DockStyle.Fill;
            grpFlightDetails.Font = new Font("Miracode", 10F, FontStyle.Bold);
            grpFlightDetails.ForeColor = Color.FromArgb(241, 207, 71);
            grpFlightDetails.Location = new Point(0, 344);
            grpFlightDetails.Name = "grpFlightDetails";
            grpFlightDetails.Padding = new Padding(10);
            grpFlightDetails.Size = new Size(786, 283);
            grpFlightDetails.TabIndex = 6;
            grpFlightDetails.TabStop = false;
            grpFlightDetails.Text = "✈️ Flight Seat Map";
            // 
            // lblSelectedSeatInfo
            // 
            lblSelectedSeatInfo.Dock = DockStyle.Bottom;
            lblSelectedSeatInfo.Font = new Font("Miracode", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblSelectedSeatInfo.Location = new Point(10, 251);
            lblSelectedSeatInfo.Name = "lblSelectedSeatInfo";
            lblSelectedSeatInfo.Size = new Size(766, 22);
            lblSelectedSeatInfo.TabIndex = 2;
            lblSelectedSeatInfo.Text = "Selected Seat: (None)";
            lblSelectedSeatInfo.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // pnlSeatMapPlaceholder
            // 
            pnlSeatMapPlaceholder.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlSeatMapPlaceholder.BackColor = Color.FromArgb(42, 45, 46);
            pnlSeatMapPlaceholder.BorderStyle = BorderStyle.FixedSingle;
            pnlSeatMapPlaceholder.Location = new Point(13, 59);
            pnlSeatMapPlaceholder.Name = "pnlSeatMapPlaceholder";
            pnlSeatMapPlaceholder.Size = new Size(760, 183);
            pnlSeatMapPlaceholder.TabIndex = 1;
            // 
            // lblSelectedFlightInfo
            // 
            lblSelectedFlightInfo.Dock = DockStyle.Top;
            lblSelectedFlightInfo.Font = new Font("Miracode", 9F);
            lblSelectedFlightInfo.Location = new Point(10, 29);
            lblSelectedFlightInfo.Name = "lblSelectedFlightInfo";
            lblSelectedFlightInfo.Size = new Size(766, 20);
            lblSelectedFlightInfo.TabIndex = 0;
            lblSelectedFlightInfo.Text = "Selected Flight: (None)";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 21F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.AliceBlue;
            ClientSize = new Size(786, 757);
            Controls.Add(grpFlightDetails);
            Controls.Add(grpPassengerSearch);
            Controls.Add(pnlConnection);
            Controls.Add(lblTitle);
            Controls.Add(lstLogMessages);
            Font = new Font("Miracode", 9F);
            MinimumSize = new Size(800, 600);
            Name = "Form1";
            Text = "Flight Check-in Agent Terminal ✈️";
            FormClosing += Form1_FormClosing;
            pnlConnection.ResumeLayout(false);
            grpPassengerSearch.ResumeLayout(false);
            grpPassengerSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvBookings).EndInit();
            grpFlightDetails.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox lstLogMessages;
        private System.Windows.Forms.Panel pnlConnection;
        private System.Windows.Forms.Button btnConnectSocket;
        private System.Windows.Forms.Button btnManageFlightStatus;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox grpPassengerSearch;
        private System.Windows.Forms.Button btnSearchPassenger;
        private System.Windows.Forms.TextBox txtFlightNumberSearch;
        private System.Windows.Forms.Label lblFlightNumberSearch;
        private System.Windows.Forms.TextBox txtPassportNumber;
        private System.Windows.Forms.Label lblPassportNumber;
        private System.Windows.Forms.DataGridView dgvBookings;
        private System.Windows.Forms.GroupBox grpFlightDetails;
        private System.Windows.Forms.Panel pnlSeatMapPlaceholder;
        private System.Windows.Forms.Label lblSelectedFlightInfo;

        private System.Windows.Forms.Label lblSelectedSeatInfo;
        private Button btnDisconnectSocket;
    }
}