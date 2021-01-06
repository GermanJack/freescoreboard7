namespace FreeScoreBoard
{
	partial class ServerSettings
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
			this.CmbIP = new System.Windows.Forms.ComboBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.NudHTTPPort = new System.Windows.Forms.NumericUpDown();
			this.NudWSPort = new System.Windows.Forms.NumericUpDown();
			this.GrpSettings = new System.Windows.Forms.GroupBox();
			this.TxtPasswort = new System.Windows.Forms.TextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.ChkServer = new System.Windows.Forms.CheckBox();
			this.BtnOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.NudHTTPPort)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.NudWSPort)).BeginInit();
			this.GrpSettings.SuspendLayout();
			this.SuspendLayout();
			// 
			// CmbIP
			// 
			this.CmbIP.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.CmbIP.FormattingEnabled = true;
			this.CmbIP.Location = new System.Drawing.Point(128, 23);
			this.CmbIP.Name = "CmbIP";
			this.CmbIP.Size = new System.Drawing.Size(212, 21);
			this.CmbIP.TabIndex = 0;
			this.CmbIP.SelectedIndexChanged += new System.EventHandler(this.CmbIP_SelectedIndexChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(29, 22);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(67, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Server aktiv:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(24, 26);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(20, 13);
			this.label2.TabIndex = 3;
			this.label2.Text = "IP:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(24, 53);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(61, 13);
			this.label3.TabIndex = 4;
			this.label3.Text = "HTTP Port:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(24, 79);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(50, 13);
			this.label4.TabIndex = 5;
			this.label4.Text = "WS Port:";
			// 
			// NudHTTPPort
			// 
			this.NudHTTPPort.Location = new System.Drawing.Point(128, 51);
			this.NudHTTPPort.Maximum = new decimal(new int[] {
            49150,
            0,
            0,
            0});
			this.NudHTTPPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.NudHTTPPort.Name = "NudHTTPPort";
			this.NudHTTPPort.Size = new System.Drawing.Size(109, 20);
			this.NudHTTPPort.TabIndex = 6;
			this.NudHTTPPort.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.NudHTTPPort.ValueChanged += new System.EventHandler(this.NudHTTPPort_ValueChanged);
			// 
			// NudWSPort
			// 
			this.NudWSPort.Location = new System.Drawing.Point(128, 77);
			this.NudWSPort.Maximum = new decimal(new int[] {
            49150,
            0,
            0,
            0});
			this.NudWSPort.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.NudWSPort.Name = "NudWSPort";
			this.NudWSPort.Size = new System.Drawing.Size(109, 20);
			this.NudWSPort.TabIndex = 7;
			this.NudWSPort.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			this.NudWSPort.ValueChanged += new System.EventHandler(this.NudWSPort_ValueChanged);
			// 
			// GrpSettings
			// 
			this.GrpSettings.Controls.Add(this.TxtPasswort);
			this.GrpSettings.Controls.Add(this.label5);
			this.GrpSettings.Controls.Add(this.NudWSPort);
			this.GrpSettings.Controls.Add(this.NudHTTPPort);
			this.GrpSettings.Controls.Add(this.label4);
			this.GrpSettings.Controls.Add(this.label3);
			this.GrpSettings.Controls.Add(this.label2);
			this.GrpSettings.Controls.Add(this.CmbIP);
			this.GrpSettings.Location = new System.Drawing.Point(14, 51);
			this.GrpSettings.Name = "GrpSettings";
			this.GrpSettings.Size = new System.Drawing.Size(366, 148);
			this.GrpSettings.TabIndex = 8;
			this.GrpSettings.TabStop = false;
			this.GrpSettings.Text = "Einstellungen";
			// 
			// TxtPasswort
			// 
			this.TxtPasswort.Location = new System.Drawing.Point(178, 112);
			this.TxtPasswort.Name = "TxtPasswort";
			this.TxtPasswort.Size = new System.Drawing.Size(162, 20);
			this.TxtPasswort.TabIndex = 9;
			this.TxtPasswort.TextChanged += new System.EventHandler(this.TxtPasswort_TextChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(24, 115);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(148, 13);
			this.label5.TabIndex = 8;
			this.label5.Text = "Passwort für Remotekontrolle:";
			// 
			// ChkServer
			// 
			this.ChkServer.AutoCheck = false;
			this.ChkServer.AutoSize = true;
			this.ChkServer.Location = new System.Drawing.Point(142, 22);
			this.ChkServer.Name = "ChkServer";
			this.ChkServer.Size = new System.Drawing.Size(15, 14);
			this.ChkServer.TabIndex = 9;
			this.ChkServer.UseVisualStyleBackColor = true;
			this.ChkServer.Click += new System.EventHandler(this.ChkServer_Click);
			// 
			// BtnOK
			// 
			this.BtnOK.Location = new System.Drawing.Point(313, 220);
			this.BtnOK.Name = "BtnOK";
			this.BtnOK.Size = new System.Drawing.Size(67, 29);
			this.BtnOK.TabIndex = 10;
			this.BtnOK.Text = "OK";
			this.BtnOK.UseVisualStyleBackColor = true;
			this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
			// 
			// ServerSettings
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(399, 261);
			this.ControlBox = false;
			this.Controls.Add(this.BtnOK);
			this.Controls.Add(this.ChkServer);
			this.Controls.Add(this.GrpSettings);
			this.Controls.Add(this.label1);
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(415, 300);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(415, 250);
			this.Name = "ServerSettings";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Server ...";
			this.Load += new System.EventHandler(this.ServerSettings_Load);
			((System.ComponentModel.ISupportInitialize)(this.NudHTTPPort)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.NudWSPort)).EndInit();
			this.GrpSettings.ResumeLayout(false);
			this.GrpSettings.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ComboBox CmbIP;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.NumericUpDown NudHTTPPort;
		private System.Windows.Forms.NumericUpDown NudWSPort;
		private System.Windows.Forms.GroupBox GrpSettings;
		private System.Windows.Forms.CheckBox ChkServer;
		private System.Windows.Forms.TextBox TxtPasswort;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button BtnOK;
	}
}