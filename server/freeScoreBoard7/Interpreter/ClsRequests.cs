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
using FreeScoreBoard.Core.DB;
using FreeScoreBoard.Core.Display;
using FreeScoreBoard.Core.Kontrolle;
using FreeScoreBoard.Core.Timer;
using FreeScoreBoard.Core.Turnier;
using FreeScoreBoard.Core.Variablen;
using FreeScoreBoard.Server;
using WebSocketSharp;

namespace FreeScoreBoard.Interpreter
{
	public class ClsRequests
	{
		//  Requests werden je Client session hier behandelt und die Daten dierekt nur an den anfragenden Client gesendet

		public static string DataRequest(ClsCommand Request)
		{
			if (Request.Command == "Version")
			{
				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "";
				cmd.Type = "Txt";
				cmd.Command = "Version";
				cmd.Value1 = System.Windows.Forms.Application.ProductVersion;
				return new JavaScriptSerializer().Serialize(cmd);
			}

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
				string json = "";
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

					List<Core.DB.Spieler> man = Core.Mannschaften.ClsCommon.SpielerListe(Request.Team);
					json = new JavaScriptSerializer().Serialize(man);
				}
				else if (Request.Team == "ALL")
				{
					Request.Value1 = "ALL";
					List<Core.DB.Spieler> sp = Core.Mannschaften.ClsCommon.SpielerListe();
					json = new JavaScriptSerializer().Serialize(sp);
				}


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
				var json = new JavaScriptSerializer().Serialize(ClsPageSets.PageSets());

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "PageSets";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);

			}

			if (Request.Command == "Pages")
			{
				int psid = Convert.ToInt32(Request.PageSet);
				var json = new JavaScriptSerializer().Serialize(ClsPages.PageList(psid));

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
				int psid = Convert.ToInt32(Request.PageSet);
				int PageID = Convert.ToInt32(Request.Page);
				var json = new JavaScriptSerializer().Serialize(ClsDivs.DivList(psid, PageID));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "Divs";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "DivsActivePage")
			{
				int ActivePageSet = ClsPageSets.ActivePageSet();
				int ActivePage = ClsGlobal.Instance.ActivePage;
				// int PageID = Convert.ToInt32(Request.Page);
				var json = new JavaScriptSerializer().Serialize(ClsDivs.DivList(ActivePageSet, ActivePage));

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "DivsActivePage";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "Page")
			{
				int psid = Convert.ToInt32(Request.PageSet);
				int pid = Convert.ToInt32(Request.Page);
				DisplayPage aPage = ClsPages.Page(psid, pid);

				var json = new JavaScriptSerializer().Serialize(aPage);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "Page";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "ActivePage")
			{
				int ActivePageSet = ClsPageSets.ActivePageSet();
				int ActivePage = ClsGlobal.Instance.ActivePage;
				DisplayPage aPage = ClsPages.Page(ActivePageSet, ActivePage);

				var json = new JavaScriptSerializer().Serialize(aPage);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "DD";
				cmd.Type = "JN";
				cmd.Command = "ActivePage";
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

			if (Request.Command == "TabellenSort")
			{
				List<Core.DB.TabellenSort> ol = Core.DBControler.ClsOptionsControler.Tabellensortierung();

				var json = new JavaScriptSerializer().Serialize(ol);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "Opt";
				cmd.Type = "JN";
				cmd.Command = "TabellenSort";
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

			if (Request.Command == "TimerObjects")
			{
				List<ClsClock> cl = ClsZeitkontroler.Instance.PubClockList();
				var json = new JavaScriptSerializer().Serialize(cl);

				ClsCommand cmd = new ClsCommand();
				cmd.Domain = "Opt";
				cmd.Type = "JN";
				cmd.Command = "TimerObjects";
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
				string psn = ClsPageSets.GetPageSetNameFromID(ClsPageSets.ActivePageSet());
				string pn = ClsPages.GetPageNameFromID(ClsGlobal.Instance.ActivePage);

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "GotoPage";
				cmd.PageSet = psn;
				cmd.Page = pn;
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

			if (Request.Command == "TableFilter")
			{
				var json = new JavaScriptSerializer().Serialize(ClsGlobal.Instance.TableFilterList);

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "TableFilter";
				cmd.Type = "JN";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "Anzeigetabellen")
			{
				var json = new JavaScriptSerializer().Serialize(Core.DBControler.ClsOptionsControler.AnzeigetabelleAll());

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "Anzeigetabellen";
				cmd.Type = "JN";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "TurniereKomplett")
			{
				var json = new JavaScriptSerializer().Serialize(Core.DBControler.ClsTurnierControler.TurniereKomplett(Request.Value1));

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "TurniereKomplett";
				cmd.Type = "JN";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "TurnierSystemeKomplett")
			{
				var json = new JavaScriptSerializer().Serialize(Core.DBControler.ClsTurnierControler.TurniereKomplett("System"));

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "TurnierSystemeKomplett";
				cmd.Type = "JN";
				cmd.Value1 = json;
				return new JavaScriptSerializer().Serialize(cmd);
			}

			if (Request.Command == "turnierID")
			{
				int tid = ClsGlobal.Instance.TurnierID;

				ClsCommand cmd = new ClsCommand();
				cmd.Command = "turnierID";
				cmd.Type = "JN";
				cmd.Value1 = tid.ToString();
				return new JavaScriptSerializer().Serialize(cmd);
			}

			return "";
		}

	}
}
