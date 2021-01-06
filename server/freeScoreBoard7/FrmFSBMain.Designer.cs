namespace FreeScoreBoard
{
	partial class FrmFSBMain
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmFSBMain));
			this.BtnKontrolle = new System.Windows.Forms.Button();
			this.BtnAnzeige = new System.Windows.Forms.Button();
			this.BtnServer = new System.Windows.Forms.Button();
			this.BtnAnzeigeFenster = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// BtnKontrolle
			// 
			this.BtnKontrolle.Location = new System.Drawing.Point(12, 70);
			this.BtnKontrolle.Name = "BtnKontrolle";
			this.BtnKontrolle.Size = new System.Drawing.Size(272, 52);
			this.BtnKontrolle.TabIndex = 0;
			this.BtnKontrolle.Text = "Kontrollfenster";
			this.BtnKontrolle.UseVisualStyleBackColor = true;
			this.BtnKontrolle.Click += new System.EventHandler(this.BtnKontrolle_Click);
			// 
			// BtnAnzeige
			// 
			this.BtnAnzeige.Location = new System.Drawing.Point(151, 12);
			this.BtnAnzeige.Name = "BtnAnzeige";
			this.BtnAnzeige.Size = new System.Drawing.Size(133, 52);
			this.BtnAnzeige.TabIndex = 1;
			this.BtnAnzeige.Text = "Anzeigefenster Browser";
			this.BtnAnzeige.UseVisualStyleBackColor = true;
			this.BtnAnzeige.Click += new System.EventHandler(this.BtnAnzeige_Click);
			// 
			// BtnServer
			// 
			this.BtnServer.Location = new System.Drawing.Point(12, 128);
			this.BtnServer.Name = "BtnServer";
			this.BtnServer.Size = new System.Drawing.Size(272, 52);
			this.BtnServer.TabIndex = 2;
			this.BtnServer.Text = "Servereinstellungen";
			this.BtnServer.UseVisualStyleBackColor = true;
			this.BtnServer.Click += new System.EventHandler(this.BtnServer_Click);
			// 
			// BtnAnzeigeFenster
			// 
			this.BtnAnzeigeFenster.Location = new System.Drawing.Point(12, 12);
			this.BtnAnzeigeFenster.Name = "BtnAnzeigeFenster";
			this.BtnAnzeigeFenster.Size = new System.Drawing.Size(133, 52);
			this.BtnAnzeigeFenster.TabIndex = 3;
			this.BtnAnzeigeFenster.Text = "Anzeigefenster Lokal";
			this.BtnAnzeigeFenster.UseVisualStyleBackColor = true;
			this.BtnAnzeigeFenster.Click += new System.EventHandler(this.BtnAnzeigeFenster_Click);
			// 
			// FrmFSBMain
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(296, 191);
			this.Controls.Add(this.BtnAnzeigeFenster);
			this.Controls.Add(this.BtnServer);
			this.Controls.Add(this.BtnAnzeige);
			this.Controls.Add(this.BtnKontrolle);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximumSize = new System.Drawing.Size(312, 230);
			this.MinimumSize = new System.Drawing.Size(312, 230);
			this.Name = "FrmFSBMain";
			this.Text = "FrmFSBMain";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmFSBMain_FormClosing);
			this.Load += new System.EventHandler(this.FrmFSBMain_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnKontrolle;
		private System.Windows.Forms.Button BtnAnzeige;
		private System.Windows.Forms.Button BtnServer;
		private System.Windows.Forms.Button BtnAnzeigeFenster;
	}
}