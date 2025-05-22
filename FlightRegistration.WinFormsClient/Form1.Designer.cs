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
            this.lstLogMessages = new System.Windows.Forms.ListBox();
            this.pnlConnection = new System.Windows.Forms.Panel();
            this.btnConnectSocket = new System.Windows.Forms.Button();
            this.btnDisconnectSocket = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpPassengerSearch = new System.Windows.Forms.GroupBox();
            this.btnAssignSeatPlaceholder = new System.Windows.Forms.Button();
            this.dgvBookings = new System.Windows.Forms.DataGridView();
            this.btnSearchPassenger = new System.Windows.Forms.Button();
            this.txtFlightNumberSearch = new System.Windows.Forms.TextBox();
            this.lblFlightNumberSearch = new System.Windows.Forms.Label();
            this.txtPassportNumber = new System.Windows.Forms.TextBox();
            this.lblPassportNumber = new System.Windows.Forms.Label();
            this.grpFlightDetails = new System.Windows.Forms.GroupBox();
            this.lblSelectedSeatInfo = new System.Windows.Forms.Label();
            this.pnlSeatMapPlaceholder = new System.Windows.Forms.Panel();
            this.lblSelectedFlightInfo = new System.Windows.Forms.Label();
            this.pnlConnection.SuspendLayout();
            this.grpPassengerSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).BeginInit();
            this.grpFlightDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstLogMessages
            // 
            this.lstLogMessages.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lstLogMessages.FormattingEnabled = true;
            this.lstLogMessages.HorizontalScrollbar = true;
            this.lstLogMessages.ItemHeight = 15;
            this.lstLogMessages.Location = new System.Drawing.Point(0, 552); // Adjusted based on other controls
            this.lstLogMessages.Name = "lstLogMessages";
            this.lstLogMessages.Size = new System.Drawing.Size(784, 139); // Example height
            this.lstLogMessages.TabIndex = 2;
            // 
            // pnlConnection
            // 
            this.pnlConnection.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlConnection.Controls.Add(this.btnConnectSocket);
            this.pnlConnection.Controls.Add(this.btnDisconnectSocket);
            this.pnlConnection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConnection.Location = new System.Drawing.Point(0, 40);
            this.pnlConnection.Name = "pnlConnection";
            this.pnlConnection.Size = new System.Drawing.Size(784, 40);
            this.pnlConnection.TabIndex = 4;
            // 
            // btnConnectSocket
            // 
            this.btnConnectSocket.Location = new System.Drawing.Point(12, 8);
            this.btnConnectSocket.Name = "btnConnectSocket";
            this.btnConnectSocket.Size = new System.Drawing.Size(120, 23);
            this.btnConnectSocket.TabIndex = 0;
            this.btnConnectSocket.Text = "🔗 Connect";
            this.btnConnectSocket.UseVisualStyleBackColor = true;
            this.btnConnectSocket.Click += new System.EventHandler(this.btnConnectSocket_Click);
            // 
            // btnDisconnectSocket
            // 
            this.btnDisconnectSocket.Enabled = false;
            this.btnDisconnectSocket.Location = new System.Drawing.Point(138, 8);
            this.btnDisconnectSocket.Name = "btnDisconnectSocket";
            this.btnDisconnectSocket.Size = new System.Drawing.Size(120, 23);
            this.btnDisconnectSocket.TabIndex = 1;
            this.btnDisconnectSocket.Text = "🔌 Disconnect";
            this.btnDisconnectSocket.UseVisualStyleBackColor = true;
            this.btnDisconnectSocket.Click += new System.EventHandler(this.btnDisconnectSocket_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(0, 10, 0, 10);
            this.lblTitle.Size = new System.Drawing.Size(784, 40);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Agent Terminal";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grpPassengerSearch
            // 
            this.grpPassengerSearch.Controls.Add(this.btnAssignSeatPlaceholder);
            this.grpPassengerSearch.Controls.Add(this.dgvBookings);
            this.grpPassengerSearch.Controls.Add(this.btnSearchPassenger);
            this.grpPassengerSearch.Controls.Add(this.txtFlightNumberSearch);
            this.grpPassengerSearch.Controls.Add(this.lblFlightNumberSearch);
            this.grpPassengerSearch.Controls.Add(this.txtPassportNumber);
            this.grpPassengerSearch.Controls.Add(this.lblPassportNumber);
            this.grpPassengerSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpPassengerSearch.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpPassengerSearch.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.grpPassengerSearch.Location = new System.Drawing.Point(0, 80);
            this.grpPassengerSearch.Name = "grpPassengerSearch";
            this.grpPassengerSearch.Padding = new System.Windows.Forms.Padding(10);
            this.grpPassengerSearch.Size = new System.Drawing.Size(784, 250); // Example height
            this.grpPassengerSearch.TabIndex = 5;
            this.grpPassengerSearch.TabStop = false;
            this.grpPassengerSearch.Text = "🔍 Passenger Search & Seat Assignment";
            // 
            // btnAssignSeatPlaceholder
            // 
            this.btnAssignSeatPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAssignSeatPlaceholder.Enabled = false;
            this.btnAssignSeatPlaceholder.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnAssignSeatPlaceholder.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnAssignSeatPlaceholder.Location = new System.Drawing.Point(13, 214);
            this.btnAssignSeatPlaceholder.Name = "btnAssignSeatPlaceholder";
            this.btnAssignSeatPlaceholder.Size = new System.Drawing.Size(250, 23);
            this.btnAssignSeatPlaceholder.TabIndex = 6;
            this.btnAssignSeatPlaceholder.Text = "Assign Seat for Selected Booking (Placeholder)";
            this.btnAssignSeatPlaceholder.UseVisualStyleBackColor = true;
            // this.btnAssignSeatPlaceholder.Click += new System.EventHandler(this.btnAssignSeatPlaceholder_Click); // Add this if you have the handler
            // 
            // dgvBookings
            // 
            this.dgvBookings.AllowUserToAddRows = false;
            this.dgvBookings.AllowUserToDeleteRows = false;
            this.dgvBookings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBookings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBookings.Location = new System.Drawing.Point(13, 67);
            this.dgvBookings.MultiSelect = false;
            this.dgvBookings.Name = "dgvBookings";
            this.dgvBookings.ReadOnly = true;
            this.dgvBookings.RowTemplate.Height = 25;
            this.dgvBookings.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBookings.Size = new System.Drawing.Size(758, 141); // Example size
            this.dgvBookings.TabIndex = 5;
            // this.dgvBookings.SelectionChanged += new System.EventHandler(this.dgvBookings_SelectionChanged); // Already in Form1.cs logic
            // 
            // btnSearchPassenger
            // 
            this.btnSearchPassenger.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSearchPassenger.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSearchPassenger.Location = new System.Drawing.Point(470, 35);
            this.btnSearchPassenger.Name = "btnSearchPassenger";
            this.btnSearchPassenger.Size = new System.Drawing.Size(120, 25);
            this.btnSearchPassenger.TabIndex = 4;
            this.btnSearchPassenger.Text = "Search Bookings";
            this.btnSearchPassenger.UseVisualStyleBackColor = true;
            this.btnSearchPassenger.Click += new System.EventHandler(this.btnSearchPassenger_Click);
            // 
            // txtFlightNumberSearch
            // 
            this.txtFlightNumberSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtFlightNumberSearch.Location = new System.Drawing.Point(310, 36);
            this.txtFlightNumberSearch.Name = "txtFlightNumberSearch";
            this.txtFlightNumberSearch.Size = new System.Drawing.Size(150, 23);
            this.txtFlightNumberSearch.TabIndex = 3;
            // 
            // lblFlightNumberSearch
            // 
            this.lblFlightNumberSearch.AutoSize = true;
            this.lblFlightNumberSearch.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFlightNumberSearch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblFlightNumberSearch.Location = new System.Drawing.Point(240, 40);
            this.lblFlightNumberSearch.Name = "lblFlightNumberSearch";
            this.lblFlightNumberSearch.Size = new System.Drawing.Size(63, 15);
            this.lblFlightNumberSearch.TabIndex = 2;
            this.lblFlightNumberSearch.Text = "Flight No.:";
            // 
            // txtPassportNumber
            // 
            this.txtPassportNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txtPassportNumber.Location = new System.Drawing.Point(90, 36);
            this.txtPassportNumber.Name = "txtPassportNumber";
            this.txtPassportNumber.Size = new System.Drawing.Size(140, 23);
            this.txtPassportNumber.TabIndex = 1;
            // 
            // lblPassportNumber
            // 
            this.lblPassportNumber.AutoSize = true;
            this.lblPassportNumber.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblPassportNumber.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lblPassportNumber.Location = new System.Drawing.Point(10, 40);
            this.lblPassportNumber.Name = "lblPassportNumber";
            this.lblPassportNumber.Size = new System.Drawing.Size(75, 15);
            this.lblPassportNumber.TabIndex = 0;
            this.lblPassportNumber.Text = "Passport No:";
            // 
            // grpFlightDetails
            // 
            this.grpFlightDetails.Controls.Add(this.lblSelectedSeatInfo);
            this.grpFlightDetails.Controls.Add(this.pnlSeatMapPlaceholder);
            this.grpFlightDetails.Controls.Add(this.lblSelectedFlightInfo);
            this.grpFlightDetails.Dock = System.Windows.Forms.DockStyle.Fill; // Fill remaining space before log
            this.grpFlightDetails.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.grpFlightDetails.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.grpFlightDetails.Location = new System.Drawing.Point(0, 330); // After grpPassengerSearch
            this.grpFlightDetails.Name = "grpFlightDetails";
            this.grpFlightDetails.Padding = new System.Windows.Forms.Padding(10);
            this.grpFlightDetails.Size = new System.Drawing.Size(784, 222); // Adjusted
            this.grpFlightDetails.TabIndex = 6;
            this.grpFlightDetails.TabStop = false;
            this.grpFlightDetails.Text = "✈️ Flight Details & Seat Map";
            // 
            // lblSelectedSeatInfo
            // 
            this.lblSelectedSeatInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblSelectedSeatInfo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic);
            this.lblSelectedSeatInfo.Location = new System.Drawing.Point(10, 190); // Adjusted
            this.lblSelectedSeatInfo.Name = "lblSelectedSeatInfo";
            this.lblSelectedSeatInfo.Size = new System.Drawing.Size(764, 22);
            this.lblSelectedSeatInfo.TabIndex = 2;
            this.lblSelectedSeatInfo.Text = "Selected Seat: (None)";
            this.lblSelectedSeatInfo.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlSeatMapPlaceholder
            // 
            this.pnlSeatMapPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSeatMapPlaceholder.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlSeatMapPlaceholder.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlSeatMapPlaceholder.Location = new System.Drawing.Point(13, 50); // Adjusted
            this.pnlSeatMapPlaceholder.Name = "pnlSeatMapPlaceholder";
            this.pnlSeatMapPlaceholder.Size = new System.Drawing.Size(758, 137); // Adjusted
            this.pnlSeatMapPlaceholder.TabIndex = 1;
            // 
            // lblSelectedFlightInfo
            // 
            this.lblSelectedFlightInfo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblSelectedFlightInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblSelectedFlightInfo.Location = new System.Drawing.Point(10, 28); // Adjusted
            this.lblSelectedFlightInfo.Name = "lblSelectedFlightInfo";
            this.lblSelectedFlightInfo.Size = new System.Drawing.Size(764, 19);
            this.lblSelectedFlightInfo.TabIndex = 0;
            this.lblSelectedFlightInfo.Text = "Selected Flight: (None)";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(784, 691); // Adjusted for all controls
            this.Controls.Add(this.grpFlightDetails);
            this.Controls.Add(this.grpPassengerSearch);
            this.Controls.Add(this.pnlConnection);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.lstLogMessages);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.MinimumSize = new System.Drawing.Size(800, 600); // Example minimum size
            this.Name = "Form1";
            this.Text = "Flight Check-in Agent Terminal ✈️";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.pnlConnection.ResumeLayout(false);
            this.grpPassengerSearch.ResumeLayout(false);
            this.grpPassengerSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBookings)).EndInit();
            this.grpFlightDetails.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox lstLogMessages;
        private System.Windows.Forms.Panel pnlConnection;
        private System.Windows.Forms.Button btnConnectSocket;
        private System.Windows.Forms.Button btnDisconnectSocket;
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
        private System.Windows.Forms.Button btnAssignSeatPlaceholder;
        private System.Windows.Forms.Label lblSelectedSeatInfo;
    }
}