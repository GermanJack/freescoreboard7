namespace FreeScoreBoard.ExportImport
{
	partial class DlgAppend
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
			this.BtnAppend = new System.Windows.Forms.Button();
			this.BtnOverwrite = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// BtnAppend
			// 
			this.BtnAppend.Location = new System.Drawing.Point(12, 12);
			this.BtnAppend.Name = "BtnAppend";
			this.BtnAppend.Size = new System.Drawing.Size(152, 33);
			this.BtnAppend.TabIndex = 0;
			this.BtnAppend.Text = "hinzufügen";
			this.BtnAppend.UseVisualStyleBackColor = true;
			this.BtnAppend.Click += new System.EventHandler(this.BtnAppend_Click);
			// 
			// BtnOverwrite
			// 
			this.BtnOverwrite.Location = new System.Drawing.Point(12, 51);
			this.BtnOverwrite.Name = "BtnOverwrite";
			this.BtnOverwrite.Size = new System.Drawing.Size(152, 33);
			this.BtnOverwrite.TabIndex = 1;
			this.BtnOverwrite.Text = "überschreiben";
			this.BtnOverwrite.UseVisualStyleBackColor = true;
			this.BtnOverwrite.Click += new System.EventHandler(this.BtnOverwrite_Click);
			// 
			// DlgAppend
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(176, 99);
			this.Controls.Add(this.BtnOverwrite);
			this.Controls.Add(this.BtnAppend);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "DlgAppend";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "DlgAppend";
			this.Load += new System.EventHandler(this.DlgAppend_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button BtnAppend;
		private System.Windows.Forms.Button BtnOverwrite;
	}
}