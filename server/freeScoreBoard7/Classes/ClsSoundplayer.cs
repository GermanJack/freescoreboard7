using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using FreeScoreBoard.Interpreter.Error;

namespace FreeScoreBoard.Classes
{
	public sealed class ClsSoundplayer
	{
		private static readonly System.Lazy<ClsSoundplayer> Lazy =
		 new System.Lazy<ClsSoundplayer>(() => new ClsSoundplayer());

		public static ClsSoundplayer Instance
		{
			get { return Lazy.Value; }
		}

		private string err;
		private string Name = MethodBase.GetCurrentMethod().DeclaringType.Name;
		private bool[] isOpen = new bool[100];

		/* Deklaration der MCI-Funktionen, -Konstanten und -Strukturen */

		[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
		private static extern int mciSendString(string lpstrCommand, StringBuilder lpstrReturnString, int uReturnLength, IntPtr hwndCallback);

		[DllImport("winmm.dll", CharSet = CharSet.Unicode)]
		private static extern int mciGetErrorString(int dwError, StringBuilder lpstrBuffer, int uLength);

		public void Play(string filename, string alias)
		{
			try
			{
				string a = filename + "|" + alias;
				Thread threadPlayMode = new Thread(new ParameterizedThreadStart(this.TPlay)); // .Start(filename, alias);
																							  // threadPlayMode.SetApartmentState(ApartmentState.STA);
				threadPlayMode.Start(a);
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public bool IsOpen(string alias)
		{
			{
				return this.isOpen[Convert.ToInt32(alias)];
			}
		}

		public void TPlay(object a)
		{
			try
			{
				string[] arr = a.ToString().Split('|');

				string ServerAudio = Core.DBControler.ClsOptionsControler.Options3("ServerAudio").Value;
				string KontrolleAudio = Core.DBControler.ClsOptionsControler.Options3("KontrolleAudio").Value;
				string AnzeigeAudio = Core.DBControler.ClsOptionsControler.Options3("AnzeigeAudio").Value;

				if (KontrolleAudio == "True" || AnzeigeAudio == "True")
				{
					Server.ClsServer.Instance.SendMessage(Interpreter.ClsVarCom.PlayAudioString(arr[0]));
				}

				if (ServerAudio == "False")
				{
					return;
				}

				string filename = Path.Combine(ClsMain.AudioFolder, arr[0]);
				string alias = arr[1];
				int ialias = Convert.ToInt32(alias);

				//// wenn es offen ist und das ende noch nicht erreicht ist dann schliessen
				//// wenn es offen ist und das ende erreicht ist dann schliessen und weiter
				bool offen = this.isOpen[ialias];
				if (offen)
				{
					int posit = this.Position(alias);
					int länge = this.Length(alias);
					this.Close(alias);
					if (länge != posit)
					{
						return;
					}
				}

				// Überprüfen, ob die Datei existiert
				if (File.Exists(filename) == false)
				{
					return;
				}

				// MCI-Befehlsstring zum Öffnen zusammensetzen
				//string mciString = string.Format("open \"{0}\" type mpegvideo alias {1}", filename, alias);
				string mciString = "open \"" + filename + "\" type mpegvideo alias " + alias;

				int result = mciSendString(mciString, null, 0, IntPtr.Zero);
				if (result != 0)
				{
					this.err = this.GetMciError(result);
				}

				this.isOpen[ialias] = true;

				//// Das Zeitformat für Längen- und Positionsangaben explizit
				//// auf Millisekunden setzen
				mciString = "set " + alias + " time format ms";
				result = mciSendString(mciString, null, 0, IntPtr.Zero);

				this.err = this.GetMciError(result);
				this.Start(alias);
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
			}
		}

		public int Length(string alias)
		{
			try
			{
				StringBuilder buffer = new StringBuilder(300);
				int result = mciSendString("status " + alias + " length", buffer, buffer.Capacity, IntPtr.Zero);
				if (result != 0)
				{
					this.err = this.GetMciError(result);
				}

				return int.Parse(buffer.ToString());
			}
			catch (Exception ex)
			{
				ClsError.Error(this.Name, MethodBase.GetCurrentMethod().ToString(), ex);
				return 0;
			}
		}

		public void Start(string alias)
		{
			string mciString = "play " + alias; // +
			int result = mciSendString(mciString, null, 0, IntPtr.Zero);
			if (result != 0)
			{
				// throw new MciException("Fehler beim Aufruf von 'Play': " +
				//    GetMciError(result));
				this.err = this.GetMciError(result);
			}
		}

		public int Position(string alias)
		{
			{
				StringBuilder buffer = new StringBuilder(261);
				int result = mciSendString("status " + alias + " position", buffer, buffer.Capacity, IntPtr.Zero);
				if (result != 0)
				{
					// throw new MciException("Fehler beim Lesen von 'Position': " +
					//    GetMciError(result));
					this.err = this.GetMciError(result);
				}

				return int.Parse(buffer.ToString());
			}
		}

		public void Close(string alias)
		{
			if (this.isOpen[Convert.ToInt32(alias)])
			{
				string mciString = "close " + alias;
				int result = mciSendString(mciString, null, 0, IntPtr.Zero);
				if (result != 0)
				{
					// throw new MciException("Fehler beim Aufruf von 'Close': " +
					//    GetMciError(result));
					this.err = this.GetMciError(result);
				}

				this.isOpen[Convert.ToInt32(alias)] = false;
			}
		}

		public void Dispose(string alias)
		{
			this.Close(alias);
		}

		private string GetMciError(int errorCode)
		{
			StringBuilder errorMessage = new StringBuilder(255);
			if (mciGetErrorString(errorCode, errorMessage, errorMessage.Capacity) == 0)
			{
				return "MCI-Fehler " + errorCode.ToString();
			}
			else
			{
				return errorMessage.ToString();
			}
		}
	}
}