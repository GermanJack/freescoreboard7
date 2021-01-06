using System;
using System.Windows.Forms;
using FreeScoreBoard.Core;

namespace FreeScoreBoard.Forms.Dialoge.Forms.Dialoge
{

	public partial class DlgError : Form
	{
		private Exception myex;
		private string myFunction;
		private string myModul;

		public DlgError()
		{
			this.InitializeComponent();
			//ClsTranslateControls.ChangeControls(this);
		}

		public Exception Ex
		{
			get { return this.myex; }
			set { this.myex = value; }
		}

		public string Function
		{
			get { return this.myFunction; }
			set { this.myFunction = value; }
		}

		public string Modul
		{
			get { return this.myModul; }
			set { this.myModul = value; }
		}

		private void DlgError_Load(object sender, EventArgs e)
		{
			// this.Lbl1.Text = "Es ist ein Programmfehler aufgetreten.\n";

			// Zentrale ausgabe der fehlermeldung
			this.txtError.Text = ClsFunktionen.MakeErrorMessage(this.myModul, this.myFunction, this.myex);
			//Clipboard.SetText(stb);
		}
	}
}
