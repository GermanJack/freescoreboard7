using System.Windows.Forms;
using FreeScoreBoard.Forms.Controls;
using FreeScoreBoard.Server;

namespace FreeScoreBoard.Forms.Display
{
	public partial class FrmDisplay : Form
	{
		private readonly CtrlBrowser CtrlBrowser_Display1;

		public FrmDisplay(bool DevTools = false)
		{
			this.InitializeComponent();
			this.CtrlBrowser_Display1 = new CtrlBrowser(DevTools);
			this.CtrlBrowser_Display1.Dock = DockStyle.Fill;
			this.CtrlBrowser_Display1.URL = ClsServer.Instance.LinkDisplay;
			this.Controls.Add(this.CtrlBrowser_Display1);
		}

		public void ShowDevTools()
		{
			this.CtrlBrowser_Display1.ShowDevTools(this, null);
		}

		public void RefreshBrowser()
		{
			this.CtrlBrowser_Display1.RefreshBrowser();
		}
	}
}