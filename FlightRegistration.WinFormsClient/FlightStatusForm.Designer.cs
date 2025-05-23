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
            lblSearchFlightIdPrompt = new Label();
            txtFlightIdSearch = new TextBox();
            btnSearchFlight = new Button();
            grpLoadedFlightInfo = new GroupBox();
            lblLoadedCurrentStatusValue = new Label();
            lblLoadedCurrentStatusStatic = new Label();
            lblLoadedArrTimeValue = new Label();
            lblLoadedArrTimeStatic = new Label();
            lblLoadedArrivalValue = new Label();
            lblLoadedArrivalStatic = new Label();
            lblLoadedDepTimeValue = new Label();
            lblLoadedDepTimeStatic = new Label();
            lblLoadedDepartureValue = new Label();
            lblLoadedDepartureStatic = new Label();
            lblLoadedFlightNumberValue = new Label();
            lblLoadedFlightNumberStatic = new Label();
            grpUpdateStatusControls = new GroupBox();
            lblSelectNewStatus = new Label();
            cmbNewStatus = new ComboBox();
            btnUpdateStatusAction = new Button();
            btnFSFClose = new Button();
            grpLoadedFlightInfo.SuspendLayout();
            grpUpdateStatusControls.SuspendLayout();
            SuspendLayout();
            // 
            // lblSearchFlightIdPrompt
            // 
            lblSearchFlightIdPrompt.AutoSize = true;
            lblSearchFlightIdPrompt.Font = new Font("Miracode", 9.75F);
            lblSearchFlightIdPrompt.ForeColor = Color.FromArgb(238, 206, 89);
            lblSearchFlightIdPrompt.Location = new Point(12, 24);
            lblSearchFlightIdPrompt.Name = "lblSearchFlightIdPrompt";
            lblSearchFlightIdPrompt.Size = new Size(120, 24);
            lblSearchFlightIdPrompt.TabIndex = 0;
            lblSearchFlightIdPrompt.Text = "Flight ID:";
            // 
            // txtFlightIdSearch
            // 
            txtFlightIdSearch.BackColor = Color.FromArgb(42, 45, 46);
            txtFlightIdSearch.BorderStyle = BorderStyle.FixedSingle;
            txtFlightIdSearch.Font = new Font("Miracode", 9.75F);
            txtFlightIdSearch.ForeColor = Color.FromArgb(238, 206, 89);
            txtFlightIdSearch.Location = new Point(137, 25);
            txtFlightIdSearch.Name = "txtFlightIdSearch";
            txtFlightIdSearch.Size = new Size(138, 25);
            txtFlightIdSearch.TabIndex = 1;
            // 
            // btnSearchFlight
            // 
            btnSearchFlight.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSearchFlight.BackColor = Color.FromArgb(241, 207, 71);
            btnSearchFlight.FlatAppearance.BorderColor = Color.FromArgb(238, 206, 89);
            btnSearchFlight.FlatAppearance.BorderSize = 0;
            btnSearchFlight.FlatStyle = FlatStyle.Flat;
            btnSearchFlight.Font = new Font("Miracode", 10F, FontStyle.Bold);
            btnSearchFlight.ForeColor = Color.FromArgb(42, 45, 46);
            btnSearchFlight.Location = new Point(323, 22);
            btnSearchFlight.Name = "btnSearchFlight";
            btnSearchFlight.Size = new Size(150, 32);
            btnSearchFlight.TabIndex = 2;
            btnSearchFlight.Text = "Search Flight";
            btnSearchFlight.UseVisualStyleBackColor = false;
            btnSearchFlight.Click += btnSearchFlight_Click;
            // 
            // grpLoadedFlightInfo
            // 
            grpLoadedFlightInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpLoadedFlightInfo.Controls.Add(lblLoadedCurrentStatusValue);
            grpLoadedFlightInfo.Controls.Add(lblLoadedCurrentStatusStatic);
            grpLoadedFlightInfo.Controls.Add(lblLoadedArrTimeValue);
            grpLoadedFlightInfo.Controls.Add(lblLoadedArrTimeStatic);
            grpLoadedFlightInfo.Controls.Add(lblLoadedArrivalValue);
            grpLoadedFlightInfo.Controls.Add(lblLoadedArrivalStatic);
            grpLoadedFlightInfo.Controls.Add(lblLoadedDepTimeValue);
            grpLoadedFlightInfo.Controls.Add(lblLoadedDepTimeStatic);
            grpLoadedFlightInfo.Controls.Add(lblLoadedDepartureValue);
            grpLoadedFlightInfo.Controls.Add(lblLoadedDepartureStatic);
            grpLoadedFlightInfo.Controls.Add(lblLoadedFlightNumberValue);
            grpLoadedFlightInfo.Controls.Add(lblLoadedFlightNumberStatic);
            grpLoadedFlightInfo.Font = new Font("Miracode", 10F, FontStyle.Bold);
            grpLoadedFlightInfo.ForeColor = Color.FromArgb(238, 206, 89);
            grpLoadedFlightInfo.Location = new Point(15, 60);
            grpLoadedFlightInfo.Name = "grpLoadedFlightInfo";
            grpLoadedFlightInfo.Padding = new Padding(10, 5, 10, 10);
            grpLoadedFlightInfo.Size = new Size(458, 185);
            grpLoadedFlightInfo.TabIndex = 3;
            grpLoadedFlightInfo.TabStop = false;
            grpLoadedFlightInfo.Text = "CURRENT FLIGHT INFORMATION";
            // 
            // lblLoadedCurrentStatusValue
            // 
            lblLoadedCurrentStatusValue.AutoSize = true;
            lblLoadedCurrentStatusValue.Font = new Font("Miracode", 9.75F, FontStyle.Bold);
            lblLoadedCurrentStatusValue.Location = new Point(214, 155);
            lblLoadedCurrentStatusValue.Name = "lblLoadedCurrentStatusValue";
            lblLoadedCurrentStatusValue.Size = new Size(46, 24);
            lblLoadedCurrentStatusValue.TabIndex = 0;
            lblLoadedCurrentStatusValue.Text = "N/A";
            // 
            // lblLoadedCurrentStatusStatic
            // 
            lblLoadedCurrentStatusStatic.AutoSize = true;
            lblLoadedCurrentStatusStatic.Font = new Font("Miracode", 9.75F);
            lblLoadedCurrentStatusStatic.Location = new Point(15, 155);
            lblLoadedCurrentStatusStatic.Name = "lblLoadedCurrentStatusStatic";
            lblLoadedCurrentStatusStatic.Size = new Size(175, 24);
            lblLoadedCurrentStatusStatic.TabIndex = 1;
            lblLoadedCurrentStatusStatic.Text = "Current Status:";
            // 
            // lblLoadedArrTimeValue
            // 
            lblLoadedArrTimeValue.AutoSize = true;
            lblLoadedArrTimeValue.Font = new Font("Miracode", 9.75F, FontStyle.Bold);
            lblLoadedArrTimeValue.Location = new Point(214, 130);
            lblLoadedArrTimeValue.Name = "lblLoadedArrTimeValue";
            lblLoadedArrTimeValue.Size = new Size(46, 24);
            lblLoadedArrTimeValue.TabIndex = 2;
            lblLoadedArrTimeValue.Text = "N/A";
            // 
            // lblLoadedArrTimeStatic
            // 
            lblLoadedArrTimeStatic.AutoSize = true;
            lblLoadedArrTimeStatic.Font = new Font("Miracode", 9.75F);
            lblLoadedArrTimeStatic.Location = new Point(15, 130);
            lblLoadedArrTimeStatic.Name = "lblLoadedArrTimeStatic";
            lblLoadedArrTimeStatic.Size = new Size(153, 24);
            lblLoadedArrTimeStatic.TabIndex = 3;
            lblLoadedArrTimeStatic.Text = "Arrival Time:";
            // 
            // lblLoadedArrivalValue
            // 
            lblLoadedArrivalValue.AutoSize = true;
            lblLoadedArrivalValue.Font = new Font("Miracode", 9.75F, FontStyle.Bold);
            lblLoadedArrivalValue.Location = new Point(214, 105);
            lblLoadedArrivalValue.Name = "lblLoadedArrivalValue";
            lblLoadedArrivalValue.Size = new Size(46, 24);
            lblLoadedArrivalValue.TabIndex = 4;
            lblLoadedArrivalValue.Text = "N/A";
            // 
            // lblLoadedArrivalStatic
            // 
            lblLoadedArrivalStatic.AutoSize = true;
            lblLoadedArrivalStatic.Font = new Font("Miracode", 9.75F);
            lblLoadedArrivalStatic.Location = new Point(15, 105);
            lblLoadedArrivalStatic.Name = "lblLoadedArrivalStatic";
            lblLoadedArrivalStatic.Size = new Size(153, 24);
            lblLoadedArrivalStatic.TabIndex = 5;
            lblLoadedArrivalStatic.Text = "Arrival City:";
            // 
            // lblLoadedDepTimeValue
            // 
            lblLoadedDepTimeValue.AutoSize = true;
            lblLoadedDepTimeValue.Font = new Font("Miracode", 9.75F, FontStyle.Bold);
            lblLoadedDepTimeValue.Location = new Point(214, 80);
            lblLoadedDepTimeValue.Name = "lblLoadedDepTimeValue";
            lblLoadedDepTimeValue.Size = new Size(46, 24);
            lblLoadedDepTimeValue.TabIndex = 6;
            lblLoadedDepTimeValue.Text = "N/A";
            // 
            // lblLoadedDepTimeStatic
            // 
            lblLoadedDepTimeStatic.AutoSize = true;
            lblLoadedDepTimeStatic.Font = new Font("Miracode", 9.75F);
            lblLoadedDepTimeStatic.Location = new Point(15, 80);
            lblLoadedDepTimeStatic.Name = "lblLoadedDepTimeStatic";
            lblLoadedDepTimeStatic.Size = new Size(175, 24);
            lblLoadedDepTimeStatic.TabIndex = 7;
            lblLoadedDepTimeStatic.Text = "Departure Time:";
            // 
            // lblLoadedDepartureValue
            // 
            lblLoadedDepartureValue.AutoSize = true;
            lblLoadedDepartureValue.Font = new Font("Miracode", 9.75F, FontStyle.Bold);
            lblLoadedDepartureValue.Location = new Point(214, 55);
            lblLoadedDepartureValue.Name = "lblLoadedDepartureValue";
            lblLoadedDepartureValue.Size = new Size(46, 24);
            lblLoadedDepartureValue.TabIndex = 8;
            lblLoadedDepartureValue.Text = "N/A";
            // 
            // lblLoadedDepartureStatic
            // 
            lblLoadedDepartureStatic.AutoSize = true;
            lblLoadedDepartureStatic.Font = new Font("Miracode", 9.75F);
            lblLoadedDepartureStatic.Location = new Point(15, 55);
            lblLoadedDepartureStatic.Name = "lblLoadedDepartureStatic";
            lblLoadedDepartureStatic.Size = new Size(175, 24);
            lblLoadedDepartureStatic.TabIndex = 9;
            lblLoadedDepartureStatic.Text = "Departure City:";
            // 
            // lblLoadedFlightNumberValue
            // 
            lblLoadedFlightNumberValue.AutoSize = true;
            lblLoadedFlightNumberValue.Font = new Font("Miracode", 9.75F, FontStyle.Bold);
            lblLoadedFlightNumberValue.Location = new Point(214, 30);
            lblLoadedFlightNumberValue.Name = "lblLoadedFlightNumberValue";
            lblLoadedFlightNumberValue.Size = new Size(46, 24);
            lblLoadedFlightNumberValue.TabIndex = 10;
            lblLoadedFlightNumberValue.Text = "N/A";
            // 
            // lblLoadedFlightNumberStatic
            // 
            lblLoadedFlightNumberStatic.AutoSize = true;
            lblLoadedFlightNumberStatic.Font = new Font("Miracode", 9.75F);
            lblLoadedFlightNumberStatic.Location = new Point(15, 30);
            lblLoadedFlightNumberStatic.Name = "lblLoadedFlightNumberStatic";
            lblLoadedFlightNumberStatic.Size = new Size(131, 24);
            lblLoadedFlightNumberStatic.TabIndex = 11;
            lblLoadedFlightNumberStatic.Text = "Flight No.:";
            // 
            // grpUpdateStatusControls
            // 
            grpUpdateStatusControls.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            grpUpdateStatusControls.Controls.Add(lblSelectNewStatus);
            grpUpdateStatusControls.Controls.Add(cmbNewStatus);
            grpUpdateStatusControls.Font = new Font("Miracode", 10F, FontStyle.Bold);
            grpUpdateStatusControls.ForeColor = Color.FromArgb(238, 206, 89);
            grpUpdateStatusControls.Location = new Point(15, 255);
            grpUpdateStatusControls.Name = "grpUpdateStatusControls";
            grpUpdateStatusControls.Padding = new Padding(10, 5, 10, 10);
            grpUpdateStatusControls.Size = new Size(458, 70);
            grpUpdateStatusControls.TabIndex = 4;
            grpUpdateStatusControls.TabStop = false;
            grpUpdateStatusControls.Text = "SELECT NEW STATUS";
            // 
            // lblSelectNewStatus
            // 
            lblSelectNewStatus.AutoSize = true;
            lblSelectNewStatus.Font = new Font("Miracode", 9.75F);
            lblSelectNewStatus.ForeColor = Color.FromArgb(238, 206, 89);
            lblSelectNewStatus.Location = new Point(15, 30);
            lblSelectNewStatus.Name = "lblSelectNewStatus";
            lblSelectNewStatus.Size = new Size(131, 24);
            lblSelectNewStatus.TabIndex = 1;
            lblSelectNewStatus.Text = "New Status:";
            // 
            // cmbNewStatus
            // 
            cmbNewStatus.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            cmbNewStatus.BackColor = Color.FromArgb(42, 45, 46);
            cmbNewStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNewStatus.FlatStyle = FlatStyle.Flat;
            cmbNewStatus.Font = new Font("Miracode", 9.75F);
            cmbNewStatus.ForeColor = Color.FromArgb(238, 206, 89);
            cmbNewStatus.FormattingEnabled = true;
            cmbNewStatus.Location = new Point(164, 26);
            cmbNewStatus.Name = "cmbNewStatus";
            cmbNewStatus.Size = new Size(281, 31);
            cmbNewStatus.TabIndex = 0;
            // 
            // btnUpdateStatusAction
            // 
            btnUpdateStatusAction.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnUpdateStatusAction.BackColor = Color.FromArgb(0, 100, 0);
            btnUpdateStatusAction.FlatAppearance.BorderColor = Color.FromArgb(238, 206, 89);
            btnUpdateStatusAction.FlatAppearance.BorderSize = 0;
            btnUpdateStatusAction.FlatStyle = FlatStyle.Flat;
            btnUpdateStatusAction.Font = new Font("Miracode", 10F, FontStyle.Bold);
            btnUpdateStatusAction.ForeColor = Color.White;
            btnUpdateStatusAction.Location = new Point(313, 340);
            btnUpdateStatusAction.Name = "btnUpdateStatusAction";
            btnUpdateStatusAction.Size = new Size(160, 32);
            btnUpdateStatusAction.TabIndex = 5;
            btnUpdateStatusAction.Text = "Update Status";
            btnUpdateStatusAction.UseVisualStyleBackColor = false;
            btnUpdateStatusAction.Click += btnUpdateStatusAction_Click;
            // 
            // btnFSFClose
            // 
            btnFSFClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnFSFClose.BackColor = Color.Maroon;
            btnFSFClose.DialogResult = DialogResult.Cancel;
            btnFSFClose.FlatAppearance.BorderColor = Color.FromArgb(0, 0, 0, 0);
            btnFSFClose.FlatAppearance.BorderSize = 0;
            btnFSFClose.FlatStyle = FlatStyle.Flat;
            btnFSFClose.Font = new Font("Miracode", 10F, FontStyle.Bold);
            btnFSFClose.ForeColor = Color.White;
            btnFSFClose.Location = new Point(179, 340);
            btnFSFClose.Name = "btnFSFClose";
            btnFSFClose.Size = new Size(116, 32);
            btnFSFClose.TabIndex = 6;
            btnFSFClose.Text = "Close";
            btnFSFClose.UseVisualStyleBackColor = false;
            btnFSFClose.Click += btnFSFClose_Click;
            // 
            // FlightStatusForm
            // 
            AcceptButton = btnSearchFlight;
            AutoScaleDimensions = new SizeF(11F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 30, 30);
            CancelButton = btnFSFClose;
            ClientSize = new Size(488, 384);
            Controls.Add(btnUpdateStatusAction);
            Controls.Add(btnFSFClose);
            Controls.Add(grpUpdateStatusControls);
            Controls.Add(grpLoadedFlightInfo);
            Controls.Add(btnSearchFlight);
            Controls.Add(txtFlightIdSearch);
            Controls.Add(lblSearchFlightIdPrompt);
            Font = new Font("Miracode", 9.75F);
            ForeColor = Color.FromArgb(238, 206, 89);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FlightStatusForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Manage Flight Status";
            grpLoadedFlightInfo.ResumeLayout(false);
            grpLoadedFlightInfo.PerformLayout();
            grpUpdateStatusControls.ResumeLayout(false);
            grpUpdateStatusControls.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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