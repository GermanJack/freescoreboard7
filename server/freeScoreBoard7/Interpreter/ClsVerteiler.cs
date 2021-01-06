using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.Windows;
using FreeScoreBoard.Classes;
using FreeScoreBoard.Core;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Core.Kontrolle;
using FreeScoreBoard.Core.Localisation;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Server;
using WebSocketSharp;

namespace FreeScoreBoard.Interpreter
{
	public class ClsVerteiler
	{
		//private string MyName;
		//private static int MyNumber = 0;
		//private string MyPrefix;
		//private string binPfad = "";
		//private string binTyp = "";
		//private string binPageSet = "";

		//  Requests werden je Client session hier behandelt und die Daten dierekt nur an den anfragenden Client gesendet

		#region Requests
		public static string DataRequest(ClsCommand Request)
		{
			if (Request.Command == "TeamList")
			{
				List<Core.DB.Mannschaften> man = Core.Mannschaften.ClsCommon.Mannschaften();
				var json = new JavaScriptSerializer().Serialize(man);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "MM";
				cmd.Type = "JN";
				cmd.Command = "TeamList";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "TeamID")
			{
				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "MM";
				cmd.Type = "JN";
				cmd.Command = "TeamID";

				if (Request.Team == "A")
				{
					cmd.Team = "A";
					cmd.Value1 = ClsGlobal.Instance.MansschaftAID;
				}

				if (Request.Team == "B")
				{
					cmd.Team = "B";
					cmd.Value1 = ClsGlobal.Instance.MansschaftBID;
				}

				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "PlayerList")
			{
				if (string.IsNullOrEmpty(Request.Team))
				{
					if (Request.Value1 == "A")
					{
						Request.Team = ClsGlobal.Instance.MansschaftAID;
					}

					if (Request.Value1 == "B")
					{
						Request.Team = ClsGlobal.Instance.MansschaftBID;
					}
				}

				List<Core.DB.Spieler> man = Core.Mannschaften.ClsCommon.SpielerListe(Request.Team);
				var json = new JavaScriptSerializer().Serialize(man);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "MM";
				cmd.Type = "JN";
				cmd.Command = "PlayerList";
				cmd.Team = Request.Team;
				cmd.Value1 = Request.Value1;
				cmd.Value2 = json;

				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "SVariablen")
			{
				// Variablenliste senden
				List<ClsTextVariabeln> svl = ClsDBVariablen.Instance.GetAllTextVariablen();
				var json = new JavaScriptSerializer().Serialize(svl);

				ClsCommand cmd = new ClsCommand();
				//cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "ListeTextVariablen";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "BVariablen")
			{
				// Variablenliste senden
				List<ClsBildVariabeln> bvl = ClsDBVariablen.Instance.GetAllBildVariablen();
				var json = new JavaScriptSerializer().Serialize(bvl);

				ClsCommand cmd = new ClsCommand();
				//cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "ListeBildVariablen";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "TVariablen")
			{
				// Variablenliste senden
				List<ClsTabellenVariabeln> bvl = ClsDBVariablen.Instance.GetAllTabellenVariablen();
				var json = new JavaScriptSerializer().Serialize(bvl);

				ClsCommand cmd = new ClsCommand();
				//cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "ListeTabellenVariablen";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "PictureList")
			{
				// send PictureList
				var json = new JavaScriptSerializer().Serialize(ClsCommon.FileNames("pictures"));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "PictureList";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "AudioFileList")
			{
				// send PictureList
				var json = new JavaScriptSerializer().Serialize(ClsCommon.FileNames("sounds"));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "AudioFileList";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "FontList")
			{
				// send FontList
				var json = new JavaScriptSerializer().Serialize(ClsCommon.FileNames("fonts", true));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "FontList";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "PageSets")
			{
				var json = new JavaScriptSerializer().Serialize(ClsPageSets.PageSetNames());

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "PageSets";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "Pages")
			{
				var json = new JavaScriptSerializer().Serialize(ClsPages.PageList(Request.PageSet));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "Pages";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "PageList")
			{
				var json = new JavaScriptSerializer().Serialize(ClsPages.PageList(ClsPageSets.ActivePageSet()));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "KO";
				cmd.Type = "JN";
				cmd.Command = "PageList";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "PageColor")
			{
				var json = new JavaScriptSerializer().Serialize(ClsPages.GetStyleValue(Request.PageSet, Request.Page, "background-color"));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "PageColor";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "Divs")
			{
				var json = new JavaScriptSerializer().Serialize(ClsDivs.DivList(Request.PageSet, Request.Page));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "Divs";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "Page")
			{
				Page aPage = ClsPages.Page(Request.PageSet, Request.Page);

				var json = new JavaScriptSerializer().Serialize(aPage);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "Page";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "Options")
			{
				List<Core.DB.Options3> ol = Core.DBControler.ClsOptionsControler.Options3();

				var json = new JavaScriptSerializer().Serialize(ol);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "Opt";
				cmd.Type = "JN";
				cmd.Command = "Options";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "HeartBeatStatus")
			{
				bool hs = ClsZeitkontroler.Instance.HeartBeatStatus;

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "Status";
				cmd.Type = "JN";
				cmd.Command = "HeartBeatStatus";
				cmd.Value1 = hs.ToString();
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "Timers")
			{
				List<Core.DB.Timer> ol = Core.DBControler.ClsTimerControler.Timers();

				var json = new JavaScriptSerializer().Serialize(ol);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "Opt";
				cmd.Type = "JN";
				cmd.Command = "Timers";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "Timerevents")
			{
				try
				{
					int TimerNr = Convert.ToInt32(Request.Value1);
					List<Core.DB.Timerevent> ol = Core.DBControler.ClsTimerControler.TimerEvents(TimerNr);

					var json = new JavaScriptSerializer().Serialize(ol);

					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "Opt";
					cmd.Type = "JN";
					cmd.Command = "Timerevents";
					cmd.Value1 = json;
					return new JavaScriptSerializer().Serialize(cmd);
				}
				catch
				{
					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "Opt";
					cmd.Type = "JN";
					cmd.Command = "Timerevents";
					cmd.Value1 = "";
					return new JavaScriptSerializer().Serialize(cmd);
				}
			}

			if (Request.Command == "Events")
			{
				try
				{
					List<Core.DB.EreignissTyp> el = Core.DBControler.ClsEreignisControler.EreignissTypen();
					foreach (Core.DB.EreignissTyp k in el)
					{
						//k.Nummer = ClsLocalisationFunctions.Keytext("Ereignis", k.Nummer).Substring(5);
						k.Nummer = Core.DBControler.ClsTextControler.TextByNameAndNumber("Ereignis", k.Nummer);
					}

					var json = new JavaScriptSerializer().Serialize(el);

					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "Opt";
					cmd.Type = "JN";
					cmd.Command = "Events";
					cmd.Value1 = json;
					return new JavaScriptSerializer().Serialize(cmd);
				}
				catch
				{
					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "Opt";
					cmd.Type = "JN";
					cmd.Command = "Events";
					cmd.Value1 = "";
					return new JavaScriptSerializer().Serialize(cmd);
				}
			}

			if (Request.Command == "Penalties")
			{
				try
				{
					List<Core.DB.Strafen> pl = Core.DBControler.ClsOptionsControler.Strafen();
					var json = new JavaScriptSerializer().Serialize(pl);

					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "Opt";
					cmd.Type = "JN";
					cmd.Command = "Penalties";
					cmd.Value1 = json;
					return new JavaScriptSerializer().Serialize(cmd);
				}
				catch
				{
					ClsCommand cmd = new ClsCommand();
					cmd.Domain = "Opt";
					cmd.Type = "JN";
					cmd.Command = "Penalties";
					cmd.Value1 = "";
					return new JavaScriptSerializer().Serialize(cmd);
				}
			}

			if (Request.Command == "WhereToGo")
			{
				ClsCommand cmd = new ClsCommand();
				cmd.Command = "GotoPage";
				cmd.PageSet = ClsPageSets.ActivePageSet();
				cmd.Page = ClsGlobal.Instance.ActivePage;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "WebKontrols")
			{
				List<Core.DB.WebKontrols> wc = Core.DBControler.ClsKontrolControler.WebKontrols();
				var json = new JavaScriptSerializer().Serialize(wc);

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "WebKontrols";
				cmd.Type = "JN";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			return "";
		}
		#endregion

		#region Command
		public static string Command(ClsCommand rb)
		{
			string ret = "";
			try
			{
				object[] o = new object[1];
				o[0] = rb;
				//Type.GetType("void").GetMethod(cmd).Invoke(null, o);
				Type thisType = typeof(ClsVerteiler); // Type.GetType("void");
				MethodInfo theMethod = thisType.GetMethod(rb.Command, new Type[] { typeof(ClsCommand) });
				theMethod.Invoke(null, new Object[] { rb });
				return ret;
			}
			catch (Exception ex)
			{
				ret = "Fehler Command: " + rb.Command + " " + ex.InnerException;
				Console.Write(ret);
				return ret;
			}
		}


		#region PageSetfunktionen
		public static void NewPageSet(ClsCommand rb)
		{
			ClsPageSets.NewPageSet(rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PageSets"))));
		}

		public static void CopyPageSet(ClsCommand rb)
		{
			ClsPageSets.CopyPageSet(rb.PageSet, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PageSets"))));
		}

		public static void RenamePageSet(ClsCommand rb)
		{
			ClsPageSets.RenamePageSet(rb.PageSet, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PageSets"))));
		}

		public static void DelPageSet(ClsCommand rb)
		{
			ClsPageSets.DeletePageSet(rb.PageSet);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PageSets"))));
		}
		#endregion

		#region Pagefunktionen
		public static void NewPage(ClsCommand rb)
		{
			ClsPages.NewPage(rb.PageSet, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void DelPage(ClsCommand rb)
		{
			ClsPages.DelPage(rb.PageSet, rb.Page);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void CopyPage(ClsCommand rb)
		{
			ClsPages.CopyPage(rb.PageSet, rb.Page, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void RenamePage(ClsCommand rb)
		{
			ClsPages.RenamePage(rb.PageSet, rb.Page, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Pages", pageSet: rb.PageSet))));
		}

		public static void SetPageStyle(ClsCommand rb)
		{
			ClsPages.SetStyleWert(rb.PageSet, rb.Page, rb.Property, rb.Value1);

			// answer
			//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet))));
		}

		public static void Pagefarbe(ClsCommand rb)
		{
			ClsPages.SetStyleWert(rb.PageSet, rb.Page, "background-color", rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void MarkColor(ClsCommand rb)
		{
			ClsPages.SetMarkStyleWert(rb.PageSet, rb.Page, "border-color", rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Page", pageSet: rb.PageSet, page: rb.Page))));
		}
		#endregion

		#region Objektfunktionen
		public static void NewDiv(ClsCommand rb)
		{
			ClsDivs.NewDiv(rb.PageSet, rb.Page);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void DelDivs(ClsCommand rb)
		{
			ClsDivs.DeActivateDiv(rb.PageSet, rb.Page);
			ClsDivs.DelDiv(rb.PageSet, rb.Page, rb.Divs);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void CopyDivs(ClsCommand rb)
		{
			ClsDivs.CopyDiv(rb.PageSet, rb.Page, rb.Divs[0]);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void UpDivs(ClsCommand rb)
		{
			ClsDivs.UpDivs(rb.PageSet, rb.Page, rb.Divs);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void DownDivs(ClsCommand rb)
		{
			ClsDivs.DownDivs(rb.PageSet, rb.Page, rb.Divs);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Divs", pageSet: rb.PageSet, page: rb.Page))));
		}

		public static void MarkObjekt(ClsCommand rb)
		{
			ClsDivs.ActivateDiv(rb.PageSet, rb.Page, rb.Divs[0]);
		}

		public static void UnmarkObjekt(ClsCommand rb)
		{
			ClsDivs.DeActivateDiv(rb.PageSet, rb.Page);
		}

		public static void setDivStyle(ClsCommand rb)
		{
			ClsDivs.SetStyleValue(rb.PageSet, rb.Page, rb.Divs, rb.Property, rb.Value1);
		}

		public static void setDivText(ClsCommand rb)
		{
			ClsDivs.SetInnerHTML(rb.PageSet, rb.Page, rb.Divs, rb.Value1, rb.Value2);
		}

		public static void setDivAttribute(ClsCommand rb)
		{
			ClsDivs.setDivAttribute(rb.PageSet, rb.Page, rb.Divs, rb.Property, rb.Value1);
		}

		public static void RefreshDivPos(ClsCommand rb)
		{
			//this.Sessions.BroadcastAsync("DD|SB|refreshDivPos|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2, null);
			//string sendstr = "DD|SB|refreshDivPos|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2;
			//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(sendstr));

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "DD";
			cmd.Type = "SB";
			cmd.Command = "refreshDivPos";
			cmd.PageSet = rb.PageSet;
			cmd.Page = rb.Page;
			cmd.Value1 = rb.Value1;
			cmd.Value2 = rb.Value2;

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}

		public static void RefreshDivSiz(ClsCommand rb)
		{
			//this.Sessions.BroadcastAsync("DD|SB|refreshDivSiz|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2, null);
			//string sendstr = "DD|SB|refreshDivSiz|" + rb.PageSet + "|" + rb.Page + "|" + rb.Value1 + "|" + rb.Value2;
			//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(sendstr));

			ClsCommand cmd = new ClsCommand();
			cmd.Domain = "DD";
			cmd.Type = "SB";
			cmd.Command = "refreshDivSiz";
			cmd.PageSet = rb.PageSet;
			cmd.Page = rb.Page;
			cmd.Value1 = rb.Value1;
			cmd.Value2 = rb.Value2;

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(new JavaScriptSerializer().Serialize(cmd)));
		}
		#endregion

		#region Teams
		public static void TeamNew(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.NewMannschaft(rb.Team);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "TeamList"))));
		}

		public static void TeamDel(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.DelMannschaft(rb.Team);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "TeamList"))));
		}

		public static void TeamChange(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.ChangeMannschaft(rb.Team, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "TeamList"))));
		}

		public static void PlayerNew(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.NewSpieler(rb.Team, rb.Player);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PlayerList", team: rb.Team))));
		}

		public static void PlayerDel(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.DelSpieler(rb.Player);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PlayerList", team: rb.Team))));
		}

		public static void PlayerChange(ClsCommand rb)
		{
			Core.Mannschaften.ClsCommon.ChangeSpieler(rb.Player, rb.Property, rb.Value1);

			// answer
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PlayerList", team: rb.Team))));
		}

		#endregion

		#region control

		public static void SwitchPage(ClsCommand rb)
		{
			ClsDisplay.SetPage(ClsPageSets.ActivePageSet(), rb.Page);
		}

		public static void ToogleDisplay(ClsCommand rb)
		{
			ClsGlobalEvents.Fire_ToogleDisplay(null, null);
			//ClsDisplay.SetPage(ClsPageSets.ActivePageSet(), rb.Page);
		}

		public static void SetB05(ClsCommand rb)
		{
			string picfile = Core.DBControler.ClsOptionsControler.Options3("SlideshowBild" + rb.Value1).Value;
			ClsDBVariablen.Instance.SetBildVariableWert("B05", picfile);
		}

		public static void SetMannschaft(ClsCommand rb)
		{
			// rb.Value1 = A or B
			// rb.Value2 = teamID
			// rb.Value3 = maual enterd team name

			string Mannschaft = rb.Value1;
			string mIDstr = rb.Value2;
			string nameneu = rb.Value3;
			Core.DB.Mannschaften m = null;
			if (mIDstr != "0")
			{

				long mID = Convert.ToInt64(mIDstr);
				m = Core.DBControler.ClsMannschaftenControler.Mannschaft(mID);
				nameneu = m.Name;
			}

			if (Mannschaft == "A")
			{
				ClsGlobal.Instance.MansschaftAID = mIDstr;
				ClsDBVariablen.Instance.SetTextVariableWert("S01", nameneu);
				if (m != null)
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B01", m.Bild1);
					ClsDBVariablen.Instance.SetBildVariableWert("B03", m.Bild2);
					ClsDBVariablen.Instance.SetTextVariableWert("S03", m.Kurzname);
				}
				else
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B01", "");
					ClsDBVariablen.Instance.SetBildVariableWert("B03", "");
					ClsDBVariablen.Instance.SetTextVariableWert("S03", "");
				}

				//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs("|JN|TeamID|A|" + mIDstr));
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "TeamID", team: "A"))));

				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PlayerList", team: mIDstr, value1: Mannschaft))));
			}
			else
			{
				ClsGlobal.Instance.MansschaftBID = mIDstr;
				ClsDBVariablen.Instance.SetTextVariableWert("S02", nameneu);
				if (m != null)
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B02", m.Bild1);
					ClsDBVariablen.Instance.SetBildVariableWert("B04", m.Bild2);
					ClsDBVariablen.Instance.SetTextVariableWert("S04", m.Kurzname);
				}
				else
				{
					ClsDBVariablen.Instance.SetBildVariableWert("B02", "");
					ClsDBVariablen.Instance.SetBildVariableWert("B04", "");
					ClsDBVariablen.Instance.SetTextVariableWert("S04", "");
				}

				//GlobalServerEvents.SendMessage(null, new ClsStringEventArgs("|JN|TeamID|B|" + mIDstr));
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "TeamID", team: "B"))));

				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PlayerList", team: mIDstr, value1: Mannschaft))));
			}
		}

		public static void SetPlayer(ClsCommand rb)
		{
			string playerID = rb.Value1;
			ClsGlobal.Instance.SpielerID = playerID;

			Core.DB.Spieler p = null;

			if (playerID != "0")
			{
				int pID = Convert.ToInt32(playerID);
				p = Core.DBControler.ClsMannschaftenControler.Spieler(pID);

				ClsDBVariablen.Instance.SetTextVariableWert("S36", p.Vorname + ", " + p.Nachname);
				ClsDBVariablen.Instance.SetTextVariableWert("S37", p.NickName);
				ClsDBVariablen.Instance.SetBildVariableWert("B14", p.Bild);
			}
			else
			{
				ClsDBVariablen.Instance.SetTextVariableWert("S36", "");
				ClsDBVariablen.Instance.SetTextVariableWert("S37", "");
				ClsDBVariablen.Instance.SetBildVariableWert("B14", "");
			}
		}

		public static void Tor(ClsCommand rb)
		{
			string Mannschaft = "A";
			if (rb.Value1 == "S06")
			{
				Mannschaft = "B";
			}

			int Add = 1;
			if (rb.Value2 == "-")
			{
				Add = -1;
			}

			int spielerID = 0;
			try
			{
				spielerID = Convert.ToInt32(rb.Value3);
			}
			catch
			{
				spielerID = 0;
			}

			ClsKontrolfunktionen.Tore_change(spiel: ClsGlobal.Instance.AktivesSpiel, aoderb: Mannschaft, add: Add, spielerID);
		}

		public static void Tore_Reset(ClsCommand rb)
		{
			ClsKontrolfunktionen.Tore_Reset(spiel: ClsGlobal.Instance.AktivesSpiel);
		}

		public static void Foul(ClsCommand rb)
		{
			string Mannschaft = "A";
			if (rb.Value1 == "S19")
			{
				Mannschaft = "B";
			}

			int Add = 1;
			if (rb.Value2 == "-")
			{
				Add = -1;
			}

			int spielerID = 0;
			try
			{
				spielerID = Convert.ToInt32(rb.Value3);
			}
			catch
			{
				spielerID = 0;
			}

			ClsKontrolfunktionen.Foul_change(ClsGlobal.Instance.AktivesSpiel, Mannschaft, Add, spielerID);
		}

		public static void Foul_Reset(ClsCommand rb)
		{
			ClsKontrolfunktionen.Foul_Reset(spiel: ClsGlobal.Instance.AktivesSpiel);
		}

		public static void VarCount(ClsCommand rb)
		{
			bool Add = true;
			if (rb.Value2 == "-")
			{
				Add = false;
			}

			string wert = ClsDBVariablen.Instance.GetTextVariableWert(rb.Value1);
			int numwert = Convert.ToInt32(wert);

			// cancel conditions
			// Schüielabschnitt nicht kleiner als 1
			if (rb.Value1 == "S09" & numwert <= 1 && !Add)
			{
				return;
			}

			// Paintballzähler M1 nicht kleiner als 0
			if (rb.Value1 == "S46" & numwert <= 0 && !Add)
			{
				return;
			}

			// Paintballzähler M1 nicht grösser als 4
			if (rb.Value1 == "S46" & numwert >= 5 && Add)
			{
				return;
			}

			// Paintballzähler M2 nicht kleiner als 0
			if (rb.Value1 == "S47" & numwert <= 0 && !Add)
			{
				return;
			}

			// Paintballzähler M2 nicht grösser als 4
			if (rb.Value1 == "S47" & numwert >= 5 && Add)
			{
				return;
			}




			if (Add)
			{
				numwert += 1;
			}

			if (!Add && numwert > 0)
			{
				numwert -= 1;
			}

			ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, numwert.ToString());
		}

		public static void VarReset(ClsCommand rb)
		{
			// Spielabschnitt
			if (rb.Value1 == "S09")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "1");
			}

			// Paintballzähler M1
			if (rb.Value1 == "S46")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "5");
			}

			// Paintballzähler M2
			if (rb.Value1 == "S47")
			{
				ClsDBVariablen.Instance.SetTextVariableWert(rb.Value1, "5");
			}
		}

		public static void TimerOn(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			ClsZeitkontroler.Instance.ClockStart(tNr);
		}

		public static void TimerOff(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			ClsZeitkontroler.Instance.ClockStop(tNr);
		}

		public static void TimerReset(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			ClsZeitkontroler.Instance.ResetUhr(tNr);
		}

		public static void TimerManipulate(ClsCommand rb)
		{
			long tNr = Convert.ToInt32(rb.Value1);
			int sec = Convert.ToInt32(rb.Value2);

			Core.Timer.ClsZeitkontroler.Instance.ManipulateClock(tNr, sec);
		}

		public static void SetTicker(ClsCommand rb)
		{
			ClsDBVariablen.Instance.SetTextVariableWert("S34", rb.Value1);
		}

		public static void SetTablefilter(ClsCommand rb)
		{
			if (rb.Value1 == "T03")
			{
				if (rb.Value2 == "Spiel")
				{
					ClsGlobal.Instance.FilterE.SpielNr = rb.Value3;
				}

				if (rb.Value2 == "Ereignisart")
				{
					ClsGlobal.Instance.FilterE.Ereignisart = rb.Value3;
				}

				ClsDBVariablen.Instance.SetTabellenVariableWert("T03", ClsFunktionen.EreignisseToVariable_Json(ClsGlobal.Instance.TurnierID));
			}
		}

		#endregion

		#region Options

		public static void SetOptValue(ClsCommand rb)
		{
			Core.DB.Options3 o3 = Core.DBControler.ClsOptionsControler.Options3(rb.Value1);
			o3.Value = rb.Value2;
			Core.DBControler.ClsOptionsControler.SaveOptions3(o3);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Options"))));

			if (rb.Value1.Length > 12)
			{
				if (rb.Value1.Substring(0, 13) == "DisplayScreen")
				{
					ClsGlobalEvents.Fire_DisplayPropChanged(null, null);
				}
			}
		}

		public static void SetTimer(ClsCommand rb)
		{
			Core.DB.Timer t = new JavaScriptSerializer().Deserialize<Core.DB.Timer>(rb.Value1);

			Core.DBControler.ClsTimerControler.SaveTimer(t);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Timers"))));
		}

		public static void SetTimerEvent(ClsCommand rb)
		{
			Core.DB.Timerevent t = new JavaScriptSerializer().Deserialize<Core.DB.Timerevent>(rb.Value1);
			Core.DBControler.ClsTimerControler.SaveTimerEvent(t);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Timerevents", value1: t.TimerNr.ToString()))));
		}

		public static void AddTimerEvent(ClsCommand rb)
		{
			int tnr = Convert.ToInt32(rb.Value1);
			Core.DB.Timerevent m = new Core.DB.Timerevent();
			m.TimerNr = tnr;
			m.Active = true;
			m.Sekunden = 0;
			m.Eventtype = 0;
			//m.Name = "";
			m.Soundfile = "";
			m.Layer = -1;
			m.AndereTimerNr = -1;

			Core.DBControler.ClsTimerControler.AddTimerEvent(m);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Timerevents", value1: rb.Value1))));
		}

		public static void DelTimerEvent(ClsCommand rb)
		{
			int tnr = Convert.ToInt32(rb.Value1);
			Core.DBControler.ClsTimerControler.DelTimerEvent(tnr);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Timerevents", value1: rb.Value2))));
		}

		public static void delPic(ClsCommand rb)
		{
			string file = Path.Combine(ClsMain.AppFolder, "web", "pictures", rb.Value1);
			if (File.Exists(file))
			{
				File.Delete(file);
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "PictureList"))));
			}
		}

		public static void delSnd(ClsCommand rb)
		{
			string file = Path.Combine(ClsMain.AppFolder, "web", "sounds", rb.Value1);
			if (File.Exists(file))
			{
				File.Delete(file);
				GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "AudioFileList"))));
			}
		}

		public static void delFont(ClsCommand rb)
		{
			string file = Path.Combine(ClsMain.AppFolder, "web", "css", "fonts.css");

			// delete files
			string searchfolder = Path.Combine(ClsMain.AppFolder, "web", "fonts");
			string[] files = Directory.GetFiles(searchfolder, rb.Value1 + ".*");
			for (int i = 0; i < files.Count(); i++)
			{
				if (File.Exists(files[i]))
				{
					File.Delete(files[i]);

				}
			}

			// delet font-face entries
			// ??



			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "FontList"))));
		}

		public static void SetEvent(ClsCommand rb)
		{
			int id = Convert.ToInt32(rb.Value1);
			Core.DB.EreignissTyp erg = Core.DBControler.ClsEreignisControler.EreignissTyp(id);
			erg.Log = Convert.ToBoolean(rb.Value2);
			Core.DBControler.ClsEreignisControler.SaveEreignissTyp(erg);

			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Events"))));
		}

		public static void SetPenaltie(ClsCommand rb)
		{
			Core.DB.Strafen s = new JavaScriptSerializer().Deserialize<Core.DB.Strafen>(rb.Value1);
			Core.DBControler.ClsOptionsControler.SaveStrafe(s);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Penalties", value1: rb.Value1))));
		}

		public static void AddPenaltie(ClsCommand rb)
		{
			Core.DB.Strafen n = new Core.DB.Strafen();
			n.Bezeichnung = rb.Value1;
			Core.DBControler.ClsOptionsControler.AddStrafe(n);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Penalties", value1: rb.Value1))));
		}

		public static void DelPenaltie(ClsCommand rb)
		{
			Core.DBControler.ClsOptionsControler.DelStrafe(rb.Value1);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "Penalties", value1: rb.Value1))));
		}

		#endregion

		#region Debug
		public static void SetHeartBeatStatus(ClsCommand rb)
		{
			ClsZeitkontroler.Instance.HeartBeatStatus = Convert.ToBoolean(rb.Value1);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "HeartBeatStatus"))));
		}
		#endregion

		#region WebKontrols
		public static void SaveWebKontrol(ClsCommand rb)
		{
			Core.DB.WebKontrols wk = new JavaScriptSerializer().Deserialize<Core.DB.WebKontrols>(rb.Value1);
			Core.DBControler.ClsKontrolControler.SaveWebKontrol(wk);
			GlobalServerEvents.SendMessage(null, new ClsStringEventArgs(DataRequest(new ClsCommand(command: "WebKontrols"))));
		}
		#endregion
		#endregion
	}
}
