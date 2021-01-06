using System.Windows.Forms;

namespace FreeScoreBoard.Forms.Dialoge
{
	public partial class FrmSplash : Form
	{
		public FrmSplash()
		{
			this.InitializeComponent();
			//ClsTranslateControls.ChangeControls(this);
		}

		private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start("http://www.FreeScoreBoard.org");
		}

		private void FrmSplash_Load(object sender, System.EventArgs e)
		{
			this.LblVer.Text = Application.ProductVersion;
		}
	}
}
