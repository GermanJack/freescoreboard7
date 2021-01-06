using System;
using System.Windows.Forms;

namespace FreeScoreBoard.ExportImport
{
	public partial class CtrlExpImp : UserControl
	{
		public CtrlExpImp()
		{
			InitializeComponent();
		}

		private void BtnOptionenExp_Click(object sender, EventArgs e)
		{
			string filename = ClsDialog.Save(ClsFileExtentions.Optionen);
			ClsOptionenImpExp.ExportToFile(filename);
		}

		private void BtnOptionenImp_Click(object sender, EventArgs e)
		{
			string filename = ClsDialog.Open(ClsFileExtentions.Optionen);
			ClsOptionenImpExp.ImportFromFile(filename);
		}

		private void BtnTabellenExp_Click(object sender, EventArgs e)
		{
			string filename = ClsDialog.Save(ClsFileExtentions.Tabellen);
			ClsTabellendarstellungImpExp.ExportToFile(filename);
		}

		private void BtnTabellenImp_Click(object sender, EventArgs e)
		{
			string filename = ClsDialog.Open(ClsFileExtentions.Tabellen);
			ClsTabellendarstellungImpExp.ImportFromFile(filename);
		}

		private void BtnMannschaftenExp_Click(object sender, EventArgs e)
		{
			string filename = ClsDialog.Save(ClsFileExtentions.Mannschaften);
			ClsMannschaftenImpExp.ExportToFile(filename);
		}

		private void BtnMannschaftenImp_Click(object sender, EventArgs e)
		{
			string filename = ClsDialog.Open(ClsFileExtentions.Tabellen);
			ClsMannschaftenImpExp.ImportFromFile(filename);
		}
	}
}
