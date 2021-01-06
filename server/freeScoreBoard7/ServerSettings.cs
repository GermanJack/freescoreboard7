using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Properties;
using FreeScoreBoard.Server;

namespace FreeScoreBoard
{
	public partial class ServerSettings : Form
	{
		public ServerSettings()
		{
			this.InitializeComponent();
		}

		public void Ini()
		{
			// HG für Schalter setzten
			this.Schalter();

			// IP's füllen und setzten
			List<string> ips = new List<string>();
			var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					ips.Add(ip.ToString());
				}
			}

			ips.Add(System.Net.IPAddress.Loopback.ToString());

			if (ips.Count == 0)
			{
				MessageBox.Show("Kein Netzwerkadapter mit einer IPv4 Adresse gefunden!");
			}
			else
			{
				this.CmbIP.DataSource = ips;
				this.CmbIP.Text = ClsServer.Instance.IP;
			}

			// HTTP Port setzten
			this.NudHTTPPort.Value = ClsServer.Instance.HttpsvPort;


			// WS Port setzten
			this.NudWSPort.Value = ClsServer.Instance.WsPort;

			// Passwort:
			this.TxtPasswort.Text = Core.DBControler.ClsOptionsControler.Password("Password");
		}

		private void ServerSettings_Load(object sender, EventArgs e)
		{
		}

		private void Schalter()
		{
			bool s = ClsServer.Instance.ServerOn;
			this.ChkServer.Checked = s;
			if (s)
			{
				this.GrpSettings.Enabled = false;
			}
			else
			{
				this.GrpSettings.Enabled = true;
			}
		}

		private void ChkServer_Click(object sender, EventArgs e)
		{
			bool s = ClsServer.Instance.ServerOn;
			if (s)
			{
				ClsServer.Instance.ServerOn = false;
			}
			else
			{
				ClsServer.Instance.ServerOn = true;
			}
			this.Schalter();
		}

		private void CmbIP_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.CmbIP.ContainsFocus)
			{
				ClsServer.Instance.IP = this.CmbIP.Text;
			}
		}

		private void NudHTTPPort_ValueChanged(object sender, EventArgs e)
		{
			if (this.NudHTTPPort.ContainsFocus)
			{
				ClsServer.Instance.HttpsvPort = (int)this.NudHTTPPort.Value;
			}
		}

		private void NudWSPort_ValueChanged(object sender, EventArgs e)
		{
			if (this.NudWSPort.ContainsFocus)
			{
				ClsServer.Instance.WsPort = (int)this.NudWSPort.Value;
			}
		}

		private void TxtPasswort_TextChanged(object sender, EventArgs e)
		{
			Core.DBControler.ClsOptionsControler.SavePassword("Password", this.TxtPasswort.Text);
		}

		private void BtnOK_Click(object sender, EventArgs e)
		{
			this.Hide();
		}
	}
}
