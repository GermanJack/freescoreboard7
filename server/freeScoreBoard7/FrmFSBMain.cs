using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core;
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.DBControler;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Core.Objekte;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Forms.Dialoge;
using FreeScoreBoard.Forms.Display;
using FreeScoreBoard.Forms.Objekte;
using FreeScoreBoard.Server;

namespace FreeScoreBoard
{
	public partial class FrmFSBMain : Form
	{
		private readonly FrmSplash mysplash;
		private FrmDisplay DisplayWindow;
		private bool myShowDisplay = false;
		private ServerSettings ServerSettingWindow = new ServerSettings();

		public FrmFSBMain(FrmSplash Splash)
		{
			this.mysplash = Splash;

			this.InitializeComponent();
			string programdata = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
			ClsMain.AppFolder = Path.Combine(programdata, "FreeScoreBoard7");
			ClsMain.WebFolder = Path.Combine(ClsMain.AppFolder, "web");
			ClsMain.DisplayFolder = Path.Combine(ClsMain.WebFolder, "display");
			ClsMain.ControlFolder = Path.Combine(ClsMain.WebFolder, "");
			ClsMain.PictureFolder = Path.Combine(ClsMain.WebFolder, "pictures");
			ClsMain.AudioFolder = Path.Combine(ClsMain.WebFolder, "sounds");
			ClsMain.SlideshowFolder = Path.Combine(ClsMain.WebFolder, "slideshowpictures");
			ClsMain.FontFolder = Path.Combine(ClsMain.WebFolder, "fonts");
			ClsMain.CSSFolder = Path.Combine(ClsMain.WebFolder, "css");
			ClsMain.TPMLFolder = Path.Combine(ClsMain.WebFolder, "designs_tmpl");
			ClsMain.WebDesignsFolder = "designs";

			if (!Directory.Exists(ClsMain.AppFolder))
			{
				Directory.CreateDirectory(ClsMain.AppFolder);
			}

			// Eventhandler initialisieren
			ClsGlobalEvents.DisplayPropChanged += this.Display_PropChanged;
			ClsGlobalEvents.ToogleDisplay += this.ToogleDisplay;
			ClsGlobalEvents.PlayAudio += this.PlayAudio;
		}

		private void BtnKontrolle_Click(object sender, EventArgs e)
		{
			string url = ClsServer.Instance.LinkBedienung;
			System.Diagnostics.Process.Start(url);
		}

		private void BtnAnzeige_Click(object sender, EventArgs e)
		{
			string url = ClsServer.Instance.LinkDisplay;
			System.Diagnostics.Process.Start(url);
		}

		private void PlayAudio(object sender, ClsStringEventArgs e)
		{
			MethodInvoker methodInvokerDelegate = delegate ()
			{
				string no = e.Argument;
				if (no != "0")
				{
					string ton = Core.DBControler.ClsOptionsControler.Options3("Ton" + no).Value;
					ClsSoundplayer.Instance.TPlay(ton + "|" + no);

				}
				else
				{
					for (int i = 1; i < 21; i += 1)
					{
						ClsSoundplayer.Instance.Close(i.ToString());

						// Web
						ClsServer.Instance.SendMessage(Interpreter.ClsVarCom.AllAudioStopString());
					}
				}
			};

			if (this.InvokeRequired)
			{
				this.Invoke(methodInvokerDelegate);
			}
			else
			{
				methodInvokerDelegate();
			}
		}

		private void FrmFSBMain_Load(object sender, EventArgs e)

		{
			try
			{
				// DB Prüfen
				if (!File.Exists(Path.Combine(ClsMain.AppFolder, "freescoreboard.sqlite")))
				{
					MessageBox.Show("Datenbank nicht gefunden.\r\nProgramm wird beendet.");
					Application.Exit();
				}

				// Anzeigeseite Prüfen
				if (!File.Exists(Path.Combine(ClsMain.DisplayFolder, "index.html")))
				{
					MessageBox.Show("Anzeigeseite nicht gefunden.\r\nProgramm wird beendet.");
					Application.Exit();
				}

				// Kontrollseite Prüfen
				if (!File.Exists(Path.Combine(ClsMain.WebFolder, "control/index.html")))
				{
					MessageBox.Show("Kontrollseite nicht gefunden.\r\nProgramm wird beendet.");
					Application.Exit();
				}

				// Load Main window size and position
				this.Load_PosSiz();

				// initialize server
				ClsServer.Instance.IniAddresses();
				//ClsServer.Instance.IniServerWS();
				//ClsServer.Instance.IniServerHTTP();


				ClsServer.Instance.ServerOn = true;

				// initialize variable values
				ClsDBVariablen.Instance.Ini();

				// initialize Timer and clocks
				ClsZeitkontroler.Instance.IniUhren();

				// load Display window
				bool DevTools = false;
				this.DisplayWindow = new FrmDisplay(DevTools);
				this.IniDisplay();
				string o = ClsOptionsControler.Options3("DisplayAtStartup").Value;
				if (o == "on")
				{
					this.ShowDisplay = true;
				}
				else
				{
					this.ShowDisplay = false;
				}

				// load inital display page
				ClsGlobal.Instance.ActivePage = ClsPages.GetFirstPageName(ClsPageSets.ActivePageSet());

				ClsTabellenfunktionen.IniTableFilter();

				// Debugmode
				string mode = System.Configuration.ConfigurationManager.AppSettings["Modus"];

				// Fenstertitel setzten
				string programmtitel = ClsOptionsControler.Options3("Programmtitel").Value;
				if (programmtitel == "")
				{
					programmtitel = "http://www.FreeScoreBoard.org";
				}
				this.Text = programmtitel + " " + Application.ProductVersion;

				// Versionscheck
				string vc = ClsOptionsControler.Options3("Versionscheck").Value;
				if (vc == "True")
				{
					ClsFunktionen.Versioncheck2(false);
				}

				// Turnier 0 laden
				ClsGlobal.Instance.TurnierID = 0;

				// registrierung prüfen
				Registrierung reg = ClsRegistrierungsControler.registrierung();
				if (reg == null)
				{
					// DlgRegistrieren regdlg = new DlgRegistrieren();
					// regdlg.ShowDialog();
					reg = new Registrierung
					{
						extID = Guid.NewGuid().ToString()
					};
				}

				ClsWebservice c = new ClsWebservice();
				ClsWebservice.Senden(reg);

				// hotkeys laden
				//this.LoadHotKeys();

				// set initial teams
				string teamAid = ClsOptionsControler.Options3("TeamA").Value;
				string teamBid = ClsOptionsControler.Options3("TeamB").Value;
				ClsGlobal.Instance.MansschaftAID = teamAid;
				ClsGlobal.Instance.MansschaftBID = teamBid;

				// set fist page
				//this.ctrlPageSelector1.SetAnzMod(ClsPages.GetFirstPageName(activeSet));

				// Timer starten
				ClsZeitkontroler.Instance.HeartBeatStatus = true;
				//ClsZeitkontroler.Instance.HeartBeatStatus = false;
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				this.mysplash.Close();
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
			finally
			{
				this.Cursor = Cursors.Default;
				this.mysplash.Close();
				//this.Check_Eventamount();
			}
		}

		private void Save_PosSiz()
		{
			FormWindowState wstate = this.WindowState;
			if (wstate == FormWindowState.Minimized)
			{
				return;
			}

			StringBuilder sb = new StringBuilder();
			sb.Append(this.Location.X.ToString()).Append(",");
			sb.Append(this.Location.Y.ToString()).Append(",");
			sb.Append(this.Size.Width.ToString()).Append(",");
			sb.Append(this.Size.Height.ToString());

			Options3 o = ClsOptionsControler.Options3("MainPosSiz");
			o.Value = sb.ToString();
			ClsOptionsControler.SaveOptions3(o);


			Options3 s = ClsOptionsControler.Options3("MainWindowState");
			s.Value = this.WindowState.ToString();
			ClsOptionsControler.SaveOptions3(s);
		}

		private void Load_PosSiz()
		{
			string s = ClsOptionsControler.Options3("MainWindowState").Value;
			this.WindowState = (FormWindowState)Enum.Parse(typeof(FormWindowState), s);

			string[] possiz = ClsOptionsControler.Options3("MainPosSiz").Value.Split(',');

			Point pos = new Point(Convert.ToInt32(possiz[0]), Convert.ToInt32(possiz[1]));
			this.Location = pos;

			Size siz = new Size(Convert.ToInt32(possiz[2]), Convert.ToInt32(possiz[3]));
			this.Size = siz;

			if (!this.IsOnScreen(this))
			{
				this.Location = new Point(10, 10);
			}

		}

		/// <summary>
		/// Check with this function if Form is fully on screen
		/// </summary>
		/// <param name="form"></param>
		/// <returns></returns>
		public bool IsOnScreen(Form form)
		{
			Screen[] screens = Screen.AllScreens;
			foreach (Screen screen in screens)
			{
				Rectangle formRectangle = new Rectangle(form.Left, form.Top,
														 form.Width, form.Height);

				if (screen.WorkingArea.Contains(formRectangle))
				{
					return true;
				}
			}

			return false;
		}

		private void BtnServer_Click(object sender, EventArgs e)
		{
			this.ServerSettingWindow.Ini();
			this.ServerSettingWindow.Show();
		}

		private void BtnAnzeigeFenster_Click(object sender, EventArgs e)
		{
			this.ToogleDisplay(sender, e);
			this.BringToFront();
		}

		private void IniDisplay()
		{
			MethodInvoker methodInvokerDelegate = delegate ()
			{
				int displaynumber = Screen.AllScreens.Length - 1;
				int MonNo = Convert.ToInt32(ClsOptionsControler.Options3("DisplayScreenNumber").Value);
				if (displaynumber > MonNo)
				{
					displaynumber = MonNo;
				}

				if (ClsOptionsControler.Options3("DisplayScreenFull").Value == "on")
				{
					this.DisplayWindow.Location = Screen.AllScreens[displaynumber].WorkingArea.Location;
					this.DisplayWindow.Height = Screen.AllScreens[displaynumber].WorkingArea.Height;
					this.DisplayWindow.Width = Screen.AllScreens[displaynumber].WorkingArea.Width;
				}
				else
				{
					int posx = Convert.ToInt32(ClsOptionsControler.Options3("DisplayScreenLeft").Value);
					int posy = Convert.ToInt32(ClsOptionsControler.Options3("DisplayScreenTop").Value);
					int sizx = Convert.ToInt32(ClsOptionsControler.Options3("DisplayScreenWidth").Value);
					int sizy = Convert.ToInt32(ClsOptionsControler.Options3("DisplayScreenHeight").Value);

					this.DisplayWindow.Location = new Point(posx, posy);
					this.DisplayWindow.Height = sizy;
					this.DisplayWindow.Width = sizx;
				}

			};

			if (this.InvokeRequired)
			{
				this.Invoke(methodInvokerDelegate);
			}
			else
			{
				methodInvokerDelegate();
			}
		}

		/// <summary>
		/// show display Window and set size and pos or hide it
		/// </summary>
		public bool ShowDisplay
		{
			set
			{
				try
				{
					this.myShowDisplay = value;

					if (this.DisplayWindow == null)
					{
						return;
					}

					this.DisplayWindow.Hide();

					if (this.myShowDisplay == true)
					{

						this.DisplayWindow.Show();
					}
				}
				catch (Exception ex)
				{
					ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				}
			}

			get
			{
				return this.myShowDisplay;
			}
		}

		private void ToogleDisplay(object sender, EventArgs e)
		{
			MethodInvoker methodInvokerDelegate = delegate ()
			{
				this.ShowDisplay = !this.ShowDisplay;
			};

			if (this.InvokeRequired)
			{
				this.Invoke(methodInvokerDelegate);
			}
			else
			{
				methodInvokerDelegate();
			}
		}

		private void Display_PropChanged(object sender, ClsObjectEventArgs e)
		{
			this.IniDisplay();
		}

		private void FrmFSBMain_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (this.ServerSettingWindow != null)
			{
				this.ServerSettingWindow.Hide();
			}

			if (this.DisplayWindow != null)
			{
				this.ShowDisplay = false;
				this.DisplayWindow.Close();
			}

			ClsServer.Instance.ServerOn = false;

			this.Save_PosSiz();
		}
	}
}
