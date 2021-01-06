namespace FreeScoreBoard.Forms.Controls
{
    partial class CtrlBrowser
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
            this.BtnR = new System.Windows.Forms.Button();
            this.BtnD = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BtnR
            // 
            this.BtnR.Location = new System.Drawing.Point(0, 0);
            this.BtnR.Name = "BtnR";
            this.BtnR.Size = new System.Drawing.Size(20, 20);
            this.BtnR.TabIndex = 0;
            this.BtnR.Text = "R";
            this.BtnR.UseVisualStyleBackColor = true;
            this.BtnR.Visible = false;
            this.BtnR.Click += new System.EventHandler(this.BtnR_Click);
            // 
            // BtnD
            // 
            this.BtnD.Location = new System.Drawing.Point(21, 0);
            this.BtnD.Name = "BtnD";
            this.BtnD.Size = new System.Drawing.Size(20, 20);
            this.BtnD.TabIndex = 1;
            this.BtnD.Text = "D";
            this.BtnD.UseVisualStyleBackColor = true;
            this.BtnD.Visible = false;
            this.BtnD.Click += new System.EventHandler(this.BtnD_Click);
            // 
            // CtrlBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BtnD);
            this.Controls.Add(this.BtnR);
            this.Name = "CtrlBrowser";
            this.ControlRemoved += new System.Windows.Forms.ControlEventHandler(this.CtrlBrowser_ControlRemoved);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnR;
        private System.Windows.Forms.Button BtnD;
    }
}
