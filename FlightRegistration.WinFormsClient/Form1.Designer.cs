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
            this.btnConnectSocket = new System.Windows.Forms.Button();
            this.btnDisconnectSocket = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlConnection = new System.Windows.Forms.Panel();
            this.pnlConnection.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstLogMessages
            // 
            this.lstLogMessages.Dock = System.Windows.Forms.DockStyle.Fill; // Changed from previous description for simplicity
            this.lstLogMessages.FormattingEnabled = true;
            this.lstLogMessages.HorizontalScrollbar = true;
            this.lstLogMessages.ItemHeight = 15; // Default, adjust if font changes this
            this.lstLogMessages.Location = new System.Drawing.Point(0, 80); // Assuming lblTitle and pnlConnection take 80px
            this.lstLogMessages.Name = "lstLogMessages";
            this.lstLogMessages.Size = new System.Drawing.Size(784, 381); // Example size
            this.lstLogMessages.TabIndex = 2;
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
            this.lblTitle.Size = new System.Drawing.Size(784, 40); // Example height
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Agent Terminal";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlConnection
            // 
            this.pnlConnection.BackColor = System.Drawing.Color.LightSteelBlue;
            this.pnlConnection.Controls.Add(this.btnConnectSocket);
            this.pnlConnection.Controls.Add(this.btnDisconnectSocket);
            this.pnlConnection.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlConnection.Location = new System.Drawing.Point(0, 40); // After lblTitle
            this.pnlConnection.Name = "pnlConnection";
            this.pnlConnection.Size = new System.Drawing.Size(784, 40);
            this.pnlConnection.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F); // Default Font for Form
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.AliceBlue;
            this.ClientSize = new System.Drawing.Size(784, 461); // Example size
            this.Controls.Add(this.lstLogMessages);
            this.Controls.Add(this.pnlConnection);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 9F); // Default Font for Form
            this.Name = "Form1";
            this.Text = "Flight Check-in Agent Terminal ✈️";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.pnlConnection.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.ListBox lstLogMessages;
        private System.Windows.Forms.Button btnConnectSocket;
        private System.Windows.Forms.Button btnDisconnectSocket;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlConnection;
    }
}