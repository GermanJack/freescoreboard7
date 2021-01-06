namespace FreeScoreBoard.Server
{
    partial class FrmUser
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
            this.TxtPW1 = new System.Windows.Forms.TextBox();
            this.TxtPW2 = new System.Windows.Forms.TextBox();
            this.BtnShow = new System.Windows.Forms.Button();
            this.BtnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.TxtName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TxtPW1
            // 
            this.TxtPW1.Location = new System.Drawing.Point(131, 43);
            this.TxtPW1.Name = "TxtPW1";
            this.TxtPW1.PasswordChar = '*';
            this.TxtPW1.Size = new System.Drawing.Size(197, 20);
            this.TxtPW1.TabIndex = 7;
            // 
            // TxtPW2
            // 
            this.TxtPW2.Location = new System.Drawing.Point(131, 69);
            this.TxtPW2.Name = "TxtPW2";
            this.TxtPW2.PasswordChar = '*';
            this.TxtPW2.Size = new System.Drawing.Size(197, 20);
            this.TxtPW2.TabIndex = 8;
            // 
            // BtnShow
            // 
            this.BtnShow.Location = new System.Drawing.Point(334, 72);
            this.BtnShow.Name = "BtnShow";
            this.BtnShow.Size = new System.Drawing.Size(21, 19);
            this.BtnShow.TabIndex = 11;
            this.BtnShow.Text = "?";
            this.BtnShow.UseVisualStyleBackColor = true;
            this.BtnShow.Click += new System.EventHandler(this.BtnShow_Click);
            // 
            // BtnOK
            // 
            this.BtnOK.Location = new System.Drawing.Point(255, 109);
            this.BtnOK.Name = "BtnOK";
            this.BtnOK.Size = new System.Drawing.Size(73, 27);
            this.BtnOK.TabIndex = 9;
            this.BtnOK.Text = "OK";
            this.BtnOK.UseVisualStyleBackColor = true;
            this.BtnOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Passwoerwiederholung:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Passwort:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Benutzername:";
            // 
            // TxtName
            // 
            this.TxtName.Location = new System.Drawing.Point(131, 17);
            this.TxtName.Name = "TxtName";
            this.TxtName.Size = new System.Drawing.Size(197, 20);
            this.TxtName.TabIndex = 6;
            // 
            // FrmUser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(370, 150);
            this.Controls.Add(this.TxtPW1);
            this.Controls.Add(this.TxtPW2);
            this.Controls.Add(this.BtnShow);
            this.Controls.Add(this.BtnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TxtName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmUser";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Text = "Benutzername / Passwort";
            this.Load += new System.EventHandler(this.FrmUser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TxtPW1;
        private System.Windows.Forms.TextBox TxtPW2;
        private System.Windows.Forms.Button BtnShow;
        private System.Windows.Forms.Button BtnOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TxtName;
    }
}