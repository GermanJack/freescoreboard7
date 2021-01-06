using System;
using System.Windows.Forms;

namespace FreeScoreBoard.ExportImport
{
	public partial class DlgAppend : Form
	{
		public string Header
		{
			get
			{
				return this.Text;
			}

			set
			{
				this.Text = value;
			}
		}
		
		public DlgAppend()
		{
			InitializeComponent();
		}

		private void BtnAppend_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.No;
			this.Close();
		}

		private void BtnOverwrite_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Yes;
			this.Close();
		}

		private void DlgAppend_Load(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
		}
	}
}
