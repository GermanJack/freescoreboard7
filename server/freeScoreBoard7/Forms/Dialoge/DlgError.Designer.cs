namespace FreeScoreBoard.Forms.Dialoge.Forms.Dialoge
{
    partial class DlgError
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
			this.cmdGo = new System.Windows.Forms.Button();
			this.cmdStop = new System.Windows.Forms.Button();
			this.LblText1 = new System.Windows.Forms.Label();
			this.txtError = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// cmdGo
			// 
			this.cmdGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.cmdGo.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.cmdGo.Location = new System.Drawing.Point(12, 198);
			this.cmdGo.Name = "cmdGo";
			this.cmdGo.Size = new System.Drawing.Size(125, 26);
			this.cmdGo.TabIndex = 0;
			this.cmdGo.Text = "Programm fortstzen";
			this.cmdGo.UseVisualStyleBackColor = true;
			// 
			// cmdStop
			// 
			this.cmdStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cmdStop.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cmdStop.Location = new System.Drawing.Point(541, 198);
			this.cmdStop.Name = "cmdStop";
			this.cmdStop.Size = new System.Drawing.Size(125, 26);
			this.cmdStop.TabIndex = 1;
			this.cmdStop.Text = "Programm beenden";
			this.cmdStop.UseVisualStyleBackColor = true;
			// 
			// LblText1
			// 
			this.LblText1.AutoSize = true;
			this.LblText1.Location = new System.Drawing.Point(12, 9);
			this.LblText1.Name = "LblText1";
			this.LblText1.Size = new System.Drawing.Size(185, 13);
			this.LblText1.TabIndex = 2;
			this.LblText1.Text = "Es ist ein Programmfehler aufgetreten.";
			// 
			// txtError
			// 
			this.txtError.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtError.Location = new System.Drawing.Point(15, 25);
			this.txtError.Multiline = true;
			this.txtError.Name = "txtError";
			this.txtError.ReadOnly = true;
			this.txtError.ScrollBars = System.Windows.Forms.ScrollBars.Both;
			this.txtError.Size = new System.Drawing.Size(651, 167);
			this.txtError.TabIndex = 3;
			// 
			// DlgError
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(678, 236);
			this.ControlBox = false;
			this.Controls.Add(this.txtError);
			this.Controls.Add(this.LblText1);
			this.Controls.Add(this.cmdStop);
			this.Controls.Add(this.cmdGo);
			this.Name = "DlgError";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Programmfehler...";
			this.Load += new System.EventHandler(this.DlgError_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdGo;
        private System.Windows.Forms.Button cmdStop;
        private System.Windows.Forms.Label LblText1;
        private System.Windows.Forms.TextBox txtError;
    }
}