// File: FlightRegistration.WinFormsClient/FlightStatusForm.designer.cs
namespace FlightRegistration.WinFormsClient
{
    partial class FlightStatusForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.lblSearchFlightIdPrompt = new System.Windows.Forms.Label();
            this.txtFlightIdSearch = new System.Windows.Forms.TextBox();
            this.btnSearchFlight = new System.Windows.Forms.Button();
            this.grpLoadedFlightInfo = new System.Windows.Forms.GroupBox();
            this.lblLoadedCurrentStatusValue = new System.Windows.Forms.Label();
            this.lblLoadedCurrentStatusStatic = new System.Windows.Forms.Label();
            this.lblLoadedArrTimeValue = new System.Windows.Forms.Label();
            this.lblLoadedArrTimeStatic = new System.Windows.Forms.Label();
            this.lblLoadedArrivalValue = new System.Windows.Forms.Label();
            this.lblLoadedArrivalStatic = new System.Windows.Forms.Label();
            this.lblLoadedDepTimeValue = new System.Windows.Forms.Label();
            this.lblLoadedDepTimeStatic = new System.Windows.Forms.Label();
            this.lblLoadedDepartureValue = new System.Windows.Forms.Label();
            this.lblLoadedDepartureStatic = new System.Windows.Forms.Label();
            this.lblLoadedFlightNumberValue = new System.Windows.Forms.Label();
            this.lblLoadedFlightNumberStatic = new System.Windows.Forms.Label();
            this.grpUpdateStatusControls = new System.Windows.Forms.GroupBox();
            this.btnUpdateStatusAction = new System.Windows.Forms.Button();
            this.cmbNewStatus = new System.Windows.Forms.ComboBox();
            this.lblSelectNewStatus = new System.Windows.Forms.Label();
            this.btnFSFClose = new System.Windows.Forms.Button();
            this.grpLoadedFlightInfo.SuspendLayout();
            this.grpUpdateStatusControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblSearchFlightIdPrompt
            // 
            this.lblSearchFlightIdPrompt.AutoSize = true;
            this.lblSearchFlightIdPrompt.Font = new System.Drawing.Font("Miracode", 9.75F);
            this.lblSearchFlightIdPrompt.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.lblSearchFlightIdPrompt.Location = new System.Drawing.Point(12, 24);
            this.lblSearchFlightIdPrompt.Name = "lblSearchFlightIdPrompt";
            this.lblSearchFlightIdPrompt.Size = new System.Drawing.Size(76, 17);
            this.lblSearchFlightIdPrompt.TabIndex = 0;
            this.lblSearchFlightIdPrompt.Text = "Flight ID:";
            // 
            // txtFlightIdSearch
            // 
            this.txtFlightIdSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(66)))));
            this.txtFlightIdSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtFlightIdSearch.Font = new System.Drawing.Font("Miracode", 9.75F);
            this.txtFlightIdSearch.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.txtFlightIdSearch.Location = new System.Drawing.Point(95, 20);
            this.txtFlightIdSearch.Name = "txtFlightIdSearch";
            this.txtFlightIdSearch.Size = new System.Drawing.Size(130, 25);
            this.txtFlightIdSearch.TabIndex = 1;
            // 
            // btnSearchFlight
            // 
            this.btnSearchFlight.BackColor = System.Drawing.Color.DarkCyan;
            this.btnSearchFlight.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.btnSearchFlight.FlatAppearance.BorderSize = 1;
            this.btnSearchFlight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearchFlight.Font = new System.Drawing.Font("Miracode", 10F, System.Drawing.FontStyle.Bold);
            this.btnSearchFlight.ForeColor = System.Drawing.Color.White;
            this.btnSearchFlight.Location = new System.Drawing.Point(235, 16);
            this.btnSearchFlight.Name = "btnSearchFlight";
            this.btnSearchFlight.Size = new System.Drawing.Size(150, 32);
            this.btnSearchFlight.TabIndex = 2;
            this.btnSearchFlight.Text = "Search Flight";
            this.btnSearchFlight.UseVisualStyleBackColor = false;
            this.btnSearchFlight.Click += new System.EventHandler(this.btnSearchFlight_Click);
            // 
            // grpLoadedFlightInfo
            // 
            this.grpLoadedFlightInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedCurrentStatusValue);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedCurrentStatusStatic);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedArrTimeValue);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedArrTimeStatic);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedArrivalValue);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedArrivalStatic);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedDepTimeValue);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedDepTimeStatic);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedDepartureValue);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedDepartureStatic);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedFlightNumberValue);
            this.grpLoadedFlightInfo.Controls.Add(this.lblLoadedFlightNumberStatic);
            this.grpLoadedFlightInfo.Font = new System.Drawing.Font("Miracode", 10F, System.Drawing.FontStyle.Bold);
            this.grpLoadedFlightInfo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.grpLoadedFlightInfo.Location = new System.Drawing.Point(15, 60);
            this.grpLoadedFlightInfo.Name = "grpLoadedFlightInfo";
            this.grpLoadedFlightInfo.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.grpLoadedFlightInfo.Size = new System.Drawing.Size(458, 185);
            this.grpLoadedFlightInfo.TabIndex = 3;
            this.grpLoadedFlightInfo.TabStop = false;
            this.grpLoadedFlightInfo.Text = "CURRENT FLIGHT INFORMATION";
            // 
            // lblLoadedFlightNumberStatic
            // 
            this.lblLoadedFlightNumberStatic.AutoSize = true; this.lblLoadedFlightNumberStatic.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblLoadedFlightNumberStatic.Location = new System.Drawing.Point(15, 30); this.lblLoadedFlightNumberStatic.Name = "lblLoadedFlightNumberStatic"; this.lblLoadedFlightNumberStatic.Size = new System.Drawing.Size(84, 17); this.lblLoadedFlightNumberStatic.Text = "Flight No.:";
            // 
            // lblLoadedFlightNumberValue
            // 
            this.lblLoadedFlightNumberValue.AutoSize = true; this.lblLoadedFlightNumberValue.Font = new System.Drawing.Font("Miracode", 9.75F, System.Drawing.FontStyle.Bold); this.lblLoadedFlightNumberValue.Location = new System.Drawing.Point(160, 30); this.lblLoadedFlightNumberValue.Name = "lblLoadedFlightNumberValue"; this.lblLoadedFlightNumberValue.Size = new System.Drawing.Size(34, 17); this.lblLoadedFlightNumberValue.Text = "N/A";
            // 
            // lblLoadedDepartureStatic
            // 
            this.lblLoadedDepartureStatic.AutoSize = true; this.lblLoadedDepartureStatic.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblLoadedDepartureStatic.Location = new System.Drawing.Point(15, 55); this.lblLoadedDepartureStatic.Name = "lblLoadedDepartureStatic"; this.lblLoadedDepartureStatic.Size = new System.Drawing.Size(110, 17); this.lblLoadedDepartureStatic.Text = "Departure City:";
            // 
            // lblLoadedDepartureValue
            // 
            this.lblLoadedDepartureValue.AutoSize = true; this.lblLoadedDepartureValue.Font = new System.Drawing.Font("Miracode", 9.75F, System.Drawing.FontStyle.Bold); this.lblLoadedDepartureValue.Location = new System.Drawing.Point(160, 55); this.lblLoadedDepartureValue.Name = "lblLoadedDepartureValue"; this.lblLoadedDepartureValue.Size = new System.Drawing.Size(34, 17); this.lblLoadedDepartureValue.Text = "N/A";
            //
            // lblLoadedDepTimeStatic
            //
            this.lblLoadedDepTimeStatic.AutoSize = true; this.lblLoadedDepTimeStatic.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblLoadedDepTimeStatic.Location = new System.Drawing.Point(15, 80); this.lblLoadedDepTimeStatic.Name = "lblLoadedDepTimeStatic"; this.lblLoadedDepTimeStatic.Size = new System.Drawing.Size(118, 17); this.lblLoadedDepTimeStatic.Text = "Departure Time:";
            //
            // lblLoadedDepTimeValue
            //
            this.lblLoadedDepTimeValue.AutoSize = true; this.lblLoadedDepTimeValue.Font = new System.Drawing.Font("Miracode", 9.75F, System.Drawing.FontStyle.Bold); this.lblLoadedDepTimeValue.Location = new System.Drawing.Point(160, 80); this.lblLoadedDepTimeValue.Name = "lblLoadedDepTimeValue"; this.lblLoadedDepTimeValue.Size = new System.Drawing.Size(34, 17); this.lblLoadedDepTimeValue.Text = "N/A";
            // 
            // lblLoadedArrivalStatic
            // 
            this.lblLoadedArrivalStatic.AutoSize = true; this.lblLoadedArrivalStatic.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblLoadedArrivalStatic.Location = new System.Drawing.Point(15, 105); this.lblLoadedArrivalStatic.Name = "lblLoadedArrivalStatic"; this.lblLoadedArrivalStatic.Size = new System.Drawing.Size(92, 17); this.lblLoadedArrivalStatic.Text = "Arrival City:";
            // 
            // lblLoadedArrivalValue
            // 
            this.lblLoadedArrivalValue.AutoSize = true; this.lblLoadedArrivalValue.Font = new System.Drawing.Font("Miracode", 9.75F, System.Drawing.FontStyle.Bold); this.lblLoadedArrivalValue.Location = new System.Drawing.Point(160, 105); this.lblLoadedArrivalValue.Name = "lblLoadedArrivalValue"; this.lblLoadedArrivalValue.Size = new System.Drawing.Size(34, 17); this.lblLoadedArrivalValue.Text = "N/A";
            //
            // lblLoadedArrTimeStatic
            //
            this.lblLoadedArrTimeStatic.AutoSize = true; this.lblLoadedArrTimeStatic.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblLoadedArrTimeStatic.Location = new System.Drawing.Point(15, 130); this.lblLoadedArrTimeStatic.Name = "lblLoadedArrTimeStatic"; this.lblLoadedArrTimeStatic.Size = new System.Drawing.Size(100, 17); this.lblLoadedArrTimeStatic.Text = "Arrival Time:";
            //
            // lblLoadedArrTimeValue
            //
            this.lblLoadedArrTimeValue.AutoSize = true; this.lblLoadedArrTimeValue.Font = new System.Drawing.Font("Miracode", 9.75F, System.Drawing.FontStyle.Bold); this.lblLoadedArrTimeValue.Location = new System.Drawing.Point(160, 130); this.lblLoadedArrTimeValue.Name = "lblLoadedArrTimeValue"; this.lblLoadedArrTimeValue.Size = new System.Drawing.Size(34, 17); this.lblLoadedArrTimeValue.Text = "N/A";
            // 
            // lblLoadedCurrentStatusStatic
            // 
            this.lblLoadedCurrentStatusStatic.AutoSize = true; this.lblLoadedCurrentStatusStatic.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblLoadedCurrentStatusStatic.Location = new System.Drawing.Point(15, 155); this.lblLoadedCurrentStatusStatic.Name = "lblLoadedCurrentStatusStatic"; this.lblLoadedCurrentStatusStatic.Size = new System.Drawing.Size(108, 17); this.lblLoadedCurrentStatusStatic.Text = "Current Status:";
            // 
            // lblLoadedCurrentStatusValue
            // 
            this.lblLoadedCurrentStatusValue.AutoSize = true; this.lblLoadedCurrentStatusValue.Font = new System.Drawing.Font("Miracode", 9.75F, System.Drawing.FontStyle.Bold); this.lblLoadedCurrentStatusValue.Location = new System.Drawing.Point(160, 155); this.lblLoadedCurrentStatusValue.Name = "lblLoadedCurrentStatusValue"; this.lblLoadedCurrentStatusValue.Size = new System.Drawing.Size(34, 17); this.lblLoadedCurrentStatusValue.Text = "N/A";
            // 
            // grpUpdateStatusControls
            // 
            this.grpUpdateStatusControls.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpUpdateStatusControls.Controls.Add(this.cmbNewStatus);
            this.grpUpdateStatusControls.Controls.Add(this.lblSelectNewStatus);
            this.grpUpdateStatusControls.Font = new System.Drawing.Font("Miracode", 10F, System.Drawing.FontStyle.Bold);
            this.grpUpdateStatusControls.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.grpUpdateStatusControls.Location = new System.Drawing.Point(15, 255); // Adjusted Y
            this.grpUpdateStatusControls.Name = "grpUpdateStatusControls";
            this.grpUpdateStatusControls.Padding = new System.Windows.Forms.Padding(10, 5, 10, 10);
            this.grpUpdateStatusControls.Size = new System.Drawing.Size(458, 70);
            this.grpUpdateStatusControls.TabIndex = 4;
            this.grpUpdateStatusControls.TabStop = false;
            this.grpUpdateStatusControls.Text = "SELECT NEW STATUS";
            // 
            // lblSelectNewStatus
            // 
            this.lblSelectNewStatus.AutoSize = true; this.lblSelectNewStatus.Font = new System.Drawing.Font("Miracode", 9.75F); this.lblSelectNewStatus.Location = new System.Drawing.Point(15, 30); this.lblSelectNewStatus.Name = "lblSelectNewStatus"; this.lblSelectNewStatus.Size = new System.Drawing.Size(88, 17); this.lblSelectNewStatus.Text = "New Status:"; this.grpUpdateStatusControls.Controls.Add(this.lblSelectNewStatus);
            // 
            // cmbNewStatus
            // 
            this.cmbNewStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbNewStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(62)))), ((int)(((byte)(62)))), ((int)(((byte)(66)))));
            this.cmbNewStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewStatus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cmbNewStatus.Font = new System.Drawing.Font("Miracode", 9.75F);
            this.cmbNewStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.cmbNewStatus.FormattingEnabled = true;
            this.cmbNewStatus.Location = new System.Drawing.Point(120, 26);
            this.cmbNewStatus.Name = "cmbNewStatus";
            this.cmbNewStatus.Size = new System.Drawing.Size(325, 25);
            this.cmbNewStatus.TabIndex = 0; this.grpUpdateStatusControls.Controls.Add(this.cmbNewStatus);
            // 
            // btnUpdateStatusAction
            // 
            this.btnUpdateStatusAction.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateStatusAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(100)))), ((int)(((byte)(0))))); // Darker Green
            this.btnUpdateStatusAction.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.btnUpdateStatusAction.FlatAppearance.BorderSize = 1;
            this.btnUpdateStatusAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUpdateStatusAction.Font = new System.Drawing.Font("Miracode", 10F, System.Drawing.FontStyle.Bold);
            this.btnUpdateStatusAction.ForeColor = System.Drawing.Color.White;
            this.btnUpdateStatusAction.Location = new System.Drawing.Point(187, 340); // Adjusted Y
            this.btnUpdateStatusAction.Name = "btnUpdateStatusAction";
            this.btnUpdateStatusAction.Size = new System.Drawing.Size(160, 32);
            this.btnUpdateStatusAction.TabIndex = 5;
            this.btnUpdateStatusAction.Text = "Update Status";
            this.btnUpdateStatusAction.UseVisualStyleBackColor = false;
            this.btnUpdateStatusAction.Click += new System.EventHandler(this.btnUpdateStatusAction_Click);
            // 
            // btnFSFClose
            // 
            this.btnFSFClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFSFClose.BackColor = System.Drawing.Color.Maroon;
            this.btnFSFClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnFSFClose.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.btnFSFClose.FlatAppearance.BorderSize = 1;
            this.btnFSFClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFSFClose.Font = new System.Drawing.Font("Miracode", 10F, System.Drawing.FontStyle.Bold);
            this.btnFSFClose.ForeColor = System.Drawing.Color.White;
            this.btnFSFClose.Location = new System.Drawing.Point(357, 340); // Adjusted Y
            this.btnFSFClose.Name = "btnFSFClose";
            this.btnFSFClose.Size = new System.Drawing.Size(116, 32);
            this.btnFSFClose.TabIndex = 6;
            this.btnFSFClose.Text = "Close";
            this.btnFSFClose.UseVisualStyleBackColor = false;
            this.btnFSFClose.Click += new System.EventHandler(this.btnFSFClose_Click);
            // 
            // FlightStatusForm
            // 
            this.AcceptButton = this.btnSearchFlight;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 17F); // Set by this.Font
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.CancelButton = this.btnFSFClose;
            this.ClientSize = new System.Drawing.Size(488, 384); // Adjusted ClientSize
            this.Controls.Add(this.btnUpdateStatusAction); // Added here
            this.Controls.Add(this.btnFSFClose);
            this.Controls.Add(this.grpUpdateStatusControls);
            this.Controls.Add(this.grpLoadedFlightInfo);
            this.Controls.Add(this.btnSearchFlight);
            this.Controls.Add(this.txtFlightIdSearch);
            this.Controls.Add(this.lblSearchFlightIdPrompt);
            this.Font = new System.Drawing.Font("Miracode", 9.75F);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(206)))), ((int)(((byte)(89)))));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FlightStatusForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Manage Flight Status";
            this.grpLoadedFlightInfo.ResumeLayout(false);
            this.grpLoadedFlightInfo.PerformLayout();
            this.grpUpdateStatusControls.ResumeLayout(false);
            this.grpUpdateStatusControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
        #endregion
        // Declarations will be auto-generated by the designer
        private System.Windows.Forms.Label lblSearchFlightIdPrompt;
        private System.Windows.Forms.TextBox txtFlightIdSearch;
        private System.Windows.Forms.Button btnSearchFlight;
        private System.Windows.Forms.GroupBox grpLoadedFlightInfo;
        private System.Windows.Forms.Label lblLoadedFlightNumberStatic;
        private System.Windows.Forms.Label lblLoadedFlightNumberValue;
        private System.Windows.Forms.Label lblLoadedDepartureStatic;
        private System.Windows.Forms.Label lblLoadedDepartureValue;
        private System.Windows.Forms.Label lblLoadedDepTimeStatic;
        private System.Windows.Forms.Label lblLoadedDepTimeValue;
        private System.Windows.Forms.Label lblLoadedArrivalStatic;
        private System.Windows.Forms.Label lblLoadedArrivalValue;
        private System.Windows.Forms.Label lblLoadedArrTimeStatic;
        private System.Windows.Forms.Label lblLoadedArrTimeValue;
        private System.Windows.Forms.Label lblLoadedCurrentStatusStatic;
        private System.Windows.Forms.Label lblLoadedCurrentStatusValue;
        private System.Windows.Forms.GroupBox grpUpdateStatusControls;
        private System.Windows.Forms.Label lblSelectNewStatus;
        private System.Windows.Forms.ComboBox cmbNewStatus;
        private System.Windows.Forms.Button btnUpdateStatusAction;
        private System.Windows.Forms.Button btnFSFClose;
    }
}