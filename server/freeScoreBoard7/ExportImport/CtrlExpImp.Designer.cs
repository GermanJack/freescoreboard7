namespace FreeScoreBoard.ExportImport
{
	partial class CtrlExpImp
	{
		/// <summary> 
		/// Erforderliche Designervariable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary> 
		/// Verwendete Ressourcen bereinigen.
		/// </summary>
		/// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Vom Komponenten-Designer generierter Code

		/// <summary> 
		/// Erforderliche Methode für die Designerunterstützung. 
		/// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.BtnTabellenImp = new System.Windows.Forms.Button();
			this.BtnTabellenExp = new System.Windows.Forms.Button();
			this.BtnMannschaftenImp = new System.Windows.Forms.Button();
			this.BtnMannschaftenExp = new System.Windows.Forms.Button();
			this.BtnOptionenImp = new System.Windows.Forms.Button();
			this.BtnOptionenExp = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.BtnTabellenImp);
			this.groupBox1.Controls.Add(this.BtnTabellenExp);
			this.groupBox1.Controls.Add(this.BtnMannschaftenImp);
			this.groupBox1.Controls.Add(this.BtnMannschaftenExp);
			this.groupBox1.Controls.Add(this.BtnOptionenImp);
			this.groupBox1.Controls.Add(this.BtnOptionenExp);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox1.Location = new System.Drawing.Point(0, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(333, 170);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Export / Import";
			// 
			// BtnTabellenImp
			// 
			this.BtnTabellenImp.Location = new System.Drawing.Point(169, 72);
			this.BtnTabellenImp.Name = "BtnTabellenImp";
			this.BtnTabellenImp.Size = new System.Drawing.Size(140, 33);
			this.BtnTabellenImp.TabIndex = 5;
			this.BtnTabellenImp.Text = "Import Tabellendarstellung";
			this.BtnTabellenImp.UseVisualStyleBackColor = true;
			this.BtnTabellenImp.Click += new System.EventHandler(this.BtnTabellenImp_Click);
			// 
			// BtnTabellenExp
			// 
			this.BtnTabellenExp.Location = new System.Drawing.Point(23, 72);
			this.BtnTabellenExp.Name = "BtnTabellenExp";
			this.BtnTabellenExp.Size = new System.Drawing.Size(140, 33);
			this.BtnTabellenExp.TabIndex = 4;
			this.BtnTabellenExp.Text = "Export Tabellendarstellung";
			this.BtnTabellenExp.UseVisualStyleBackColor = true;
			this.BtnTabellenExp.Click += new System.EventHandler(this.BtnTabellenExp_Click);
			// 
			// BtnMannschaftenImp
			// 
			this.BtnMannschaftenImp.Location = new System.Drawing.Point(169, 111);
			this.BtnMannschaftenImp.Name = "BtnMannschaftenImp";
			this.BtnMannschaftenImp.Size = new System.Drawing.Size(140, 33);
			this.BtnMannschaftenImp.TabIndex = 3;
			this.BtnMannschaftenImp.Text = "Import Mannaschaften";
			this.BtnMannschaftenImp.UseVisualStyleBackColor = true;
			this.BtnMannschaftenImp.Click += new System.EventHandler(this.BtnMannschaftenImp_Click);
			// 
			// BtnMannschaftenExp
			// 
			this.BtnMannschaftenExp.Location = new System.Drawing.Point(23, 111);
			this.BtnMannschaftenExp.Name = "BtnMannschaftenExp";
			this.BtnMannschaftenExp.Size = new System.Drawing.Size(140, 33);
			this.BtnMannschaftenExp.TabIndex = 2;
			this.BtnMannschaftenExp.Text = "Export Mannschaften";
			this.BtnMannschaftenExp.UseVisualStyleBackColor = true;
			this.BtnMannschaftenExp.Click += new System.EventHandler(this.BtnMannschaftenExp_Click);
			// 
			// BtnOptionenImp
			// 
			this.BtnOptionenImp.Location = new System.Drawing.Point(169, 33);
			this.BtnOptionenImp.Name = "BtnOptionenImp";
			this.BtnOptionenImp.Size = new System.Drawing.Size(140, 33);
			this.BtnOptionenImp.TabIndex = 1;
			this.BtnOptionenImp.Text = "Import Optionen";
			this.BtnOptionenImp.UseVisualStyleBackColor = true;
			this.BtnOptionenImp.Click += new System.EventHandler(this.BtnOptionenImp_Click);
			// 
			// BtnOptionenExp
			// 
			this.BtnOptionenExp.Location = new System.Drawing.Point(23, 33);
			this.BtnOptionenExp.Name = "BtnOptionenExp";
			this.BtnOptionenExp.Size = new System.Drawing.Size(140, 33);
			this.BtnOptionenExp.TabIndex = 0;
			this.BtnOptionenExp.Text = "Export Optionen";
			this.BtnOptionenExp.UseVisualStyleBackColor = true;
			this.BtnOptionenExp.Click += new System.EventHandler(this.BtnOptionenExp_Click);
			// 
			// CtrlExpImp
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Name = "CtrlExpImp";
			this.Size = new System.Drawing.Size(333, 170);
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button BtnTabellenImp;
		private System.Windows.Forms.Button BtnTabellenExp;
		private System.Windows.Forms.Button BtnMannschaftenImp;
		private System.Windows.Forms.Button BtnMannschaftenExp;
		private System.Windows.Forms.Button BtnOptionenImp;
		private System.Windows.Forms.Button BtnOptionenExp;
	}
}
