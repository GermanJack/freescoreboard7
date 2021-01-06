using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Interpreter;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace FreeScoreBoard.Server
{
	public class FSB : WebSocketBehavior
	{
		private string MyName;
		private static int MyNumber = 0;
		private readonly string MyPrefix;
		private string binPfad = "";
		private string binTyp = "";
		//private string binProfil = "";
		private bool loggedon = false;

		//private Display.ClsKommunikation ddk = new Display.ClsKommunikation();

		protected override void OnMessage(MessageEventArgs e)
		{
			try
			{
				var name = this.Context.QueryString["name"];

				if (e.IsBinary && this.loggedon)
				{
					byte[] bytearr = e.RawData;
					File.WriteAllBytes(this.binPfad, bytearr);

					if (this.binTyp == "Bild")
					{
						// Bildliste senden
						string ret = ClsRequests.DataRequest(new ClsCommand(command: "PictureList"));
						this.SendAsync(ret, null);
					}

					if (this.binTyp == "Font")
					{
						// Fontliste senden
						string ret = ClsRequests.DataRequest(new ClsCommand(command: "FontList"));
						this.SendAsync(ret, null);
					}

					if (this.binTyp == "Audio")
					{
						// Audiofileliste senden
						string ret = ClsRequests.DataRequest(new ClsCommand(command: "AudioFileList"));
						this.SendAsync(ret, null);
					}

					return;
				}

				string x = !name.IsNullOrEmpty() ? String.Format("\"{0}\" to {1}", e.Data, name) : e.Data;

				GlobalServerEvents.Fire_GotMessage(this, new ClsStringEventArgs(x));

				ClsCommand rb = new JavaScriptSerializer().Deserialize<ClsCommand>(e.Data);

				if (rb.Command == "strPic")
				{
					this.binTyp = "Bild";
					this.binPfad = Folder(rb.Value1, @"pictures\" + rb.Value2);
					return;
				}

				if (rb.Command == "strFont")
				{
					this.binTyp = "Font";
					this.binPfad = Folder(rb.Value1, "fonts");
					return;
				}

				if (rb.Command == "strSnd")
				{
					this.binTyp = "Audio";
					this.binPfad = Folder(rb.Value1, "sounds");
					return;
				}

				if (rb.Domain == "AN")
				{
					if (rb.Command == "allVarValues")
					{
						List<ClsTextVariabeln> tvl = ClsDBVariablen.Instance.GetAllTextVariablen();
						for (int i = 0; i < tvl.Count; i++)
						{
							GlobalServerEvents.SendMessage("server", new ClsStringEventArgs(ClsVarCom.UpdateDivContentStr(tvl[i].ID, tvl[i].Wert)));
						}
					}
				}

				if (rb.Type == "req")
				{
					string ret = ClsRequests.DataRequest(rb);
					//int z = 0;
					do
					{
						//z++;
						//if (z > 10000)
						//{
						//    throw new Exception("Connection not open");
						//}
					} while (this.State != WebSocketState.Open);

					this.SendAsync(ret, null);
				}
				else if (rb.Type == "logon") // Anmeldung
				{
					//string user = Core.DBControler.ClsOptionsControler.Options3("UserName").Value;
					string pwd = Core.DBControler.ClsOptionsControler.Password("Password");

					if (pwd == rb.Value1)
					{
						this.loggedon = true;
					}

					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "loggedon";
					cmd.Value1 = this.loggedon.ToString();
					this.SendAsync(new JavaScriptSerializer().Serialize(cmd), null);
					return;
				}
				else if (rb.Type == "loggedon") // Fragen ob angemeldet
				{
					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "loggedon";
					cmd.Value1 = this.loggedon.ToString();
					this.SendAsync(new JavaScriptSerializer().Serialize(cmd), null);
					return;
				}
				else
				{
					if (!this.loggedon)
					{
						ClsCommand cmd = new ClsCommand();
						cmd.Command = "nologgedon";
						this.SendAsync(new JavaScriptSerializer().Serialize(cmd), null);
						return;
					}

					ClsCommands.Command(rb);
				}
			}
			catch (Exception)
			{

			}
		}

		protected override void OnOpen()
		{
			System.Net.IPAddress clientIPAddress = this.Context.UserEndPoint.Address;
			this.MyName = clientIPAddress.ToString();
			GlobalServerEvents.Fire_NewClient(this, new ClsStringEventArgs(this.MyName));

			// Wenn Webseite lokal Ausgeführt wird, dann automatisch anmelden
			if (this.MyName == GlobalServerEvents.ServerIP)
			{
				this.loggedon = true;
			}
		}

		protected override void OnClose(CloseEventArgs e)
		{
			//Sessions.Broadcast(String.Format("{0} got logged off...", this.myName));
			//System.Net.IPAddress clientIPAddress = Context.UserEndPoint.Address;
			// this.myName = clientIPAddress.ToString();
			GlobalServerEvents.Fire_ClientClosed(this, new ClsStringEventArgs(this.MyName));
		}

		public FSB()
		  : this(null)
		{
		}

		public FSB(string prefix)
		{
			this.MyPrefix = !prefix.IsNullOrEmpty() ? prefix : "anon#";
		}

		private string GetName()
		{
			var name = this.Context.QueryString["name"];
			return !name.IsNullOrEmpty() ? name : this.MyPrefix + GetNumber();
		}

		private static int GetNumber()
		{
			return Interlocked.Increment(ref MyNumber);
		}

		private static string HtmlFileName(string FileName)
		{
			char[] tmp = FileName.ToLower().ToCharArray();
			for (int i = 0; i < tmp.Length; i++)
			{
				int c = (int)tmp[i];
				if (c > 127)
				{
					tmp[i] = '_';
				}
			}

			return new string(tmp);
		}

		private static string Folder(string FileName, string SubFolder)
		{
			string f = FileName.Replace(' ', '_');
			f = HtmlFileName(f);

			string folder1 = Path.Combine(ClsMain.AppFolder, "web", SubFolder, f);
			string fName = Path.GetFileNameWithoutExtension(folder1);
			string ext = Path.GetExtension(folder1);

			// falls Dateiname schon existiert wird eine Zahl angehängt
			int i = 0;
			while (File.Exists(folder1))
			{
				string c = "(" + i + ")";
				folder1 = Path.Combine(ClsMain.AppFolder, "web", SubFolder, fName + c + ext);
				i++;
			}

			return folder1;
		}
	}
}
