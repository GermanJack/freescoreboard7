namespace FreeScoreBoard.Forms.Dialoge
{
    partial class FrmSplash
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
			this.LblPrg = new System.Windows.Forms.Label();
			this.LblDeveloper = new System.Windows.Forms.Label();
			this.LblUrl = new System.Windows.Forms.Label();
			this.LblVer = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// LblPrg
			// 
			this.LblPrg.BackColor = System.Drawing.Color.Transparent;
			this.LblPrg.Font = new System.Drawing.Font("Microsoft Sans Serif", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblPrg.Location = new System.Drawing.Point(270, 116);
			this.LblPrg.Name = "LblPrg";
			this.LblPrg.Size = new System.Drawing.Size(349, 61);
			this.LblPrg.TabIndex = 0;
			this.LblPrg.Text = "FreeScoreBoard";
			this.LblPrg.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// LblDeveloper
			// 
			this.LblDeveloper.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.LblDeveloper.AutoSize = true;
			this.LblDeveloper.BackColor = System.Drawing.Color.Transparent;
			this.LblDeveloper.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblDeveloper.Location = new System.Drawing.Point(273, 269);
			this.LblDeveloper.Name = "LblDeveloper";
			this.LblDeveloper.Size = new System.Drawing.Size(175, 24);
			this.LblDeveloper.TabIndex = 1;
			this.LblDeveloper.Text = "Jürgen Schneiderat";
			this.LblDeveloper.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblUrl
			// 
			this.LblUrl.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.LblUrl.AutoSize = true;
			this.LblUrl.BackColor = System.Drawing.Color.Transparent;
			this.LblUrl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblUrl.Location = new System.Drawing.Point(273, 310);
			this.LblUrl.Name = "LblUrl";
			this.LblUrl.Size = new System.Drawing.Size(275, 24);
			this.LblUrl.TabIndex = 2;
			this.LblUrl.Text = "http://www.FreeScoreBoard.org";
			this.LblUrl.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LblVer
			// 
			this.LblVer.Anchor = System.Windows.Forms.AnchorStyles.Right;
			this.LblVer.BackColor = System.Drawing.Color.Transparent;
			this.LblVer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.LblVer.Location = new System.Drawing.Point(273, 168);
			this.LblVer.Name = "LblVer";
			this.LblVer.Size = new System.Drawing.Size(75, 24);
			this.LblVer.TabIndex = 3;
			this.LblVer.Text = "Version";
			this.LblVer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// FrmSplash
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::FreeScoreBoard.Properties.Resources.Splash_v7;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.ClientSize = new System.Drawing.Size(600, 400);
			this.ControlBox = false;
			this.Controls.Add(this.LblVer);
			this.Controls.Add(this.LblUrl);
			this.Controls.Add(this.LblDeveloper);
			this.Controls.Add(this.LblPrg);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.MaximizeBox = false;
			this.MaximumSize = new System.Drawing.Size(600, 400);
			this.MinimizeBox = false;
			this.MinimumSize = new System.Drawing.Size(600, 400);
			this.Name = "FrmSplash";
			this.ShowIcon = false;
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Load += new System.EventHandler(this.FrmSplash_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LblPrg;
        private System.Windows.Forms.Label LblDeveloper;
        private System.Windows.Forms.Label LblUrl;
        private System.Windows.Forms.Label LblVer;

    }
}