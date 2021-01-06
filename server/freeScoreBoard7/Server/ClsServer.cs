using System;
using System.Diagnostics;
using System.IO;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using FreeScoreBoard.Classes;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace FreeScoreBoard.Server
{
	public sealed class ClsServer
	{
		private static readonly System.Lazy<ClsServer> Lazy = new System.Lazy<ClsServer>(() => new ClsServer());
		private readonly static string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;

		public static ClsServer Instance
		{
			get { return Lazy.Value; }
		}

		public bool ServerOn
		{
			get
			{
				return this.myServerOn;
			}

			set
			{
				if (value != this.myServerOn)
				{
					this.SwitchServerStatus(value);
				}

				this.myServerOn = value;
			}
		}

		public string LinkBedienung
		{
			get
			{
				return this.myLinkBedienung;
			}
		}

		public string LinkDisplay
		{
			get
			{
				return this.myLinkDisplay;
			}

			set
			{
				this.myLinkDisplay = "http://" + this.ip + ":" + this.httpsvport.ToString() + ClsMain.DisplayFolder + value + ".html" + "?wsp=" + this.wsport.ToString();
			}
		}

		public string IP
		{
			get
			{
				return this.ip;
			}

			set
			{
				this.ip = value;
			}
		}

		public int HttpsvPort
		{
			get
			{
				return this.httpsvport;
			}

			set
			{
				this.httpsvport = this.NextFreePort(value);
			}
		}

		public int WsPort
		{
			get
			{
				return this.wsport;
			}
			set
			{
				this.wsport = this.NextFreePort(value);
			}

		}

		private string myLinkBedienung;
		private string myLinkDisplay;

		private bool myServerOn = false;

		private HttpServer httpsv;

		private WebSocketServer wssv;

		private string ip;
		private int httpsvport;
		private int wsport;

		//public event OwnStringEventHandler ClientChange;
		public event OwnStringEventHandler ServerOnChange;

		public void IniAddresses()
		{
			this.ip = this.GetLocalIPAddress().ToString();
			this.httpsvport = this.NextFreePort(4846);
			this.wsport = this.NextFreePort(4646);
		}

		public void IniServerWS()
		{
			// this.ip = this.GetLocalIPAddress().ToString();

			// this.wsport = this.NextFreePort(4646);
			this.wssv = null;
			this.wssv = new WebSocketServer("ws://" + this.ip + ":" + this.wsport.ToString());
			GlobalServerEvents.ServerIP = this.ip;

			// Add the WebSocket services.
			this.wssv.AddWebSocketService<FSB>("/FSB");
		}

		public void IniServerHTTP()
		{
			// this.ip = this.GetLocalIPAddress().ToString();

			// this.httpsvport = this.NextFreePort(4846);
			this.httpsv = null;
			this.httpsv = new HttpServer(this.httpsvport);

			// Set the document root path.
			this.httpsv.DocumentRootPath = Path.Combine(Application.StartupPath, "web");
			if (!string.IsNullOrEmpty(ClsMain.AppFolder))
			{
				this.httpsv.DocumentRootPath = Path.Combine(ClsMain.AppFolder, ClsMain.WebFolder);
			}

			// Set the HTTP GET request event.
			this.httpsv.OnGet += this.Http_OnGet;

			this.myLinkDisplay = "http://" + this.ip + ":" + this.httpsvport.ToString() + "/display/index.html" + "?wsp=" + this.wsport.ToString();


			this.myLinkBedienung = "http://" + this.ip + ":" + this.httpsvport.ToString() + "/control/index.html?wsp=" + this.wsport.ToString();
		}

		public int SendMessage(string Text)
		{
			try
			{
				if (this.wssv == null)
				{
					return 1;
				}

				if (this.ServerOn)
				{
					this.wssv.WebSocketServices.BroadcastAsync(Text, null);
					return 0;
				}

				return 1;
			}
			catch (Exception ex)
			{
				Debug.Print(ex.Message);
				return 2;
			}
		}

		private void SwitchServerStatus(bool value)
		{
			if (!value)
			{
				//this.httpsvpw.Stop();
				this.httpsv.Stop();
				this.httpsv.OnGet -= this.Http_OnGet;
				this.wssv.Stop();
				ServerOnChange?.Invoke(this, new ClsStringEventArgs(""));

				this.myLinkDisplay = "";
				this.myLinkBedienung = "";
			}
			else
			{
				this.IniServerHTTP();
				this.IniServerWS();

				//this.httpsvpw.Start();
				this.httpsv.Start();
				this.wssv.Start();

				if (this.wssv.IsListening)
				{
					ServerOnChange?.Invoke(this, new ClsStringEventArgs(""));
				}
			}
		}

		private System.Net.IPAddress GetLocalIPAddress()
		{
			var host = System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName());
			foreach (var ip in host.AddressList)
			{
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
				{
					return ip;
				}
			}

			throw new Exception("Kein Netzwerkadapter mit einer IPv4 Adresse gefunden!");
		}

		private bool IsPortFree(int port)
		{
			bool inFree = true;

			IPGlobalProperties ipProperties = IPGlobalProperties.GetIPGlobalProperties();
			foreach (System.Net.IPEndPoint endPoint in ipProperties.GetActiveTcpListeners())
			{
				if (endPoint.Port == port)
				{
					inFree = false;
					break;
				}
			}

			return inFree;
		}

		private int NextFreePort(int StartPort)
		{
			int port = StartPort;

			while (!this.IsPortFree(port))
			{
				port++;
			}

			return port;
		}

		private void Http_OnGet(object sender, HttpRequestEventArgs e)
		{
			var req = e.Request;
			var res = e.Response;

			Debug.Print(e.Request.HttpMethod.ToString());

			var path = req.RawUrl.Split('?')[0];
			if (path == "/")
				path += "index.html";

			if (!e.TryReadFile(path, out byte[] contents))
			{
				res.StatusCode = (int)WebSocketSharp.Net.HttpStatusCode.NotFound;
				return;
			}

			if (path.EndsWith(".html"))
			{
				res.ContentType = "text/html";
				res.ContentEncoding = Encoding.UTF8;
			}
			else if (path.EndsWith(".js"))
			{
				res.ContentType = "application/javascript";
				res.ContentEncoding = Encoding.UTF8;
			}

			res.ContentLength64 = contents.LongLength;
			try
			{
				res.Close(contents, true);
			}
			catch (Exception ex)
			{
				Interpreter.Error.ClsError.Error(Name, MethodBase.GetCurrentMethod().ToString(), ex, true);
			}
		}
	}
}
